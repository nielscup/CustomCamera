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
        //void Start(CameraSelection selectedCamera = CameraSelection.Back, CameraOrientation orientation = CameraOrientation.Automatic);
        void Start(CameraSelection selectedCamera = CameraSelection.Back);

        /// <summary>
        /// Stops the camera
        /// </summary>
        void Stop();

        /// <summary>
        /// Call this method this to reset the camera after taking a picture
        /// </summary>
        void Reset();
    }
}
