using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.CustomCamera.Abstractions
{
    /// <summary>
    /// The seleected camera, front or back
    /// </summary>
    public enum CameraSelection
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        None,

        /// <summary>
        /// The front camera selection
        /// </summary>
        Front,

        /// <summary>
        /// The back camera selection
        /// </summary>
        Back
    }

    /// <summary>
    /// The camera orientation
    /// </summary>
    public enum CameraOrientation
    {
        /// <summary>
        /// Nothing selected
        /// </summary>
        None,
        /// <summary>
        /// Automatic
        /// </summary>
        Automatic = -1,
        /// <summary>
        /// Rotation0
        /// </summary>
        Rotation0 = 0,
        /// <summary>
        /// Rotation90
        /// </summary>
        Rotation90 = 90,
        /// <summary>
        /// Rotation180
        /// </summary>
        Rotation180 = 180,
        /// <summary>
        /// Rotation270
        /// </summary>
        Rotation270 = 270
    }
}
