using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace OVRDP;

public static class OverlayThread
{
    private static CancellationToken _ct;

    #region Main thread

    private static Thread? _thread = null;

    public static void Start( CancellationToken ct )
    {
        _ct = ct;

        _thread = new Thread( Threaded );
        _thread.Start();

        Log.Text( "Started overlay thread" );
    }

    public static void WaitForStop()
    {
        if (  _thread is null )
        {
            Log.Text( "Error: Overlay thread is null, can't block" );
            return;
        }

        _thread.Join( 1000 );

        _thread = null;

        Log.Text( "Stopped overlay thread" );
    }

    #endregion

    #region Overlay thread

    private static float _frameCap = 90.0f;
    private static Sdl2Window _window;
    private static GraphicsDevice _device;
    private static ImGuiRenderer _renderer;
    private static CommandList _commandList;

    private static OVROverlayWrapper? _ovrOverlay = null;

    private static void Threaded()
    {
        Thread.Sleep( 500 );

        Init();
        Loop();
        Cleanup();
    }

    private static void Init()
    {
        if ( !OVRManager.Init( _ct ) )
        {
            // We hit a fatal error
            Environment.Exit( 1 );
        }



        Log.Text( "Veldrid init" );
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo( 100, 100, OverlayConstants.RenderWidth, OverlayConstants.RenderHeight, WindowState.Normal, "Overlay Test" ),
            new GraphicsDeviceOptions() { SyncToVerticalBlank = true },
            GraphicsBackend.Direct3D11,
            out _window, out _device );

        Log.Text( $"Backend: {_device.BackendType}" );

        _renderer = new ImGuiRenderer( _device, _device.MainSwapchain.Framebuffer.OutputDescription,
            (int) _device.MainSwapchain.Framebuffer.Width, (int) _device.MainSwapchain.Framebuffer.Height );

        _commandList = _device.ResourceFactory.CreateCommandList();

        Theme.SetDefault();

        _ovrOverlay = OVRManager.CreateOverlay(
            _device.ResourceFactory,
            OverlayConstants.RenderWidth, OverlayConstants.RenderHeight,
            "ovrdp-overlay", "ovrdp"
        );
        if ( _ovrOverlay is null )
        {
            OVRManager.Shutdown();
            Environment.Exit( 1 );
        }

        OverlayUI.Open( _ovrOverlay );
    }

    // Cancellable by main thread
    private static void Loop()
    {
        while ( !_ct.IsCancellationRequested )
        {
            var input = _window.PumpEvents();
            if ( !_window.Exists ) { break; }
            _renderer.Update( 1f / 60f, input ); // Compute actual value for deltaSeconds.

            OverlayUI.UpdateUI();

            _commandList.Begin();
            _commandList.SetFramebuffer( _device.MainSwapchain.Framebuffer );
            _commandList.ClearColorTarget( 0, RgbaFloat.Black );
            _renderer.Render( _device, _commandList );
            _commandList.End();
            _device.SubmitCommands( _commandList );
            _device.SwapBuffers( _device.MainSwapchain );
        }
    }

    private static void Cleanup()
    {
        Log.Text( "Overlay cleanup" );
        OverlayUI.Close();
    }

    #endregion
}
