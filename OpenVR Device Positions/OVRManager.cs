using System.Diagnostics;
using System.Text;
using Valve.VR;
using Veldrid;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OVRDP;

public struct OVRDeviceInfo
{
    public string Manufacturer;
    public string HardwareModel;
    public string RenderModel;
}

/// <summary>
/// Wrapper around OpenVR APIs
/// </summary>
public static class OVRManager
{
    private static Dictionary<EVRInitError, string> InitErrorReasons = new Dictionary<EVRInitError, string>()
    {
        
    };

    private static bool _initialized = false;

    /// <summary>
    /// Initialize OpenVR
    /// </summary>
    /// <returns>Initialization was cancelled</returns>
    public static bool Init( CancellationToken ct )
    {
        EVRInitError initError = EVRInitError.None;
        OpenVR.Init( ref initError, EVRApplicationType.VRApplication_Overlay );

        if ( initError == EVRInitError.Init_NoServerForBackgroundApp )
        {
            // Server isn't running, let's wait for it to launch 
            Log.Text( "Waiting for VR..." );

            while ( initError == EVRInitError.Init_NoServerForBackgroundApp )
            {
                if ( ct.IsCancellationRequested )
                    return false;

                Thread.Sleep( 500 );
                OpenVR.Init( ref initError, EVRApplicationType.VRApplication_Background );
            }
        }

        if ( initError != EVRInitError.None )
        {
            string errorString = InitErrorReasons.GetValueOrDefault( initError ) ?? initError.ToString();
            string reason = $"Couldn't connect to VR: {errorString}";
            throw new OverlayFatalException( reason );
        }

        Log.Text( "Connected to VR" );

        _initialized = true;
        return true;
    }

    /// <summary>
    /// Wrapper around OpenVR.Shutdown() for consistency's sake
    /// </summary>
    public static void Shutdown()
    {
        if ( _initialized )
        {
            OpenVR.Shutdown();
            _initialized = false;
        }
    }

    /// <summary>
    /// Get the refresh rate of the HMD
    /// </summary>
    /// <returns></returns>
    public static float GetRefreshRate()
    {
        ETrackedPropertyError error = default;
        
        float rate = OpenVR.System.GetFloatTrackedDeviceProperty(
            OpenVR.k_unTrackedDeviceIndex_Hmd,
            ETrackedDeviceProperty.Prop_DisplayFrequency_Float,
            ref error
        );

        if ( error != ETrackedPropertyError.TrackedProp_Success )
            rate = 90.0f; // Sane default

        return rate;
    }

    /// <summary>
    /// Create an OpenVR overlay instance
    /// </summary>
    public static OVROverlayWrapper? CreateOverlay( string key, string name )
    {
        ulong handle = 0;
        ulong thumbnailHandle = 0;
        
        EVROverlayError error = OpenVR.Overlay.CreateDashboardOverlay( key, name, ref handle, ref thumbnailHandle );
        if ( error != EVROverlayError.None )
        {
            return null;
        }

        return new OVROverlayWrapper( handle, thumbnailHandle );
    }

    public static bool GetDeviceInfo( uint deviceID, out OVRDeviceInfo info )
    {
        ETrackedPropertyError error = default;

        StringBuilder builder = new();
        const uint bufferSize = 64;

        info = default;

        OpenVR.System.GetStringTrackedDeviceProperty(
            deviceID,
            ETrackedDeviceProperty.Prop_ManufacturerName_String,
            builder,
            bufferSize,
            ref error
        );
        if ( error != ETrackedPropertyError.TrackedProp_Success )
            return false;

        info.Manufacturer = builder.ToString();


        OpenVR.System.GetStringTrackedDeviceProperty(
            deviceID,
            ETrackedDeviceProperty.Prop_ModelNumber_String,
            builder,
            bufferSize,
            ref error
        );
        if ( error != ETrackedPropertyError.TrackedProp_Success )
            return false;

        info.HardwareModel = builder.ToString();


        OpenVR.System.GetStringTrackedDeviceProperty(
            deviceID,
            ETrackedDeviceProperty.Prop_RenderModelName_String,
            builder,
            bufferSize,
            ref error
        );
        if ( error != ETrackedPropertyError.TrackedProp_Success )
            return false;

        info.RenderModel = builder.ToString();


        return true;
    }

    public static OVRRenderModel? GetRenderModel( string modelName )
    {
        IntPtr handle = IntPtr.Zero;
        EVRRenderModelError error = default;

        while (
            ( error = OpenVR.RenderModels.LoadRenderModel_Async( modelName, ref handle ) )
                == EVRRenderModelError.Loading )
        {
            Thread.Sleep( 5 );
        }

        if ( error != EVRRenderModelError.None )
        {
            Log.Text( $"Error getting render model for {modelName}: {error}" );
            return null;
        }

        var model = new OVRRenderModel( handle );

        OpenVR.RenderModels.FreeRenderModel( handle );

        return model;
    }
}
