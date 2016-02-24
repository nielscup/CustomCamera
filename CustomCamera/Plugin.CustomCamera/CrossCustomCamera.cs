using Plugin.CustomCamera.Abstractions;
using System;

namespace Plugin.CustomCamera
{
  /// <summary>
  /// Cross platform CustomCamera implemenations
  /// </summary>
  public class CrossCustomCamera
  {
    static Lazy<ICustomCamera> Implementation = new Lazy<ICustomCamera>(() => CreateCustomCamera(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static ICustomCamera Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static ICustomCamera CreateCustomCamera()
    {
#if PORTABLE
        return null;
#else
        return new CustomCameraImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
