using Aspose.ThreeD;
using Aspose.ThreeD.Entities;
using Valve.VR;

namespace OVRDP;

public static class MeshStore
{
    private const string DefaultDeviceMeshFile = "DefaultDeviceMesh.obj";

    private static Mesh? _defaultDeviceMesh = null;

    public static Mesh DefaultDeviceMesh => _defaultDeviceMesh ?? LoadDefaultDeviceMesh();

    private static Mesh LoadDefaultDeviceMesh()
    {
        Log.Text( $"Loading default device render model" );
        var path = Path.Join( Util.AssetDirectory, DefaultDeviceMeshFile );
        var scene = Scene.FromFile( path );
        _defaultDeviceMesh = (Mesh) scene.RootNode.ChildNodes[0].Entity;
        return _defaultDeviceMesh;
    }


    private static Dictionary<string, Mesh> _meshes = new();

    public static Mesh GetRenderModel( string? modelName )
    {
        if ( modelName is null )
            return DefaultDeviceMesh;

        // Try to get existing meshes first
        if ( _meshes.TryGetValue( modelName, out var existingMesh ) )
        {
            return existingMesh;
        }

        Log.Text( $"Loading render model for {modelName}" );

        // We didn't have it, let's load it

        Mesh newMesh;

        OVRRenderModel? renderModel = OVRManager.GetRenderModel( modelName );
        if ( renderModel is null )
        {
            Log.Text( $"Falling back to default model" );
            newMesh = DefaultDeviceMesh;
        }
        else
        {
            newMesh = GetMeshFromRenderModel( renderModel.Value );
            Log.Text( $"Render model loaded" );
        }

        _meshes.Add( modelName, newMesh );

        return newMesh;
    }

    private static Mesh GetMeshFromRenderModel( OVRRenderModel renderModel )
    {
        Mesh mesh = new();

        AsposeVector4[] controlPoints = new AsposeVector4[renderModel.VertexCount];

        for ( int i = 0; i < renderModel.VertexCount; i++ )
        {
            HmdVector3_t position = renderModel.Vertices[i].vPosition;
            controlPoints[i] = new AsposeVector4( position.v0, position.v1, position.v2, 1.0f );
        }

        mesh.ControlPoints.AddRange( controlPoints );

        PolygonBuilder builder = new PolygonBuilder( mesh );

        int indexCount = 0;
        for ( int i = 0; i < renderModel.TriangleCount; i++ )
        {
            builder.Begin();
            builder.AddVertex( renderModel.Indices[indexCount++] );
            builder.AddVertex( renderModel.Indices[indexCount++] );
            builder.AddVertex( renderModel.Indices[indexCount++] );
            builder.End();
        }

        return mesh;
    }
}
