'CID:''+v@@@R~:#72                             update#=  144;         ''~v@@@R~
Imports System.Drawing.Imaging
Imports System.IO ''~7522I~                                            ''~v@@@M~
Imports System.Runtime.CompilerServices                                ''~v@@@M~
Imports System.Threading.Tasks                                         ''~v@@@I~
Imports Windows.Globalization
Imports Windows.Storage
Imports Windows.Storage.Streams

Public Class Form1                                                     ''~v@@@R~

    Const FILTER_DEFAULT_IMAGE = "bmp"
    Const SCALE_INITIAL = 1.0                                            ''~v@@@I~
    Const SCALE_RATE = 0.1                                               ''~v@@@I~
    Const SCALE_LIMIT_LOW = 0.01                                       ''~v@@@I~
    Const LANG_TAG_JP = "ja"                                             ''~v@@@I~
    Private imageFileFilterIndex As Integer = 0
    Private imageFileName As String = ""
    '    Private imageFileFilter = "Image Files|*.bmp;*.jpg;*.png|All Files (*.*)|*.*"''~v@@@R~
    Private imageFileFilter as String = "Image Files|*.bmp;*.jpg;*.png;*.tiff|All Files (*.*)|*.*"''+v@@@R~
    Private image As System.Drawing.Image
    Private bmpZoom As Bitmap
    Private imageW, imageH As Integer
    Private orgBMP As Bitmap = Nothing                                   ''~v@@@I~
    Private wordBMP As Bitmap = Nothing                                ''~v@@@I~
    Private scaleNew As Double = SCALE_INITIAL                         ''~v@@@I~
    Private scrollbarH As Integer = SystemInformation.HorizontalScrollBarHeight ''~v@@@I~
    Private scrollbarW As Integer = SystemInformation.VerticalScrollBarWidth ''~v@@@I~
    Private swWordBMP As Boolean = False                                ''~v@@@I~
    Private iOCR As Cocr = Nothing                                       ''~v@@@I~
    Private cbLang As ToolStripComboBox                                ''~v@@@I~
    Private Shared statusLabel As ToolStripStatusLabel                ''~v@@@I~
    Private swLangJP, swSetText, swSaved As Boolean                        ''~v@@@R~
    '**************************************************************************************''~v@@@I~
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ''~v@@@I~
        cbLang = ToolStripComboBoxLang                                   ''~v@@@I~
        statusLabel = ToolStripStatusLabel1                              ''~v@@@I~
        Trace.fsOpen("W:\testvbwpfocr.trc")                            ''~v@@@M~
        Trace.swTrace = True                                           ''~v@@@M~
        setIsLangJP()                                                  ''~v@@@I~
        iOCR = New Cocr()                                              ''~v@@@R~
        setupComboBoxLang()                                            ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************************************************''~v@@@I~
    Private Sub Form1_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing ''~v@@@I~
        If onClose(e) Then      'Continue close; may set cancel to e        ''~v@@@I~
            Trace.fsClose()                                            ''~v@@@I~
        End If                                                         ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************
    Private Sub ToolStripButtonOpen_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonOpen.Click ''~v@@@R~
        openFileDialog_Image()
    End Sub
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonExtract_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonExtract.Click ''~v@@@R~
        extractText()
    End Sub
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonRotateCCW_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonRotateCCW.Click ''~v@@@R~
        rotate(-1)                                                     ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonRotateCW_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonRotateCW.Click ''~v@@@I~
        rotate(1)                                                      ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonZoomIn_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonZoomIn.Click ''~v@@@I~
        drawZoom(1)                                                    ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonZoomOut_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonZoomOut.Click ''~v@@@I~
        drawZoom(-1)                                                   ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonHelp.Click ''~v@@@I~
        showHelp()                                                     ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub ToolStripButtonSave_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButtonSave.Click ''~v@@@I~
        saveExtracted()                                                ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub setIsLangJP()                                          ''~v@@@I~
        Dim defaulttag As String = Language.CurrentInputMethodLanguageTag ''~v@@@I~
        swLangJP = defaulttag.CompareTo(LANG_TAG_JP) = 0                    ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub setupComboBoxLang()                                    ''~v@@@I~
        iOCR.setupComboBoxLang(cbLang)                                 ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Function getSelectedLangTag() As String                    ''~v@@@I~
        Return iOCR.getSelectedLangTag(cbLang)                         ''~v@@@I~
    End Function                                                            ''~v@@@I~
    '*************************************************************
    Private Sub openFileDialog_Image()
        OpenFileDialog1.Filter = imageFileFilter
        OpenFileDialog1.FileName = imageFileName
        OpenFileDialog1.AddExtension = True   'add extension if missing
        OpenFileDialog1.DefaultExt = FILTER_DEFAULT_IMAGE
        OpenFileDialog1.FilterIndex = imageFileFilterIndex
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Dim fnm As String = OpenFileDialog1.FileName
            '           insertMRUList(1, fnm)      '1:imagefile
            Dim basename As String = System.IO.Path.GetFileNameWithoutExtension(fnm)
            imageFileName = basename
            '           kanjiFilename = basename
            imageFileFilterIndex = OpenFileDialog1.FilterIndex    'save for next open
            openImageBox(fnm)
        End If
    End Sub
    '*************************************************************
    Private Sub openImageBox(Pfnm As String)
        imageFileName = Pfnm
        Me.Text = Pfnm                                                  ''~v@@@R~
        Dim newbmp = System.Drawing.Image.FromFile(Pfnm)                ''~v@@@R~
        saveOrgBMP(newbmp)                                             ''~v@@@I~
        scaleNew = adjustScale(newbmp, scaleNew)                          ''~v@@@I~
        drawZoom(newbmp, scaleNew)                                      ''~v@@@I~
    End Sub
    '*************************************************************     ''~v@@@I~
    Private Sub extractText()                                          ''~v@@@R~
        Dim langTag As String = iOCR.getSelectedLangTag(cbLang)        ''~v@@@I~
        Try                                                            ''~v@@@I~
            Dim xText As String = ""                                        ''~v@@@I~
            Dim swOK = iOCR.extractText(imageFileName, orgBMP, langTag, xText)     ''~v@@@R~
            setText(xText)                                             ''~v@@@I~
            If swOK Then                                                    ''~v@@@I~
                showWords()                                      ''~v@@@R~
                swSaved = False                                          ''~v@@@I~
            End If                                                     ''~v@@@I~
            swSetText = swOK                                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("extractText exception:" & ex.Message)     ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub
    '*************************************************************     ''~v@@@I~
    Private Sub setText(Ptext As String)                               ''~v@@@I~
        TextBox1.Text = Ptext                                          ''~v@@@I~
        TextBox1.SelectionLength = 0                                     ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Function Index2NonIndex(Psrc As Bitmap) As Bitmap          ''~v@@@I~
        Dim r As New Rectangle(0, 0, Psrc.Width, Psrc.Height)             ''~v@@@I~
        Dim bmpNonIndexed = Psrc.Clone(r, Imaging.PixelFormat.Format32bppArgb) ''~v@@@I~
        Dim fmt As Imaging.PixelFormat = bmpNonIndexed.PixelFormat     ''~v@@@I~
        Return bmpNonIndexed                                           ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub showWords()                        ''~v@@@R~
        '** avoid exceotion:Indexed Pixel at Graphics.FromImage for mono color image''~v@@@I~
        Try                                                            ''~v@@@I~
            '********************                                      ''~v@@@I~
            Dim g As Graphics                                          ''~v@@@I~
            Dim bmpDraw As Bitmap                                      ''~v@@@I~
            Try   ' chk indexd pixcel format                           ''~v@@@R~
                g = Graphics.FromImage(orgBMP)                         ''~v@@@R~
                bmpDraw = orgBMP.Clone()     'Not Indexed Pixel format,draw to clone''~v@@@I~
            Catch ex As Exception                                          ''~v@@@I~
                bmpDraw = Index2NonIndex(orgBMP)                       ''~v@@@R~
            End Try                                                        ''~v@@@I~
            If Not iOCR.markWords(bmpDraw) Then                              ''~v@@@I~
                Exit Sub                                               ''~v@@@I~
            End If                                                     ''~v@@@I~
            saveWordBMP(bmpDraw)                                       ''~v@@@I~
            If drawZoom(bmpDraw, scaleNew) Then                              ''~v@@@R~
                bmpDraw.Dispose() 'clone was cleaated by zoom env      ''~v@@@I~
            End If                                                     ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("ShowWords :" & ex.Message)                ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub rotate(Pdest As Integer)                               ''~v@@@R~
        ' Pdest: 1:ClockWise, -1:CounterClockWise                          ''~v@@@I~
        Try                                                            ''~v@@@I~
            '** rotate image                                               ''~v@@@I~
            Dim rt As System.Drawing.RotateFlipType                    ''~v@@@I~
            Dim bmp As Bitmap = orgBMP                                 ''~v@@@R~
            If Pdest > 0 Then                                                ''~v@@@I~
                rt = RotateFlipType.Rotate90FlipNone                       ''~v@@@I~
            Else                                                       ''~v@@@I~
                rt = RotateFlipType.Rotate270FlipNone                      ''~v@@@I~
            End If                                                     ''~v@@@I~
            Dim ww, hh As Integer                                       ''~v@@@I~
            ww = bmp.Width                                             ''~v@@@R~
            hh = bmp.Height                                            ''~v@@@R~
            Dim bitmapZoom As Bitmap = New Bitmap(bmp, ww, hh)         ''~v@@@R~
            bitmapZoom.RotateFlip(rt)                                  ''~v@@@I~
            saveOrgBMP(bitmapZoom)                                     ''~v@@@I~
            If drawZoom(bitmapZoom, scaleNew) Then                           ''~v@@@I~
                bitmapZoom.Dispose()                                   ''~v@@@I~
            End If                                                     ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("Rotate :" & ex.Message)                   ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub saveOrgBMP(Pbmp As Bitmap)                             ''~v@@@I~
        Dim oldbmp = orgBMP                                              ''~v@@@I~
        orgBMP = Pbmp.Clone()                                          ''~v@@@I~
        If oldbmp IsNot Nothing Then                                        ''~v@@@I~
            oldbmp.Dispose()                                           ''~v@@@I~
        End If                                                         ''~v@@@I~
        swWordBMP = False    'zoom use orgBMP                            ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub saveWordBMP(Pbmp As Bitmap)                            ''~v@@@I~
        Dim oldbmp = wordBMP                                             ''~v@@@I~
        wordBMP = Pbmp.Clone()                                         ''~v@@@I~
        If oldbmp IsNot Nothing Then                                        ''~v@@@I~
            oldbmp.Dispose()                                           ''~v@@@I~
        End If                                                         ''~v@@@I~
        swWordBMP = True    'zoom use wordBMP                            ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub drawZoom(Pzoom As Integer)                             ''~v@@@R~
        '**Pzoom 1:zoomin(enlarge) , -1,zoomout                        ''~v@@@R~
        Dim zoomBMP As Bitmap                                          ''~v@@@I~
        If swWordBMP Then                                                   ''~v@@@I~
            zoomBMP = wordBMP                                            ''~v@@@I~
        Else                                                           ''~v@@@I~
            zoomBMP = orgBMP                                             ''~v@@@I~
        End If                                                         ''~v@@@I~
        Try                                                            ''~v@@@I~
            Dim scaleNext As Double                     ''~v@@@I~
            Dim hh, ww As Integer                                      ''~v@@@I~
            If Pzoom <> 0 Then                                               ''~v@@@I~
#If False Then                                                              ''~v@@@I~
                scaleNext = scaleNew + SCALE_RATE * Pzoom              ''~v@@@R~
#Else                                                                  ''~v@@@I~
                scaleNext = scaleNew * (1.0 + SCALE_RATE * Pzoom) 'small rate when zoomout(-1)''~v@@@R~
#End If                                                                ''~v@@@I~
            Else                                                       ''~v@@@I~
                scaleNext = scaleNew                                   ''~v@@@I~
            End If                                                     ''~v@@@I~
            ''~v@@@I~
            scaleNext = adjustScale(zoomBMP, Pzoom, scaleNext, scaleNew) ''~v@@@R~
            hh = orgBMP.Height * scaleNext                             ''~v@@@R~
            ww = orgBMP.Width * scaleNext                              ''~v@@@R~
            If scaleNext < SCALE_LIMIT_LOW Then                        ''~v@@@M~
                Exit Sub                                               ''~v@@@M~
            End If                                                     ''~v@@@M~
            Dim bitmapZoom = New Bitmap(zoomBMP, ww, hh)               ''~v@@@R~
            scaleNew = scaleNext                                       ''~v@@@I~
            setPictureBoxImage(bitmapZoom)                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("Zoom I/Out:" & ex.Message)                ''~v@@@R~
        End Try                                                        ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Function drawZoom(Pbitmap As Bitmap, Pzoom As Double) As Boolean ''~v@@@R~
        If Pzoom = SCALE_INITIAL Then                                        ''~v@@@I~
            setPictureBoxImage(Pbitmap)                                ''~v@@@R~
            Return False     'Dont dispose                             ''~v@@@R~
        End If                                                          ''~v@@@I~
        Try
            Dim bitmapZoom As Bitmap                                   ''~v@@@I~
            Dim hh, ww As Integer ''~v@@@I~
            hh = orgBMP.Height * Pzoom                                 ''~v@@@I~
            ww = orgBMP.Width * Pzoom                                  ''~v@@@I~
            bitmapZoom = New Bitmap(Pbitmap, ww, hh)                   ''~v@@@R~
            setPictureBoxImage(bitmapZoom)                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("Zoom by Rate:" & ex.Message)              ''~v@@@I~
            Return False     'Dont dispose                             ''~v@@@I~
        End Try                                                        ''~v@@@I~
        Return True 'Dispose parm bitmap                               ''~v@@@I~
    End Function                                                       ''~v@@@R~
    '*************************************************************     ''~v@@@I~
    Private Function adjustScale(Pbmp As Bitmap, Pzoom As Integer, PscaleNext As Double, PscaleOld As Double) As Double ''~v@@@R~
        Dim hhNew, wwNew, hhOld, wwOld As Integer                        ''~v@@@I~
        Dim hhPanel As Integer = PanelPictureBox.Height - scrollbarH              ''~v@@@I~
        Dim wwPanel As Integer = PanelPictureBox.Width - scrollbarW               ''~v@@@I~
        Dim scaleNext As Double = PscaleNext                             ''~v@@@I~
        '********                                                      ''~v@@@I~
        hhNew = orgBMP.Height * PscaleNext                             ''~v@@@R~
        wwNew = orgBMP.Width * PscaleNext                              ''~v@@@R~
        hhOld = orgBMP.Height * PscaleOld                              ''~v@@@R~
        wwOld = orgBMP.Width * PscaleOld                               ''~v@@@R~
        If Pzoom < 0 Then  'Zoom out(-)                                       ''~v@@@I~
            If hhNew < hhPanel AndAlso wwNew < wwPanel Then                ''~v@@@I~
                If hhOld >= hhPanel OrElse wwOld >= wwPanel Then 'wholly included at this zoomout''~v@@@I~
                    Dim rateH As Double = hhPanel / hhOld       'zoomout to panel            ''~v@@@I~
                    Dim rateW As Double = wwPanel / wwOld                  ''~v@@@I~
                    scaleNext = Math.Min(rateH, rateW) * PscaleOld       ''~v@@@R~
                End If                                                 ''~v@@@I~
            End If                                                     ''~v@@@I~
        End If                                                         ''~v@@@I~
        Return scaleNext                                               ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    '*initial scale to fit all in panel                                ''~v@@@I~
    Private Function adjustScale(Pbmp As Bitmap, PscaleOld As Double) As Double ''~v@@@I~
        Dim hhOld, wwOld As Integer                      ''~v@@@I~
        Dim hhPanel As Integer = PanelPictureBox.Height - scrollbarH   ''~v@@@I~
        Dim wwPanel As Integer = PanelPictureBox.Width - scrollbarW    ''~v@@@I~
        Dim scaleNext As Double = PscaleOld                            ''~v@@@I~
        Dim rateH, rateW As Double                                      ''~v@@@I~
        '********                                                      ''~v@@@I~
        hhOld = Pbmp.Height * PscaleOld                                ''~v@@@I~
        wwOld = Pbmp.Width * PscaleOld                                 ''~v@@@I~
        If hhOld > hhPanel OrElse wwOld > wwPanel Then 'overflow panel size''~v@@@I~
            rateH = hhPanel / hhOld       'shrink rate                  ''~v@@@I~
            rateW = wwPanel / wwOld                                     ''~v@@@I~
            scaleNext = Math.Min(rateH, rateW) * PscaleOld             ''~v@@@I~
        ElseIf hhOld < hhPanel OrElse wwOld < wwPanel Then 'wholely in panel''~v@@@I~
            rateH = hhPanel / hhOld       'shrink rate                  ''~v@@@I~
            rateW = wwPanel / wwOld                                     ''~v@@@I~
            scaleNext = Math.Min(rateH, rateW) * PscaleOld             ''~v@@@I~
        End If                                                         ''~v@@@I~
        Return scaleNext                                               ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub setPictureBoxImage(Pbmp As Bitmap)                     ''~v@@@I~
        Dim oldbmp = PictureBox1.Image                                   ''~v@@@I~
        PictureBox1.Image = Pbmp                                       ''~v@@@I~
        If oldbmp IsNot Nothing Then                                       ''~v@@@I~
            Try                                                        ''~v@@@I~
                oldbmp.Dispose()                                       ''~v@@@R~
            Catch ex As Exception                                      ''~v@@@I~
                MessageBox.Show("setPictureBoxImage dispose :" & ex.Message) ''~v@@@I~
            End Try                                                    ''~v@@@I~
            ''~v@@@I~
        End If                                                         ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************
    Public Shared Sub showStatus(Pmsg As String)                       ''~v@@@I~
        statusLabel.Text = Pmsg                                          ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub saveExtracted()                                        ''~v@@@I~
        If Not swSetText Then                                               ''~v@@@I~
            showStatus("No extracted text")                            ''~v@@@I~
            Return                                                     ''~v@@@I~
        End If                                                         ''~v@@@I~
        Dim dlg = SaveFileDialog1                                      ''~v@@@I~
        If dlg.ShowDialog() <> DialogResult.OK Then                    ''~v@@@I~
            showStatus("Save canceled")                                ''~v@@@I~
            Return                                                     ''~v@@@I~
        End If                                                         ''~v@@@I~
        Dim fnm As String = dlg.FileName                               ''~v@@@I~
        saveFile(fnm)                                                  ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub saveFile(Pfnm As String)                               ''~v@@@I~
        Dim txt = TextBox1.Text                                         ''~v@@@I~
        Try                                                            ''~v@@@I~
            System.IO.File.WriteAllText(Pfnm, txt, System.Text.Encoding.Default) ''~v@@@I~
            showStatus("Saved : " & Pfnm)                              ''~v@@@I~
            swSaved = True                                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus("Saved(" & Pfnm & ") failed by " & ex.Message)  ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************************************************''~v@@@I~
    Private Function onClose(e As System.ComponentModel.CancelEventArgs) As Boolean ''~v@@@I~
        '* return true :continue close                                     ''~v@@@I~
        If Not (swSetText AndAlso Not swSaved) Then                         ''~v@@@I~
            Return True                                                ''~v@@@I~
        End If                                                         ''~v@@@I~
        Dim rc As Boolean = confirmDiscard(Me.Text)                                   ''~v@@@I~
        If Not rc Then 'not continue Close                                  ''~v@@@I~
            e.Cancel = True                                        ''~v@@@I~
        End If                                                         ''~v@@@I~
        Return rc                                                      ''~v@@@I~
    End Function                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function confirmDiscard(Ptitle As String) As Boolean        ''~v@@@I~
        '* Ptitle:filename is set                                          ''~v@@@I~
        ' rc:true=continue close                                       ''~v@@@I~
        Dim rc As Boolean = MessageBox.Show("Discard extracted Text?", Ptitle, MessageBoxButtons.YesNo) = DialogResult.Yes ''~v@@@R~
        If Not rc Then 'not discard OK                                 ''~v@@@I~
            showStatus("Discard canceled")
        End If ''~v@@@I~
        Return rc                                                      ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub showHelp()                                             ''~v@@@I~
        Dim txt As String                                              ''~v@@@I~
        If swLangJP Then                                                    ''~v@@@I~
            txt = My.Resources.help_Form1                              ''~v@@@I~
        Else                                                           ''~v@@@I~
            txt = My.Resources.help_Form1E                             ''~v@@@I~
        End If                                                         ''~v@@@I~
        MessageBox.Show(txt, Me.Text)                                  ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Shared Sub NotFound(Pfnm As String)
        showStatus("ファイル（" & Pfnm & "）がみつかりません")         ''~v@@@R~
    End Sub
    Public Shared Sub ReadError(Pfnm As String, Pex As Exception)
        showStatus("ファイル（" & Pfnm & "）が読めません:" & Pex.Message) ''~v@@@R~
    End Sub
    Public Shared Sub ExtractError(Pfnm As String, Pex As Exception)    ''~v@@@I~
        showStatus("ファイル（" & Pfnm & "）がテキスト抽出エラー:" & Pex.Message) ''~v@@@R~
    End Sub                                                            ''~v@@@I~
End Class
