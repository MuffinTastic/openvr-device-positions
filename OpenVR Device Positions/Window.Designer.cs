namespace OVRDP;

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
        logBox = new ListBox();
        quitButton = new Button();
        openSaveFolderButton = new Button();
        SuspendLayout();
        // 
        // logBox
        // 
        logBox.ForeColor = SystemColors.WindowFrame;
        logBox.FormattingEnabled = true;
        logBox.ItemHeight = 15;
        logBox.Location = new Point( 12, 12 );
        logBox.Name = "logBox";
        logBox.Size = new Size( 360, 199 );
        logBox.TabIndex = 0;
        // 
        // quitButton
        // 
        quitButton.Location = new Point( 195, 217 );
        quitButton.Name = "quitButton";
        quitButton.Size = new Size( 177, 32 );
        quitButton.TabIndex = 2;
        quitButton.Text = "Quit";
        quitButton.UseVisualStyleBackColor = true;
        quitButton.Click +=  quitButton_Click ;
        // 
        // openSaveFolderButton
        // 
        openSaveFolderButton.Location = new Point( 12, 217 );
        openSaveFolderButton.Name = "openSaveFolderButton";
        openSaveFolderButton.Size = new Size( 177, 32 );
        openSaveFolderButton.TabIndex = 3;
        openSaveFolderButton.Text = "Open Output Folder";
        openSaveFolderButton.UseVisualStyleBackColor = true;
        openSaveFolderButton.Click +=  openSaveFolderButton_Click ;
        // 
        // Window
        // 
        AutoScaleDimensions = new SizeF( 7F, 15F );
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size( 384, 261 );
        Controls.Add( openSaveFolderButton );
        Controls.Add( quitButton );
        Controls.Add( logBox );
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "Window";
        Text = "OpenVR Device Positions";
        ResumeLayout( false );
    }

    #endregion

    private ListBox logBox;
    private Button quitButton;
    private Button openSaveFolderButton;
}
