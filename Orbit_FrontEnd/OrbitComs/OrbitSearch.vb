Public Class OrbitSearch
    Private OrbitControllers As New List(Of ControllerComm)
    Public Sub New()

    End Sub

    Private Sub SeekOrbits()
        While (True)
            For Each S In IO.Ports.SerialPort.GetPortNames()
                Dim Existing = OrbitControllers.Find(Function(x)
                                                         Return x.SerialPortName = S
                                                     End Function)

                If IsNothing(Existing) Then
                    Dim SerP As New System.IO.Ports.SerialPort(S, 115200, IO.Ports.Parity.None, 8, 1)
                    Dim tryC As Integer = 0
                    While (SerP.IsOpen = False)
                        Try
                            SerP.Open()
                        Catch ex As Exception
                            tryC += 1
                            If tryC > 5 Then
                                SerP.Dispose()
                                Continue For
                            End If
                        End Try
                    End While
                    Dim ID = ControllerComm.Echo(SerP)
                    If ID <> 0 Then
                        OrbitControllers.Add(New ControllerComm(SerP, ID))
                    Else
                        SerP.Dispose()
                    End If
                End If
            Next
        End While
        

    End Sub

End Class
