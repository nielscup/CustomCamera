//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.Graphics;
//using Android.Util;
//using Plugin.CustomCamera.Abstractions;

//namespace CustomCameraTest.Droid
//{
//    [Register("CustomCameraTest.Droid.CustomCameraView")]
//    public class CustomCameraView : FrameLayout, ICustomCameraView, TextureView.ISurfaceTextureListener
//    {
//        TextureView _textureView;
//        Android.Hardware.Camera _camera;
        
//        Activity _activity;

//        public CustomCameraView(Context context, IAttributeSet attrs): base(context, attrs)
//        {
//            this._activity = (Activity)context;

//            _textureView = new TextureView(context);
//            _textureView.SurfaceTextureListener = this;
//            AddView(_textureView);
//        }
                        
//        public void OnSurfaceTextureAvailable(Android.Graphics.SurfaceTexture surface, int w, int h)
//        {
//            _camera = Android.Hardware.Camera.Open();

//            switch (_activity.WindowManager.DefaultDisplay.Rotation)
//            {
//                case SurfaceOrientation.Rotation0:
//                    _camera.SetDisplayOrientation(90);
//                    break;
//                case SurfaceOrientation.Rotation180:
//                    _camera.SetDisplayOrientation(270);
//                    break;
//                case SurfaceOrientation.Rotation270:
//                    _camera.SetDisplayOrientation(180);
//                    break;
//                case SurfaceOrientation.Rotation90:
//                    _camera.SetDisplayOrientation(0);
//                    break;
//                default:
//                    break;
//            }

//            //this.LayoutParameters = new FrameLayout.LayoutParams(w, h);
//            this.LayoutParameters.Width = w;
//            this.LayoutParameters.Height = h;

//            //this.LayoutParameters = new RelativeLayout.LayoutParams(w, h);
//            //this.LayoutParameters = new LinearLayout.LayoutParams(w, h);

//            try
//            {
//                _camera.SetPreviewTexture(surface);
//                _camera.StartPreview();
//            }
//            catch (Java.IO.IOException ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//        }


//        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
//        {
//            // ??
//        }

//        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
//        {
//            // Fires whenever the surface change (moving the camera etc...)

//        }

//        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
//        {
//            if (_camera != null)
//            {
//                _camera.StopPreview();
//                _camera.Release();
//            }

//            return true;
//        }
//    }
//}