Imports System.IO

Public Class AdvancedPathFollower
    Private SetPosition As Double
    Private CurPosition As Double
    Private CurSpeed As Double
    Private SetSpeed As Double
    Private CurAcceleration As Double
    Private PathPointIdx As Integer
    Private PathPoints As New List(Of PathPoint)
    Private log As StreamWriter
    Public Sub New()
        log = New StreamWriter("C:\log.csv")
    End Sub

    Public Sub close()
        log.Flush()
        log.Dispose()
    End Sub

    Public Function Tick() As Double
        If CurSpeed > SetSpeed Then
            SetSpeed -= (CurAcceleration / 500)
            If CurSpeed < SetSpeed Then CurSpeed = SetSpeed
        ElseIf CurSpeed < SetSpeed Then
            SetSpeed += (CurAcceleration / 500)
            If CurSpeed > SetSpeed Then CurSpeed = SetSpeed
        End If
        CurPosition += (CurSpeed / 500)

        If CurPosition > (SetPosition - (CurSpeed / 500)) AndAlso CurPosition < (SetPosition + (CurSpeed / 500)) Then
            PathPointIdx += 1
            SetSpeed = PathPoints(PathPointIdx).Speed
            SetPosition = PathPoints(PathPointIdx).PositionAt
            CurAcceleration = PathPoints(PathPointIdx).Acceleration
        End If
        log.WriteLine(SetPosition.ToString)
        Return SetPosition
    End Function

End Class

Public Structure PathPoint
    Public Speed As Double
    Public Acceleration As Double
    Public PositionAt As Double
End Structure
