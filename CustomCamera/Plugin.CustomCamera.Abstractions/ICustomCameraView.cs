using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.CustomCamera.Abstractions
{
    public interface ICustomCameraView
    {
        /// <summary>
        /// The selected camera, front or back
        /// </summary>
        CameraSelection SelectedCamera { get; set; }

        /// <summary>
        /// The camera orientation
        /// </summary>
        CameraOrientation CameraOrientation { get; set; }

        /// <summary>
        /// Take a picture
        /// </summary>
        /// <param name="callback"></param>
        void TakePicture(Action<string> callback);

        /// <summary>
        /// Starts the camera
        /// </summary>
        /// <param name="selectedCamera">The selected camera, default: Back</param>
        /// <param name="orientation">the camera orientation, default: Automatic</param>
        void StartCamera(CameraSelection selectedCamera = CameraSelection.Back, CameraOrientation orientation = CameraOrientation.Automatic);

        /// <summary>
        /// Stops the camera
        /// </summary>
        /// <param name="callback"></param>
        void StopCamera();
    }
}
