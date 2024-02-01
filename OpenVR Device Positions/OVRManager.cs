using System.Diagnostics;
using System.Text;
using Valve.VR;
using Veldrid;

namespace OVRDP;

public static class OVRManager
{
    private static Dictionary<EVRInitError, string> InitErrorReasons = new Dictionary<EVRInitError, string>()
    {
        
    };

    public static bool Init( CancellationToken ct )
    {
        EVRInitError initError = EVRInitError.None;
        OpenVR.Init( ref initError, EVRApplicationType.VRApplication_Background );

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
            string reason = $"Error: Couldn't connect to VR: {errorString}";
            Log.Text( reason );
            MessageBox.Show( reason, "OpenVR Device Positions", MessageBoxButtons.OK, MessageBoxIcon.Error );
            return false;
        }

        Log.Text( "Connected to VR" );

        return true;
    }

    // Wrapper, keeps access consistent
    public static void Shutdown()
    {
        OpenVR.Shutdown();
    }

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

    public static OVROverlayWrapper? CreateOverlay( ResourceFactory resourceFactory, int width, int height, string key, string name )
    {
        ulong handle = 0;
        
        EVROverlayError error = OpenVR.Overlay.CreateOverlay( key, name, ref handle );
        if ( error != EVROverlayError.None )
        {
            return null;
        }

        return new OVROverlayWrapper( handle, resourceFactory, width, height );
    }

    public static void SavePositions( SaveSettings saveSettings )
    {
        HashSet<ETrackedDeviceClass> desiredClasses = GetDesiredClasses( saveSettings );

        Log.Text( $"Saving devices:" );

        for ( uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++ )
        {
            var deviceClass = OpenVR.System.GetTrackedDeviceClass( i );
            if ( !desiredClasses.Contains( deviceClass ) ) continue;

            Log.Text( $"  {deviceClass}" );
        }

        Log.Text( "Done" );
    }

    private static HashSet<ETrackedDeviceClass> GetDesiredClasses( SaveSettings saveSettings )
    {
        HashSet<ETrackedDeviceClass> desiredClasses = new();

        if ( saveSettings.SaveBaseStations )
            desiredClasses.Add( ETrackedDeviceClass.TrackingReference );
        if ( saveSettings.SaveHMD )
            desiredClasses.Add( ETrackedDeviceClass.HMD );
        if ( saveSettings.SaveControllers )
            desiredClasses.Add( ETrackedDeviceClass.Controller );
        if ( saveSettings.SaveTrackers )
            desiredClasses.Add( ETrackedDeviceClass.GenericTracker );

        return desiredClasses;
    }
}
