using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using static System.Net.Mime.MediaTypeNames;

namespace OVRDP;

/// <summary>
/// Handles logic for VR overlay positioning and ImGui
/// </summary>
public static class OverlayUI
{
    private const int ICountdownMin = 0;
    private const int ICountdownMax = 15;

    private const int HelpHeight = 96;
    private const int HelpOffset = 4;


    private static Vector3 _position;
    private static Quaternion _rotation;

    private static OVROverlayWrapper? _ovrOverlay = null;

    public static bool Open( OVROverlayWrapper ovrOverlay )
    {
        _ovrOverlay = ovrOverlay;

        _ovrOverlay.SetWidthInMeters( OverlayConstants.VRWidth );
        _ovrOverlay.SetColor( new Vector3( 1.0f ) );

        VRTextureBounds_t bounds = new()
        {
            uMin = 0,
            uMax = 1,
            vMin = 0,
            vMax = 1
        };
        _ovrOverlay.SetTextureBounds( bounds );

        _ovrOverlay.Show();

        Log.Text( "Created overlay" );

        return true;
    }

    public static void Close()
    {
        if ( _ovrOverlay is null )
            return;

        _ovrOverlay.Hide();
        _ovrOverlay.Close();

        Log.Text( "Closed overlay" );
    }

    /// <summary>
    /// Updates the position and rotation of the overlay
    /// </summary>
    public static void UpdateTransform()
    {

    }

    /// <summary>
    /// Submits the position and rotation to OpenVR
    /// </summary>
    private static void SubmitTransform()
    {

    }


    // i for input
    private static bool _iUseDeviceModels = true;
    private static bool _iCenterOnHMD = true;
    private static bool _iSaveInvalidPoses = true;
    private static int _iCountdownSeconds = 5;

    private static bool _iSaveBaseStations = true;
    private static bool _iSaveHMD = true;
    private static bool _iSaveControllers = true;
    private static bool _iSaveTrackers = true;

    private static bool _focusHelp = false;
    private static string? _helpText = null;

    private class CountdownState
    {
        required public int MaxSeconds { get; set; }
        public int Current { get; set; } = 0;
        public bool Cancelled { get; set; } = false;
    }

    private static CountdownState? _saveCountdownState = null;
    private static bool _iSaveDisabled = false;

    /// <summary>
    /// Update ImGui UI
    /// </summary>
    public static void UpdateUI()
    {
        ImGui.SetNextWindowPos( new Vector2( 0.0f, 0.0f ) );
        ImGui.SetNextWindowSize( new Vector2( OverlayConstants.RenderWidth, OverlayConstants.RenderHeight ) );

        ImGui.Begin( OverlayConstants.ProgramNameReadable,
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
        ImGui.Checkbox( "Save Invalid Poses", ref _iSaveInvalidPoses );
        ImGui.SameLine(); HelpMarker( "Save devices to the FBX even if they've lost tracking" );
        ImGui.PushItemWidth( availableSpace.X );
        string format = ( _iCountdownSeconds > 0 ) ? "%ds" : "Off";
        ImGui.SliderInt( "", ref _iCountdownSeconds, ICountdownMin, ICountdownMax, $"Countdown: {format}" );
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
                    SaveInvalidPoses = _iSaveInvalidPoses,
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
            ShowHelpWindow();
        }
    }

    /// <summary>
    /// Help marker widget, click to show help window
    /// </summary>
    private static void HelpMarker( string helpText )
    {
        ImGui.SetCursorPos( ImGui.GetCursorPos() + new Vector2( 10.0f, 0.0f ) );

        ImGui.PushStyleColor( ImGuiCol.Border, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.BorderShadow, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Button, new Vector4( 0.0f ) );
        ImGui.PushStyleColor( ImGuiCol.Text, Theme.HelpMarkerTextColor );
        ImGui.PushStyleColor( ImGuiCol.ButtonHovered, Theme.HelpMarkerHoverColor );
        ImGui.PushStyleColor( ImGuiCol.ButtonActive, Theme.HelpMarkerHoverColor );

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

    private static void ShowHelpWindow()
    {
        var childBg = ImGui.GetStyle().Colors[(int) ImGuiCol.ChildBg];

        ImGui.PushStyleColor( ImGuiCol.WindowBg, childBg );
        ImGui.SetNextWindowPos( new Vector2( HelpOffset, OverlayConstants.RenderHeight - HelpOffset - HelpHeight ) );
        ImGui.SetNextWindowSize( new Vector2( OverlayConstants.RenderWidth - HelpOffset * 2, HelpHeight ) );
        ImGui.Begin( "Help",
            ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse );

        ImGui.TextWrapped( _helpText );

        if ( !ImGui.IsWindowAppearing() && !ImGui.IsWindowFocused() )
            _helpText = null;

        ImGui.End();
        ImGui.PopStyleColor();
    }

    /// <summary>
    /// Run a countdown asynchronously. 
    /// </summary>
    /// <param name="state">The state to run with. Max seconds of 0 makes it 'finish' instantly.</param>
    /// <param name="saveSettings">The user's save settings at countdown start</param>
    private static async void RunSaveCountdown( CountdownState state, SaveSettings saveSettings )
    {
        state.Current = state.MaxSeconds;

        if ( state.Current != 0 )
            Log.Text( "Starting countdown" );

        while ( state.Current > 0 )
        {
            Log.Text( $"{state.Current}..." );
            await Task.Delay( 1000 );
            
            if ( state.Cancelled )
            {
                Log.Text( "Countdown cancelled" );
                return;
            }

            state.Current--;
        }

        _saveCountdownState = null;

        _iSaveDisabled = true;
        Devices.SavePositions( saveSettings );
        await Task.Delay( 750 );
        _iSaveDisabled = false;
    }
}
