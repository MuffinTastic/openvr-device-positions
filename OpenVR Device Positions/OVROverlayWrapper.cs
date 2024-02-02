using Aspose.ThreeD.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using Veldrid;

namespace OVRDP;

public class OVROverlayWrapper
{
    private ulong _handle;
    private Vector2 _mouseRemap = new Vector2( 1.0f );

    public OVROverlayWrapper( ulong handle )
    {
        _handle = handle;

        // Sane defaults

        SetColor( new Vector3( 1.0f ) );

        VRTextureBounds_t bounds = new()
        {
            uMin = 0,
            uMax = 1,
            vMin = 0,
            vMax = 1
        };

        SetTextureBounds( bounds );
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

    public void SetTransform( Vector3 position, Quaternion rotation )
    {
        var mat = HmdMatrix34_tExtensions.FromVectorAndQuat( position, rotation );
        OpenVR.Overlay.SetOverlayTransformAbsolute( _handle, ETrackingUniverseOrigin.TrackingUniverseStanding, ref mat );
    }

    public void SetColor( Vector3 color )
    {
        OpenVR.Overlay.SetOverlayColor( _handle, color.X, color.Y, color.Z );
    }

    public void SetTextureBounds( VRTextureBounds_t bounds )
    {
        OpenVR.Overlay.SetOverlayTextureBounds( _handle, ref bounds );
    }

    public void SetInputMethod( VROverlayInputMethod inputMethod )
    {
        OpenVR.Overlay.SetOverlayInputMethod( _handle, inputMethod );
    }

    public void SetInputRemapping( Vector2 remap )
    {
        _mouseRemap = remap;
    }

    OverlayInputSnapshot lastSnapshot = new();

    public OverlayInputSnapshot PollEvents()
    {
        OverlayInputSnapshot snapshot = new();

        lastSnapshot.Clear();
        lastSnapshot.CopyTo( snapshot );

        VREvent_t vrEvent = default;

        // This function call is wack. Blame Valve
        while ( OpenVR.Overlay.PollNextOverlayEvent( _handle, ref vrEvent, (uint) Marshal.SizeOf<VREvent_t>() ) )
        {
            MouseButton button = MapOVRToImGuiMouseButton( (EVRMouseButton) vrEvent.data.mouse.button );

            if ( vrEvent.eventType == (uint) EVREventType.VREvent_MouseMove )
            {
                snapshot.MousePosition = new Vector2( vrEvent.data.mouse.x, 1.0f - vrEvent.data.mouse.y ) * _mouseRemap;
            }
            else if ( vrEvent.eventType == (uint) EVREventType.VREvent_MouseButtonDown )
            {
                snapshot.MouseDown[(int) button] = true;

                snapshot.MouseEventsList.Add(
                    new MouseEvent( button, down: true )
                );
            }
            else if ( vrEvent.eventType == (uint) EVREventType.VREvent_MouseButtonUp )
            {
                snapshot.MouseDown[(int) button] = false;
                snapshot.MouseEventsList.Add(
                    new MouseEvent( button, down: false )
                );
            }
        }

        snapshot.CopyTo( lastSnapshot );

        return snapshot;
    }

    private MouseButton MapOVRToImGuiMouseButton( EVRMouseButton button )
    {
        switch ( button )
        {
            default:
            case EVRMouseButton.Left:
                return MouseButton.Left;
            case EVRMouseButton.Middle:
                return MouseButton.Middle;
            case EVRMouseButton.Right:
                return MouseButton.Right;
        }
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
