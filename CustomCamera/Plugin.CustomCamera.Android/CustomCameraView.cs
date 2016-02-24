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

namespace Plugin.CustomCamera
{
    [Register("plugin.customcamera.android.CustomCameraView")]
    public class CustomCameraView : FrameLayout, ICustomCameraView, TextureView.ISurfaceTextureListener
    {
        Camera _camera;
        Activity _activity;
        Android.Graphics.SurfaceTexture _surface;
        int _w;
        int _h;
        
        public CustomCameraView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this._activity = (Activity)context;

            var _textureView = new TextureView(context);
            _textureView.SurfaceTextureListener = this;
            AddView(_textureView);
        }

        #region interface implementation
        CameraSelection _selectedCamera;
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

                _selectedCamera = value;
                OpenCamera();
                SetTexture(_surface, _w, _h);
            }
        }

        public void Capture()
        {
            if (_camera == null)
                return;
            //_camera.TakePicture()
        }

        #endregion

        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            _surface = surface;
            _w = w;
            _h = h;
            OpenCamera();
            SetTexture(_surface, _w, _h);            
        }

        private void SetTexture(Android.Graphics.SurfaceTexture surface, int w, int h)
        {
            switch (_activity.WindowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    _camera.SetDisplayOrientation(90);
                    break;
                case SurfaceOrientation.Rotation180:
                    _camera.SetDisplayOrientation(270);
                    break;
                case SurfaceOrientation.Rotation270:
                    _camera.SetDisplayOrientation(180);
                    break;
                case SurfaceOrientation.Rotation90:
                    _camera.SetDisplayOrientation(0);
                    break;
                default:
                    break;
            }

            this.LayoutParameters.Width = w;
            this.LayoutParameters.Height = h;

            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnSurfaceTextureSizeChanged(Android.Graphics.SurfaceTexture surface, int width, int height)
        {
            // ??

        }

        public void OnSurfaceTextureUpdated(Android.Graphics.SurfaceTexture surface)
        {
            // Fires whenever the surface change (moving the camera etc...)

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

        private void OpenCamera()
        {
            //if (_camera != null && _camera == facing)
            //    return;

            //_selectedCamera = facing;
            CloseCamera();

            int cameraCount = 0;
            //Camera cam = null;
            Camera.CameraInfo cameraInfo = new Camera.CameraInfo();
            cameraCount = Camera.NumberOfCameras;
            for (int camIdx = 0; camIdx < cameraCount; camIdx++)
            {
                Camera.GetCameraInfo(camIdx, cameraInfo);
                if (cameraInfo.Facing == _selectedCamera.ToCameraFacing())
                {
                    try
                    {
                        _camera = Camera.Open(camIdx);
                    }
                    catch (Exception e)
                    {
                        //Log.e(TAG, "Camera failed to open: " + e.getLocalizedMessage());
                    }
                }
            }
        }

        private void CloseCamera()
        {
            if (_camera != null)
            {
                _camera.StopPreview();
                _camera.Release();
            }
        }                        
    }
}