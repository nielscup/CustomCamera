using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.CustomCamera.Abstractions;
using System.IO;
using AVFoundation;

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
    }
}