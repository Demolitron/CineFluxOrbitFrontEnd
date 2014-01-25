Public Class CircularSelector
    Public Property CurrentPosition As Double
        Get
            Return _CurrentPosition
        End Get
        Set(ByVal value As Double)
            _CurrentPosition = value
            Dispatcher.Invoke(Sub()
                                  RotateMe.RenderTransform = New RotateTransform(_CurrentPosition Mod 360, Container.ActualWidth / 2, Container.ActualHeight / 2)
                                  RaiseEvent OnPositionChanging(_CurrentPosition)
                              End Sub)
        End Set
    End Property
    Public Event OnPositionChanging(ByVal Position As Double)
    Public Event OnPositionChangeCompleted(ByVal Position As Double)
    Public Event OnPositionChangeStart()

    Private Captured As Boolean
    Private Offset As Double
    Private _CurrentPosition As Double
    Private Sub Thumb_PreviewMouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Thumb.PreviewMouseDown
        Container.CaptureMouse()
        Dim ThisAngle = GetAngle(e.GetPosition(Container))
        Offset = ThisAngle
        Captured = True
        RaiseEvent OnPositionChangeStart()
    End Sub
    Private Function GetAngle(ByVal position As Point)
        position.X -= (Container.ActualWidth / 2)
        position.Y -= (Container.ActualHeight / 2)
        Dim Angle = Math.Atan2(position.Y, position.X)
        Angle *= (180 / Math.PI)
        Angle += 90
        While (Angle > 360)
            Angle -= 360
        End While
        While (Angle < 0)
            Angle = 360 + Angle
        End While
        Return Angle
    End Function

    Private Sub Thumb_PreviewMouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Container.PreviewMouseMove
        If Captured = False Then Return
        Dim ThisAngle = GetAngle(e.GetPosition(Container))
        Dim CurrentAngle As Double
        Dim CW_Delta As Double
        Dim CCW_Delta As Double

        If ThisAngle > Offset Then
            CW_Delta = (360 - ThisAngle) + Offset
            CCW_Delta = -(ThisAngle - Offset)
        Else
            CW_Delta = Offset - ThisAngle
            CCW_Delta = -(ThisAngle + (360 - Offset))
        End If
        Offset = ThisAngle

        Dim Delta As Double
        If Math.Abs(CW_Delta) < Math.Abs(CCW_Delta) Then
            Delta = CW_Delta
        Else
            Delta = CCW_Delta
        End If
        _CurrentPosition -= Delta
        CurrentAngle = _CurrentPosition Mod 360
        RotateMe.RenderTransform = New RotateTransform(CurrentAngle, Container.ActualWidth / 2, Container.ActualHeight / 2)
        RaiseEvent OnPositionChanging(_CurrentPosition)
    End Sub

    Private Sub Thumb_PreviewMouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Container.PreviewMouseUp
        Container.ReleaseMouseCapture()
        Captured = False
        RaiseEvent OnPositionChangeCompleted(_CurrentPosition)
    End Sub
End Class
