using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Plugin.CustomCamera.Abstractions;
using Android.Hardware;
using Java.IO;

namespace Plugin.CustomCamera
{
    [Register("plugin.customcamera.android.CustomCameraView")]
    public class CustomCameraView : 
        FrameLayout, 
        ICustomCameraView, 
        TextureView.ISurfaceTextureListener, 
        Camera.IPictureCallback, 
        Camera.IPreviewCallback, 
        Camera.IShutterCallback 
        //ISurfaceHolderCallback
    {
        Camera _camera;
        Activity _activity;
        Android.Graphics.SurfaceTexture _surface;
        Camera.CameraInfo _cameraInfo;
        int _w;
        int _h;
        string pictureName = "picture.jpg";
        bool _isCameraStarted = false;
        
        public CustomCameraView(Context context, IAttributeSet attrs): base(context, attrs)
        {
            this._activity = (Activity)context;

            if (CustomCameraInstance.CustomCameraView != null)
            {
                // set properties, otherwise they will be cleared
                _selectedCamera = CustomCameraInstance.CustomCameraView.SelectedCamera;
                _cameraOrientation = CustomCameraInstance.CustomCameraView.CameraOrientation;
            }

            var _textureView = new TextureView(context);
            _textureView.SurfaceTextureListener = this;
            AddView(_textureView);            

            // make this view available in the PCL
            CustomCameraInstance.CustomCameraView = this;
        }

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
                if (_selectedCamera == value)
                    return;
               
                OpenCamera(value);
                SetTexture(_surface, _w, _h);
            }
        }

        /// <summary>
        /// The camera orientation
        /// </summary>
        public CameraOrientation CameraOrientation
        {
            get
            {                
                return _cameraOrientation;
            }
            set
            {
                if (_cameraOrientation == value)
                    return;

                _cameraOrientation = value;
                SetCameraOrientation();
            }
        }

        /// <summary>
        /// Take a picture
        /// </summary>
        /// <param name="callback"></param>
        public void TakePicture(Action<string> callback)
        {
            if (_camera == null)
                return;

            _callback = callback;

            Android.Hardware.Camera.Parameters p = _camera.GetParameters();
            p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
            _camera.SetParameters(p);
            _camera.TakePicture(this, this, this);
        }

        /// <summary>
        /// Starts the camera
        /// </summary>
        /// <param name="selectedCamera">The selected camera, default: Back</param>
        /// <param name="orientation">the camera orientation, default: Automatic</param>
        public void StartCamera(CameraSelection selectedCamera = CameraSelection.Back, CameraOrientation orientation = Abstractions.CameraOrientation.Automatic)
        {
            //if (_isCameraStarted)
            //    return;

            if (_cameraOrientation == CameraOrientation.None)
                _cameraOrientation = orientation;

            if(_selectedCamera == CameraSelection.None)
                _selectedCamera = selectedCamera;

            _isCameraStarted = true;    
        
            if(_surface != null)
                OpenCamera(_selectedCamera);
        }

        /// <summary>
        /// Stops the camera
        /// </summary>
        /// <param name="callback"></param>
        public void StopCamera()
        {
            CloseCamera();
        }
                   
        #endregion

        private void Callback(string path)
        {
            var cb = _callback;
            _callback = null;

            cb(path);
        }

        //https://forums.xamarin.com/discussion/17625/custom-camera-takepicture
        void Camera.IPictureCallback.OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
        {
            FileOutputStream outStream = null;
            File dataDir = Android.OS.Environment.ExternalStorageDirectory;
            if (data != null)
            {
                try
                {
                    var path = dataDir + "/" + pictureName;
                    outStream = new FileOutputStream(path);
                    outStream.Write(data);
                    outStream.Close();
                    Callback(path);
                }
                catch (FileNotFoundException e)
                {
                    System.Console.Out.WriteLine(e.Message);
                }
                catch (IOException ie)
                {
                    System.Console.Out.WriteLine(ie.Message);
                }
            }
        }

        void Camera.IPreviewCallback.OnPreviewFrame(byte[] b, Android.Hardware.Camera c)
        {
        }

        void Camera.IShutterCallback.OnShutter()
        {
        }

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            _surface = surface;
            _w = w;
            _h = h;

            if (!_isCameraStarted)
                return;

            OpenCamera(_selectedCamera);
            SetTexture(_surface, _w, _h);
        }

        private void SetTexture(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            if (_camera == null)
                return;

            SetCameraOrientation();

            this.LayoutParameters.Width = w;
            this.LayoutParameters.Height = h;

            try
            {
                //_camera.SetPreviewCallback(this);
                //_camera.Lock();
                _camera.SetPreviewTexture(surface);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }
        
        public void SetCameraOrientation()
        {
            // Google's camera orientation vs device orientation is all over the place, it changes per api version, per device type (phone/tablet) and per device brand
            // Credits: http://stackoverflow.com/questions/4645960/how-to-set-android-camera-orientation-properly
            if (_cameraInfo == null)
                return;

            Display display = _activity.WindowManager.DefaultDisplay;
            var rotation = display.Rotation;
            int degrees = 0;

            switch (rotation)
	        {
		        case SurfaceOrientation.Rotation0:
                    degrees = 0;
                    break;
                    case SurfaceOrientation.Rotation90:
                    degrees = 90;
                    break;
                case SurfaceOrientation.Rotation180:
                    degrees = 180;
                    break;
                case SurfaceOrientation.Rotation270:
                    degrees = 270;
                    break;                
                default:
                    break;
	        }
            
            int result;
            if (SelectedCamera == CameraSelection.Front)
            {
                result = (_cameraInfo.Orientation + degrees) % 360;
                result = (360 - result) % 360;  // compensate the mirror
            }
            else
            {  // back-facing
                result = (_cameraInfo.Orientation - degrees + 360) % 360;
            }

            _camera.SetDisplayOrientation(result);
        }


        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            // ??
        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {
            // Fires whenever the surface change (moving the camera etc.)
        }

        public void OnSurfaceChanged(Android.Graphics.SurfaceTexture holder, int format, int w, int h)
        {
            // Now that the size is known, set up the camera parameters and begin
            // the preview.
            //Camera.Parameters parameters = mCamera.getParameters();
            //parameters.setPreviewSize(mPreviewSize.width, mPreviewSize.height);
            //requestLayout();
            //mCamera.setParameters(parameters);

            //// Important: Call startPreview() to start updating the preview surface.
            //// Preview must be started before you can take a picture.
            //mCamera.startPreview();
        }

        public bool OnSurfaceTextureDestroyed(Android.Graphics.SurfaceTexture surface)
        {
            CloseCamera();
            return true;
        }

        private void OpenCamera(CameraSelection cameraSelection)
        {
            CloseCamera();

            int cameraCount = 0;
            _cameraInfo = new Camera.CameraInfo();
            cameraCount = Camera.NumberOfCameras;
            for (int camIdx = 0; camIdx < cameraCount; camIdx++)
            {
                Camera.GetCameraInfo(camIdx, _cameraInfo);
                if (_cameraInfo.Facing == cameraSelection.ToCameraFacing())
                {
                    try
                    {
                        _camera = Camera.Open(camIdx);
                        
                        //Android.Hardware.Camera.Parameters p = _camera.GetParameters();
                        //p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
                        //_camera.SetParameters(p);

                        // SetPreviewCallback crashes when camera is released and called again
                        //_camera.SetPreviewCallback(this);
                        //_camera.Lock();

                        _selectedCamera = cameraSelection;
                        _isCameraStarted = true;
                    }
                    catch (Exception e)
                    {                        
                        CloseCamera();
                        Log.Error("CustomCameraView OpenCamera", e.Message);
                    }
                }
            }
        }

        private void CloseCamera()
        {
            if (_camera != null)
            {                
                //_camera.Unlock();                
                _camera.StopPreview();
                //_camera.SetPreviewCallback(null);

                _camera.Release();
                _camera = null;
                _isCameraStarted = false;
            }
        }                   
    }
}