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
        Log.RegisterSink( verbose: true, ( text ) => Debug.Write( $"{text}\n" ) );

        var overlayThread = new OverlayThread();

        overlayThread.Start();

        ApplicationConfiguration.Initialize();
        Application.Run( new Window() );

        overlayThread.Stop();
    }
}