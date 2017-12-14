﻿'CID:''+dateR~:#72                          update#= 13;              ''~7619R~
Imports System.IO                                                      ''~7619I~
Public Class Trace                                                     ''~7619R~
    Public Shared swTrace As Boolean = false                           ''~7619R~
    Private Shared fs As System.IO.TextWriter=Nothing                          ''~7619I~''+7C13R~
    Private Shared fnm As String                                       ''~7619I~
    Public Shared Sub W(Ptext As String)                               ''~7619R~
#if DEBUG                                                              ''+7C13I~
    	if not swTrace                                                 ''~7619I~
        	exit sub                                                   ''~7619I~
        end if                                                         ''~7619I~
        Try                                                            ''~7619I~
            If isNothing(fs) Then                                               ''~7619I~
                fsOpen()                                                   ''~7619I~
            End If                                                         ''~7619I~
            fsWrite(Ptext)                                                 ''~7619I~
        Catch ex As IOException                                        ''~7619I~
            MessageBox.Show(ex.ToString, "Trace")                            ''~7619I~
        End Try                                                        ''~7619I~
#end if                                                                ''+7C13I~
    End Sub                                                            ''~7619R~
    Public Shared Sub fsOpen(Pfnm As String)                           ''~7619I~
#if DEBUG                                                              ''~v068I~''+7C13I~
        fs = New StreamWriter(Pfnm)                                      ''~7619I~
#end if                                                                ''+7C13I~
    End Sub                                                            ''~7619I~
    Private Shared Sub fsOpen()                                        ''~7619M~
        fnm = ".\trace.txt"                                            ''~7619R~
        fs = New StreamWriter(fnm)                                       ''~7619I~
    End Sub                                                            ''~7619M~
    Public Shared Sub fsClose()                                        ''~7619I~
#if DEBUG                                                              ''+7C13I~
        If Not isNothing(fs) Then                                      ''~7619R~
            fs.close()                                                 ''~7619I~
            fs=Nothing                                                 ''~7619I~
        End If                                                         ''~7619I~
#end if                                                                ''+7C13I~
    End Sub                                                            ''~7619I~
    Private Shared Sub fsWrite(Ptext As String)                        ''~7619I~
        If Not isNothing(fs) Then                                      ''~7619I~
        fs.WriteLine(Ptext)                                            ''~7619I~
        fs.Flush()                                                     ''~7619I~
        End If                                                         ''~7619I~
    End Sub                                                            ''~7619I~
End Class
