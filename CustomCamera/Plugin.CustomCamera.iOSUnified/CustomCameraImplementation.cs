using Plugin.CustomCamera.Abstractions;
using System;

namespace Plugin.CustomCamera
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class CustomCameraImplementation : ICustomCamera
    {
        internal CustomCameraImplementation()
        {
            CustomCameraInstance.CustomCameraView = new CustomCameraView();
        }

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
        static ICustomCameraView _customCameraView;
        internal static ICustomCameraView CustomCameraView 
        { 
            get
            {
                return _customCameraView;
            }
            set
            {
                _customCameraView = value;
            }
        }
    }
}