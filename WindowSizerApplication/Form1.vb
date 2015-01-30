Imports System.Runtime.InteropServices
Imports System.Text
Public Class Form1

    Public Enum WindowStyles As Long
        WS_OVERLAPPED
        WS_POPUP = 2147483648L
        WS_CHILD = 1073741824L
        WS_MINIMIZE = 536870912L
        WS_VISIBLE = 268435456L
        WS_DISABLED = 134217728L
        WS_CLIPSIBLINGS = 67108864L
        WS_CLIPCHILDREN = 33554432L
        WS_MAXIMIZE = 16777216L
        WS_BORDER = 8388608L
        WS_DLGFRAME = 4194304L
        WS_VSCROLL = 2097152L
        WS_HSCROLL = 1048576L
        WS_SYSMENU = 524288L
        WS_THICKFRAME = 262144L
        WS_GROUP = 131072L
        WS_TABSTOP = 65536L
        WS_MINIMIZEBOX = 131072L
        WS_MAXIMIZEBOX = 65536L
        WS_CAPTION = 12582912L
        WS_TILED = 0L
        WS_ICONIC = 536870912L
        WS_SIZEBOX = 262144L
        WS_TILEDWINDOW = 13565952L
        WS_OVERLAPPEDWINDOW = 13565952L
        WS_POPUPWINDOW = 2156396544L
        WS_CHILDWINDOW = 1073741824L
        WS_EX_DLGMODALFRAME = 1L
        WS_EX_NOPARENTNOTIFY = 4L
        WS_EX_TOPMOST = 8L
        WS_EX_ACCEPTFILES = 16L
        WS_EX_TRANSPARENT = 32L
        WS_EX_MDICHILD = 64L
        WS_EX_TOOLWINDOW = 128L
        WS_EX_WINDOWEDGE = 256L
        WS_EX_CLIENTEDGE = 512L
        WS_EX_CONTEXTHELP = 1024L
        WS_EX_RIGHT = 4096L
        WS_EX_LEFT = 0L
        WS_EX_RTLREADING = 8192L
        WS_EX_LTRREADING = 0L
        WS_EX_LEFTSCROLLBAR = 16384L
        WS_EX_RIGHTSCROLLBAR = 0L
        WS_EX_CONTROLPARENT = 65536L
        WS_EX_STATICEDGE = 131072L
        WS_EX_APPWINDOW = 262144L
        WS_EX_OVERLAPPEDWINDOW = 768L
        WS_EX_PALETTEWINDOW = 392L
        WS_EX_LAYERED = 524288L
        WS_EX_NOINHERITLAYOUT = 1048576L
        WS_EX_LAYOUTRTL = 4194304L
        WS_EX_COMPOSITED = 33554432L
        WS_EX_NOACTIVATE = 67108864L
    End Enum
    Public Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Public Delegate Function EnumCallBackDelegate(hwnd As IntPtr, lparam As IntPtr) As Boolean
    Public Delegate Function EnumWindowsProc(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean

    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    Public Declare Function GetWindowThreadProcessId Lib "user32.dll" (ByVal hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    Public Declare Function GetWindowRect Lib "user32" (ByVal hwnd As IntPtr, ByRef lpRect As RECT) As Integer
    Public Declare Function SetWindowPos Lib "user32" (ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Long, ByVal X As Long, ByVal Y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
    Public Declare Auto Function SetWindowText Lib "user32" (ByVal hWnd As IntPtr, ByVal lpstring As String) As Boolean
    Public Declare Function EnumWindows Lib "user32" (lpEnumFunc As EnumCallBackDelegate, lParam As Integer) As Integer
    Public Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (hwnd As IntPtr, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpString As String, cch As Integer) As Integer
    Public Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (hWnd As IntPtr, nIndex As Integer) As Integer
    Public Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (hWnd As IntPtr, nIndex As Integer, dwNewLong As IntPtr) As Integer
    Public Declare Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    Public Declare Function ShowWindow Lib "user32" (ByVal handle As IntPtr, ByVal nCmdShow As Integer) As Integer
    Public Declare Ansi Function GetClassName Lib "user32" Alias "GetClassNameA" (hwnd As IntPtr, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpString As String, cch As Integer) As Integer
    Public Declare Auto Function SetForegroundWindow Lib "user32" (hWnd As IntPtr) As Boolean
    Public Enum WindowLongFlags As Integer
        GWL_EXSTYLE = -20
        GWLP_HINSTANCE = -6
        GWLP_HWNDPARENT = -8
        GWL_ID = -12
        GWL_STYLE = -16
        GWL_USERDATA = -21
        GWL_WNDPROC = -4
        DWLP_USER = &H8
        DWLP_MSGRESULT = &H0
        DWLP_DLGPROC = &H4
    End Enum

    Private selectedproc As Integer = -1

    Private Sub Button1_Click() Handles Button1.Click
        ListView1.Items.Clear()
        For Each proc As Process In Process.GetProcesses
            Dim windClass As String = Strings.Space(256)
            GetClassName(proc.MainWindowHandle, windClass, 256)

            Dim windowInfo As RECT

            GetWindowRect(proc.MainWindowHandle, windowInfo)

            Dim windowX As Integer = windowInfo.left
            Dim windowY As Integer = windowInfo.top
            Dim windowW As Integer = windowInfo.right - windowInfo.left
            Dim windowH As Integer = windowInfo.bottom - windowInfo.top

            If windowW <> 0 And windowH <> 0 And proc.MainWindowTitle <> "" And proc.MainWindowTitle <> Me.Text Then
                ListView1.Items.Add(New ListViewItem({proc.Id, proc.MainWindowTitle}))
            End If
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            Dim tempItem As ListViewItem = Me.ListView1.Items(Me.ListView1.FocusedItem.Index)
            Dim proc As Process = Process.GetProcessById(CType(Val(tempItem.SubItems(0).Text), Integer))
            Dim wIndex As Integer = GetWindowLong(proc.MainWindowHandle, WindowLongFlags.GWL_STYLE)
            Dim windowInfo As RECT
            GetWindowRect(proc.MainWindowHandle, windowInfo)

            Me.winX.Value = windowInfo.left
            Me.winY.Value = windowInfo.top
            Me.WinWidth.Value = windowInfo.right - windowInfo.left
            Me.WinHeight.Value = windowInfo.bottom - windowInfo.top
            Me.WindowTitle.Text = proc.MainWindowTitle

            Me.selectedproc = proc.Id
        Catch ex As Exception
            MsgBox("ERROR : " & ex.Message, MsgBoxStyle.Critical And vbMsgBoxSetForeground, "EXCEPTION")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        readPresetProc()

        For Each scr As Screen In Screen.AllScreens
            Me.ComboBox1.Items.Add(scr.DeviceName.Replace("\", "") & " (" & (scr.Bounds.Right - scr.Bounds.Left) & " : " & (scr.Bounds.Bottom - scr.Bounds.Top) & ")")
        Next
        Me.ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        previewwindow.Size = New Size(Me.WinWidth.Value, Me.WinHeight.Value)
        previewwindow.Left = Me.winX.Value
        previewwindow.Top = Me.winY.Value

        previewwindow.Show()
        previewwindow.Text = Me.WindowTitle.Text
        Me.TopMost = True
        Me.Refresh()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim proc As IntPtr = Process.GetProcessById(CType(Val(Me.selectedproc), Integer)).MainWindowHandle
            SetWindowText(proc, Me.WindowTitle.Text)
            ShowWindow(proc, 1)
            SetWindowPos(proc, 0, Me.winX.Value, Me.winY.Value, Me.WinWidth.Value, Me.WinHeight.Value, 0)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub savePresetProc()
        Dim sb As New StringBuilder
        sb.AppendLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
        sb.AppendLine("<settings>")
        For Each lvi As ListViewItem In Me.ListView2.Items
            sb.AppendLine("<set x=""" & lvi.SubItems(0).Text & """ y=""" & lvi.SubItems(1).Text & """ width=""" & lvi.SubItems(2).Text & """ height=""" & lvi.SubItems(3).Text & """ memo=""" & lvi.SubItems(4).Text & """ />")
        Next
        sb.AppendLine("</settings>")
        My.Computer.FileSystem.WriteAllText("setting.xml", sb.ToString, False, System.Text.Encoding.UTF8)
    End Sub

    Private Sub readPresetProc()
        If Not System.IO.File.Exists("setting.xml") Then
            Me.savePresetProc()
        End If

        ListView2.Items.Clear()
        Dim xmlD As New System.Xml.XmlDocument
        xmlD.Load("setting.xml")
        For Each node As System.Xml.XmlElement In xmlD.SelectNodes("/settings/*")
            If node.Name.ToLower = "set" Then
                ListView2.Items.Add(New ListViewItem({node.GetAttribute("x"), node.GetAttribute("y"), node.GetAttribute("width"), node.GetAttribute("height"), node.GetAttribute("memo")}))
            End If
        Next
    End Sub

    Private Sub SavePreset_Click(sender As Object, e As EventArgs) Handles SavePreset.Click
        ListView2.Items.Add(New ListViewItem({Me.winX.Value, Me.winY.Value, Me.WinWidth.Value, Me.WinHeight.Value, Me.TextBox1.Text}))
        savePresetProc()
        readPresetProc()
    End Sub

    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        Dim tempItem As ListViewItem = Me.ListView2.Items(Me.ListView2.FocusedItem.Index)
        Me.winX.Value = Val(tempItem.SubItems(0).Text)
        Me.winY.Value = Val(tempItem.SubItems(1).Text)
        Me.WinWidth.Value = Val(tempItem.SubItems(2).Text)
        Me.WinHeight.Value = Val(tempItem.SubItems(3).Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sb As New StringBuilder
        Dim idx As Integer = -1
        Try
            If ListView2.FocusedItem.Index < 0 Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        sb.AppendLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
        sb.AppendLine("<settings>")

        For Each lvi As ListViewItem In Me.ListView2.Items
            idx += 1
            If idx = ListView2.FocusedItem.Index Then

            Else
                sb.AppendLine("<set x=""" & lvi.SubItems(0).Text & """ y=""" & lvi.SubItems(1).Text & """ width=""" & lvi.SubItems(2).Text & """ height=""" & lvi.SubItems(3).Text & """ memo=""" & lvi.SubItems(4).Text & """ />")
            End If
        Next
        sb.AppendLine("</settings>")
        My.Computer.FileSystem.WriteAllText("setting.xml", sb.ToString, False, System.Text.Encoding.UTF8)
        readPresetProc()

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        NumericUpDown3.Maximum = Me.NumericUpDown1.Value
        NumericUpDown5.Maximum = Me.NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        NumericUpDown4.Maximum = Me.NumericUpDown2.Value
        NumericUpDown6.Maximum = Me.NumericUpDown2.Value
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim proc As IntPtr = Process.GetProcessById(CType(Val(Me.selectedproc), Integer)).MainWindowHandle
            ShowWindow(proc, 1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim prm = Screen.PrimaryScreen.Primary
        Dim dsp = Screen.AllScreens

        Dim workx As Integer = 0
        Dim worky As Integer = 0
        Dim wx As Integer = 0
        Dim wy As Integer = 0

        Dim selx As Integer = -1
        For Each scr As Screen In dsp
            selx += 1
            If selx = Me.ComboBox1.SelectedIndex Then
                workx = scr.WorkingArea.Width
                worky = scr.WorkingArea.Height
                wx = scr.WorkingArea.Left
                wy = scr.WorkingArea.Top
            End If
        Next

        Dim winW As Integer = CType(workx / Me.NumericUpDown1.Value, Integer) * Me.NumericUpDown5.Value
        Dim winH As Integer = CType(worky / Me.NumericUpDown2.Value, Integer) * Me.NumericUpDown6.Value
        Dim winX As Integer = wx + CType(winW * (Me.NumericUpDown3.Value - 1), Integer)
        Dim winY As Integer = wy + CType(winH * (Me.NumericUpDown4.Value - 1), Integer)

        Me.winX.Value = winX
        Me.winY.Value = winY
        Me.WinWidth.Value = winW
        Me.WinHeight.Value = winH

        Button1_Click()

        Try
            Dim proc As IntPtr = Process.GetProcessById(CType(Val(Me.selectedproc), Integer)).MainWindowHandle
            SetWindowText(proc, Me.WindowTitle.Text)
            ShowWindow(proc, 1)
            SetWindowPos(proc, 0, winX, winY, winW, winH, 0)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Button1_Click()
    End Sub
End Class
