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
        panel1 = new Panel();
        saveProgressBar = new ProgressBar();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // saveButton
        // 
        saveButton.Font = new Font( "Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point,  0 );
        saveButton.Location = new Point( 3, 3 );
        saveButton.Name = "saveButton";
        saveButton.Size = new Size( 466, 226 );
        saveButton.TabIndex = 0;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click +=  saveButton_Click ;
        // 
        // modelCheckbox
        // 
        modelCheckbox.AutoSize = true;
        modelCheckbox.Location = new Point( 12, 12 );
        modelCheckbox.Name = "modelCheckbox";
        modelCheckbox.Size = new Size( 137, 19 );
        modelCheckbox.TabIndex = 1;
        modelCheckbox.Text = "Use SteamVR Models";
        modelCheckbox.UseVisualStyleBackColor = true;
        // 
        // panel1
        // 
        panel1.Controls.Add( saveProgressBar );
        panel1.Controls.Add( saveButton );
        panel1.Location = new Point( 12, 37 );
        panel1.Name = "panel1";
        panel1.Size = new Size( 472, 232 );
        panel1.TabIndex = 3;
        // 
        // saveProgressBar
        // 
        saveProgressBar.Location = new Point( 3, 206 );
        saveProgressBar.Name = "saveProgressBar";
        saveProgressBar.Size = new Size( 466, 23 );
        saveProgressBar.TabIndex = 1;
        // 
        // Window
        // 
        AutoScaleDimensions = new SizeF( 7F, 15F );
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size( 496, 281 );
        Controls.Add( panel1 );
        Controls.Add( modelCheckbox );
        MaximizeBox = false;
        Name = "Window";
        Text = "SteamVR Tracker Positions";
        panel1.ResumeLayout( false );
        ResumeLayout( false );
        PerformLayout();
    }

    #endregion

    private Button saveButton;
    private CheckBox modelCheckbox;
    private Panel panel1;
    private ProgressBar saveProgressBar;
}
