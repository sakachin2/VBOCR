﻿'CID:''+v113R~:#72                             update#=  227;         ''~v113R~
'************************************************************************************''~v006I~''~v001I~
'v113 2017/12/22 put Zorder Top                                        ''~v113I~
'v110 2017/12/22 Test change of resource culture                       ''~v110I~
'v109 2017/12/21 save setting of Selected Language                     ''~v109I~
'v108 2017/12/21 apply zoom rate of marking to also orgBMP             ''~v108I~
'v106 2017/12/20 partially extract from image(box by mouse dragging)   ''~v106I~
'v001 2017/12/15 at implementing VBI2KWRT                              ''~v001I~
'************************************************************************************''~v001I~
Imports System.Globalization                                            ''~7613I~''~v110I~
Imports Windows.Globalization
Imports System.Windows.Forms                                            ''~v106I~
Imports System.Threading
Imports System.Configuration

Public Class Form1                                                     ''~v@@@R~

    Const FILTER_DEFAULT_IMAGE = "bmp"
    Const SCALE_INITIAL = 1.0                                            ''~v@@@I~
    Const SCALE_RATE = 0.1                                               ''~v@@@I~
    Const SCALE_LIMIT_LOW = 0.01                                       ''~v@@@I~
    Const LANG_TAG_JP = "ja"                                             ''~v@@@I~
    Private imageFileFilterIndex As Integer = 0
    Private imageFileName As String = ""
    '    Private imageFileFilter = "Image Files|*.bmp;*.jpg;*.png|All Files (*.*)|*.*"''~v@@@R~
    Private imageFileFilter As String = "Image Files|*.bmp;*.jpg;*.png;*.tiff|All Files (*.*)|*.*" ''~v@@@R~
    Private image As System.Drawing.Image
    Private bmpZoom As Bitmap
    Private bmpForRect As Bitmap                                       ''~v106I~
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
    Public Shared swLangJP As Boolean                        ''~v@@@R~        ''~v106R~''~v110R~
    Private swSetText, swSaved As Boolean                              ''~v106I~
    Private iIC As ImageCut = Nothing                                  ''~v106I~
    Private swSaveRectImage As Boolean                                 ''~v106I~
    Private swRectBMP As Boolean 'PictureBox.Image is rect drawn,partially extract in the rect of the image''~v106I~
    Private clipRect As Rectangle 'box on PictureBox image             ''~v106R~
    Private idxLang As Integer                                     ''~v109I~''~v106R~
    Private Shared formClip As Form2                                   ''~v106I~
    Private swInitialized As Boolean = False                             ''~v110I~
    '**************************************************************************************''~v@@@I~
    Public Sub New()       'from Main.vb                               ''~v110R~
#If DEBUG Then                                                              ''~v110I~
        setCulture()                                                   ''~v110I~
#End If                                                                ''~v110I~
        setIsLangJP()                                                  ''~v110I~
        InitializeComponent()                                          ''~v110I~
    End Sub                                                            ''~v110I~
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ''~v@@@I~''~v110R~
        '       setCulture()                                                   ''~v110R~
        disableCulture() 'enable culture setting when DEBUG for test   ''~v110I~
        cbLang = ToolStripComboBoxLang                                   ''~v@@@I~
        statusLabel = ToolStripStatusLabel1                              ''~v@@@I~
        Trace.fsOpen("W:\VBOCR.trc")                            ''~v@@@M~''~v106R~
        Trace.swTrace = True                                           ''~v@@@M~
        '       setIsLangJP()                                                  ''~v@@@I~''~v110R~
        iOCR = New Cocr()                                              ''~v@@@R~
        iIC = New ImageCut(PictureBox1)                                ''~v106I~
        idxLang = My.Settings.CFG_LangIndex                            ''~v109R~
        setupComboBoxLang()                                            ''~v@@@I~
        swInitialized = True                                             ''~v110I~
    End Sub                                                            ''~v@@@I~
    '**************************************************************************************''~v@@@I~
    Private Sub Form1_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing ''~v@@@I~
        If onClose(e) Then      'Continue close; may set cancel to e        ''~v@@@I~
            Trace.fsClose()                                            ''~v@@@I~
        End If                                                         ''~v@@@I~
        My.Settings.Save()                                             ''~v110I~
    End Sub                                                            ''~v@@@I~
    '   Private Sub Form1_Disposed(sender As System.Object, e As System.EventArgs) Handles Me.Disposed ''~v109I~''~v110R~
    '       My.Settings.CFG_LangIndex = idxLang                            ''~v109R~''~v110R~
    '   End Sub                                                            ''~v109I~''~v110R~
    '**************************************************                ''~v106I~
    Private Sub Pictureox1_MouseDown(sender As System.Object, e As MouseEventArgs) Handles PictureBox1.MouseDown ''~v106R~
        PBmouseDown(e)                                                 ''~v106R~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Private Sub Pictureox1_MouseUp(sender As System.Object, e As MouseEventArgs) Handles PictureBox1.MouseUp ''~v106R~
        PBmouseUp(e)                                                   ''~v106R~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Private Sub Pictureox1_MouseMove(sender As System.Object, e As MouseEventArgs) Handles PictureBox1.MouseMove ''~v106R~
        PBmouseMove(e)                                                 ''~v106R~
    End Sub                                                            ''~v106I~
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
    '**************************************************                ''~v110I~
    Private Sub ToolStripButtonApplyCulture_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButtonApplyCulture.Click ''~v110R~
        cultureChanged()                                               ''~v110I~
    End Sub                                                            ''~v110I~
    '**************************************************                ''~v110I~
    Private Sub ToolStripComboBoxlang_Change(sender As System.Object, e As System.EventArgs) Handles ToolStripComboBoxLang.SelectedIndexChanged ''~V110R~
        If swInitialized Then                                               ''~v110I~
            getSelectedLangTag()  'update CFG_LangIndex                    ''~v110I~
            My.Settings.CFG_LangIndex = idxLang                                ''~v110I~
        End If                                                         ''~v110I~
    End Sub                                                            ''~v110I~
    '**************************************************                ''~v@@@I~
    Private Sub setIsLangJP()                                          ''~v@@@I~
        '       Dim defaulttag As String = Language.CurrentInputMethodLanguageTag ''~v@@@I~''~v110R~
        '       swLangJP = defaulttag.CompareTo(LANG_TAG_JP) = 0                    ''~v@@@I~''~v110R~
        swLangJP = CultureInfo.CurrentCulture.Name.StartsWith(LANG_TAG_JP) ''~v110I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub setupComboBoxLang()                                    ''~v@@@I~
        idxLang = iOCR.setupComboBoxLang(cbLang, idxLang)                                 ''~v@@@R~''~v109R~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Function getSelectedLangTag() As String                    ''~v@@@I~
        Return iOCR.getSelectedLangTag(cbLang, idxLang)                         ''~v@@@I~''~v109R~''~v110R~
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
        Dim newbmp As Bitmap = DirectCast(System.Drawing.Image.FromFile(Pfnm), Bitmap)                ''~v@@@R~''~v001R~
        saveOrgBMP(newbmp)                                             ''~v@@@I~
        scaleNew = adjustScale(newbmp, scaleNew)                          ''~v@@@I~
        drawZoom(newbmp, scaleNew)                                      ''~v@@@I~
    End Sub
    '*************************************************************     ''~v@@@I~
    Private Sub extractText()                                          ''~v@@@R~
        '       Dim langTag As String = iOCR.getSelectedLangTag(cbLang)        ''~v@@@I~''~v001R~
        Dim langTag As String = getSelectedLangTag()                   ''~v001I~
        Try                                                            ''~v@@@I~
            Dim xText As String = ""                                        ''~v@@@I~
            Trace.W("Form1:extractText scaleNew=" & scaleNew)          ''~v108I~
            iOCR.setRect(swRectBMP, CType(PictureBox1.Image, Bitmap), scaleNew, clipRect) ''~v106R~
            Dim swOK = iOCR.extractText(imageFileName, orgBMP, langTag, xText)     ''~v@@@R~
            setText(swRectBMP, xText)                                             ''~v@@@I~''~v106R~
            If swOK Then                                                    ''~v@@@I~
                If xText.Length = 0 Then                                        ''~v106I~
                    '                   showStatus(Rstr.getStr("WARN_EXTRACTED_NULLSTR"))  ''~v106R~
                    showStatus(My.Resources.WARN_EXTRACTED_NULLSTR())               ''~v106I~
                Else                                                     ''~v106I~
                    showWords()                                      ''~v@@@R~
                    swSaved = False                                          ''~v@@@I~
                End If                                                   ''~v106I~
            End If                                                     ''~v@@@I~
            swSetText = swOK                                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("extractText exception:" & ex.Message)     ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub
    '*************************************************************     ''~v@@@I~
    Private Sub setText(PswClip As Boolean, Ptext As String)                               ''~v@@@I~''~v106R~
        If PswClip Then                                                      ''~v106I~
            showPartialText(Ptext)                                     ''~v106I~
        Else                                                           ''~v106I~
            TextBox1.Text = Ptext                                          ''~v@@@I~
            TextBox1.Select(0, 0)                                       ''~v106R~
        End If                                                         ''~v106I~
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
        Dim showRect As Boolean = swRectBMP   'recognize in the clipRect ''~v106I~
        Try                                                            ''~v@@@I~
            '********************                                      ''~v@@@I~
            Dim g As Graphics                                          ''~v@@@I~
            Dim bmpDraw As Bitmap                                      ''~v@@@I~
            Try   ' chk indexd pixcel format                           ''~v@@@R~
                g = Graphics.FromImage(orgBMP)                         ''~v@@@R~
                bmpDraw = DirectCast(orgBMP.Clone(), Bitmap)     'Not Indexed Pixel format,draw to clone''~v@@@I~''~v001R~
            Catch ex As Exception                                          ''~v@@@I~
                bmpDraw = Index2NonIndex(orgBMP)                       ''~v@@@R~
            End Try                                                        ''~v@@@I~
            If Not iOCR.markWords(bmpDraw) Then                              ''~v@@@I~
                Exit Sub                                               ''~v@@@I~
            End If                                                     ''~v@@@I~
            saveWordBMP(bmpDraw)                                       ''~v@@@I~
            If drawZoom(bmpDraw, scaleNew) Then                              ''~v@@@R~
                Trace.W("showWord dispose Hashcode:" & bmpDraw.GetHashCode()) ''~v108I~
                bmpDraw.Dispose() 'clone was cleaated by zoom env      ''~v@@@I~
            End If                                                     ''~v@@@I~
            If showRect Then                                           ''~v106R~
                drawClipBox(CType(PictureBox1.Image, Bitmap), clipRect)                  ''~v106I~
            End If                                                     ''~v106I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("ShowWords :" & ex.Message)                ''~v@@@I~
        End Try                                                        ''~v@@@I~
    End Sub 'showWord                                                           ''~v@@@I~''~v108R~
    '*************************************************************     ''~v106I~
    Private Sub drawClipBox(Pbmp As Bitmap, Prect As Rectangle)         ''~v106I~
        iIC.drawRect(Pbmp, Prect)                                       ''~v106I~
    End Sub                                                            ''~v106I~
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
    End Sub    'rotate                                                        ''~v@@@I~''~v108R~
    '*************************************************************     ''~v@@@I~
    Private Sub saveOrgBMP(Pbmp As Bitmap)                             ''~v@@@I~
        Dim oldbmp = orgBMP                                              ''~v@@@I~
        orgBMP = DirectCast(Pbmp.Clone(), Bitmap)                                          ''~v@@@I~''~v001R~
        If oldbmp IsNot Nothing Then                                        ''~v@@@I~
            Trace.W("saveOrgBMP dispose Hashcode:" & oldbmp.GetHashCode()) ''~v108I~
            oldbmp.Dispose()                                           ''~v@@@I~
        End If                                                         ''~v@@@I~
        swWordBMP = False    'zoom use orgBMP                            ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub saveWordBMP(Pbmp As Bitmap)                            ''~v@@@I~
        Dim oldbmp = wordBMP                                             ''~v@@@I~
        wordBMP = DirectCast(Pbmp.Clone(), Bitmap)                                         ''~v@@@I~''~v001R~
        If oldbmp IsNot Nothing Then                                        ''~v@@@I~
            Trace.W("saveWordBMP dispos Hashcodee:" & oldbmp.GetHashCode()) ''~v108I~
            oldbmp.Dispose()                                           ''~v@@@I~
        End If                                                         ''~v@@@I~
        swWordBMP = True    'zoom use wordBMP                            ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v106I~
    Private Sub saveRectBMP(Pbmp As Bitmap)                            ''~v106I~
        If swSaveRectImage Then    'called true setPictureBoxImage from setPictureBoxImageRect''~v106I~
            Return                                                     ''~v106I~
        End If                                                         ''~v106I~
        Dim oldbmp = bmpForRect                                        ''~v106I~
        bmpForRect = DirectCast(Pbmp.Clone(), Bitmap)                  ''~v106I~
        Trace.W("saveRectBMP  new bmpForRect Hashcode:" & bmpForRect.GetHashCode()) ''~v106R~''~v108R~
        If oldbmp IsNot Nothing Then                                   ''~v106I~
            Trace.W("saveWordBMP dispose Hashcode:" & oldbmp.GetHashCode()) ''~v108I~
            oldbmp.Dispose()                                           ''~v106I~
        End If                                                         ''~v106I~
    End Sub                                                            ''~v106I~
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
            hh = CType(orgBMP.Height * scaleNext, Integer)                             ''~v@@@R~''~v001R~
            ww = CType(orgBMP.Width * scaleNext, Integer)                              ''~v@@@R~''~v001R~
            If scaleNext < SCALE_LIMIT_LOW Then                        ''~v@@@M~
                Exit Sub                                               ''~v@@@M~
            End If                                                     ''~v@@@M~
            Dim bitmapZoom = New Bitmap(zoomBMP, ww, hh)               ''~v@@@R~
            scaleNew = scaleNext                                       ''~v@@@I~
            Trace.W("Form1:drawZoom scaleNew=" & scaleNew)             ''~v108I~
            setPictureBoxImage(bitmapZoom)                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            MessageBox.Show("Zoom I/Out:" & ex.Message)                ''~v@@@R~
        End Try                                                        ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Function drawZoom(Pbitmap As Bitmap, Pzoom As Double) As Boolean ''~v@@@R~
        Trace.W("Form1:drawZoom parm scaleNew=" & Pzoom)               ''~v108I~
        If Pzoom = SCALE_INITIAL Then                                        ''~v@@@I~
            setPictureBoxImage(Pbitmap)                                ''~v@@@R~
            Return False     'Dont dispose                             ''~v@@@R~
        End If                                                          ''~v@@@I~
        Try
            Dim bitmapZoom As Bitmap                                   ''~v@@@I~
            Dim hh, ww As Integer ''~v@@@I~
            hh = CType(orgBMP.Height * Pzoom, Integer)                                 ''~v@@@I~''~v001R~
            ww = CType(orgBMP.Width * Pzoom, Integer)                                  ''~v@@@I~''~v001R~
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
        '       Dim hhNew, wwNew, hhOld, wwOld As Integer                        ''~v@@@I~''~v001R~
        '       Dim hhPanel As Integer = PanelPictureBox.Height - scrollbarH              ''~v@@@I~''~v001R~
        '       Dim wwPanel As Integer = PanelPictureBox.Width - scrollbarW               ''~v@@@I~''~v001R~
        Dim hhNew, wwNew, hhOld, wwOld As Double                       ''~v001I~
        Dim hhPanel As Double = PanelPictureBox.Height - scrollbarH   ''~v001I~
        Dim wwPanel As Double = PanelPictureBox.Width - scrollbarW    ''~v001I~
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
        '       Dim hhOld, wwOld As Integer                      ''~v@@@I~     ''~v001R~
        '       Dim hhPanel As Integer = PanelPictureBox.Height - scrollbarH   ''~v@@@I~''~v001R~
        '       Dim wwPanel As Integer = PanelPictureBox.Width - scrollbarW    ''~v@@@I~''~v001R~
        Dim hhOld, wwOld As Double                                     ''~v001I~
        '       Dim hhPanel As Double = PanelPictureBox.Height - scrollbarH    ''~v001R~
        '       Dim wwPanel As Double = PanelPictureBox.Width - scrollbarW     ''~v001R~
        Dim hhPanel As Double = PanelPictureBox.Height - 1             ''~v001I~
        Dim wwPanel As Double = PanelPictureBox.Width - 1              ''~v001I~
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
        Trace.W("Form1 setPictureBoxImage scaleNew=" & scaleNew & ",swWord=" & swWordBMP) ''~v108I~
        Dim oldbmp = PictureBox1.Image                                   ''~v@@@I~
        PictureBox1.Image = Pbmp                                       ''~v@@@I~
        swRectBMP = False 'PictureBox image is not rect drawn            ''~v106I~
        '       If Not swWordBMP Then   'not zoom use wordBMP                       ''~v106I~''~v108R~
        If Not swSaveRectImage Then                                         ''~v108I~
            saveRectBMP(Pbmp)                                        ''~v106I~''~v108R~
        End If                                                         ''~v106I~
        Trace.W("setPictuBoxImage Hashcode Pbmp=" & Pbmp.GetHashCode()) ''~v108R~
        If oldbmp IsNot Nothing Then                                       ''~v@@@I~
            Try                                                        ''~v@@@I~
                Trace.W("setPictuBoxImage dispose Hashcode:" & oldbmp.GetHashCode()) ''~v108I~
                oldbmp.Dispose()                                       ''~v@@@R~
            Catch ex As Exception                                      ''~v@@@I~
                MessageBox.Show("setPictureBoxImage dispose :" & ex.Message) ''~v@@@I~
            End Try                                                    ''~v@@@I~
            ''~v@@@I~
        End If                                                         ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v106I~
    Private Sub setPictureBoxImageRect(Pbmp As Bitmap, PrestoreOrg As Boolean)                 ''~v106I~''~v108R~
        swSaveRectImage = True                                           ''~v106I~
        If PrestoreOrg Then                                                 ''~v108I~
            drawZoom(Pbmp, scaleNew)  'set clone to PictureBox.Image    ''~v108I~
        Else                                                           ''~v108I~
            setPictureBoxImage(Pbmp)                                       ''~v106I~''~v108R~
        End If                                                         ''~v108I~
        swSaveRectImage = False                                          ''~v106I~
        swRectBMP = True 'PictureBox image is rect drawn               ''~v106I~
        swWordBMP = False   'clip image is from orgBMP                   ''~v108I~
    End Sub                                                            ''~v106I~
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
    '*************************************************************     ''~v106M~
    Private Sub PBmouseDown(e As MouseEventArgs)                       ''~v106R~
        If bmpForRect Is Nothing Then                                       ''~v109I~
            Exit Sub              'not yet open image file             ''~v109I~
        End If                                                         ''~v109I~
        Dim bmprect As Bitmap = bmpForRect 'rectangle free bitmap        ''~v106I~
        Trace.W("PBmouseDown bmpForRect=" & bmprect.ToString() & ",swWordBMP=" & swWordBMP) ''~v108R~
        iIC.mouseDown(e, bmprect)                                       ''~v106I~
        setPictureBoxImageRect(bmprect, True) 'clear old box            ''~v108R~
    End Sub                                                            ''~v106M~
    '*************************************************************     ''~v106I~
    Private Sub PBmouseUp(e As MouseEventArgs)                         ''~v106R~
        Dim bmprect As Bitmap = Nothing 'output from mouseUp           ''~v109R~
        Dim rc As Boolean = iIC.mouseUp(e, bmprect, clipRect) 'get clipRect and clip drawn bmp''~v106R~
        If rc Then  'rectangle drawn                                        ''~v106I~
            setPictureBoxImageRect(bmprect, False)                            ''~v106R~''~v108R~
        End If                                                         ''~v106I~
    End Sub                                                            ''~v106I~
    '*************************************************************     ''~v106I~
    Private Sub PBmouseMove(e As MouseEventArgs)                       ''~v106R~
        Dim bmprect As Bitmap = Nothing 'output from mouseMove           ''~v106R~''~v109R~
        Dim rc As Boolean = iIC.mouseMove(e, bmprect, clipRect)        ''~v106R~
        If rc Then  'rectangle drawn                                        ''~v106I~
            setPictureBoxImageRect(bmprect, False)                            ''~v106R~''~v108R~
        End If                                                         ''~v106I~
    End Sub                                                            ''~v106I~
    '*************************************************************     ''~v106I~
    Private Sub showPartialText(Ptext As String)                       ''~v106I~
        Dim swNew AS Boolean                                           ''~v113I~
        If formClip Is Nothing OrElse formClip.IsDisposed Then         ''~v106I~
            formClip = New Form2(Me)                     ''~v106R~
        End If                                                         ''~v106I~
        formClip.setText(Ptext)                                        ''~v106I~
        if Not swNew                                                   ''~v113I~
            Form1.showTop(formClip)                                    ''~v113I~
        end if                                                         ''~v113I~
    End Sub                                                            ''~v106I~
    '*************************************************************     ''~v106I~
    Public Sub receivePartialText(Ptext As String)                   ''~v106I~
        Dim pos As Integer = TextBox1.SelectionStart                    ''~v106I~
        Dim cutlen As Integer = TextBox1.SelectionLength                   ''~v106I~
        Dim txtold As String = TextBox1.Text                             ''~v106I~
        Dim txtoldsz As Integer = txtold.Length                          ''~v106I~
        Dim txtadd As String = Ptext                                     ''~v106I~
        Dim txtnew As String                                           ''~v106I~
        pos = Math.Min(pos, txtoldsz)                                     ''~v106I~
        Dim pos2 As Integer = Math.Min(pos + cutlen, txtoldsz)              ''~v106I~
        txtnew = txtold.Substring(0, pos) & txtadd & txtold.Substring(pos2, txtoldsz - pos2) ''~v106I~
        TextBox1.Text = txtnew                                           ''~v106I~
        TextBox1.SelectionStart = pos                                    ''~v106I~
        TextBox1.SelectionLength = txtadd.Length                         ''~v106I~
        Form1.showTop(CType(Me,Form))                                  ''+v113I~
    End Sub                                                            ''~v106I~
    '*************************************************************     ''~v110I~
    Public Sub setCulture()                                            ''~v110I~
        Dim culture As CultureInfo                                     ''~v110I~
        Dim cfg As String = My.Settings.CFG_Culture 'for test culture    ''~v110I~
        If cfg.Length = 0 OrElse cfg.StartsWith(" ") Then                     ''~v110I~
            cfg = CultureInfo.CurrentCulture.Name                             ''~v110I~
            If cfg.StartsWith("ja") Then                                    ''~v110I~
                Exit Sub                                               ''~v110I~
            End If                                                     ''~v110I~
            culture = New CultureInfo("en-GB")                     ''~v110I~

        Else                                                           ''~v110I~
            culture = New CultureInfo(cfg)                             ''~v110I~
        End If                                                         ''~v110I~
        Thread.CurrentThread.CurrentCulture = culture                    ''~v110I~
        Thread.CurrentThread.CurrentUICulture = culture                ''~v110I~
    End Sub                                                            ''~v110I~
    '*************************************************************     ''~v110I~
    Public Sub disableCulture()                                        ''~v110I~
        Dim tblang As ToolStripTextBox = ToolStripTextBoxCulture         ''~v110I~
#If DEBUG Then                                                         ''~v110I~
        If My.Settings.CFG_Culture.Length = 0 Then                           ''~v110R~
            tblang.Text = CultureInfo.CurrentCulture.Name              ''~v110I~
        Else                                                           ''~v110I~
            tblang.Text = My.Settings.CFG_Culture                      ''~v110I~
        End If                                                         ''~v110I~
#Else                                                                  ''~v110I~
        tblang.Visible = False                                           ''~v110I~
        tblang.Enabled = False                                           ''~v110I~
        Dim apply As ToolStripButton = ToolStripButtonApplyCulture     ''~v110I~
        apply.Visible = False                                            ''~v110I~
        apply.Enabled = False                                            ''~v110I~
#End If                                                                ''~v110M~
    End Sub                                                            ''~v110I~
    '*************************************************************     ''~v110I~
    '* DEBUG only                                                      ''~v110I~
    Public Sub cultureChanged()                                        ''~v110I~
        Dim tblang As ToolStripTextBox = ToolStripTextBoxCulture         ''~v110I~
        Dim cfg As String = tblang.Text.Trim()                           ''~v110I~
        If cfg.Length <> 0 Then                                               ''~v110I~
            Try                                                        ''~v110I~
                Dim culture As CultureInfo                             ''~v110I~
                culture = New CultureInfo(cfg)                         ''~v110I~
            Catch ex As Exception                                      ''~v110I~
                showStatus("Culture:""" & cfg & """ is invalid culture") ''~v110R~
                Exit Sub                                               ''~v110I~
            End Try                                                    ''~v110I~
        End If                                                         ''~v110I~
        My.Settings.CFG_Culture = cfg                                    ''~v110I~
        setCulture()                                                   ''~v110I~
        If cfg.Length = 0 Then                                         ''~v110I~
            showStatus("Culture:""" & cfg & """ (Default=" & CultureInfo.CurrentCulture.Name & ") is applied, effective at restart.") ''~v110R~
        Else                                                           ''~v110I~
            showStatus("Culture:""" & cfg & """ is applied, effective at restart.") ''~v110R~
        End If                                                         ''~v110I~
    End Sub                                                            ''~v110I~
    '*************************************************************     ''~v113I~
    Public shared sub showTop(Pform as Form)                           ''~v113I~
    	Pform.BringToFront()     'ZorderTop                            ''~v113I~
    End Sub                                                            ''~v113I~
End Class
