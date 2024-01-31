using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVRDP;

public class OverlayThread
{
    #region Main thread

    private CancellationTokenSource? _cts = null;
    private Thread? _thread = null;

    public OverlayThread()
    {

    }

    public void Start()
    {
        _cts = new();
        _ct = _cts.Token;

        _thread = new Thread( Threaded );
        _thread.Start();

        Log.Text( "Started overlay thread" );
    }

    public void Stop()
    {
        if ( _cts is null )
        {
            Log.Text( "Error: Tried to cancel null token source" );
            return;
        }

        _cts.Cancel();

        if (  _thread is null )
        {
            Log.Text( "Error: Overlay thread is null, can't block" );
            return;
        }

        _thread.Join( 1000 );

        _cts = null;
        _thread = null;

        Log.Text( "Stopped overlay thread" );
    }

    #endregion

    #region Overlay thread

    private CancellationToken _ct;

    private void Threaded()
    {
        Init();
        CancellableLoop();
        Cleanup();
    }

    private void Init()
    {
        Log.Text( "Thread init" );
    }

    private void CancellableLoop()
    {
        while ( !_ct.IsCancellationRequested )
        {
            Log.Text( "Thread hello" );
            Thread.Sleep( 500 );
        }
    }

    private void Cleanup()
    {
        Log.Text( "Thread cleanup" );
    }

    #endregion
}
