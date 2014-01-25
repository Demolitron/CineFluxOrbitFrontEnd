Public Class SpeedSelector
    Private _OrbitCount As Integer
    Public Property OrbitCount As Integer
        Get
            Return _OrbitCount
        End Get
        Set(value As Integer)
            If (value <> _OrbitCount) Then
                Dispatcher.Invoke(Sub()
                                      cmbSpeedMode.Items.Clear()
                                      cmbSpeedMode.Items.Add("Manual Input")
                                      cmbSpeedMode.Items.Add("By Single Orbit Time")
                                      If value > 1 Then
                                          cmbSpeedMode.Items.Add("By All Orbits Time")
                                      End If
                                  End Sub)
                _OrbitCount = value
            End If

        End Set
    End Property
End Class
