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
    private Texture _texture;

    public OVROverlayWrapper( ulong handle, ResourceFactory resourceFactory, int width, int height )
    {
        _handle = handle;

        _texture = resourceFactory.CreateTexture(
            TextureDescription.Texture2D(
                (uint) width,
                (uint) height,
                1,
                1,
                PixelFormat.R8_G8_B8_A8_UNorm,
                TextureUsage.Storage )
        );
    }

    public void Close()
    {
        OpenVR.Overlay.DestroyOverlay( _handle );
        _texture.Dispose();
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

    public void UploadFramebuffer()
    {

    }
}
