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
        Me.ToolStripButtonSaveBMP = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonHelp = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripTextBoxCulture = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripButtonApplyCulture = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PanelCenter = New System.Windows.Forms.Panel()
        Me.PanelPictureBox = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.SaveFileDialogImage = New System.Windows.Forms.SaveFileDialog()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.PanelCenter.SuspendLayout()
        Me.PanelPictureBox.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonOpen, Me.ToolStripButtonSave, Me.ToolStripButtonZoomIn, Me.ToolStripButtonZoomOut, Me.ToolStripButtonRotateCCW, Me.ToolStripButtonRotateCW, Me.ToolStripComboBoxLang, Me.ToolStripButtonExtract, Me.ToolStripButtonSaveBMP, Me.ToolStripButtonHelp, Me.ToolStripTextBoxCulture, Me.ToolStripButtonApplyCulture})
        resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
        Me.ToolStrip1.Name = "ToolStrip1"
        '
        'ToolStripButtonOpen
        '
        Me.ToolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonOpen, "ToolStripButtonOpen")
        Me.ToolStripButtonOpen.Name = "ToolStripButtonOpen"
        '
        'ToolStripButtonSave
        '
        Me.ToolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonSave, "ToolStripButtonSave")
        Me.ToolStripButtonSave.Name = "ToolStripButtonSave"
        '
        'ToolStripButtonZoomIn
        '
        Me.ToolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonZoomIn, "ToolStripButtonZoomIn")
        Me.ToolStripButtonZoomIn.Name = "ToolStripButtonZoomIn"
        '
        'ToolStripButtonZoomOut
        '
        Me.ToolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonZoomOut, "ToolStripButtonZoomOut")
        Me.ToolStripButtonZoomOut.Name = "ToolStripButtonZoomOut"
        '
        'ToolStripButtonRotateCCW
        '
        Me.ToolStripButtonRotateCCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonRotateCCW, "ToolStripButtonRotateCCW")
        Me.ToolStripButtonRotateCCW.Name = "ToolStripButtonRotateCCW"
        '
        'ToolStripButtonRotateCW
        '
        Me.ToolStripButtonRotateCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonRotateCW, "ToolStripButtonRotateCW")
        Me.ToolStripButtonRotateCW.Name = "ToolStripButtonRotateCW"
        '
        'ToolStripComboBoxLang
        '
        Me.ToolStripComboBoxLang.Name = "ToolStripComboBoxLang"
        resources.ApplyResources(Me.ToolStripComboBoxLang, "ToolStripComboBoxLang")
        '
        'ToolStripButtonExtract
        '
        Me.ToolStripButtonExtract.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonExtract, "ToolStripButtonExtract")
        Me.ToolStripButtonExtract.Name = "ToolStripButtonExtract"
        '
        'ToolStripButtonSaveBMP
        '
        Me.ToolStripButtonSaveBMP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonSaveBMP, "ToolStripButtonSaveBMP")
        Me.ToolStripButtonSaveBMP.Name = "ToolStripButtonSaveBMP"
        '
        'ToolStripButtonHelp
        '
        Me.ToolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.ToolStripButtonHelp, "ToolStripButtonHelp")
        Me.ToolStripButtonHelp.Name = "ToolStripButtonHelp"
        '
        'ToolStripTextBoxCulture
        '
        Me.ToolStripTextBoxCulture.Name = "ToolStripTextBoxCulture"
        resources.ApplyResources(Me.ToolStripTextBoxCulture, "ToolStripTextBoxCulture")
        '
        'ToolStripButtonApplyCulture
        '
        Me.ToolStripButtonApplyCulture.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.ToolStripButtonApplyCulture, "ToolStripButtonApplyCulture")
        Me.ToolStripButtonApplyCulture.Name = "ToolStripButtonApplyCulture"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        resources.ApplyResources(Me.ToolStripStatusLabel1, "ToolStripStatusLabel1")
        '
        'PanelCenter
        '
        Me.PanelCenter.Controls.Add(Me.PanelPictureBox)
        Me.PanelCenter.Controls.Add(Me.TextBox1)
        resources.ApplyResources(Me.PanelCenter, "PanelCenter")
        Me.PanelCenter.Name = "PanelCenter"
        '
        'PanelPictureBox
        '
        resources.ApplyResources(Me.PanelPictureBox, "PanelPictureBox")
        Me.PanelPictureBox.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.PanelPictureBox.Controls.Add(Me.PictureBox1)
        Me.PanelPictureBox.Name = "PanelPictureBox"
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'TextBox1
        '
        resources.ApplyResources(Me.TextBox1, "TextBox1")
        Me.TextBox1.Name = "TextBox1"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Form1
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = Global.VBOCR.My.MySettings.Default.AS_Form1ClientSize
        Me.Controls.Add(Me.PanelCenter)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("ClientSize", Global.VBOCR.My.MySettings.Default, "AS_Form1ClientSize", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Name = "Form1"
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
    Friend WithEvents ToolStripTextBoxCulture As ToolStripTextBox
    Friend WithEvents ToolStripButtonApplyCulture As ToolStripButton
    Friend WithEvents ToolStripButtonSaveBMP As ToolStripButton
    Friend WithEvents SaveFileDialogImage As SaveFileDialog
End Class
