using System;

namespace Plugin.CustomCamera.Abstractions
{
  /// <summary>
  /// Interface for CustomCamera
  /// </summary>
  public interface ICustomCamera
  {
      ICustomCameraView CustomCameraView { get; }
  }
}
