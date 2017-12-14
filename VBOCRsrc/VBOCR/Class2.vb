'CID:''+v@@@R~:#72                             update#=  135;         ''~v@@@I~
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
    '**************************************************************************************''~v@@@I~
    Public Sub New()                                                   ''~v@@@R~
    End Sub                                                            ''~v@@@I~
    '**************************************************************************************''~v@@@I~
    Public Sub setupComboBoxLang(Pcb As ToolStripComboBox)             ''~v@@@R~
        Dim cb As ToolStripComboBox = Pcb                              ''~v@@@R~
        Dim defaulttag As String = Language.CurrentInputMethodLanguageTag ''~v@@@I~
        setupDataTableLang() 'setup tbLang                             ''~v@@@I~
        cb.ComboBox.DataSource = tbLang                                ''~v@@@I~
        cb.ComboBox.DisplayMember = LANG_NAME                          ''~v@@@I~
        cb.ComboBox.ValueMember = LANG_TAG                             ''~v@@@I~
'       cb.Text = "Language"                                           ''~v@@@R~
        For Each row as DataRow In tbLang.Rows                         ''+v@@@R~
            Dim tag As String = row(LANG_TAG)                          ''~v@@@I~
            If tag.CompareTo(defaulttag) = 0 Then                      ''~v@@@I~
                cb.Text = row(LANG_NAME)                               ''~v@@@I~
            End If                                                     ''~v@@@I~
        Next                                                           ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Private Sub setupDataTableLang()                                   ''~v@@@I~
        Dim tb As New DataTable()                                      ''~v@@@I~
        tb.Columns.Add(LANG_TAG, GetType(String))                      ''~v@@@I~
        tb.Columns.Add(LANG_NAME, GetType(String))                     ''~v@@@I~
        Dim langlist = OcrEngine.AvailableRecognizerLanguages          ''~v@@@I~
        For Each item As Language In langlist                          ''~v@@@I~
            Dim row As DataRow = tb.NewRow()                           ''~v@@@I~
            row(LANG_TAG) = item.LanguageTag                           ''~v@@@I~
            row(LANG_NAME) = item.DisplayName                          ''~v@@@I~
            tb.Rows.Add(row)                                           ''~v@@@I~
        Next                                                           ''~v@@@I~
        tb.AcceptChanges()                                             ''~v@@@I~
        tbLang = tb                                                    ''~v@@@I~
    End Sub                                                            ''~v@@@I~
    '**************************************************                ''~v@@@I~
    Public Function getSelectedLangTag(Pcb As ToolStripComboBox) As String ''~v@@@R~
        Dim cb As ToolStripComboBox = Pcb                              ''~v@@@R~
        Dim idx As Integer = cb.SelectedIndex                          ''~v@@@I~
        Dim tag As String = tbLang.Rows(idx)(LANG_TAG)                 ''~v@@@I~
        Return tag                                                     ''~v@@@I~
    End Function                                                       ''~v@@@I~
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
            showStatus(CNM & "LoadImage file exception:" & Pfnm & ":" & ex.Message)''~v@@@R~
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
            showStatus(CNM & "Extract failed:" & imageFilename & ":" & ex.Message)''~v@@@R~
        End Try                                                        ''~v@@@I~
        Return result                                                  ''~v@@@I~
    End Function                                                       ''~v@@@I~
    '*************************************************************     ''~v@@@I~
    Public Function markWords(Pbmp As Bitmap) As Boolean              ''~v@@@I~
        '** avoid exceotion:Indexed Pixel at Graphics.FromImage for mono color image''~v@@@I~
        Try                                                            ''~v@@@I~
            '********************                                      ''~v@@@I~
            Dim bmpDraw As Bitmap = Pbmp                                 ''~v@@@I~
            Dim g = Graphics.FromImage(bmpDraw)                            ''~v@@@I~
            Dim br As Brush = New SolidBrush(System.Drawing.Color.FromArgb(&H20, System.Drawing.Color.Blue)) ''~v@@@I~
            '           Dim text As String = ""                                    ''~v@@@R~
            For Each line As OcrLine In result.Lines                   ''~v@@@I~
                '               text += line.Text & " "                                ''~v@@@R~
                '               Trace.W("Line Text=" & line.Text)                      ''~v@@@R~
                For Each word As OcrWord In line.Words                 ''~v@@@I~
                    Dim brect As Windows.Foundation.Rect = word.BoundingRect ''~v@@@I~
                    Dim rect As Rectangle = New System.Drawing.Rectangle(brect.X, brect.Y, brect.Width, brect.Height) ''~v@@@I~
                    '                   Trace.W("Word Text=" & word.Text & ",X=" & brect.X & ",Y=" & brect.Y & ",W=" & brect.Width & ",H=" & brect.Height)''~v@@@R~
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
