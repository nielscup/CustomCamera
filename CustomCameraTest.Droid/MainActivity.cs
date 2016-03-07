using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Plugin.CustomCamera;
using Plugin.CustomCamera.Abstractions;

namespace CustomCameraTest.Droid
{
    [Activity(Label = "CustomCameraTest.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ICustomCameraView _customCameraView;
        ImageView _imageView;
        Button captureButton;
        Button resetCameraButton;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _customCameraView = FindViewById<CustomCameraView>(Resource.Id.customCameraView);
            _customCameraView.Start(CameraSelection.Front);

            _imageView = FindViewById<ImageView>(Resource.Id.imageView);

            captureButton = FindViewById<Button>(Resource.Id.captureButton);
            captureButton.Click += captureButton_Click;

            var frontCameraButton = FindViewById<Button>(Resource.Id.frontCameraButton);
            frontCameraButton.Click += frontCameraButton_Click;

            var backCameraButton = FindViewById<Button>(Resource.Id.backCameraButton);
            backCameraButton.Click += backCameraButton_Click;

            //var rotateLeftButton = FindViewById<Button>(Resource.Id.rotateLeftButton);
            //rotateLeftButton.Click += rotateLeftButton_Click;

            //var rotateRightButton = FindViewById<Button>(Resource.Id.rotateRightButton);
            //rotateRightButton.Click += rotateRightButton_Click;

            resetCameraButton = FindViewById<Button>(Resource.Id.resetCameraButton);
            resetCameraButton.Click += resetCameraButton_Click;
        }

        void resetCameraButton_Click(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.Reset();
            //_customCameraView.Reset();
        }

        void rotateLeftButton_Click(object sender, EventArgs e)
        {
            switch (_customCameraView.CameraOrientation)
            {
                case CameraOrientation.Rotation0:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation270;
                    break;
                case CameraOrientation.Rotation90:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation0;
                    break;
                case CameraOrientation.Rotation180:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation90;
                    break;
                case CameraOrientation.Rotation270:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation180;
                    break;
            }
        }

        void rotateRightButton_Click(object sender, EventArgs e)
        {
            switch (_customCameraView.CameraOrientation)
            {
                case CameraOrientation.Rotation0:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation90;
                    break;
                case CameraOrientation.Rotation90:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation180;
                    break;
                case CameraOrientation.Rotation180:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation270;
                    break;
                case CameraOrientation.Rotation270:
                    _customCameraView.CameraOrientation = CameraOrientation.Rotation0;
                    break;
            }
        }

        void captureButton_Click(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.TakePicture((path) => ProcessPicture(path));            
            //_customCameraView.TakePicture((path) => ProcessPicture(path));
        }

        void backCameraButton_Click(object sender, EventArgs e)
        {
            _customCameraView.SelectedCamera = CameraSelection.Back;
        }

        void frontCameraButton_Click(object sender, EventArgs e)
        {
            _customCameraView.SelectedCamera = CameraSelection.Front;
        }

        void ProcessPicture(string path)
        {
            _imageView.SetImageURI(null);
            _imageView.SetImageURI(Android.Net.Uri.Parse(path));
        }
    }
}

