using Aspose.ThreeD;
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
        fbxScene.AssetInfo.ApplicationName = OverlayConstants.ProgramNameReadable;
        //fbxScene.AssetInfo.AxisSystem = new AxisSystem( CoordinateSystem.RightHanded, Axis.YAxis, Axis.NegativeZAxis );

        var deviceRoot = fbxScene.RootNode.CreateChildNode( "VR Devices" );

        HashSet<ETrackedDeviceClass> desiredClasses = GetDesiredClasses( saveSettings );

        // Transform device position and rotation relative to
        // HMD on the XZ plane

        var trackedDevicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        OpenVR.System.GetDeviceToAbsoluteTrackingPose( ETrackingUniverseOrigin.TrackingUniverseStanding, 0.0f, trackedDevicePoses );

        // Let's assume it's being tracked...
        var hmdPose = trackedDevicePoses[OpenVR.k_unTrackedDeviceIndex_Hmd];
        var hmdMatrix34 = hmdPose.mDeviceToAbsoluteTracking;

        Vector3 hmdPositionXZ = hmdMatrix34.GetPosition() * new Vector3( 1.0f, 0.0f, 1.0f );
        Log.Text( $"{hmdPositionXZ}" );
        Vector3 hmdRotationYEuler = hmdMatrix34.GetRotation().ToEuler() * new Vector3( 0.0f, 1.0f, 0.0f );
        Quaternion hmdRotationY = hmdRotationYEuler.FromEuler();

        Matrix4x4.Invert( Matrix4x4.CreateTranslation( hmdPositionXZ ), out Matrix4x4 hmdTranslateInvert );
        Matrix4x4 hmdRotationInvert = Matrix4x4.CreateFromQuaternion( hmdRotationY );

        var centerOnHMDMatrix = hmdTranslateInvert * hmdRotationInvert;


        Matrix4x4 spaceReorient180Yaw = Matrix4x4.CreateFromAxisAngle( new Vector3( 0.0f, 1.0f, 0.0f ), MathHelper.ToRadians( 180.0f ) );


        Matrix4x4 spaceMatrix;
        if ( saveSettings.CenterOnHMD )
            spaceMatrix = centerOnHMDMatrix;
        else
            spaceMatrix = spaceReorient180Yaw;


        Log.Text( $"Saving devices:" );

        for ( int id = 0; id < OpenVR.k_unMaxTrackedDeviceCount; id++ )
        {
            var deviceClass = OpenVR.System.GetTrackedDeviceClass( (uint) id );
            if ( !desiredClasses.Contains( deviceClass ) ) continue;

            var pose = trackedDevicePoses[id];

            if ( !pose.bDeviceIsConnected )
                continue;

            if ( !pose.bPoseIsValid && !saveSettings.SaveInvalidPoses )
                continue;


            Matrix4x4 matrix;
            if ( id == OpenVR.k_unTrackedDeviceIndex_Hmd )
                matrix = hmdMatrix34.GetMatrix4x4();
            else
                matrix = pose.mDeviceToAbsoluteTracking.GetMatrix4x4();

            matrix = matrix * spaceMatrix;


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


            var deviceNode = deviceRoot.CreateChildNode( $"{deviceClass}.{id}" );
            deviceNode.Entity = MeshStore.GetRenderModel( renderModel );
            deviceNode.Transform.TransformMatrix = matrix.ToAspose();

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
