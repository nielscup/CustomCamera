using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.CustomCamera.Abstractions;
using System.IO;
using AVFoundation;
using UIKit;

namespace Plugin.CustomCamera
{
    public static class ExtensionMethods
    {
        public static AVCaptureDevicePosition ToAVCaptureDevicePosition(this CameraSelection cameraSelection)
        {
            switch (cameraSelection)
            {
                case CameraSelection.Back:
                    return AVCaptureDevicePosition.Back;
                case CameraSelection.Front:
                    return AVCaptureDevicePosition.Front;
                case CameraSelection.None:
                default:
                    return AVCaptureDevicePosition.Unspecified;
            }
        }

        public static AVCaptureVideoOrientation ToAVCaptureVideoOrientation(this CameraOrientation cameraOrientation)
        {
            switch (cameraOrientation)
            {
                case CameraOrientation.Rotation0:
                    return AVCaptureVideoOrientation.Portrait;
                case CameraOrientation.Rotation90:
                    return AVCaptureVideoOrientation.LandscapeRight;
                case CameraOrientation.Rotation180:
                    return AVCaptureVideoOrientation.PortraitUpsideDown;
                case CameraOrientation.Rotation270:
                    return AVCaptureVideoOrientation.LandscapeLeft;
                default:
                    return AVCaptureVideoOrientation.Portrait;
            }
        }

        public static CameraOrientation ToCameraOrientation(this UIInterfaceOrientation uiInterfaceOrientation)
        {
            switch (uiInterfaceOrientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return CameraOrientation.Rotation270;
                case UIInterfaceOrientation.LandscapeRight:
                    return CameraOrientation.Rotation90;
                case UIInterfaceOrientation.Portrait:
                    return CameraOrientation.Rotation0;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return CameraOrientation.Rotation180;
                case UIInterfaceOrientation.Unknown:
                    return CameraOrientation.Rotation0;
                default:
                    return CameraOrientation.Rotation0;
            }            
        }        
    }
}