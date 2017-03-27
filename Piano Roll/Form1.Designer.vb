<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnLeft = New System.Windows.Forms.Button()
        Me.btnRight = New System.Windows.Forms.Button()
        Me.btnPlay = New System.Windows.Forms.Button()
        Me.tbxDebugLog = New System.Windows.Forms.TextBox()
        Me.pbxGrid = New System.Windows.Forms.PictureBox()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.pbxGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnLeft
        '
        Me.btnLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLeft.Location = New System.Drawing.Point(12, 497)
        Me.btnLeft.Name = "btnLeft"
        Me.btnLeft.Size = New System.Drawing.Size(36, 32)
        Me.btnLeft.TabIndex = 1
        Me.btnLeft.Text = "<"
        Me.btnLeft.UseVisualStyleBackColor = True
        '
        'btnRight
        '
        Me.btnRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRight.Location = New System.Drawing.Point(54, 497)
        Me.btnRight.Name = "btnRight"
        Me.btnRight.Size = New System.Drawing.Size(36, 32)
        Me.btnRight.TabIndex = 2
        Me.btnRight.Text = ">"
        Me.btnRight.UseVisualStyleBackColor = True
        '
        'btnPlay
        '
        Me.btnPlay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlay.Location = New System.Drawing.Point(96, 497)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(106, 32)
        Me.btnPlay.TabIndex = 3
        Me.btnPlay.Text = "Play"
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'tbxDebugLog
        '
        Me.tbxDebugLog.Location = New System.Drawing.Point(330, 497)
        Me.tbxDebugLog.Multiline = True
        Me.tbxDebugLog.Name = "tbxDebugLog"
        Me.tbxDebugLog.Size = New System.Drawing.Size(642, 32)
        Me.tbxDebugLog.TabIndex = 4
        Me.tbxDebugLog.Text = "5 octaves, B7 - B2"
        '
        'pbxGrid
        '
        Me.pbxGrid.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.pbxGrid.Location = New System.Drawing.Point(12, 12)
        Me.pbxGrid.Name = "pbxGrid"
        Me.pbxGrid.Size = New System.Drawing.Size(960, 479)
        Me.pbxGrid.TabIndex = 5
        Me.pbxGrid.TabStop = False
        '
        'tmrUpdate
        '
        Me.tmrUpdate.Enabled = True
        Me.tmrUpdate.Interval = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(208, 498)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 29)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "BPM: 120"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(984, 541)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pbxGrid)
        Me.Controls.Add(Me.tbxDebugLog)
        Me.Controls.Add(Me.btnPlay)
        Me.Controls.Add(Me.btnRight)
        Me.Controls.Add(Me.btnLeft)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Form1"
        Me.Text = "Piano Roll"
        CType(Me.pbxGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents btnLeft As System.Windows.Forms.Button
    Friend WithEvents btnRight As System.Windows.Forms.Button
    Friend WithEvents btnPlay As System.Windows.Forms.Button
    Friend WithEvents tbxDebugLog As System.Windows.Forms.TextBox
    Friend WithEvents pnlNotes As System.Windows.Forms.Panel
    Friend WithEvents pbxGrid As System.Windows.Forms.PictureBox
    Friend WithEvents tmrUpdate As Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
