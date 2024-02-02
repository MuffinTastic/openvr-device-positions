using Aspose.ThreeD.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using Veldrid;

namespace OVRDP;

public class OVROverlayWrapper
{
    private ulong _handle;

    public OVROverlayWrapper( ulong handle )
    {
        _handle = handle;
    }

    public void Dispose()
    {
        OpenVR.Overlay.DestroyOverlay( _handle );
    }

    public void Show()
    {
        OpenVR.Overlay.ShowOverlay( _handle );
    }

    public void Hide()
    {
        OpenVR.Overlay.HideOverlay( _handle );
    }

    public void SetWidthInMeters( float width )
    {
        OpenVR.Overlay.SetOverlayWidthInMeters( _handle, width );
    }

    public void SetColor( Vector3 color )
    {
        OpenVR.Overlay.SetOverlayColor( _handle, color.X, color.Y, color.Z );
    }

    public void SetTextureBounds( VRTextureBounds_t bounds )
    {
        OpenVR.Overlay.SetOverlayTextureBounds( _handle, ref bounds );
    }

    public void SubmitFrame( GraphicsDevice device, Texture renderTarget )
    {
        if ( !device.GetD3D11Info( out BackendInfoD3D11 d3dInfo ) )
            throw new OverlayFatalException( "Couldn't get backend info" );

        // this will be one frame late, but that's okay
        Texture_t ovrTexture;
        ovrTexture.eType = ETextureType.DirectX;
        ovrTexture.eColorSpace = EColorSpace.Auto;
        ovrTexture.handle = d3dInfo.GetTexturePointer( renderTarget );

        OpenVR.Overlay.SetOverlayTexture( _handle, ref ovrTexture );
    }
}
