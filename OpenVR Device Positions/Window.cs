namespace OVRDP;

public partial class Window : Form
{
    private VRManager _vrManager;

    public Window( VRManager vrManager )
    {
        _vrManager = vrManager;

        InitializeComponent();
    }
}
