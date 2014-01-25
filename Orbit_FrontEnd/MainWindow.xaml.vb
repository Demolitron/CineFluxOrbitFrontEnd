Imports System.Text

Class MainWindow
    Dim S As IO.Ports.SerialPort
    Dim CTRL As ControllerComm
    Dim UpdateUI As System.Threading.Thread
    Dim Presets(4) As Object

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        UpdateUI.Abort()
        S.Close()
    End Sub

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'Dim buf(3) As Byte
        'Using fs As New IO.FileStream("C:\rawdata.dat", IO.FileMode.Open, IO.FileAccess.Read)
        '    Using sr As New IO.StreamWriter("C:\positions.csv")
        '        While fs.Position < fs.Length
        '            fs.Read(buf, 0, 4)
        '            sr.WriteLine(BitConverter.ToUInt32(buf, 0))
        '            sr.Flush()
        '        End While
        '    End Using
        'End Using

        Try
            If IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory & "ComPort.txt") = False Then
                Using fs As New IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory & "ComPort.txt")
                    fs.WriteLine("COM1")
                End Using
            End If
            Using fs As New IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory & "ComPort.txt")
                Dim ComPort = fs.ReadLine
                S = New IO.Ports.SerialPort(ComPort.Trim, 115200, IO.Ports.Parity.None, 8, 1)
            End Using
        Catch ex As Exception
            MsgBox("There was a problem..." & ex.ToString)
            Environment.Exit(-1)
        End Try


        Dim tryC As Integer = 0
        While (S.IsOpen = False)
            Try
                S.Open()
            Catch ex As Exception
                tryC += 1
                If tryC > 5 Then
                    MsgBox("Unable to open the ComPort: " & ex.ToString)
                    Environment.Exit(-1)
                End If
            End Try
        End While

        'Dim st As New Stopwatch
        ''Using fs As New IO.FileStream("C:\rawdata.dat", IO.FileMode.Create, IO.FileAccess.Write)
        'Using fs As New IO.StreamWriter("C:\Timmings.csv")
        '    While True
        '        If S.BytesToRead >= 4 Then
        '            S.ReadExisting()
        '            fs.WriteLine(st.ElapsedTicks)
        '            st.Restart()
        '            fs.Flush()
        '        End If
        '    End While
        'End Using

        CTRL = New ControllerComm(S, &HAA)

        Dim config = CTRL.GetConfig

        config.PID_Kp = 64
        config.PID_Kd = 200
        config.PID_MaxError = 20
        CTRL.SetConfig(config)

        'Dim CheckCfg = CTRL.GetConfig


        UpdateUI = New System.Threading.Thread(AddressOf Refresh)

        'Presets(0) = CTRL.ReadPreset(1)
        'Presets(1) = CTRL.ReadPreset(2)
        'Presets(2) = CTRL.ReadPreset(3)
        'Presets(3) = CTRL.ReadPreset(4)
        'Presets(4) = CTRL.ReadPreset(5)

        'CTRL.PushToExternalMode()
        'CTRL.ECMD_Waypoint_Init()
        'CTRL.ECMD_Waypoint_Add(180, 5, 1)
        'CTRL.ECMD_Waypoint_Add(180, 10, 1)
        'CTRL.ECMD_Waypoint_Add(45, 2, 2)
        'CTRL.ECMD_Waypoint_Run()
        'Dim Stat As ECMD_ControllerStatus
        'Stat = CTRL.ECMD_Status()
        'While (Stat.State <> 0)
        '    Stat = CTRL.ECMD_Status()
        'End While

        'CTRL.ECMD_PrepareMove(180, 90, 90)
        'CTRL.ECMD_ExecuteMove()
        'CTRL.ECMD_PrepareMove(-360, 90, 90)
        'Dim Stat As ECMD_ControllerStatus
        'Stat = CTRL.ECMD_Status()
        'While (Stat.State <> 0)
        '    Stat = CTRL.ECMD_Status()
        'End While
        'CTRL.ECMD_ExecuteMove()
        'Stat = CTRL.ECMD_Status()
        'While (Stat.State <> 0)
        '    Stat = CTRL.ECMD_Status()
        'End While

        'CTRL.ECMD_ReturnToUI()
        UpdateUI.Start()

    End Sub

    Private Sub Refresh()
        Dim ret As UI_ControllerStatus
        While (True)
            'Removed the delay here to see just how fast I could get this running... But this saturates the com channel so in production this shouldn't be done.
            'System.Threading.Thread.Sleep(50)
            ret = CTRL.GetStatus
            Dispatcher.Invoke(Sub()
                                  If ret.Location = UI_Location.UI_LOC_SLEEP Then
                                      Sleep.Content = "Wake"
                                  Else
                                      Sleep.Content = "Sleep"
                                  End If
                                  CircularSelector1.CurrentPosition = ret.Position
                                  txtDisplay.Text = ret.Display.Substring(0, 20) & vbCrLf & ret.Display.Substring(20, 20) & vbCrLf
                                  Dim LeftSB As New StringBuilder
                                  LeftSB.AppendFormat("Position: {0}°", Math.Round(ret.Position, 3))
                                  LeftSB.AppendLine()
                                  If ret.Position < 0 Then
                                      LeftSB.AppendFormat("   Angle: {0}°", 360 + Math.Round(ret.Position Mod 360, 3))
                                  Else
                                      LeftSB.AppendFormat("   Angle: {0}°", Math.Round(ret.Position Mod 360, 3))
                                  End If

                                  LeftSB.AppendLine()
                                  LeftSB.AppendFormat("   Speed: {0}°/s", Math.Round(ret.Speed, 2))
                                  LeftSB.AppendLine()
                                  LeftSB.AppendFormat(" Battery: {0}V", Math.Round(ret.Battery, 1))
                                  txtLeft.Text = LeftSB.ToString
                              End Sub)

        End While
    End Sub

    Private Sub btnInc_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnInc.Click
        CTRL.UI_DEC()
    End Sub

    Private Sub btnDec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDec.Click
        CTRL.UI_INC()
    End Sub

    Private Sub btnClick_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnClick.Click
        CTRL.UI_Click()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnBack.Click
        CTRL.UI_Back()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancel.Click
        CTRL.UI_Cancel()
    End Sub

    Private Sub btnPreset1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreset1.Click
        CTRL.RunPreset(0)
    End Sub

    Private Sub btnPreset2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreset2.Click
        CTRL.RunPreset(1)
    End Sub

    Private Sub btnPreset3_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreset3.Click
        CTRL.RunPreset(2)
    End Sub

    Private Sub btnPreset4_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreset4.Click
        CTRL.RunPreset(3)
    End Sub

    Private Sub btnPreset5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnPreset5.Click
        CTRL.RunPreset(4)
    End Sub

    Private Sub Sleep_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Sleep.Click
        Dim ret = CTRL.GetUILocation
        If ret = UI_Location.UI_LOC_SLEEP Then
            CTRL.Wake()
        Else
            CTRL.PushToSleep()
        End If

    End Sub

    Private Sub Orbit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Orbit.Click
        CTRL.PushToOrbitSetup()
    End Sub

    Private Sub Waypoint_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Waypoint.Click
        CTRL.PushToWaypointSetup()
    End Sub

    Private Sub Realtime_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Realtime.Click
        CTRL.PushToRealtimeMode()
    End Sub
End Class
