namespace SteamVR_Tracker_Positions;

partial class Window
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing && ( components != null ) )
        {
            components.Dispose();
        }
        base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        saveButton = new Button();
        modelCheckbox = new CheckBox();
        savePanel = new Panel();
        saveProgressBar = new ProgressBar();
        centerCheckbox = new CheckBox();
        settingsGroup = new GroupBox();
        countdownInputLabel = new Label();
        countdownInput = new NumericUpDown();
        devicesGroup = new GroupBox();
        baseStationDeviceCheckbox = new CheckBox();
        trackerDeviceCheckbox = new CheckBox();
        controllerDeviceCheckbox = new CheckBox();
        hmdDeviceCheckbox = new CheckBox();
        savePanel.SuspendLayout();
        settingsGroup.SuspendLayout();
        ( (System.ComponentModel.ISupportInitialize) countdownInput ).BeginInit();
        devicesGroup.SuspendLayout();
        SuspendLayout();
        // 
        // saveButton
        // 
        saveButton.Font = new Font( "Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point,  0 );
        saveButton.Location = new Point( 0, 0 );
        saveButton.Name = "saveButton";
        saveButton.Size = new Size( 304, 257 );
        saveButton.TabIndex = 0;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click +=  saveButton_Click ;
        // 
        // modelCheckbox
        // 
        modelCheckbox.AutoSize = true;
        modelCheckbox.Checked = true;
        modelCheckbox.CheckState = CheckState.Checked;
        modelCheckbox.Location = new Point( 6, 22 );
        modelCheckbox.Name = "modelCheckbox";
        modelCheckbox.Size = new Size( 137, 19 );
        modelCheckbox.TabIndex = 1;
        modelCheckbox.Text = "Use SteamVR Models";
        modelCheckbox.UseVisualStyleBackColor = true;
        // 
        // savePanel
        // 
        savePanel.Controls.Add( saveProgressBar );
        savePanel.Controls.Add( saveButton );
        savePanel.Location = new Point( 180, 12 );
        savePanel.Name = "savePanel";
        savePanel.Size = new Size( 304, 257 );
        savePanel.TabIndex = 3;
        // 
        // saveProgressBar
        // 
        saveProgressBar.Location = new Point( 0, 234 );
        saveProgressBar.Name = "saveProgressBar";
        saveProgressBar.Size = new Size( 304, 23 );
        saveProgressBar.TabIndex = 1;
        // 
        // centerCheckbox
        // 
        centerCheckbox.AutoSize = true;
        centerCheckbox.Checked = true;
        centerCheckbox.CheckState = CheckState.Checked;
        centerCheckbox.Location = new Point( 6, 47 );
        centerCheckbox.Name = "centerCheckbox";
        centerCheckbox.Size = new Size( 109, 19 );
        centerCheckbox.TabIndex = 4;
        centerCheckbox.Text = "Center on HMD";
        centerCheckbox.UseVisualStyleBackColor = true;
        // 
        // settingsGroup
        // 
        settingsGroup.Controls.Add( countdownInputLabel );
        settingsGroup.Controls.Add( countdownInput );
        settingsGroup.Controls.Add( modelCheckbox );
        settingsGroup.Controls.Add( centerCheckbox );
        settingsGroup.Location = new Point( 12, 12 );
        settingsGroup.Name = "settingsGroup";
        settingsGroup.Size = new Size( 162, 104 );
        settingsGroup.TabIndex = 6;
        settingsGroup.TabStop = false;
        settingsGroup.Text = "Settings";
        // 
        // countdownInputLabel
        // 
        countdownInputLabel.AutoSize = true;
        countdownInputLabel.Location = new Point( 52, 74 );
        countdownInputLabel.Name = "countdownInputLabel";
        countdownInputLabel.Size = new Size( 86, 15 );
        countdownInputLabel.TabIndex = 6;
        countdownInputLabel.Text = "Countdown (s)";
        // 
        // countdownInput
        // 
        countdownInput.Location = new Point( 6, 72 );
        countdownInput.Maximum = new decimal( new int[] { 30, 0, 0, 0 } );
        countdownInput.Name = "countdownInput";
        countdownInput.Size = new Size( 40, 23 );
        countdownInput.TabIndex = 5;
        countdownInput.Value = new decimal( new int[] { 5, 0, 0, 0 } );
        // 
        // devicesGroup
        // 
        devicesGroup.Controls.Add( baseStationDeviceCheckbox );
        devicesGroup.Controls.Add( trackerDeviceCheckbox );
        devicesGroup.Controls.Add( controllerDeviceCheckbox );
        devicesGroup.Controls.Add( hmdDeviceCheckbox );
        devicesGroup.Location = new Point( 12, 122 );
        devicesGroup.Name = "devicesGroup";
        devicesGroup.Size = new Size( 162, 147 );
        devicesGroup.TabIndex = 7;
        devicesGroup.TabStop = false;
        devicesGroup.Text = "Devices";
        // 
        // baseStationDeviceCheckbox
        // 
        baseStationDeviceCheckbox.AutoSize = true;
        baseStationDeviceCheckbox.Checked = true;
        baseStationDeviceCheckbox.CheckState = CheckState.Checked;
        baseStationDeviceCheckbox.Location = new Point( 6, 22 );
        baseStationDeviceCheckbox.Name = "baseStationDeviceCheckbox";
        baseStationDeviceCheckbox.Size = new Size( 95, 19 );
        baseStationDeviceCheckbox.TabIndex = 3;
        baseStationDeviceCheckbox.Text = "Base Stations";
        baseStationDeviceCheckbox.UseVisualStyleBackColor = true;
        // 
        // trackerDeviceCheckbox
        // 
        trackerDeviceCheckbox.AutoSize = true;
        trackerDeviceCheckbox.Checked = true;
        trackerDeviceCheckbox.CheckState = CheckState.Checked;
        trackerDeviceCheckbox.Location = new Point( 6, 97 );
        trackerDeviceCheckbox.Name = "trackerDeviceCheckbox";
        trackerDeviceCheckbox.Size = new Size( 68, 19 );
        trackerDeviceCheckbox.TabIndex = 2;
        trackerDeviceCheckbox.Text = "Trackers";
        trackerDeviceCheckbox.UseVisualStyleBackColor = true;
        // 
        // controllerDeviceCheckbox
        // 
        controllerDeviceCheckbox.AutoSize = true;
        controllerDeviceCheckbox.Checked = true;
        controllerDeviceCheckbox.CheckState = CheckState.Checked;
        controllerDeviceCheckbox.Location = new Point( 6, 72 );
        controllerDeviceCheckbox.Name = "controllerDeviceCheckbox";
        controllerDeviceCheckbox.Size = new Size( 84, 19 );
        controllerDeviceCheckbox.TabIndex = 1;
        controllerDeviceCheckbox.Text = "Controllers";
        controllerDeviceCheckbox.UseVisualStyleBackColor = true;
        // 
        // hmdDeviceCheckbox
        // 
        hmdDeviceCheckbox.AutoSize = true;
        hmdDeviceCheckbox.Checked = true;
        hmdDeviceCheckbox.CheckState = CheckState.Checked;
        hmdDeviceCheckbox.Location = new Point( 6, 47 );
        hmdDeviceCheckbox.Name = "hmdDeviceCheckbox";
        hmdDeviceCheckbox.Size = new Size( 54, 19 );
        hmdDeviceCheckbox.TabIndex = 0;
        hmdDeviceCheckbox.Text = "HMD";
        hmdDeviceCheckbox.UseVisualStyleBackColor = true;
        // 
        // Window
        // 
        AutoScaleDimensions = new SizeF( 7F, 15F );
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size( 496, 281 );
        Controls.Add( devicesGroup );
        Controls.Add( settingsGroup );
        Controls.Add( savePanel );
        MaximizeBox = false;
        Name = "Window";
        Text = "SteamVR Device Positions";
        savePanel.ResumeLayout( false );
        settingsGroup.ResumeLayout( false );
        settingsGroup.PerformLayout();
        ( (System.ComponentModel.ISupportInitialize) countdownInput ).EndInit();
        devicesGroup.ResumeLayout( false );
        devicesGroup.PerformLayout();
        ResumeLayout( false );
    }

    #endregion

    private Button saveButton;
    private CheckBox modelCheckbox;
    private Panel savePanel;
    private ProgressBar saveProgressBar;
    private CheckBox centerCheckbox;
    private GroupBox settingsGroup;
    private GroupBox devicesGroup;
    private CheckBox baseStationDeviceCheckbox;
    private CheckBox trackerDeviceCheckbox;
    private CheckBox controllerDeviceCheckbox;
    private CheckBox hmdDeviceCheckbox;
    private Label countdownInputLabel;
    private NumericUpDown countdownInput;
}
