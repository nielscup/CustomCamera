# Custom Camera Plugin for Xamarin

Custom Camera for Xamarin.iOS and Xamarin.Android. This plugin will let you add a custom camera view to your Xamarin project. Easily select either front or rear facing camera from within your code. Custom Camera currently only supports taking pictures (no video).

#### Setup
* Coming soon to NuGet
* Or run the following command to create a nuget package yourself:
```
nuget pack Plugin.ImageCrop.nuspec
```

**Supports**
* Xamarin.iOS (Unified) WORK IN PROGRESS
* Xamarin.Android

### Usage

Use the CustomCameraView in your Android or iOS Project. See the test project for a working example.

**Android**
```
<plugin.customcamera.android.CustomCameraView
    	android:id="@+id/customCameraView"
	android:layout_width="match_parent"
	android:layout_height="match_parent" />
```

**Custom Camera iOS**
```
public override void ViewDidLoad()
{
    	base.ViewDidLoad();
	((UIView)CrossCustomCamera.Current.CustomCameraView).Frame = View.Frame;
	Add((UIView)CrossCustomCamera.Current.CustomCameraView);
	CrossCustomCamera.Current.CustomCameraView.Start(CameraSelection.Front);
}
```
**Cross Platform**
```
// Change camera: 
CrossCustomCamera.Current.CustomCameraView = CameraSelection.Back;

// Take picture:
CrossCustomCamera.Current.CustomCameraView.TakePicture((path) =>
{
	// Do something with the picture path
});

// Reset the camera:
CrossCustomCamera.Current.CustomCameraView.Reset();
```
