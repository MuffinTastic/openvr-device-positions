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

    private static VRManager? _vrManager = null;

    private const int c_Width = 512;
    private const int c_Height = 512;

    private static int frameCap = 60;
    private static Sdl2Window window;
    private static GraphicsDevice device;
    private static ImGuiRenderer renderer;
    private static CommandList commandList;

    private static void Threaded()
    {
        Thread.Sleep( 500 );

        Init();
        Loop();
        Cleanup();
    }

    private static void Init()
    {
        // VRManager.Init waits indefinitely for SteamVR to launch
        // if it's not already running
        // _vrManager = VRManager.Init( _ct );
        // if ( _vrManager is null )
        // {
        //     // We hit a fatal error
        //     Environment.Exit( 1 );
        // }

        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo( 100, 100, c_Width, c_Height, Veldrid.WindowState.Normal, "test" ),
            out window, out device );

        renderer = new ImGuiRenderer( device, device.MainSwapchain.Framebuffer.OutputDescription,
            (int) device.MainSwapchain.Framebuffer.Width, (int) device.MainSwapchain.Framebuffer.Height );

        commandList = device.ResourceFactory.CreateCommandList();
    }

    // Cancellable by main thread
    private static void Loop()
    {
        while ( !_ct.IsCancellationRequested )
        {
            var input = window.PumpEvents();
            if ( !window.Exists ) { break; }
            renderer.Update( 1f / 60f, input ); // Compute actual value for deltaSeconds.

            // Draw stuff
            ImGui.Text( "Hello World" );
            if ( ImGui.Button( "Test" ) )
            {
                Log.Text( "you pressed the button" );
            }

            commandList.Begin();
            commandList.SetFramebuffer( device.MainSwapchain.Framebuffer );
            commandList.ClearColorTarget( 0, RgbaFloat.Black );
            renderer.Render( device, commandList );
            commandList.End();
            device.SubmitCommands( commandList );
            device.SwapBuffers( device.MainSwapchain );
        }
    }

    private static void Cleanup()
    {
        Log.Text( "Thread cleanup" );
    }

    #endregion
}
