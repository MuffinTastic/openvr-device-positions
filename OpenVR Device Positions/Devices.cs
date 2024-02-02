using Aspose.ThreeD;
using Aspose.ThreeD.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Valve.VR;
using Vortice.Mathematics;

namespace OVRDP;

public static class Devices
{
    /// <summary>
    /// Save the positions of all the desired devices to an FBX. The star of the show
    /// </summary>
    public static void SavePositions( SaveSettings saveSettings )
    {
        var fbxScene = new Scene();
        var deviceRoot = fbxScene.RootNode.CreateChildNode( "VR Devices" );

        HashSet<ETrackedDeviceClass> desiredClasses = GetDesiredClasses( saveSettings );

        Log.Text( $"Saving devices:" );

        var trackedDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        OpenVR.System.GetDeviceToAbsoluteTrackingPose( ETrackingUniverseOrigin.TrackingUniverseStanding, 0.0f, trackedDevicePoses );

        var hmdPose = trackedDevicePoses[OpenVR.k_unTrackedDeviceIndex_Hmd];
        var hmdMatrix = hmdPose.mDeviceToAbsoluteTracking; // Let's assume it's being tracked...
        var hmdPositionXZ = hmdMatrix.GetPosition() * new Vector3( 1.0f, 0.0f, 1.0f );
        var hmdRotationYEuler = hmdMatrix.GetRotation().ToEuler() * new Vector3( 0.0f, 1.0f, 0.0f );
        var hmdRotationYInverse = Quaternion.Inverse( hmdRotationYEuler.FromEuler() );

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

            // Transform device position and rotation relative to
            // HMD on the XZ plane
            if ( saveSettings.CenterOnHMD )
            {
                position -= hmdPositionXZ;
                position = Vector3.Transform(position, hmdRotationYInverse );
                rotation = hmdRotationYInverse * rotation;
            }

            string? renderModel = null;

            Log.Text( $" -- {deviceClass} - {pose.eTrackingResult}" );
            if ( OVRManager.GetDeviceInfo( (uint) id, out OVRDeviceInfo info ) )
            {
                Log.Text( $"      - {info.HardwareModel}" );
                if ( saveSettings.UseDeviceModels )
                {
                    Log.Text( $"      - {info.RenderModel}" );
                    renderModel = info.RenderModel;
                }
            }
            Log.Text( $"      - Pos:{position}" );
            Log.Text( $"      - Rot:{rotation}" );

            var deviceNode = deviceRoot.CreateChildNode( $"{deviceClass}.{id}" );
            deviceNode.Entity = MeshStore.GetRenderModel( renderModel );
            deviceNode.Transform.Translation = new AsposeVector3( position.X, position.Y, position.Z );
            deviceNode.Transform.Rotation = new AsposeQuaternion( rotation.W, rotation.X, rotation.Y, rotation.Z );

            Log.Text( $"Added node {deviceNode.Name}" );
        }

        string timestamp = DateTime.Now.ToString( "yyyy-MM-dd_hh.mm.ss" );
        string filename = Path.Join( Util.OutputDirectory, $"VRDevices_{timestamp}.fbx" );
        Log.Text( $"Saving to {filename}" );
        
        fbxScene.Save( filename, FileFormat.FBX7400Binary );

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
