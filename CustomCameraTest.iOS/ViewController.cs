using CoreGraphics;
using System;
using UIKit;
using AVFoundation;
using Foundation;
using Plugin.CustomCamera;
using Plugin.CustomCamera.Abstractions;

namespace CustomCameraTest.iOS
{
    public partial class ViewController : UIViewController
    {
        int yPos;
        int xPos;
        const int defaultSize = 300;
        UIImageView _previewImage;
        UIButton _captureButton;
        const string ResetCamera = "Reset";

        public ViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            // CustomCamera Plugin
            ((UIView)CrossCustomCamera.Current.CustomCameraView).Frame = View.Frame;
            Add((UIView)CrossCustomCamera.Current.CustomCameraView);
            CrossCustomCamera.Current.CustomCameraView.Start(CameraSelection.Front);

            yPos = 40;//(int)View.Bounds.Height - 60;
            var buttonWidth = (int)View.Bounds.Width / 3;
            _captureButton = AddButton("Capture", buttonWidth);
            _captureButton.TouchUpInside += takePictureButton_TouchUpInside;

            var frontCameraButton = AddButton("Front", buttonWidth, true);
            frontCameraButton.TouchUpInside += frontCameraButton_TouchUpInside;

            var backCameraButton = AddButton("Back", buttonWidth, true);
            backCameraButton.TouchUpInside += backCameraButton_TouchUpInside;

            buttonWidth = (int)View.Bounds.Width / 2;
            var orienttionUpsideDownButton = AddButton("Upside Down", buttonWidth);
            orienttionUpsideDownButton.TouchUpInside += orienttionUpsideDownButton_TouchUpInside;

            var orienttionNormalDownButton = AddButton("Normal", buttonWidth, true);
            orienttionNormalDownButton.TouchUpInside += orienttionNormalDownButton_TouchUpInside;

            _previewImage = new UIImageView(new CGRect(View.Bounds.Width / 2 - 50, View.Bounds.Height - 250, 100, 180));
            _previewImage.Hidden = true;
            Add(_previewImage);
        }

        void orienttionUpsideDownButton_TouchUpInside(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.CameraOrientation = CameraOrientation.Rotation180;
        }

        void orienttionNormalDownButton_TouchUpInside(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.CameraOrientation = CameraOrientation.Automatic;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            ((UIView)CrossCustomCamera.Current.CustomCameraView).Frame = View.Frame;
            CrossCustomCamera.Current.CustomCameraView.CameraOrientation = this.InterfaceOrientation.ToCameraOrientation();
        }

        void backCameraButton_TouchUpInside(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.SelectedCamera = CameraSelection.Back;
        }

        void frontCameraButton_TouchUpInside(object sender, EventArgs e)
        {
            CrossCustomCamera.Current.CustomCameraView.SelectedCamera = CameraSelection.Front;
        }

        void takePictureButton_TouchUpInside(object sender, EventArgs e)
        {
            // CustomCamera plugin EXAMPLE
            if (_captureButton.Title(UIControlState.Normal) == ResetCamera)
            {
                // Reset camera
                _previewImage.Hidden = true;
                CrossCustomCamera.Current.CustomCameraView.Reset();
                _captureButton.SetTitle("Take Picture", UIControlState.Normal);
                return;
            }

            // Take picture
            CrossCustomCamera.Current.CustomCameraView.TakePicture((path) =>
            {
                _previewImage.Image = new UIImage(path);
                _previewImage.Hidden = false;
                _captureButton.SetTitle(ResetCamera, UIControlState.Normal);
            });
        }

        #region ui controls
        public UIImageView AddImage(int height, int width)
        {
            var image = new UIImageView(GetFrame(height, width, xPos));
            Add(image);

            return image;
        }

        private UIButton AddButton(string title, int width = defaultSize, bool behindPreviousControl = false)
        {
            var height = 36;

            if (behindPreviousControl)
                yPos -= height;
            else
                xPos = 15;

            var button = new UIButton(GetFrame(height, width, xPos));
            button.SetTitle(title, UIControlState.Normal);
            button.SetTitleColor(new UIColor(1, 0, 0, 1), UIControlState.Normal);
            Add(button);

            xPos += width;
            yPos += height;

            return button;
        }

        private CGRect GetFrame(int height, int width, int x)
        {
            var rect = new CGRect(x, yPos, width, height);
            return rect;
        }

        #endregion        
    }
}

