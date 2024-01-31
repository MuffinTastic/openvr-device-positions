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

        var vrManager = VRManager.Init();
        if ( vrManager is null )
        {
            return;
        }    

        ApplicationConfiguration.Initialize();
        Application.Run( new Window( vrManager ) );

        VRManager.Shutdown();
    }
}