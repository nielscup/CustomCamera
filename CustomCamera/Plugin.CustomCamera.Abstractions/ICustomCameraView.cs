using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.CustomCamera.Abstractions
{
    public interface ICustomCameraView
    {
        CameraSelection SelectedCamera { get; set; }
        void Capture();        
    }
}
