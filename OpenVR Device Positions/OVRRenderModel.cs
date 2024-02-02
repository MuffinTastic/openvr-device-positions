using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace OVRDP;

public struct OVRRenderModel
{
    private bool _disposedValue;

    public RenderModel_Vertex_t[] Vertices;
    public ushort[] Indices;
    public uint TriangleCount;

    public OVRRenderModel( nint handle )
    {
        RenderModel_t renderModelStruct = Marshal.PtrToStructure<RenderModel_t>( handle );
        TriangleCount = renderModelStruct.unTriangleCount;
        
        Vertices = new RenderModel_Vertex_t[renderModelStruct.unVertexCount];

        var vertexSize = Marshal.SizeOf<RenderModel_Vertex_t>();
        for ( int i = 0; i < renderModelStruct.unVertexCount; i++ )
        {
            var ptr = renderModelStruct.rVertexData + ( i * vertexSize );
            Vertices[i] = Marshal.PtrToStructure<RenderModel_Vertex_t>( ptr );
        }

        var indexCount = TriangleCount * 3;
        Indices = new ushort[indexCount];

        Util.CopyUShorts( renderModelStruct.rIndexData, Indices, 0, (int) indexCount );
    }
}
