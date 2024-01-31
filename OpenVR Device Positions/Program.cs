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

        ApplicationConfiguration.Initialize();
        Application.Run( new Window() );

        VRManager.Shutdown();
    }
}