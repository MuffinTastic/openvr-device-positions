namespace SteamVR_Tracker_Positions;

public partial class Window : Form
{
    public Window()
    {
        InitializeComponent();
        saveProgressBar.Visible = false;
    }

    private async void saveButton_Click( object sender, EventArgs e )
    {
        saveButton.Enabled = false;
        saveProgressBar.Visible = true;

        int num = 5;
        saveProgressBar.Maximum = num;

        while ( num > 0 )
        {
            saveProgressBar.Value = num;
            saveButton.Text = $"Saving in {num}...";
            num--;
            await Task.Delay( 1000 );
        }

        saveProgressBar.Value = num;
        saveProgressBar.Visible = false;
        saveButton.Text = $"Saving...";



        saveButton.Text = $"Saved";
        await Task.Delay( 1000 );

        saveButton.Text = $"Save";
        saveButton.Enabled = true;
    }
}
