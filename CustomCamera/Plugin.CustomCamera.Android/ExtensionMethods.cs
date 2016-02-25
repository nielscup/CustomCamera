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


        //public static SurfaceOrientation ToSurfaceOrientation(this CameraOrientation cameraOrientation)
        //{
        //    switch (cameraOrientation)
        //    {
        //        case CameraOrientation.Rotation0:
        //            return SurfaceOrientation.Rotation0;
        //        case CameraOrientation.Rotation90:
        //            return SurfaceOrientation.Rotation90;
        //        case CameraOrientation.Rotation180:
        //            return SurfaceOrientation.Rotation180;
        //        case CameraOrientation.Rotation270:
        //            return SurfaceOrientation.Rotation270;
        //        default:
        //            return SurfaceOrientation.Rotation0;
        //    }
        //}

        public static CameraOrientation ToCameraOrientation(this SurfaceOrientation surfaceOrientation)
        {
            switch (surfaceOrientation)
            {
                case SurfaceOrientation.Rotation0:
                    return CameraOrientation.Rotation0;
                case SurfaceOrientation.Rotation90:
                    return CameraOrientation.Rotation90;
                case SurfaceOrientation.Rotation180:
                    return CameraOrientation.Rotation180;
                case SurfaceOrientation.Rotation270:
                    return CameraOrientation.Rotation270;
                default:
                    return CameraOrientation.Rotation0;
            }
        }
    }
}