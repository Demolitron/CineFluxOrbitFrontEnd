Public Class CircularSelector
    Private Captured As Boolean
    Private Offset As Double
    Private CurrentPosition As Double
    Private Sub Thumb_PreviewMouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Thumb.PreviewMouseDown
        Container.CaptureMouse()
        Dim ThisAngle = GetAngle(e.GetPosition(Container))
        Offset = (CurrentPosition Mod 360) - ThisAngle
        'Console.WriteLine("Offset=" & Offset)
        Captured = True
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
        'Console.WriteLine(Angle.ToString)
        Return Angle
    End Function

    Private Sub Thumb_PreviewMouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Container.PreviewMouseMove
        If Captured = False Then Return
        Dim ThisAngle = GetAngle(e.GetPosition(Container)) + Offset
        While (ThisAngle > 360)
            ThisAngle -= 360
        End While
        While (ThisAngle < 0)
            ThisAngle = 360 + ThisAngle
        End While

        Dim CurrentAngle As Double = CurrentPosition Mod 360
        Dim Delta = ThisAngle - CurrentAngle
        ' Console.WriteLine(String.Format("Angle={0}, CurrentPos={1}, CurrentAngle={2}, Delta={3}", ThisANgle, CurrentPosition, CurrentAngle, Delta))
        CurrentPosition += Delta
        CurrentAngle = CurrentPosition Mod 360
        RotateMe.RenderTransform = New RotateTransform(CurrentAngle, Container.ActualWidth / 2, Container.ActualHeight / 2)
    End Sub

    Private Sub Thumb_PreviewMouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Container.PreviewMouseUp
        Container.ReleaseMouseCapture()
        Captured = False
    End Sub
End Class
