using System.Collections.Generic;
using System.Threading.Channels;

namespace OVRDP;

public partial class Window : Form
{
    private const int c_MaxLogItems = 100;

    private CancellationToken _ct;
    private Channel<string> _channel;

    public Window( CancellationToken ct )
    {
        _ct = ct;
        _channel = Channel.CreateUnbounded<string>();

        InitializeComponent();

        logBox.DrawMode = DrawMode.OwnerDrawVariable;
        logBox.MeasureItem += logBox_MeasureItem;
        logBox.DrawItem += logBox_DrawItem;

        logBox_Pump();
        Log.RegisterSink( verbose: false, logBox_Sink );
    }

    // Log ListBox word wrapping adapted from Kosmo—ë's implementation
    // https://stackoverflow.com/questions/17613613/winforms-dotnet-listbox-items-to-word-wrap-if-content-string-width-is-bigger-tha

    private void logBox_MeasureItem( object? sender, MeasureItemEventArgs e )
    {
        e.ItemHeight = (int) e.Graphics.MeasureString( logBox.Items[e.Index].ToString(), logBox.Font, logBox.Width ).Height;
    }

    private void logBox_DrawItem( object? sender, DrawItemEventArgs e )
    {
        if ( e.Index < 0 )
            return;

        e.DrawBackground();
        e.DrawFocusRectangle();
        e.Graphics.DrawString( logBox.Items[e.Index].ToString(), e.Font!, new SolidBrush( e.ForeColor ), e.Bounds );
    }

    private void logBox_Sink( string text )
    {
        _channel.Writer.TryWrite( text );
    }

    private async void logBox_Pump()
    {
        await foreach ( var message in _channel.Reader.ReadAllAsync( _ct ) )
        {
            logBox.Items.Add( message );

            if ( logBox.Items.Count > c_MaxLogItems )
                logBox.Items.RemoveAt( 0 );

            // Scroll to bottom
            logBox.TopIndex = logBox.Items.Count - 1;
        }
    }

    private void quitButton_Click( object sender, EventArgs e )
    {
        Application.Exit();
    }
}
