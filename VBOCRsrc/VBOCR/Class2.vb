'CID:''+v139R~:#72                             update#=  168;         ''+v139R~
'************************************************************************************''~v106I~
'v139 2018/01/03 markWord position invalid when cliprect               ''+v139I~
'va05 2017/12/26 ext name Jpeg-->jpg,icon-->ico,tiff->tif              ''~va05I~
'va04 2017/12/25 save cut image to file                                ''~va04I~
'v110 2017/12/22 Test change of resource culture                       ''~v110I~
'v109 2017/12/21 save setting of Selected Language                     ''~v109I~
'v106 2017/12/20 partially extract from image(box by mouse dragging)   ''~v106I~
'************************************************************************************''~v106I~
Imports System.Drawing.Imaging                                         ''~v@@@I~
Imports System.IO                                                      ''~v@@@I~
Imports System.Runtime.CompilerServices                                ''~v@@@I~
Imports System.Threading.Tasks                                         ''~v@@@I~
Imports System.Text                                                    ''~v@@@I~
Imports Windows.Foundation                                             ''~v@@@I~
Imports Windows.Globalization                                          ''~v@@@I~
Imports Windows.Graphics.Imaging                                       ''~v@@@I~
Imports Windows.Media.Ocr                                              ''~v@@@I~
Imports Windows.Storage                                                ''~v@@@I~
Imports Windows.Storage.Pickers                                        ''~v@@@I~
Imports Windows.Storage.Streams                                        ''~v@@@I~
''~v@@@I~
Public Class Cocr                                                      ''~v@@@R~
    ''~v@@@I~
    Private Const CNM = "Cocr:"                                          ''~v@@@I~
    Private softBitmap As SoftwareBitmap                               ''~v@@@I~
    Dim tbLang As New DataTable()                                      ''~v@@@I~
    Public Const LANG_TAG = "tag"                                      ''~v@@@R~
    Public Const LANG_NAME = "name"                                    ''~v@@@R~
    Private imageFilename As String                                    ''~v@@@I~
    Private xText As String                                            ''~v@@@I~
    Private fileBMP As Bitmap                                          ''~v@@@I~
    Private result As OcrResult
    Private swOK As Boolean ''~v@@@I~
    Private clipRect As Rectangle                                       ''~v106I~''~v109R~
    Private swRectBMP As Boolean                                       ''~v106I~
    Private bmpRect As Bitmap                                          ''~v106I~
    Private scaleNew As Double                                         ''~v106I~
    '**************************************************************************************''~v@@@I~
    Public Sub New()                                                   ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************************************************''~v@@@I~
    Public Function setupComboBoxLang(Pcb As ToolStripComboBox, Pidxcfg As Integer) As Integer ''~v109I~
        Dim cb As ToolStripComboBox = Pcb                              ''~v@@@R~
        Dim defaulttag As String = Language.CurrentInputMethodLanguageTag ''~v@@@I~
        setupDataTableLang() 'setup tbLang                             ''~v@@@I~
        cb.ComboBox.DataSource = tbLang                                ''~v@@@I~
        cb.ComboBox.DisplayMember = LANG_NAME                          ''~v@@@I~
        cb.ComboBox.ValueMember = LANG_TAG                             ''~v@@@I~
        '       cb.Text = "Language"                                           ''~v@@@R~
        Dim idx As Integer = 0                                         ''~v109I~
        For Each row As DataRow In tbLang.Rows                         ''~v@@@R~
            Dim tag As String = CType(row(LANG_TAG), String)                          ''~v@@@I~
            If (Pidxcfg < 0) Then ' Not selected Then initially        ''~v109I~
                If tag.CompareTo(defaulttag) = 0 Then                  ''~v109I~
                    cb.Text = CType(row(LANG_NAME), String)            ''~v109I~
                    cb.SelectedIndex = idx                             ''~v109I~
                    ''~v109I~
                End If                                                 ''~v109I~
            Else                                                       ''~v109I~
                If idx = Pidxcfg Then    'previously selected          ''~v109I~
                    cb.Text = CType(row(LANG_NAME), String)            ''~v109I~
                    cb.SelectedIndex = idx                             ''~v109I~
                End If                                                 ''~v109I~
            End If                                                     ''~v109I~
            idx += 1                                                   ''~v109I~
        Next                                                           ''~v@@@I~
        Return cb.SelectedIndex                                        ''~v109I~
    End Function                                                          ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub setupDataTableLang()                                   ''~v@@@I~
        Dim tb As New DataTable()                                      ''~v@@@I~
        tb.Columns.Add(LANG_TAG, GetType(String))                      ''~v@@@I~
        tb.Columns.Add(LANG_NAME, GetType(String))                     ''~v@@@I~
        Dim langlist = OcrEngine.AvailableRecognizerLanguages          ''~v@@@I~
        For Each item As Language In langlist                          ''~v@@@I~
            Dim row As DataRow = tb.NewRow()                           ''~v@@@I~
            row(LANG_TAG) = item.LanguageTag                           ''~v@@@I~
            If Form1.swLangJP Then                                            ''~v110I~
                row(LANG_NAME) = item.DisplayName                          ''~v@@@I~
            Else                                                         ''~v110I~
                row(LANG_NAME) = item.NativeName                           ''~v110I~
            End If                                                       ''~v110I~
            tb.Rows.Add(row)                                           ''~v@@@I~
        Next                                                           ''~v@@@I~
        tb.AcceptChanges()                                             ''~v@@@I~
        tbLang = tb                                                    ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Public Function getSelectedLangTag(Pcb As ToolStripComboBox, ByRef Ppidxlang As Integer) As String ''~v@@@R~''~v109R~
        Dim cb As ToolStripComboBox = Pcb                              ''~v@@@R~
        Dim idx As Integer = cb.SelectedIndex                          ''~v@@@I~
        Dim tag As String = CType(tbLang.Rows(idx)(LANG_TAG), String)                 ''~v@@@I~
        Ppidxlang = idx                                                ''~v109I~
        Return tag                                                     ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '**************************************************                ''~v106I~
    '* set clip box info before extact                                 ''~v109I~
    Public Sub setRect(PswRectBMP As Boolean, PbmpRect As Bitmap, PscaleNew As Double, PclipRect As Rectangle) ''~v106I~''~v109R~
        swRectBMP = PswRectBMP                                           ''~v106I~
        bmpRect = PbmpRect                                               ''~v106I~
        scaleNew = PscaleNew                                             ''~v106I~
        clipRect = PclipRect                                               ''~v106I~''~v109R~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Public Function cutBMPRect(PorgBMP As Bitmap) As Bitmap            ''~v106I~
        Dim xx, yy, ww, hh As Integer                                    ''~v106I~
        xx = CType(clipRect.X / scaleNew, Integer) 'dest and src position''~v106R~''~v109R~
        yy = CType(clipRect.Y / scaleNew, Integer)                           ''~v106I~''~v109R~
        ww = CType(clipRect.Width / scaleNew, Integer)                       ''~v106I~''~v109R~
        hh = CType(clipRect.Height / scaleNew, Integer)                      ''~v106I~''~v109R~
        Dim tgtRect As Rectangle = New Rectangle(xx, yy, ww, hh)       ''~v106I~
#If False Then                                                              ''~va04I~
        Dim bmp As Bitmap = New Bitmap(PorgBMP.Width, PorgBMP.Height)    ''~v106I~
        Dim g = Graphics.FromImage(bmp)                                ''~v106I~
        Dim unit As GraphicsUnit = GraphicsUnit.Pixel                    ''~v106R~
        g.DrawImage(PorgBMP, tgtRect, xx, yy, ww, hh, unit)              ''~v106R~
        g.Dispose()                                                    ''~v106I~
#Else                                                                  ''~va04I~
        Dim bmp As Bitmap = cutImage(PorgBMP, tgtRect)                    ''~va04I~
#End If                                                                ''~va04I~
        Trace.W("cutBMPRect org W=" & PorgBMP.Width & ",H=" & PorgBMP.Height) ''~v106I~
        Trace.W("cutBMPRect clipRect X=" & clipRect.X & ",Y=" & clipRect.Y & ",W=" & clipRect.Width & ",H=" & clipRect.Height) ''~v106I~''~v109R~
        Trace.W("cutBMPRect scale=" & scaleNew)                        ''~v106I~
        Trace.W("cutBMPRect xx=" & xx & ",yy=" & yy & ",ww=" & ww & ",hh=" & hh) ''~v106I~
        Trace.W("cutBMPRect clip W=" & bmp.Width & ",H=" & bmp.Height) ''~v106I~
        '       saveImage(bmp, "W:\wd\ocrcut", ImageFormat.Png)                ''~va04R~
        Return bmp                                                     ''~v106I~
    End Function                                                           ''~v106I~
    '**************************************************                ''~va04I~
    Public Sub saveImage(Pbasename As String, Pextname As String, PswRectBMP As Boolean, PorgBMP As Bitmap, Pscale As Double, Prect As Rectangle) ''~va04R~
        Dim bmp As Bitmap                                              ''~va04I~
        If PswRectBMP Then 'clipped                                         ''~va04I~
            Dim xx, yy, ww, hh As Integer                              ''~va04R~
            xx = CType(Prect.X / Pscale, Integer) 'dest and src position''~va04R~
            yy = CType(Prect.Y / Pscale, Integer)                      ''~va04R~
            ww = CType(Prect.Width / Pscale, Integer)                  ''~va04R~
            hh = CType(Prect.Height / Pscale, Integer)                 ''~va04R~
            Dim tgtRect As Rectangle = New Rectangle(xx, yy, ww, hh)   ''~va04R~
            bmp = cutImage(PorgBMP, tgtRect)                           ''~va04R~
        Else                                                           ''~va04I~
            bmp = PorgBMP                                                ''~va04I~
        End If                                                         ''~va04I~
        Dim fmt As ImageFormat = str2Fmt(Pextname)                     ''~va04I~
        saveImage(bmp, Pbasename, fmt)                                 ''~va04R~
        If PswRectBMP Then                                                  ''~va04I~
            bmp.Dispose()                                              ''~va04I~
        End If                                                         ''~va04I~
    End Sub                                                       ''~va04I~
#If False Then                                                              ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function extractText(Pfnm As String, PfileBMP As Bitmap, Ptag As String, ByRef Pptext As String) As Boolean ''~v@@@R~
        imageFilename = Pfnm                                             ''~v@@@I~
        fileBMP = PfileBMP                                               ''~v@@@I~
        xText = ""                                                       ''~v@@@I~
        result = Nothing                                                 ''~v@@@I~
        swOK = await extractTextAsync(Pfnm, Ptag)                                    ''~v@@@I~
        Pptext = xText                                                   ''~v@@@I~
        Return swOK                                                    ''~v@@@I~
    End Function                                                            ''~v@@@I~
#Else                                                                  ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function extractText(Pfnm As String, PfileBMP As Bitmap, Ptag As String, ByRef Pptext As String) As Boolean ''~v@@@R~
        imageFilename = Pfnm                                           ''~v@@@I~
        fileBMP = PfileBMP                                             ''~v@@@I~
        If swRectBMP Then                                                   ''~v106I~
            fileBMP = cutBMPRect(fileBMP)                              ''~v106I~
        End If                                                         ''~v106I~
        xText = ""                                                     ''~v@@@I~
        result = Nothing                                               ''~v@@@I~
        Dim t As Task = Task.Run(Async Function()                                  ''~v@@@I~
                                     swOK = Await extractTextAsync(Pfnm, Ptag) ''~v@@@I~
                                 End Function)                           ''~v@@@I~
        t.Wait()                                                       ''~v@@@I~
        Pptext = xText                                                 ''~v@@@I~
        Return swOK                                                    ''~v@@@I~
    End Function                                                       ''~v@@@I~
#End If                                                                ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Async Function extractTextAsync(Pfnm As String, Ptag As String) As Task(Of Boolean)
        Try                                                            ''~v@@@I~
            softBitmap = Await LoadImage(Pfnm)                         ''~v@@@R~
            If softBitmap Is Nothing Then                              ''~v@@@I~
                Return False                                                 ''~v@@@I~
            End If                                                     ''~v@@@I~
            result = Await callOCR(Pfnm, softBitmap, Ptag)              ''~v@@@R~
            If result Is Nothing Then                                  ''~v@@@I~
                xText = "Extract failed"                                 ''~v@@@R~
                Return False                                                ''~v@@@I~
            End If                                                     ''~v@@@I~
            xText = result.Text                                        ''~v@@@R~
            xText = makeLines(xText.Length) 'insert crlf between lines  ''~v@@@I~
            swOK = True                                                  ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "extractText exception:" & ex.Message)    ''~v@@@R~
            xText = ex.Message                                         ''~v@@@I~
        End Try
        Return True ''~v@@@I~
    End Function                                                            ''~v@@@I~
    '*************************************************************     ''~v@@@M~
    Private Async Function LoadImage(Pfnm As String) As Task(Of SoftwareBitmap) ''~v@@@M~
        Dim buff As Byte() = getImageBuff()                            ''~v@@@M~
        Dim softbmp As SoftwareBitmap = Nothing                        ''~v@@@M~
        Try                                                            ''~v@@@M~
            Dim mem As MemoryStream = New MemoryStream(buff)           ''~v@@@M~
            mem.Position = 0                                           ''~v@@@M~
            Dim stream = Await ConvertToRandomAccessStream(mem)        ''~v@@@M~
            softbmp = Await LoadImage(stream)                          ''~v@@@M~
        Catch ex As Exception                                          ''~v@@@M~
            showStatus(CNM & "LoadImage file exception:" & Pfnm & ":" & ex.Message) ''~v@@@R~
        End Try                                                        ''~v@@@M~
        Return softbmp                                                 ''~v@@@M~
    End Function                                                       ''~v@@@M~
    '*************************************************************     ''~v@@@I~
    Private Function getImageBuff() As Byte()                          ''~v@@@I~
        Dim buff As Byte() = Nothing                                   ''~v@@@I~
        Try                                                            ''~v@@@I~
            Dim ms = New MemoryStream()                                ''~v@@@I~
            Dim bmp = fileBMP                                          ''~v@@@R~
            Dim fmt As ImageFormat                                     ''~v@@@I~
            fmt = ImageFormat.Bmp                                      ''~v@@@I~
            bmp.Save(ms, fmt)                                          ''~v@@@I~
            ms.Close()                                                 ''~v@@@I~
            buff = ms.ToArray()                                        ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "getImageBuff exception:" & ex.Message)   ''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return buff                                                    ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Async Function ConvertToRandomAccessStream(Pms As MemoryStream) As Task(Of IRandomAccessStream) ''~v@@@I~
        ''~v@@@I~
        Dim randomAccessStream As InMemoryRandomAccessStream = New InMemoryRandomAccessStream() ''~v@@@I~
        Dim outputStream As IOutputStream = randomAccessStream.GetOutputStreamAt(0) ''~v@@@I~
        Dim dw As DataWriter = New DataWriter(outputStream)            ''~v@@@I~
        Try                                                            ''~v@@@I~
            dw.WriteBytes(Pms.ToArray())                               ''~v@@@I~
            Dim memtask = New Task(Sub()                               ''~v@@@I~
                                       dw.WriteBytes(Pms.ToArray())    ''~v@@@I~
                                   End Sub)                            ''~v@@@I~
            memtask.Start()                                            ''~v@@@I~
            Await memtask                                              ''~v@@@I~
            Await dw.StoreAsync()                                      ''~v@@@I~
            Await outputStream.FlushAsync()                            ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "ConvertToRandomAccess:" & ex.Message)    ''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return randomAccessStream                                      ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Async Function LoadImage(Pstream As IRandomAccessStream) As Task(Of SoftwareBitmap) ''~v@@@I~
        Try                                                            ''~v@@@I~
            Dim decoder = Await BitmapDecoder.CreateAsync(Pstream)     ''~v@@@I~
            Dim softbmp = Await decoder.GetSoftwareBitmapAsync()       ''~v@@@I~
            Return softbmp                                             ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "LoadImage stream :" & ex.Message)        ''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return Nothing                                                 ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Async Function callOCR(Pfnm As String, Pbmp As SoftwareBitmap, PlangTag As String) As Task(Of OcrResult) ''~v@@@I~
        ' to support en languagepack will be set by ControlPanel       ''~v@@@I~
        Dim lng As Language = New Language(PlangTag)                   ''~v@@@I~
        Dim engine As OcrEngine = OcrEngine.TryCreateFromLanguage(lng) ''~v@@@I~
        Dim result As OcrResult = Nothing                              ''~v@@@I~
        Try                                                            ''~v@@@I~
            result = Await engine.RecognizeAsync(Pbmp)                 ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "Extract failed:" & imageFilename & ":" & ex.Message) ''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return result                                                  ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function markWords(Pbmp As Bitmap) As Boolean              ''~v@@@I~
        '** avoid exceotion:Indexed Pixel at Graphics.FromImage for mono color image''~v@@@I~
        Dim xx0 = 0, yy0 = 0                                           ''+v139I~
        Try                                                            ''~v@@@I~
            '********************                                      ''~v@@@I~
            If swRectBMP Then                                          ''+v139I~
                xx0 = CType(clipRect.X / scaleNew, Integer) 'dest and src position''+v139I~
                yy0 = CType(clipRect.Y / scaleNew, Integer)            ''+v139I~
            End If                                                     ''+v139I~
            Dim bmpDraw As Bitmap = Pbmp                                 ''~v@@@I~
            Dim g = Graphics.FromImage(bmpDraw)                            ''~v@@@I~
            Dim br As Brush = New SolidBrush(System.Drawing.Color.FromArgb(&H20, System.Drawing.Color.Blue)) ''~v@@@I~
            '           Dim text As String = ""                                    ''~v@@@R~
            For Each line As OcrLine In result.Lines                   ''~v@@@I~
                '               text += line.Text & " "                                ''~v@@@R~
                '               Trace.W("Line Text=" & line.Text)                      ''~v@@@R~
                For Each word As OcrWord In line.Words                 ''~v@@@I~
                    Dim brect As Windows.Foundation.Rect = word.BoundingRect ''~v@@@I~
                    Dim rect As Rectangle = New System.Drawing.Rectangle(CType(brect.X, Integer), CType(brect.Y, Integer), CType(brect.Width, Integer), CType(brect.Height, Integer)) ''~v@@@I~
                    '                   Trace.W("Word Text=" & word.Text & ",X=" & brect.X & ",Y=" & brect.Y & ",W=" & brect.Width & ",H=" & brect.Height)''~v@@@R~
                    rect.X += xx0                                      ''+v139I~
                    rect.Y += yy0                                      ''+v139I~
                    g.FillRectangle(br, rect)                          ''~v@@@I~
                    g.DrawRectangle(Pens.Red, rect)                    ''~v@@@I~
                    '                   text &= word.Text & " "                            ''~v@@@R~
                Next                                                   ''~v@@@I~
            Next                                                       ''~v@@@I~
            g.Dispose()                                                ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "markWords :" & ex.Message)               ''~v@@@R~
            Return False                                               ''~v@@@I~
        End Try                                                        ''~v@@@I~
        Return True                                                    ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function makeLines(Plen As Integer) As String               ''~v@@@I~
        Dim sb = New StringBuilder(Plen * 2)                             ''~v@@@I~
        Try                                                            ''~v@@@I~
            For Each line As OcrLine In result.Lines                   ''~v@@@I~
                sb.Append(line.Text)                                   ''~v@@@I~
                sb.Append(vbCrLf)                                      ''~v@@@I~
            Next                                                       ''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            showStatus(CNM & "makeLines :" & ex.Message)               ''~v@@@R~
            Return "Failed to Extract Lines"                           ''~v@@@I~
        End Try                                                        ''~v@@@I~
        Return sb.ToString()                                           ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Private Sub showStatus(Pmsg As String)                                 ''~v@@@I~
        Form1.showStatus(Pmsg)                                         ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '*************************************************************     ''~v110I~
    Public Sub saveCutImage(Pbmp As Bitmap, Prect As Rectangle, Pfnm As String, Pfmt As ImageFormat) ''~va04R~
        Dim cutbmp As Bitmap = cutImage(Pbmp, Prect)                   ''~va04R~
        saveImage(cutbmp, Pfnm, Pfmt)
    End Sub                                                            ''~va04R~
    '*************************************************************     ''~va04I~
    Public Sub saveImage(Pbmp As Bitmap, Pfnm As String, Pfmt As ImageFormat) ''~va04I~
        Dim ext As String = getImageFormat(Pfmt)                       ''~va04I~
        Pbmp.Save(Pfnm & "." & ext, Pfmt)                              ''~va04I~
    End Sub                                                            ''~va04I~
    '*************************************************************     ''~va04R~
    Public Function cutImage(Pbmp As Bitmap, Prect As Rectangle) As Bitmap ''~va04R~
        Dim bmp As Bitmap = Pbmp.Clone(Prect, Pbmp.PixelFormat)        ''~va04R~
        Return bmp                                                     ''~va04R~
    End Function                                                       ''~va04R~
    '*************************************************************     ''~va04R~
    Public Function getImageFormat(Pfmt As ImageFormat) As String      ''~va04R~
        If Pfmt.equals(ImageFormat.Jpeg) Then                          ''~va05R~
            Return "jpg"                                               ''~va05I~
        End If                                                         ''~va05I~
        If Pfmt.equals(ImageFormat.Icon) Then                          ''~va05R~
            Return "ico"                                               ''~va05I~
        End If                                                         ''~va05I~
        If Pfmt.equals(ImageFormat.Tiff) Then                          ''~va05R~
            Return "tif"                                               ''~va05I~
        End If                                                         ''~va05I~
        If Pfmt.equals(ImageFormat.Png) Then                           ''~va05R~
            Return "png"       '*lowercase                             ''~va05I~
        End If                                                         ''~va05I~
        If Pfmt.equals(ImageFormat.Bmp) Then                           ''~va05R~
            Return "bmp"       '*lowercase                             ''~va05I~
        End If                                                         ''~va05I~
        If Pfmt.equals(ImageFormat.Gif) Then                           ''~va05R~
            Return "gif"       '*lowercase                             ''~va05I~
        End If                                                         ''~va05I~
        Dim fmt As String = Pfmt.ToString()                            ''~va04R~
        Return fmt                                                     ''~va04R~
    End Function                                                       ''~va04R~
    '*************************************************************     ''~va04I~
    Public Function str2Fmt(Pstrfmt As String) As ImageFormat          ''~va04I~
        If String.Compare(Pstrfmt, "bmp", True) = 0 Then   'true:ignotre case   ''~va04I~
            Return ImageFormat.Bmp                                     ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "gif", True) = 0 Then   'true:ignotre case''~va04R~
            Return ImageFormat.Gif                                     ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "jpg", True) = 0 Then   'true:ignotre case   ''~va04I~
            Return ImageFormat.Jpeg                                   ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "jpeg", True) = 0 Then   'true:ignotre case  ''~va04I~
            Return ImageFormat.Jpeg                                   ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "png", True) = 0 Then   'true:ignotre case   ''~va04I~
            Return ImageFormat.Png                                     ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "tiff", True) = 0 Then   'true:ignotre case  ''~va04I~
            Return ImageFormat.Tiff                                    ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "tif", True) = 0 Then   'true:ignotre case   ''~va04I~
            Return ImageFormat.Tiff                                    ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "icon", True) = 0 Then   'true:ignotre case  ''~va04I~
            Return ImageFormat.Icon                                    ''~va04I~
        End If                                                         ''~va04I~
        If String.Compare(Pstrfmt, "ico", True) = 0 Then   'true:ignotre case   ''~va04I~
            Return ImageFormat.Icon                                    ''~va04I~
        End If                                                         ''~va04I~
        Return ImageFormat.Bmp                                         ''~va04I~
    End Function                                                       ''~va04I~
End Class                                                              ''~v@@@I~
'*************************************************************         ''~v@@@I~
' Windows manual                                                       ''~v@@@I~
'Public Delegate Sub AsyncOperationCompletehandler(IAsyncOperation, AsyncStatus)''~v@@@I~
'*************************************************************         ''~v@@@I~
Public Module TaskExtensionModule                                      ''~v@@@I~
    '***add to the class/Interface(1st parm of Function/Sub) extended Function/Sub with <Extension> prefix''~v@@@I~
    '*************************************************************     ''~v@@@I~
    <Extension()>                                                      ''~v@@@I~
    Public Function AsTask(Of TResult)(Poper As IAsyncOperation(Of TResult)) As Task(Of TResult) ''~v@@@I~
        Dim mTCS = New TaskCompletionSource(Of TResult)()              ''~v@@@I~
        '       Dim notifier = New DelegateNotifier(Of T)(AddressOf TaskNotifier(Of T))''~v@@@I~
        '       Poper.Completed = New AsyncOperationCompletedHandler(Of T)(Sub(Poper2, Pstatus2)''~v@@@I~
        '                                                                      TaskNotifier(Of T)(Poper2, Pstatus2)''~v@@@I~
        '                                                                  End Sub)''~v@@@I~
        Try                                                            ''~v@@@I~
            Poper.Completed = New AsyncOperationCompletedHandler(Of TResult)(Sub(Poper2, Pstatus2) ''~v@@@I~
                                                                                 TaskNotifier(Poper2, Pstatus2, mTCS) ''~v@@@I~
                                                                             End Sub)  'void AsyncOperationCompletionHandler(IAsyncOperation,AsyncStatus)''~v@@@I~
        Catch ex As Exception                                          ''~v@@@I~
            Form1.showStatus("AsTask :" & ex.Message)                        ''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return mTCS.Task                                               ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    <Extension()>                                                      ''~v@@@I~
    Public Function GetAwaiter(Of TResult)(Poper As IAsyncOperation(Of TResult)) As TaskAwaiter(Of TResult) ''~v@@@I~
        '       Return Poper.AsTask().GetAwaiter()                     ''~v@@@I~
        Dim tsk As Task(Of TResult) = Poper.AsTask()                   ''~v@@@I~
        Dim w As TaskAwaiter(Of TResult) = tsk.GetAwaiter()            ''~v@@@I~
        Return w                                                       ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '   <Extension()>                                                  ''~v@@@I~
    Public Sub TaskNotifier(Of TResult)(Poper As IAsyncOperation(Of TResult), Pstatus As AsyncStatus, PmTCS As TaskCompletionSource(Of TResult)) ''~v@@@I~
        Select Case Pstatus                                            ''~v@@@I~
            Case AsyncStatus.Completed                                 ''~v@@@I~
                PmTCS.SetResult(Poper.GetResults())                    ''~v@@@I~
                Poper.Close() 'IAsyncOperation Interface inherit IAsyncInfo, IAsyncInfo has Close() it is requires adter GetResult()''~v@@@I~
            Case AsyncStatus.Error                                     ''~v@@@I~
                PmTCS.SetException(Poper.ErrorCode) 'ErrorCode is in IAsyncInfo''~v@@@I~
            Case AsyncStatus.Canceled                                  ''~v@@@I~
                PmTCS.SetCanceled()                                    ''~v@@@I~
        End Select                                                     ''~v@@@I~
    End Sub                                                            ''~v@@@I~
End Module                                                             ''~v@@@I~
