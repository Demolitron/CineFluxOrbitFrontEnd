Public Class TimeSpanSelector
    Private SelectedField As FieldSelection
    Private ST As New Stopwatch
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtHour.Background = Brushes.White
        txtMin.Background = Brushes.White
        txtSec.Background = Brushes.Aquamarine
        SelectedField = FieldSelection.Second
        Span_Seconds = 0
        ST.Stop()
        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf UpdateUI)
    End Sub
    Private Enum FieldSelection
        Hour
        Minute
        Second
    End Enum

    Private Sub UpdateUI()
        While (True)
            If ST.ElapsedMilliseconds > 500 Then
                Dispatcher.Invoke(Sub()
                                      If IsNothing(txtMin) OrElse IsNothing(txtHour) OrElse IsNothing(txtSec) Then Return
                                      Dim SP As New TimeSpan(CInt(txtHour.Text), CInt(txtMin.Text), CInt(txtSec.Text))
                                      Span_Seconds = SP.TotalSeconds
                                  End Sub)
                ST.Stop()
            End If
            System.Threading.Thread.Sleep(1)
        End While
    End Sub

    Public Property Span_Seconds As Integer
        Get
            Dim SP As New TimeSpan(CInt(txtHour.Text), CInt(txtMin.Text), CInt(txtSec.Text))
            Return SP.TotalSeconds
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then value = 0
            Dim SP As TimeSpan = TimeSpan.FromSeconds(value)
            txtSec.Text = SP.Seconds.ToString("00")
            txtMin.Text = SP.Minutes.ToString("00")
            txtHour.Text = ((SP.Days * 24) + SP.Hours).ToString("00")
        End Set
    End Property

    Private Sub txtHour_GotFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtHour.GotFocus
        txtHour.Background = Brushes.Aquamarine
        txtMin.Background = Brushes.White
        txtSec.Background = Brushes.White
        SelectedField = FieldSelection.Hour
    End Sub
    Private Sub txtMinute_GotFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtMin.GotFocus
        txtHour.Background = Brushes.White
        txtMin.Background = Brushes.Aquamarine
        txtSec.Background = Brushes.White
        SelectedField = FieldSelection.Minute
    End Sub
    Private Sub txtSecond_GotFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles txtSec.GotFocus
        txtHour.Background = Brushes.White
        txtMin.Background = Brushes.White
        txtSec.Background = Brushes.Aquamarine
        SelectedField = FieldSelection.Second
    End Sub

    Private Sub Up_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Up.MouseDown
        Dim SP As New TimeSpan(CInt(txtHour.Text), CInt(txtMin.Text), CInt(txtSec.Text))
        Select Case SelectedField
            Case FieldSelection.Second
                Span_Seconds = SP.TotalSeconds + 1
            Case FieldSelection.Minute
                Span_Seconds = SP.TotalSeconds + 60
            Case FieldSelection.Hour
                Span_Seconds = SP.TotalSeconds + (60 * 60)
            Case Else
                Span_Seconds = SP.TotalSeconds + 1
        End Select
    End Sub

    Private Sub Down_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Down.MouseDown
       Dim SP As New TimeSpan(CInt(txtHour.Text), CInt(txtMin.Text), CInt(txtSec.Text))
        Select Case SelectedField
            Case FieldSelection.Second
                Span_Seconds = SP.TotalSeconds - 1
            Case FieldSelection.Minute
                Span_Seconds = SP.TotalSeconds - 60
            Case FieldSelection.Hour
                Span_Seconds = SP.TotalSeconds - (60 * 60)
            Case Else
                Span_Seconds = SP.TotalSeconds - 1
        End Select
    End Sub

    Private Sub txtHour_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtHour.TextChanged
        'If IsNothing(txtMin) OrElse IsNothing(txtHour) OrElse IsNothing(txtSec) Then Return
        'Dim SP As New TimeSpan(CInt(txtHour.Text), CInt(txtMin.Text), CInt(txtSec.Text))
        'Span_Seconds = SP.TotalSeconds
        ST.Restart()
    End Sub

    Private Sub txtMin_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtMin.TextChanged
        ST.Restart
    End Sub

    Private Sub txtSec_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtSec.TextChanged
        ST.Restart
    End Sub

    
End Class
