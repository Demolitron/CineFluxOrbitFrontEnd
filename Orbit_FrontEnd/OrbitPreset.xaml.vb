Public Class OrbitPresetBuilder
    Private _OrbitData As OrbitPreset
    Public Property OrbitData As OrbitPreset
        Get
            Return _OrbitData
        End Get
        Set(ByVal value As OrbitPreset)
            _OrbitData = value
            Dispatcher.Invoke(Sub()
                                  OrbitOrigin.CurrentPosition = OrbitData.Origin_Deg

                                  If OrbitData.IsClockwise Then
                                      OrbitDirection.SelectedIndex = 1
                                  Else
                                      OrbitDirection.SelectedIndex = 0
                                  End If

                                  OrbitEndMode.SelectedIndex = _OrbitData.EndMode
                                  OrbitSpeedMode.Items.Clear()
                                  Select Case OrbitEndMode.SelectedIndex
                                      Case 0 'By Orbit Count
                                          OrbitEndModeCounts.Text = _OrbitData.CycleCount
                                          OrbitEndModeCounts.IsEnabled = True
                                          OrbitEndModeCounts.Visibility = Windows.Visibility.Visible
                                          OrbitEndModeTime.IsEnabled = False
                                          OrbitEndModeTime.Visibility = Windows.Visibility.Collapsed
                                          OrbitSpeedMode.Items.Add("Manual")
                                          OrbitSpeedMode.Items.Add("Per Orbit")
                                          OrbitSpeedMode.Items.Add("For All Orbits")
                                      Case 1 'By Program Time
                                          OrbitEndModeCounts.IsEnabled = False
                                          OrbitEndModeCounts.Visibility = Windows.Visibility.Collapsed
                                          OrbitEndModeTime.Span_Seconds = _OrbitData.ProgramRunTime
                                          OrbitEndModeTime.IsEnabled = True
                                          OrbitEndModeTime.Visibility = Windows.Visibility.Visible
                                          OrbitSpeedMode.Items.Add("Manual")
                                          OrbitSpeedMode.Items.Add("Per Orbit")
                                      Case 2 'Never Ending.
                                          OrbitEndModeCounts.IsEnabled = False
                                          OrbitEndModeCounts.Visibility = Windows.Visibility.Collapsed
                                          OrbitEndModeTime.IsEnabled = False
                                          OrbitEndModeTime.Visibility = Windows.Visibility.Collapsed
                                          OrbitSpeedMode.Items.Add("Manual")
                                          OrbitSpeedMode.Items.Add("Per Orbit")
                                  End Select



                              End Sub)
        End Set
    End Property

    Private Sub OrbitPresetBuilder_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

    End Sub
End Class
