using Plugin.CustomCamera.Abstractions;
using System;
using Android.App;

[assembly: Permission(Name = "android.permission.CAMERA")]
namespace Plugin.CustomCamera
{
  /// <summary>
  /// Implementation for Feature
  /// </summary>
  public class CustomCameraImplementation : ICustomCamera
  {

  }
}