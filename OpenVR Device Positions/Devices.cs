using System;
using System.Collections.Generic;
using System.Linq;
using Valve.VR;

namespace OVRDP;

public static class Devices
{
    /// <summary>
    /// Save the positions of all the desired devices to an FBX. The star of the show
    /// </summary>
    public static void SavePositions( SaveSettings saveSettings )
    {
        HashSet<ETrackedDeviceClass> desiredClasses = GetDesiredClasses( saveSettings );

        Log.Text( $"Saving devices:" );

        var trackedDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        OpenVR.System.GetDeviceToAbsoluteTrackingPose( ETrackingUniverseOrigin.TrackingUniverseStanding, 0.0f, trackedDevicePoses );

        var hmdPose = trackedDevicePoses[OpenVR.k_unTrackedDeviceIndex_Hmd];
        var hmdMatrix = hmdPose.mDeviceToAbsoluteTracking; // Let's assume it's being tracked...
        var hmdPosition = hmdMatrix.GetPosition();
        var hmdRotation = hmdMatrix.GetRotation();

        for ( int id = 0; id < OpenVR.k_unMaxTrackedDeviceCount; id++ )
        {
            var deviceClass = OpenVR.System.GetTrackedDeviceClass( (uint) id );
            if ( !desiredClasses.Contains( deviceClass ) ) continue;

            var pose = trackedDevicePoses[id];

            if ( !pose.bDeviceIsConnected )
                continue;

            if ( !pose.bPoseIsValid && !saveSettings.SaveInvalidPoses )
                continue;

            var matrix = pose.mDeviceToAbsoluteTracking;
            var position = matrix.GetPosition();
            var rotation = matrix.GetRotation();

            Log.Text( $" -- {deviceClass} - {pose.eTrackingResult}" );
            if ( OVRManager.GetDeviceInfo( (uint) id, out OVRDeviceInfo info ) )
            {
                Log.Text( $"      - {info.HardwareModel}" );
                Log.Text( $"      - {info.RenderModel}" );
            }
            Log.Text( $"      - Pos:{position}" );
            Log.Text( $"      - Rot:{rotation}" );
        }

        //var test = new FBXScene( "test", 2.0f );

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
