<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonOpen = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonZoomIn = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonZoomOut = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonRotateCCW = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonRotateCW = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripComboBoxLang = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripButtonExtract = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonHelp = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PanelCenter = New System.Windows.Forms.Panel()
        Me.PanelPictureBox = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.PanelCenter.SuspendLayout()
        Me.PanelPictureBox.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonOpen, Me.ToolStripButtonSave, Me.ToolStripButtonZoomIn, Me.ToolStripButtonZoomOut, Me.ToolStripButtonRotateCCW, Me.ToolStripButtonRotateCW, Me.ToolStripComboBoxLang, Me.ToolStripButtonExtract, Me.ToolStripButtonHelp})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(512, 42)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonOpen
        '
        Me.ToolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonOpen.Image = CType(resources.GetObject("ToolStripButtonOpen.Image"), System.Drawing.Image)
        Me.ToolStripButtonOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonOpen.Name = "ToolStripButtonOpen"
        Me.ToolStripButtonOpen.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonOpen.Text = "Open"
        Me.ToolStripButtonOpen.ToolTipText = "ファイルを開く"
        '
        'ToolStripButtonSave
        '
        Me.ToolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSave.Image = CType(resources.GetObject("ToolStripButtonSave.Image"), System.Drawing.Image)
        Me.ToolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSave.Name = "ToolStripButtonSave"
        Me.ToolStripButtonSave.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonSave.Text = "ToolStripButton1"
        Me.ToolStripButtonSave.ToolTipText = "テキストを保存"
        '
        'ToolStripButtonZoomIn
        '
        Me.ToolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonZoomIn.Image = CType(resources.GetObject("ToolStripButtonZoomIn.Image"), System.Drawing.Image)
        Me.ToolStripButtonZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonZoomIn.Name = "ToolStripButtonZoomIn"
        Me.ToolStripButtonZoomIn.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonZoomIn.Text = "ToolStripButton2"
        Me.ToolStripButtonZoomIn.ToolTipText = "ズームアップ 拡大"
        '
        'ToolStripButtonZoomOut
        '
        Me.ToolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonZoomOut.Image = CType(resources.GetObject("ToolStripButtonZoomOut.Image"), System.Drawing.Image)
        Me.ToolStripButtonZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonZoomOut.Name = "ToolStripButtonZoomOut"
        Me.ToolStripButtonZoomOut.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonZoomOut.Text = "ToolStripButton2"
        Me.ToolStripButtonZoomOut.ToolTipText = "ズームアウト 縮小"
        '
        'ToolStripButtonRotateCCW
        '
        Me.ToolStripButtonRotateCCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonRotateCCW.Image = CType(resources.GetObject("ToolStripButtonRotateCCW.Image"), System.Drawing.Image)
        Me.ToolStripButtonRotateCCW.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonRotateCCW.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonRotateCCW.Name = "ToolStripButtonRotateCCW"
        Me.ToolStripButtonRotateCCW.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonRotateCCW.Text = "ToolStripButton2"
        Me.ToolStripButtonRotateCCW.ToolTipText = "左90度回転"
        '
        'ToolStripButtonRotateCW
        '
        Me.ToolStripButtonRotateCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonRotateCW.Image = CType(resources.GetObject("ToolStripButtonRotateCW.Image"), System.Drawing.Image)
        Me.ToolStripButtonRotateCW.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonRotateCW.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonRotateCW.Name = "ToolStripButtonRotateCW"
        Me.ToolStripButtonRotateCW.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonRotateCW.Text = "ToolStripButton2"
        Me.ToolStripButtonRotateCW.ToolTipText = "右90度回転"
        '
        'ToolStripComboBoxLang
        '
        Me.ToolStripComboBoxLang.Name = "ToolStripComboBoxLang"
        Me.ToolStripComboBoxLang.Size = New System.Drawing.Size(75, 42)
        Me.ToolStripComboBoxLang.Text = "言語"
        Me.ToolStripComboBoxLang.ToolTipText = "読み取りテキストの言語を指定する"
        '
        'ToolStripButtonExtract
        '
        Me.ToolStripButtonExtract.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonExtract.Image = CType(resources.GetObject("ToolStripButtonExtract.Image"), System.Drawing.Image)
        Me.ToolStripButtonExtract.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonExtract.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonExtract.Name = "ToolStripButtonExtract"
        Me.ToolStripButtonExtract.Size = New System.Drawing.Size(53, 39)
        Me.ToolStripButtonExtract.Text = "ToolStripButton1"
        Me.ToolStripButtonExtract.ToolTipText = "画像からテキストを読み取る"
        '
        'ToolStripButtonHelp
        '
        Me.ToolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonHelp.Image = CType(resources.GetObject("ToolStripButtonHelp.Image"), System.Drawing.Image)
        Me.ToolStripButtonHelp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonHelp.Name = "ToolStripButtonHelp"
        Me.ToolStripButtonHelp.Size = New System.Drawing.Size(36, 39)
        Me.ToolStripButtonHelp.Text = "ToolStripButton1"
        Me.ToolStripButtonHelp.ToolTipText = "ヘルプ"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 413)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(512, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'PanelCenter
        '
        Me.PanelCenter.Controls.Add(Me.PanelPictureBox)
        Me.PanelCenter.Controls.Add(Me.TextBox1)
        Me.PanelCenter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelCenter.Location = New System.Drawing.Point(0, 42)
        Me.PanelCenter.Name = "PanelCenter"
        Me.PanelCenter.Size = New System.Drawing.Size(512, 371)
        Me.PanelCenter.TabIndex = 2
        '
        'PanelPictureBox
        '
        Me.PanelPictureBox.AutoScroll = True
        Me.PanelPictureBox.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.PanelPictureBox.Controls.Add(Me.PictureBox1)
        Me.PanelPictureBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPictureBox.Location = New System.Drawing.Point(0, 0)
        Me.PanelPictureBox.Name = "PanelPictureBox"
        Me.PanelPictureBox.Size = New System.Drawing.Size(512, 254)
        Me.PanelPictureBox.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(521, 254)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TextBox1.Location = New System.Drawing.Point(0, 254)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(512, 117)
        Me.TextBox1.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 435)
        Me.Controls.Add(Me.PanelCenter)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "Form1"
        Me.Text = "VBOCR"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.PanelCenter.ResumeLayout(False)
        Me.PanelCenter.PerformLayout()
        Me.PanelPictureBox.ResumeLayout(False)
        Me.PanelPictureBox.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents PanelCenter As Panel
    Friend WithEvents PanelPictureBox As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ToolStripButtonOpen As ToolStripButton
    Friend WithEvents ToolStripButtonSave As ToolStripButton
    Friend WithEvents ToolStripButtonExtract As ToolStripButton
    Friend WithEvents ToolStripButtonZoomIn As ToolStripButton
    Friend WithEvents ToolStripButtonZoomOut As ToolStripButton
    Friend WithEvents ToolStripButtonRotateCCW As ToolStripButton
    Friend WithEvents ToolStripButtonRotateCW As ToolStripButton
    Friend WithEvents ToolStripComboBoxLang As ToolStripComboBox
    Friend WithEvents ToolStripButtonHelp As ToolStripButton
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
End Class
