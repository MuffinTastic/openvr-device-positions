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
    private Texture _overlayTexture;

    public OVROverlayWrapper( ulong handle, ResourceFactory resourceFactory, int width, int height )
    {
        _handle = handle;

        _overlayTexture = resourceFactory.CreateTexture(
            TextureDescription.Texture2D(
                (uint) width,
                (uint) height,
                1,
                1,
                Veldrid.PixelFormat.R8_G8_B8_A8_UNorm,
                TextureUsage.RenderTarget )
        );
    }

    public void Dispose()
    {
        OpenVR.Overlay.DestroyOverlay( _handle );
        _overlayTexture.Dispose();
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
    public void SubmitFrame( GraphicsDevice device, CommandList commandList, Texture renderTarget )
    {
        if ( !device.GetD3D11Info( out BackendInfoD3D11 d3dInfo ) )
            throw new OverlayFatalException( "Couldn't get backend info" );

        commandList.CopyTexture( renderTarget, _overlayTexture );

        // this will be one frame late, but that's okay
        Texture_t ovrTexture;
        ovrTexture.eType = ETextureType.DirectX;
        ovrTexture.eColorSpace = EColorSpace.Auto;
        ovrTexture.handle = d3dInfo.GetTexturePointer( _overlayTexture );

        OpenVR.Overlay.SetOverlayTexture( _handle, ref ovrTexture );
    }
}
