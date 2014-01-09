Class MainWindow 
    Dim S As New IO.Ports.SerialPort("COM5", 115200, IO.Ports.Parity.None, 8, 1)
    Dim CTRL As ControllerComm
    Dim UpdateUI As System.Threading.Thread

    Private Sub MainWindow_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        UpdateUI.Abort()
        S.Close()
    End Sub
    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Console.WriteLine(String.Format("@{0,2:X2}{1,2:X2}#", 1, 14))
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try


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
        Dim ret As ControllerStatus
        While (True)
            'System.Threading.Thread.Sleep(50)
            ret = CTRL.GetStatus
            Dispatcher.Invoke(Sub()
                                  txtDisplay.Text = ret.Display.Substring(0, 20) & vbCrLf & ret.Display.Substring(20, 20) & vbCrLf
                                  txtDisplay.Text += "Position=" & ret.Position & vbCrLf
                                  txtDisplay.Text += "Battery=" & ret.Battery & vbCrLf
                                  txtDisplay.Text += "Speed=" & ret.Speed
                              End Sub)

        End While
    End Sub

    Private Sub btnInc_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnInc.Click
        CTRL.UI_INC()
    End Sub

    Private Sub btnDec_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnDec.Click
        CTRL.UI_DEC()

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
