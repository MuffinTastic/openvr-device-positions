using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OVRDP;

public class Overlay
{
    public const int Width = 320;
    public const int Height = 288;

    private const int HelpHeight = 96;
    private const int HelpOffset = 4;

    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    private VRManager _vrManager;

    private bool useDeviceModels = true;
    private bool centerOnHMD = true;
    private int countdownSeconds = 5;

    private bool saveBaseStations = true;
    private bool saveHMD = true;
    private bool saveControllers = true;
    private bool saveTrackers = true;

    public Overlay( VRManager vrManager )
    {
        _vrManager = vrManager;

    }

    public void Close()
    {

    }

    public void UpdateTransform()
    {

    }

    private void SubmitTransform()
    {

    }

    private bool _focusHelp = false;
    private string? _helpText = null;

    public void UpdateUI()
    {
        ImGui.SetNextWindowPos( new Vector2( 0.0f, 0.0f ) );
        ImGui.SetNextWindowSize( new Vector2( Width, Height ) );

        ImGui.Begin( "OpenVR Device Positions",
            ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse );

        var availableSpace = ImGui.GetContentRegionAvail();
        float halfWidth = availableSpace.X / 2.0f;

        ImGui.SeparatorText( "Settings" );

        var secretButtonSize = new Vector2( 20.0f );
        ImGui.SameLine( availableSpace.X - secretButtonSize.X ); // little easter egg
        // Supplying no text breaks the slider below
        if ( ImGui.InvisibleButton( ":)", secretButtonSize ) ) Theme.Toggle();
        
        ImGui.Checkbox( "Use Device Models", ref useDeviceModels );
        ImGui.SameLine(); HelpMarker( "Save to the FBX with actual VR device models instead of basic spheres" );
        ImGui.Checkbox( "Center on HMD", ref centerOnHMD );
        ImGui.SameLine(); HelpMarker( "Put the HMD at the origin of the FBX and reposition everything else relative to it" );
        ImGui.PushItemWidth( availableSpace.X );
        string format = ( countdownSeconds > 0 ) ? "%ds" : "Off";
        ImGui.SliderInt( "", ref countdownSeconds, 0, 10, $"Countdown: {format}" );
        ImGui.PopItemWidth();

        ImGui.SeparatorText( "Devices" );
        ImGui.Checkbox( "Base Stations  ", ref saveBaseStations );
        ImGui.SameLine( halfWidth );
        ImGui.Checkbox( "HMD            ", ref saveHMD );
        ImGui.Checkbox( "Controllers    ", ref saveControllers );
        ImGui.SameLine( halfWidth );
        ImGui.Checkbox( "Trackers       ", ref saveTrackers );

        ImGui.SeparatorText( "" );

        if ( ImGui.Button( "Save", ImGui.GetContentRegionAvail() ) )
            Log.Text( "hehe" );
        ImGui.End();

        if ( _focusHelp )
        {
            _focusHelp = false;
            ImGui.SetNextWindowFocus();
        }

        if ( _helpText is not null )
        {
            var childBg = ImGui.GetStyle().Colors[(int) ImGuiCol.ChildBg];

            ImGui.PushStyleColor( ImGuiCol.WindowBg, childBg );
            ImGui.SetNextWindowPos( new Vector2( HelpOffset, Height - HelpOffset - HelpHeight ) );
            ImGui.SetNextWindowSize( new Vector2( Width - HelpOffset * 2, HelpHeight ) );
            ImGui.Begin( "Help",
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse );

            ImGui.TextWrapped( _helpText );

            if ( !ImGui.IsWindowAppearing() && !ImGui.IsWindowFocused() )
                _helpText = null;

            ImGui.End();
            ImGui.PopStyleColor();
        }
    }

    private void HelpMarker( string helpText )
    {
        ImGui.SetCursorPos( ImGui.GetCursorPos() + new Vector2( 10.0f, 0.0f ) );

        var textDisabled = ImGui.GetStyle().Colors[(int) ImGuiCol.TextDisabled];
        ImGui.PushStyleColor( ImGuiCol.Border, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.BorderShadow, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Text, textDisabled );
        ImGui.PushStyleColor( ImGuiCol.ButtonHovered, Theme.HelpMarkerHover );
        ImGui.PushStyleColor( ImGuiCol.ButtonActive, Theme.HelpMarkerActive );

        if ( ImGui.Button( "(?)" ) )
        {
            _helpText = helpText;
            _focusHelp = true;
        }

        ImGui.PopStyleColor(6);
    }
}
