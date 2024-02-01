using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OVRDP;

public class OverlayUI
{
    public const int Width = 320;
    public const int Height = 300;

    private const int HelpHeight = 96;
    private const int HelpOffset = 4;

    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    private VRManager _vrManager;

    public OverlayUI( VRManager vrManager )
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


    // i for input
    private bool _iUseDeviceModels = true;
    private bool _iCenterOnHMD = true;
    private const int _iCountdownMin = 0;
    private const int _iCountdownMax = 15;
    private int _iCountdownSeconds = 5;

    private bool _iSaveBaseStations = true;
    private bool _iSaveHMD = true;
    private bool _iSaveControllers = true;
    private bool _iSaveTrackers = true;

    private bool _focusHelp = false;
    private string? _helpText = null;

    private class CountdownState
    {
        required public int MaxSeconds { get; set; }
        public int Current { get; set; } = 0;
        public bool Cancelled { get; set; } = false;
    }

    private CountdownState? _saveCountdownState = null;
    private bool _iSaveDisabled = false;

    public void UpdateUI()
    {
        ImGui.SetNextWindowPos( new Vector2( 0.0f, 0.0f ) );
        ImGui.SetNextWindowSize( new Vector2( Width, Height ) );

        ImGui.Begin( "OpenVR Device Positions",
            ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse );

        var availableSpace = ImGui.GetContentRegionAvail();
        float halfWidth = availableSpace.X / 2.0f;

        if ( _saveCountdownState is not null )
            ImGui.BeginDisabled();
        ImGui.SeparatorText( "Settings" );
        if ( _saveCountdownState is not null )
            ImGui.EndDisabled();

        // Little easter egg
        var secretButtonSize = new Vector2( 20.0f );
        ImGui.SameLine( availableSpace.X - secretButtonSize.X );
        // Supplying no text breaks the slider below
        if ( ImGui.InvisibleButton( ":)", secretButtonSize ) ) Theme.Toggle();

        if ( _saveCountdownState is not null )
            ImGui.BeginDisabled();

        ImGui.Checkbox( "Use Device Models", ref _iUseDeviceModels );
        ImGui.SameLine(); HelpMarker( "Save to the FBX with actual VR device models instead of basic spheres" );
        ImGui.Checkbox( "Center on HMD", ref _iCenterOnHMD );
        ImGui.SameLine(); HelpMarker( "Put the HMD at the origin of the FBX and reposition everything else relative to it" );
        ImGui.PushItemWidth( availableSpace.X );
        string format = ( _iCountdownSeconds > 0 ) ? "%ds" : "Off";
        ImGui.SliderInt( "", ref _iCountdownSeconds, _iCountdownMin, _iCountdownMax, $"Countdown: {format}" );
        ImGui.PopItemWidth();

        ImGui.SeparatorText( "Devices" );
        // Extra spaces make it easier to click
        ImGui.Checkbox( "Base Stations  ", ref _iSaveBaseStations );
        ImGui.SameLine( halfWidth );
        ImGui.Checkbox( "HMD            ", ref _iSaveHMD );
        ImGui.Checkbox( "Controllers    ", ref _iSaveControllers );
        ImGui.SameLine( halfWidth );
        ImGui.Checkbox( "Trackers       ", ref _iSaveTrackers );

        if ( _saveCountdownState is not null )
            ImGui.EndDisabled();

        ImGui.SeparatorText( "" );

        if ( _saveCountdownState is not null )
        {
            // Do this up here to avoid null refs
            float frac = (float) _saveCountdownState.Current / _saveCountdownState.MaxSeconds;

            if ( ImGui.Button( $"Abort ({_saveCountdownState.Current})", ImGui.GetContentRegionAvail() ) )
            {
                _saveCountdownState.Cancelled = true;
                _saveCountdownState = null;
            }

            var verticalPadding = ImGui.GetStyle().CellPadding.Y;
            float barHeight = 6.0f;
            ImGui.SetCursorPos( ImGui.GetCursorPos() - new Vector2( 0.0f, verticalPadding * 2 + barHeight ) );
            ImGui.ProgressBar( frac, new Vector2( availableSpace.X, barHeight ), "" );
        }
        else
        {
            if ( _iSaveDisabled )
                ImGui.BeginDisabled();

            if ( ImGui.Button( _iSaveDisabled ? "Saved" : "Save", ImGui.GetContentRegionAvail() ) )
            {
                _saveCountdownState = new CountdownState
                {
                    MaxSeconds = _iCountdownSeconds
                };

                var saveSettings = new SaveSettings
                {
                    UseDeviceModels = _iUseDeviceModels,
                    CenterOnHMD = _iCenterOnHMD,
                    SaveBaseStations = _iSaveBaseStations,
                    SaveHMD = _iSaveHMD,
                    SaveControllers = _iSaveControllers,
                    SaveTrackers = _iSaveTrackers
                };

                RunSaveCountdown( _saveCountdownState, saveSettings );
            }

            if ( _iSaveDisabled )
                ImGui.EndDisabled();
        }

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

        ImGui.PushStyleColor( ImGuiCol.Border, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.BorderShadow, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Text, Theme.HelpMarkerText );
        ImGui.PushStyleColor( ImGuiCol.ButtonHovered, Theme.HelpMarkerHover );
        ImGui.PushStyleColor( ImGuiCol.ButtonActive, Theme.HelpMarkerHover );

        // You can't select the second marker if you
        // call Button() normally here... Hate.
        ImGui.Button( "(?)" );
        if ( ImGui.IsItemClicked() )
        {
            _helpText = helpText;
            _focusHelp = true;
        }


        ImGui.PopStyleColor(6);
    }

    private async void RunSaveCountdown( CountdownState state, SaveSettings saveSettings )
    {
        state.Current = state.MaxSeconds;

        while ( state.Current > 0 )
        {
            await Task.Delay( 1000 );
            
            if ( state.Cancelled )
            {
                return;
            }

            state.Current--;
        }

        _saveCountdownState = null;

        _iSaveDisabled = true;
        _vrManager.SavePositions( saveSettings );
        await Task.Delay( 750 );
        _iSaveDisabled = false;
    }
}
