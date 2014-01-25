Imports System.IO
Imports System.Runtime.InteropServices

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

<StructLayout(LayoutKind.Sequential)> _
Public Structure ECMD_ControllerStatus
    Public ReadOnly State As Byte
    Public ReadOnly Prepmove_Ready As Byte
    Public ReadOnly Speed As Single
    Public ReadOnly Battery As Single
    Public ReadOnly Position As Single
    Public ReadOnly Time_sec As Single
    Public Sub New(ByVal St As Byte, ByVal PMR As Byte, ByVal S As Single, ByVal B As Single, ByVal P As Single, ByVal Tm As Single)
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
    Public ReadOnly Location As UI_Location
    Public Sub New(ByVal P As Single, ByVal S As Single, ByVal B As Single, ByVal D As String, ByVal L As UI_Location)
        Position = P
        Speed = S
        Battery = B
        Display = D
        Location = L
    End Sub
End Structure

<StructLayout(LayoutKind.Sequential)> _
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

    Public Shared Function Deserialize(ByVal DataBytes() As Byte) As OrbitPreset
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
            For idx = 0 To 97
                ms.WriteByte(0)
            Next
            Return ms.ToArray
        End Using
    End Function
End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure WaypointPreset
    Public Const Type As Byte = 1
    Public Origin As UInt16
    Public PointCount As Byte
    Public Bounce As Byte
    Public LoopCount As UInt16
    Public Distances() As Int16
    Public TravelTimes() As UInt16
    Public DwellTimes() As UInt16

    Public Shared Function Deserialize(ByVal DataBytes() As Byte) As WaypointPreset
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
            ret.Distances(x) = BitConverter.ToInt16(DataBytes, idx) : idx += 2
        Next
        For x = 0 To 17
            ret.TravelTimes(x) = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        Next
        For x = 0 To 18
            ret.DwellTimes(x) = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        Next
        Return ret
    End Function

    Public Function Serialize() As Byte()
        Using ms As New MemoryStream(120)
            ms.WriteByte(Type)
            ms.Write(BitConverter.GetBytes(Origin), 0, 2)
            ms.WriteByte(PointCount)
            ms.WriteByte(Bounce)
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
            For idx = 0 To 2
                ms.WriteByte(0)
            Next
            Return ms.ToArray
        End Using
    End Function

End Structure

<StructLayout(LayoutKind.Sequential)> _
Public Structure OrbitConfigStruct
    Public Version As Int32
    Public BatteryLowVoltageLevel As UInt16
    Public Battery12V_Q8 As UInt16
    Public PowerResponseLimiter_Slope As Byte
    Public PowerResponseLimiter_Intercept As Byte
    Public Counts_Per_Volt As Single
    Public Volts_Per_Count As Single
    Public BlacklightPWM_Duty As Byte
    Public BacklightIdleTImeout As UInt16
    Public SystemAcceleration As Single
    Public MaxSpeed As Single
    Public PID_MaxError As Int16
    Public PID_Kp As UInt16
    Public PID_Kd As UInt16
    Public Counts_Per_Degree As Single
    Public Degrees_Per_Count As Single
    Public MyID As Byte

    Public Function Serialize() As Byte()
        Using ms As New MemoryStream()
            ms.Write(BitConverter.GetBytes(Version), 0, 4)
            ms.Write(BitConverter.GetBytes(BatteryLowVoltageLevel), 0, 2)
            ms.Write(BitConverter.GetBytes(Battery12V_Q8), 0, 2)
            ms.WriteByte(PowerResponseLimiter_Slope)
            ms.WriteByte(PowerResponseLimiter_Intercept)
            ms.Write(BitConverter.GetBytes(Counts_Per_Volt), 0, 4)
            ms.Write(BitConverter.GetBytes(Volts_Per_Count), 0, 4)
            ms.WriteByte(BlacklightPWM_Duty)
            ms.Write(BitConverter.GetBytes(BacklightIdleTImeout), 0, 2)
            ms.Write(BitConverter.GetBytes(SystemAcceleration), 0, 4)
            ms.Write(BitConverter.GetBytes(MaxSpeed), 0, 4)
            ms.Write(BitConverter.GetBytes(PID_MaxError), 0, 2)
            ms.Write(BitConverter.GetBytes(PID_Kp), 0, 2)
            ms.Write(BitConverter.GetBytes(PID_Kd), 0, 2)
            ms.Write(BitConverter.GetBytes(Counts_Per_Degree), 0, 4)
            ms.Write(BitConverter.GetBytes(Degrees_Per_Count), 0, 4)
            ms.WriteByte(MyID)
            Return ms.ToArray
        End Using
    End Function

    Public Shared Function Deserialize(ByVal DataBytes() As Byte) As OrbitConfigStruct
        Dim idx As Integer = 0
        Dim ret As New OrbitConfigStruct
        ret.Version = BitConverter.ToInt32(DataBytes, idx) : idx += 4
        ret.BatteryLowVoltageLevel = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.Battery12V_Q8 = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.PowerResponseLimiter_Slope = DataBytes(idx) : idx += 1
        ret.PowerResponseLimiter_Intercept = DataBytes(idx) : idx += 1
        ret.Counts_Per_Volt = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.Volts_Per_Count = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.BlacklightPWM_Duty = DataBytes(idx) : idx += 1
        ret.BacklightIdleTImeout = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.SystemAcceleration = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.MaxSpeed = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.PID_MaxError = BitConverter.ToInt16(DataBytes, idx) : idx += 2
        ret.PID_Kp = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.PID_Kd = BitConverter.ToUInt16(DataBytes, idx) : idx += 2
        ret.Counts_Per_Degree = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.Degrees_Per_Count = BitConverter.ToSingle(DataBytes, idx) : idx += 4
        ret.MyID = DataBytes(idx) : idx += 1
        Return ret
    End Function

End Structure