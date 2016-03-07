using AVFoundation;
using CoreGraphics;
using Foundation;
using Plugin.CustomCamera.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.CustomCamera
{
    public class CustomCameraView : UIImageView, ICustomCameraView
    {        
        #region ICustomCameraView interface implementation

        CameraOrientation _cameraOrientation = CameraOrientation.Automatic;
        static CameraSelection _selectedCamera;
        Action<string> _callback;

        /// <summary>
        /// The selected camera, front or back
        /// </summary>
        public CameraSelection SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            set
            {
                if (value == _selectedCamera)
                    return;

                _selectedCamera = value;
                SetSelectedCamera(_selectedCamera);
            }
        }

        /// <summary>
        /// The camera orientation
        /// </summary>
        public CameraOrientation CameraOrientation {get; set;}

        /// <summary>
        /// Take a picture
        /// </summary>
        /// <param name="callback"></param>
        public void TakePicture(Action<string> callback)
        {
            _callback = callback;
            TakePicture();
        }

        /// <summary>
        /// Starts the camera
        /// </summary>
        /// <param name="selectedCamera">The selected camera, default: Back</param>
        public void Start(CameraSelection selectedCamera = CameraSelection.Back)
        {
            try
            {
                SetupUserInterface();
                //SetupEventHandlers();                
                SetupLiveCameraStream();
                SetSelectedCamera(selectedCamera);
                AuthorizeCameraUse();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        /// <summary>
        /// Stops the camera
        /// </summary>
        public void Stop()
        {
            // Do nothing
        }

        /// <summary>
        /// Call this method this to reset the camera after taking a picture
        /// </summary>
        public void Reset()
        {
            SetPicture(false);
        }
        #endregion

        AVCaptureSession _captureSession;
        AVCaptureDeviceInput _captureDeviceInput;
        AVCaptureStillImageOutput _stillImageOutput;
        UIView _liveCameraStream;
        bool _resetCamera = false;

        void SetupUserInterface()
        {
            var centerButtonX = this.Bounds.GetMidX() - 35f;
            var topLeftX = Bounds.X + 25;
            var topRightX = Bounds.Right - 65;
            var bottomButtonY = Bounds.Bottom - 150;
            var topButtonY = Bounds.Top + 15;

            _liveCameraStream = new UIView() { Frame = new CGRect(0f, 0f, Bounds.Width, Bounds.Height) };
            
            Add(_liveCameraStream);
            //Add(_picture);
            //Add(_takePictureButton);
            //Add(_toggleCameraButton);
            //Add(_toggleFlashButton);
        }

        void SetupEventHandlers()
        {
            //_takePictureButton.TouchUpInside += (object sender, EventArgs e) =>
            //{
            //    if (_resetCamera)
            //    {
            //        SetPicture(null);
            //        return;
            //    }
            
            //    TakePicture();
            //};

            //_toggleCameraButton.TouchUpInside += (object sender, EventArgs e) =>
            //{
            //    ToggleFrontBackCamera();
            //};

            //_toggleFlashButton.TouchUpInside += (object sender, EventArgs e) =>
            //{
            //    ToggleFlash();
            //};
        }

        async void TakePicture()
        {
            if (_resetCamera)
                return;

            var videoConnection = _stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await _stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            var picture = new UIImage(jpegImage);
            SetPicture(true, picture);

            NSData imgData = picture.AsJPEG();
            NSError err = null;
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var destinationPath = System.IO.Path.Combine(documentsDirectory, "picture.jpg");
            if (imgData.Save(destinationPath, false, out err))
            {
                Console.WriteLine("saved as " + destinationPath);
                Callback(destinationPath);                
            }
            else
            {
                Console.WriteLine("NOT saved as " + destinationPath + " because" + err.LocalizedDescription);
            }
        }

        void Callback(string path)
        {
            var cb = _callback;
            _callback = null;

            cb(path);
        }

        void SetPicture(bool reset, UIImage picture = null)
        {
            this.Image = picture;
            _liveCameraStream.Hidden = reset;
            _resetCamera = reset;
        }

        void SetSelectedCamera(CameraSelection selectedCamera)
        {
            if (selectedCamera.ToAVCaptureDevicePosition() == _captureDeviceInput.Device.Position)
                return;
            
            var device = GetCameraForOrientation(selectedCamera.ToAVCaptureDevicePosition());
            ConfigureCameraForDevice(device);

            _captureSession.BeginConfiguration();
            _captureSession.RemoveInput(_captureDeviceInput);
            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.CommitConfiguration();
        }

        void ToggleFlash()
        {
            var device = _captureDeviceInput.Device;

            var error = new NSError();
            if (device.HasFlash)
            {
                if (device.FlashMode == AVCaptureFlashMode.On)
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.Off;
                    device.UnlockForConfiguration();
                    //_toggleFlashButton.SetBackgroundImage(UIImage.FromFile("NoFlashButton.png"), UIControlState.Normal);
                }
                else
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.On;
                    device.UnlockForConfiguration();
                    //_toggleFlashButton.SetBackgroundImage(UIImage.FromFile("FlashButton.png"), UIControlState.Normal);
                }
            }
        }

        AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }
            return null;
        }

        void SetupLiveCameraStream()
        {
            _captureSession = new AVCaptureSession();

            var viewLayer = _liveCameraStream.Layer;
            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(_captureSession)
            {
                Frame = _liveCameraStream.Bounds
            };
            _liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            _captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            _stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            _captureSession.AddOutput(_stillImageOutput);
            _captureSession.AddInput(_captureDeviceInput);
            _captureSession.StartRunning();
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        async void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }
    }
}
