using Plugin.CustomCamera.Abstractions;
using System;

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