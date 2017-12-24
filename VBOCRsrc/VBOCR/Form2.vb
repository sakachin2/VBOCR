'CID:''+v106R~:#72                             update#=  196;         ''~v106I~
'************************************************************************************''~v106I~
'v106 2017/12/20 partially extract from image(box by mouse dragging)   ''+v106I~
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
        parentFrm.receivePartialText(TextBox.Text)                     ''~v106R~
    End sub                                                            ''~v106I~
End Class