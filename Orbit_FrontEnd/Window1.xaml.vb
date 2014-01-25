Public Class Window1

    Private Sub CircularSelector1_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CircularSelector1.Loaded

    End Sub

    Private Sub CircularSelector1_OnPositionChangeCompleted(ByVal Position As Double) Handles CircularSelector1.OnPositionChangeCompleted
        Console.WriteLine(Position)
    End Sub

    Private Sub CircularSelector1_OnPositionChanging(ByVal Position As Double) Handles CircularSelector1.OnPositionChanging
        Console.WriteLine(Position)
    End Sub
End Class
