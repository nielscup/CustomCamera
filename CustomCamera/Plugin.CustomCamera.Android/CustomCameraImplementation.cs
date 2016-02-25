using Plugin.CustomCamera.Abstractions;
using System;
using Android.App;

[assembly: Permission(Name = "android.permission.READ_EXTERNAL_STORAGE")]
[assembly: Permission(Name = "android.permission.WRITE_EXTERNAL_STORAGE")]
[assembly: Permission(Name = "android.permission.CAMERA")]
namespace Plugin.CustomCamera
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class CustomCameraImplementation : ICustomCamera
    {
        /// <summary>
        /// Instance of the Custom Camera View
        /// </summary>
        public ICustomCameraView CustomCameraView
        {
            get { return CustomCameraInstance.CustomCameraView; }
        }
    }

    internal static class CustomCameraInstance
    {
        internal static ICustomCameraView CustomCameraView { get; set; }
    }
}