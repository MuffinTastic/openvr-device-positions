using System.Runtime.InteropServices;
using Valve.VR;

namespace OVRDP;

public struct OVRRenderModel
{
    public uint TriangleCount;
    public uint VertexCount;
    public RenderModel_Vertex_t[] Vertices;
    public ushort[] Indices;

    public OVRRenderModel( nint handle )
    {
        RenderModel_t renderModelStruct = Marshal.PtrToStructure<RenderModel_t>( handle );
        TriangleCount = renderModelStruct.unTriangleCount;

        VertexCount = renderModelStruct.unVertexCount;
        Vertices = new RenderModel_Vertex_t[VertexCount];

        var vertexSize = Marshal.SizeOf<RenderModel_Vertex_t>();
        for ( int i = 0; i < VertexCount; i++ )
        {
            var ptr = renderModelStruct.rVertexData + ( i * vertexSize );
            Vertices[i] = Marshal.PtrToStructure<RenderModel_Vertex_t>( ptr );
        }

        var indexCount = TriangleCount * 3;
        Indices = new ushort[indexCount];

        Util.CopyUShorts( renderModelStruct.rIndexData, Indices, 0, (int) indexCount );
    }
}
