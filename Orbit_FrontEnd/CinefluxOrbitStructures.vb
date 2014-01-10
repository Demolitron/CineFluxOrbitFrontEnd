Imports System.IO

Public Enum UI_Location
    UI_LOC_MAINMENU = 1
    UI_LOC_ORBITSETUP = 2
    UI_LOC_ORBITRUN = 3
    UI_LOC_WAYPOINTSETUP = 4
    UI_LOC_WAYPOINTRUN = 5
    UI_LOC_REALTIMERUN = 6
    UI_LOC_SLEEP = 7
    UI_LOC_EXTMODE = 8
End Enum

Public Structure ECMD_ControllerStatus
    Public ReadOnly State As Byte
    Public ReadOnly Prepmove_Ready As Byte
    Public ReadOnly Speed As Single
    Public ReadOnly Battery As Single
    Public ReadOnly Position As Single
    Public ReadOnly Time_sec As Single
    Public Sub New(St As Byte, PMR As Byte, S As Single, B As Single, P As Single, Tm As Single)
        State = St
        Prepmove_Ready = PMR
        Speed = S
        Battery = B
        Position = P
        Time_sec = Tm
    End Sub
End Structure

Public Structure UI_ControllerStatus
    Public ReadOnly Position As Single
    Public ReadOnly Speed As Single
    Public ReadOnly Battery As Single
    Public ReadOnly Display As String
    Public Sub New(P As Single, S As Single, B As Single, D As String)
        Position = P
        Speed = S
        Battery = B
        Display = D
    End Sub
End Structure

Public Structure OrbitPreset
    Public Const Type As Byte = 2
    Public Origin_Deg As UInt16
    Public EndMode As Byte
    Public IsClockwise As Byte
    Public ProgramRunTime As Single
    Public CycleCount As Single
    Public CycleTime As Single
    Public Speed As Single
    Public SpeedMode As Byte

    Public Shared Function Deserialize(DataBytes() As Byte) As OrbitPreset
        Dim idx As Integer = 0
        Dim ret As New OrbitPreset
        idx += 1
        ret.Origin_Deg = BitConverter.ToInt16(DataBytes, idx) : idx += 2
        ret.EndMode = DataBytes(idx) : idx += 1
        ret.IsClockwise = DataBytes(idx) : idx += 1
        ret.ProgramRunTime = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.CycleCount = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.CycleTime = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.Speed = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.SpeedMode = DataBytes(idx) : idx += 1
        Return ret
    End Function

    Public Function Serialize() As Byte()
        Using ms As New MemoryStream(120)
            ms.WriteByte(Type)
            ms.Write(BitConverter.GetBytes(Origin_Deg), 0, 2)
            ms.WriteByte(EndMode)
            ms.WriteByte(IsClockwise)
            ms.Write(BitConverter.GetBytes(ProgramRunTime), 0, 4)
            ms.Write(BitConverter.GetBytes(CycleCount), 0, 4)
            ms.Write(BitConverter.GetBytes(CycleTime), 0, 4)
            ms.Write(BitConverter.GetBytes(Speed), 0, 4)
            ms.WriteByte(SpeedMode)
            Return ms.ToArray
        End Using
    End Function
End Structure

Public Structure WaypointPreset
    Public Const Type As Byte = 1
    Public Origin As UInt16
    Public PointCount As Byte
    Public Bounce As Byte
    Public LoopCount As UInt16
    Public Distances() As Int16
    Public TravelTimes() As UInt16
    Public DwellTimes() As UInt16

    Public Shared Function Deserialize(DataBytes() As Byte) As WaypointPreset
        Dim ret As New WaypointPreset
        ReDim ret.TravelTimes(17)
        ReDim ret.DwellTimes(18)
        ReDim ret.Distances(17)
        Dim idx As Integer = 0

        idx += 1
        ret.Origin = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.PointCount = DataBytes(idx) : idx += 1
        ret.Bounce = DataBytes(idx) : idx += 1
        ret.LoopCount = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        For x = 0 To 17
            ret.Distances(x) = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        Next
        For idx = 0 To 17
            ret.TravelTimes(idx) = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        Next
        For idx = 0 To 18
            ret.DwellTimes(idx) = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        Next
        Return ret
    End Function

    Public Function Serialize() As Byte()
        Using ms As New MemoryStream(120)
            ms.WriteByte(Type)
            ms.Write(BitConverter.GetBytes(Origin), 0, 2)
            ms.WriteByte(PointCount)
            ms.WriteByte(Bounce)
            ms.WriteByte(PointCount)
            ms.Write(BitConverter.GetBytes(LoopCount), 0, 2)
            For x = 0 To 17
                ms.Write(BitConverter.GetBytes(Distances(x)), 0, 2)
            Next
            For x = 0 To 17
                ms.Write(BitConverter.GetBytes(TravelTimes(x)), 0, 2)
            Next
            For x = 0 To 18
                ms.Write(BitConverter.GetBytes(DwellTimes(x)), 0, 2)
            Next
            Return ms.ToArray
        End Using
    End Function

End Structure