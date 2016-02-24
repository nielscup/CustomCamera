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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            _customCameraView = FindViewById<CustomCameraView>(Resource.Id.customCameraView);

            var captureButton = FindViewById<Button>(Resource.Id.captureButton);
            captureButton.Click += captureButton_Click;            

            var frontCameraButton = FindViewById<Button>(Resource.Id.frontCameraButton);
            frontCameraButton.Click += frontCameraButton_Click;

            var backCameraButton = FindViewById<Button>(Resource.Id.backCameraButton);
            backCameraButton.Click += backCameraButton_Click;
        }

        void captureButton_Click(object sender, EventArgs e)
        {
            _customCameraView.Capture();
        }

        void backCameraButton_Click(object sender, EventArgs e)
        {
            _customCameraView.SelectedCamera = CameraSelection.Back;
        }

        void frontCameraButton_Click(object sender, EventArgs e)
        {
            _customCameraView.SelectedCamera = CameraSelection.Front;
        }
    }
}

