using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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
        _vrManager = VRManager.Init( _ct );
        if ( _vrManager is null )
        {
            // We hit a fatal error
            Environment.Exit( 1 );
        }
    }

    // Cancellable by main thread
    private static void Loop()
    {
        while ( !_ct.IsCancellationRequested )
        {
            Thread.Sleep( 500 );
        }
    }

    private static void Cleanup()
    {
        Log.Text( "Thread cleanup" );
    }

    #endregion
}
