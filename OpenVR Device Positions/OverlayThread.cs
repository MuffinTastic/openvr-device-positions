using System.Diagnostics;
using System.Numerics;
using Valve.VR;
using Veldrid;

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

    private static GraphicsDevice _device;
    private static Texture _renderTarget;
    private static Framebuffer _frameBuffer;
    private static Pipeline _pipeLine;
    private static CommandList _commandList;

    private static ImGuiRenderer _uiRenderer;

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
            MessageBox.Show( message, OverlayConstants.ProgramNameReadable, MessageBoxButtons.OK, MessageBoxIcon.Error );
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


        //VeldridStartup.CreateWindowAndGraphicsDevice(
        //    new WindowCreateInfo( 100, 100, OverlayConstants.RenderWidth, OverlayConstants.RenderHeight, WindowState.Normal, OverlayConstants.ProgramNameReadable ),
        //    new GraphicsDeviceOptions() { SyncToVerticalBlank = true },
        //    GraphicsBackend.Direct3D11,
        //    out _window, out _device );

        _device = GraphicsDevice.CreateD3D11( new GraphicsDeviceOptions()
        {
            HasMainSwapchain = false,
            PreferStandardClipSpaceYDirection = true,
            PreferDepthRangeZeroToOne = true
        } );

        ResourceFactory factory = _device.ResourceFactory;

        _renderTarget = factory.CreateTexture(
            TextureDescription.Texture2D(
                OverlayConstants.RenderWidth, OverlayConstants.RenderHeight,
                1,
                1,
                Veldrid.PixelFormat.R8_G8_B8_A8_UNorm,
                TextureUsage.RenderTarget
            )
        );

        _frameBuffer = factory.CreateFramebuffer( new FramebufferDescription( null, _renderTarget ) );

        _commandList = factory.CreateCommandList();

        Log.Text( "Graphics started" );


        _ovrOverlay = OVRManager.CreateOverlay(
            OverlayConstants.OverlayKeyName, OverlayConstants.ProgramNameReadable
        );
        if ( _ovrOverlay is null )
        {
            throw new OverlayFatalException( "Couldn't create OpenVR overlay" );
        }

        _ovrOverlay.SetInputMethod( VROverlayInputMethod.Mouse );
        _ovrOverlay.SetInputRemapping( new Vector2( OverlayConstants.RenderWidth, OverlayConstants.RenderHeight ) );

        _uiRenderer = new ImGuiRenderer( _device, _frameBuffer.OutputDescription,
            (int) _frameBuffer.Width, (int) _frameBuffer.Height );

        Theme.SetDefault();


        OverlayUI.Open( _ovrOverlay );

        Log.Text( "Opened overlay" );


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
            // var input = _window.PumpEvents();
            // if ( !_window.Exists ) { break; }
            var snapshot = _ovrOverlay!.PollEvents();
            _uiRenderer.Update( delta, snapshot );

            OverlayUI.UpdateUI();

            _commandList.Begin();
            _commandList.SetFramebuffer( _frameBuffer );
            _commandList.ClearColorTarget( 0, RgbaFloat.Black );
            
            _uiRenderer.Render( _device, _commandList );

            _commandList.End();
            _device.SubmitCommands( _commandList );
            _device.WaitForIdle();
            _ovrOverlay!.SubmitFrame( _device, _renderTarget );

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
        OverlayUI.Close();

        _ovrOverlay?.Dispose();

        _commandList?.Dispose();
        _frameBuffer?.Dispose();
        _renderTarget?.Dispose();
        _device?.Dispose();

        OVRManager.Shutdown();

        Log.Text( "Closed overlay" );
    }

    #endregion
}
