using System.Diagnostics;
using Valve.VR;

namespace SteamVR_Device_Positions;

public class VRManager
{
    private static Dictionary<EVRInitError, string> InitErrorReasons = new Dictionary<EVRInitError, string>()
    {
        { EVRInitError.Init_NoServerForBackgroundApp, "SteamVR is not running" },
    };

    private CVRSystem _cVRSystem;

    private VRManager( CVRSystem cVRSystem )
    {
        _cVRSystem = cVRSystem;
    }

    public static VRManager? Init()
    {
        EVRInitError initError = EVRInitError.None;
        var cVRSystem = OpenVR.Init( ref initError, EVRApplicationType.VRApplication_Background );
        if ( initError != EVRInitError.None )
        {
            string errorReason = InitErrorReasons.GetValueOrDefault( initError ) ?? initError.ToString();
            MessageBox.Show( $"SteamVR Init error: {errorReason}", "SteamVR Tracker Positions", MessageBoxButtons.OK, MessageBoxIcon.Error );
            return null;
        }

        return new VRManager( cVRSystem );
    }

    // Wrapper, keeps access consistent
    public static void Shutdown()
    {
        OpenVR.Shutdown();
    }

    internal void SavePositions( SaveSettings saveSettings )
    {
        HashSet<ETrackedDeviceClass> desiredClasses = GetDesiredClasses( saveSettings );

        Debug.Write( $"\nSaving devices: \n" );

        for ( uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++ )
        {
            var deviceClass = _cVRSystem.GetTrackedDeviceClass( i );
            if ( !desiredClasses.Contains( deviceClass ) ) continue;

            Debug.Write( $"{deviceClass}\n" );
        }
    }

    private HashSet<ETrackedDeviceClass> GetDesiredClasses( SaveSettings saveSettings )
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
