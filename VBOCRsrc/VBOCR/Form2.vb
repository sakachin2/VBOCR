'CID:''+v166R~:#72                             update#=  199;         ''~v106I~''+v166R~
'************************************************************************************''~v106I~
'v166 2018/03/04 partial text send;consider selection start/length     ''+v166I~
'v106 2017/12/20 partially extract from image(box by mouse dragging)   ''~v106I~
'************************************************************************************''~v106I~
Public Class Form2                                                     ''~v106R~
    Private parentFrm As Form1                                         ''~v106R~
    '**************************************************                ''~v106I~
    Public Sub New(PparentFrm As Form1)                                ''~v106R~
        parentFrm = PparentFrm                                         ''~v106R~
        InitializeComponent()                                          ''~v106I~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Private Sub ToolStripButtonOK_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ToolStripButtonOK.Click ''~v106I~
        sendText()                                                     ''~v106I~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Private Sub ToolStripButtonHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonHelp.Click ''~v106I~
        showHelp()                                                     ''~v106I~
    End Sub                                                            ''~v106I~
    '*************************************************************     ''~v106I~
    Private Sub showHelp()                                             ''~v106I~
        Dim txt As String                                              ''~v106I~
        If Form1.swLangJP Then                                     ''~v106R~
            txt = My.Resources.help_Form2                              ''~v106I~
        Else                                                           ''~v106I~
            txt = My.Resources.help_Form2E                             ''~v106I~
        End If                                                         ''~v106I~
        MessageBox.Show(txt, Me.Text)                                  ''~v106I~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Public Sub setText(Ptext As String)                                ''~v106R~
        TextBox.Text = Ptext                                           ''~v106I~
        TextBox.SelectionStart = 0                                     ''~v106R~
        TextBox.SelectionLength = 0                                    ''~v106I~
        Show()                                                         ''~v106I~
    End Sub                                                            ''~v106I~
    '**************************************************                ''~v106I~
    Public Sub sendText()                                              ''~v106I~
    	Dim spos as integer=TextBox.SelectionStart                     ''+v166I~
    	Dim len as integer=TextBox.SelectionLength                     ''+v166I~
        if spos>=0 andalso len>0                                       ''+v166I~
            Dim epos As Integer = Math.Min(TextBox.Text.Length, spos + len)''+v166I~
            len =epos-spos                                             ''+v166I~
            parentFrm.receivePartialText(TextBox.Text.Substring(spos, len))''+v166I~
        Else                                                           ''+v166I~
        parentFrm.receivePartialText(TextBox.Text)                     ''~v106R~
        end if                                                         ''+v166I~
    End Sub                                                            ''~v106I~

    Private Sub TextBox_Enter(sender As Object, e As EventArgs) Handles TextBox.Enter
        Trace.W("Form2 TB Enter")                                     ''~v106R~
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs) Handles TextBox.Leave
        Trace.W("Form2 TB Leave")                                     ''~v106R~
    End Sub
End Class