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

        Log.RegisterSink( verbose: true, ( text ) => Debug.Write( $"{text}\n" ) );

        Log.Text( "Starting OVRDP" );

        Util.EnsureOutputDirectoryExists();

        OverlayThread.Start( overlayCTS.Token );
        
        ApplicationConfiguration.Initialize();
        Application.Run( new Window( mainCTS.Token ) );

        Log.Text( "Quitting OVRDP" );

        overlayCTS.Cancel();
        OverlayThread.WaitForStop();

        mainCTS.Cancel();
    }
}