using System.Diagnostics;

namespace OVRDP;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        CancellationTokenSource mainCTS = new();
        CancellationTokenSource overlayCTS = new();

        GCLoop( mainCTS.Token );

        Log.RegisterSink( verbose: true, ( text ) => Debug.Write( $"{text}\n" ) );

        Log.Text( "Starting OVRDP" );
        OverlayThread.Start( overlayCTS.Token );
        
        ApplicationConfiguration.Initialize();
        Application.Run( new Window( mainCTS.Token ) );

        Log.Text( "Quitting OVRDP" );

        overlayCTS.Cancel();
        OverlayThread.WaitForStop();

        mainCTS.Cancel();
    }

    // VR is a tad memory intensive. Despite this being C#
    // we still want to keep it a little slim, so let's
    // call GC every once in a while.
    async static void GCLoop( CancellationToken ct )
    {
        while ( !ct.IsCancellationRequested )
        {
            // 15 seconds
            await Task.Delay( 15 * 1000 );
            GC.Collect();
        }
    }
}