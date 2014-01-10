Imports System.Text

Class MainWindow
    Dim S As New IO.Ports.SerialPort("COM5", 115200, IO.Ports.Parity.None, 8, 1)
    Dim CTRL As ControllerComm
    Dim UpdateUI As System.Threading.Thread

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        UpdateUI.Abort()
        S.Close()
    End Sub

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        While (S.IsOpen = False)
            Try
                S.Open()
            Catch ex As Exception

            End Try
        End While
        CTRL = New ControllerComm(S, &HAA)
        UpdateUI = New System.Threading.Thread(AddressOf Refresh)
        UpdateUI.Start()
    End Sub

    Private Sub Refresh()
        Dim ret As UI_ControllerStatus
        While (True)
            'Removed the delay here to see just how fast I could get this running... But this saturates the com channel so in production this shouldn't be done.
            'System.Threading.Thread.Sleep(50)
            ret = CTRL.GetStatus
            Dispatcher.Invoke(Sub()
                                  DirectionIndicator.RenderTransform = New RotateTransform(ret.Position Mod 360, 100, 100)
                                  txtDisplay.Text = ret.Display.Substring(0, 20) & vbCrLf & ret.Display.Substring(20, 20) & vbCrLf
                                  Dim LeftSB As New StringBuilder
                                  LeftSB.AppendFormat("Position: {0}°", Math.Round(ret.Position, 1))
                                  LeftSB.AppendLine()
                                  LeftSB.AppendFormat("   Angle: {0}°", Math.Round(ret.Position Mod 360, 1))
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
End Class
