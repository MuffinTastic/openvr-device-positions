using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace OVRDP;

/// <summary>
/// Manages overlay thread and resources
/// </summary>
public static class OverlayThread
{
    private static CancellationToken _ct;

    #region Main thread

    private static Thread? _thread = null;

    public static void Start( CancellationToken ct )
    {
        _ct = ct;

        _thread = new Thread( Entry );
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

    private const float _minFrameTarget = 10.0f;
    private const int _maxWaitTimeMS = (int) (1.0f / _minFrameTarget);

    private static float _frameCap = 90.0f;
    private static float _targetFrameTimeFloat = 1.0f / _frameCap;
    private static TimeSpan _targetFrameTime = default;
    private static Sdl2Window _window;
    private static GraphicsDevice _device;
    private static ImGuiRenderer _renderer;
    private static CommandList _commandList;

    private static OVROverlayWrapper? _ovrOverlay = null;

    /// <summary>
    /// Thread entry point
    /// </summary>
    private static void Entry()
    {
        Thread.Sleep( 750 );

        try
        {
            if ( !Init() )
                return; // Cancelled

            Loop();
        }
        catch ( Exception ex )
        {
            var message = $"Overlay error: {ex.Message}";
            Log.Text( message );
            MessageBox.Show( message, "OpenVR Device Positions", MessageBoxButtons.OK, MessageBoxIcon.Error );
        }
        finally
        {
            Cleanup();
        }
    }

    /// <summary>
    /// Initialize the overlay
    /// </summary>
    /// <returns>Initialization was cancelled</returns>
    private static bool Init()
    {
        if ( !OVRManager.Init( _ct ) )
        {
            return false;
        }

        _frameCap = OVRManager.GetRefreshRate();
        _targetFrameTimeFloat = 1.0f / _frameCap;
        _targetFrameTime = TimeSpan.FromSeconds( _targetFrameTimeFloat );

        Log.Text( "Veldrid init" );
        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo( 100, 100, OverlayConstants.RenderWidth, OverlayConstants.RenderHeight, WindowState.Normal, "Overlay Test" ),
            new GraphicsDeviceOptions() { SyncToVerticalBlank = true },
            GraphicsBackend.Direct3D11,
            out _window, out _device );

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

        return true;
    }

    /// <summary>
    /// Main loop for the overlay
    /// Cancellable by the main thread
    /// </summary>
    private static void Loop()
    {
        var stopwatch = new Stopwatch();
        float delta = _targetFrameTimeFloat;

        while ( !_ct.IsCancellationRequested )
        {
            stopwatch.Restart();
            var input = _window.PumpEvents();
            if ( !_window.Exists ) { break; }
            _renderer.Update( delta, input );

            OverlayUI.UpdateUI();

            _commandList.Begin();
            _commandList.SetFramebuffer( _device.MainSwapchain.Framebuffer );
            _commandList.ClearColorTarget( 0, RgbaFloat.Black );
            _renderer.Render( _device, _commandList );
            _commandList.End();
            _device.SubmitCommands( _commandList );
            _device.SwapBuffers( _device.MainSwapchain );

            var wait = _targetFrameTime - stopwatch.Elapsed;
            int waitMS = wait.Milliseconds;

            if ( waitMS > 0 )
            {
                if ( waitMS > _maxWaitTimeMS )
                    waitMS = _maxWaitTimeMS;

                Thread.Sleep( waitMS );
            }

            delta = (float) stopwatch.Elapsed.TotalSeconds;
        }

        stopwatch.Stop();
    }

    /// <summary>
    /// Overlay resource cleanup
    /// Always runs, even after exceptions
    /// </summary>
    private static void Cleanup()
    {
        Log.Text( "Overlay cleanup" );
        OverlayUI.Close();
    }

    #endregion
}
