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
        CancellationTokenSource cts = new();
        CancellationToken ct = cts.Token;

        Log.RegisterSink( verbose: true, ( text ) => Debug.Write( $"{text}\n" ) );

        Log.Text( "Starting OVRDP" );
        OverlayThread.Start( ct );
        
        ApplicationConfiguration.Initialize();
        Application.Run( new Window( ct ) );

        Log.Text( "Quitting OVRDP" );
        cts.Cancel();
        OverlayThread.WaitForStop();
    }
}