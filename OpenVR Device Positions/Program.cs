namespace OVRDP;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
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