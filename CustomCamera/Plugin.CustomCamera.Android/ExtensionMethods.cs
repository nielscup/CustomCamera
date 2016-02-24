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
using Plugin.CustomCamera.Abstractions;
using Android.Hardware;

namespace Plugin.CustomCamera
{
    public static class ExtensionMethods
    {
        public static CameraFacing ToCameraFacing(this CameraSelection cameraSelection)
        {
            switch (cameraSelection)
            {
                case CameraSelection.Back:
                    return CameraFacing.Back;
                case CameraSelection.Front:
                    return CameraFacing.Front;
                default:
                    return CameraFacing.Back;
            }
        }
    }
}