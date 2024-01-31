using System.Collections.Generic;

namespace OVRDP;

public partial class Window : Form
{
    private VRManager _vrManager;

    private const int c_MaxLogItems = 100;

    public Window( VRManager vrManager )
    {
        _vrManager = vrManager;

        InitializeComponent();

        logBox.DrawMode = DrawMode.OwnerDrawVariable;
        logBox.MeasureItem += logBox_MeasureItem;
        logBox.DrawItem += logBox_DrawItem;

        Log.RegisterSink( verbose: false, logBox_Record );

        newMsgLoop();
    }

    private async void newMsgLoop()
    {
        while ( true )
        {
            Log.Text( " test " );
            await Task.Delay( 750 );
        }
    }

    // Log ListBox word wrapping adapted from Kosmo��'s implementation
    // https://stackoverflow.com/questions/17613613/winforms-dotnet-listbox-items-to-word-wrap-if-content-string-width-is-bigger-tha

    private void logBox_MeasureItem( object? sender, MeasureItemEventArgs e )
    {
        e.ItemHeight = (int) e.Graphics.MeasureString( logBox.Items[e.Index].ToString(), logBox.Font, logBox.Width ).Height;
    }

    private void logBox_DrawItem( object? sender, DrawItemEventArgs e )
    {
        e.DrawBackground();
        e.DrawFocusRectangle();
        e.Graphics.DrawString( logBox.Items[e.Index].ToString(), e.Font!, new SolidBrush( e.ForeColor ), e.Bounds );
    }

    private void logBox_Record( string text )
    {
        logBox.Items.Add( text );

        if ( logBox.Items.Count > c_MaxLogItems )
            logBox.Items.RemoveAt( 0 );

        // Scroll to bottom
        logBox.TopIndex = logBox.Items.Count - 1;
    }
}
