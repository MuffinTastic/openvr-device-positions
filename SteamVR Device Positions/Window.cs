namespace SteamVR_Device_Positions;

public partial class Window : Form
{
    private VRManager _vrManager;

    public Window( VRManager vrManager )
    {
        _vrManager = vrManager;

        InitializeComponent();
        saveProgressBar.Visible = false;
    }

    private async void saveButton_Click( object sender, EventArgs e )
    {
        saveButton.Enabled = false;
        saveProgressBar.Visible = true;

        int countdown = (int) countdownInput.Value;
        saveProgressBar.Maximum = countdown;

        while ( countdown > 0 )
        {
            saveProgressBar.Value = countdown;
            saveButton.Text = $"Saving in {countdown}...";
            countdown--;
            await Task.Delay( 1000 );
        }

        saveProgressBar.Value = countdown;
        saveProgressBar.Visible = false;
        saveButton.Text = $"Saving...";

        bool centerOnHMD = centerCheckbox.Checked;
        _vrManager.GetPositions( centerOnHMD );

        saveButton.Text = $"Saved";
        await Task.Delay( 500 );

        saveButton.Text = $"Save";
        saveButton.Enabled = true;
    }
}
