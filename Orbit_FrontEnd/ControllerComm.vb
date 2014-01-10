Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices

Public Class ControllerComm
    Const CMD_SET_PRESET = "01"
    Const CMD_GET_PRESET = "02"
    Const CMD_GETDISPLAY = "10"
    Const CMD_UI_CLICK = "11"
    Const CMD_UI_BACK = "12"
    Const CMD_UI_CANCEL = "13"
    Const CMD_UI_INC = "14"
    Const CMD_UI_DEC = "15"
    Const CMD_GET_POS = "16"
    Const CMD_GET_SPEED = "17"
    Const CMD_GET_BATTERY = "18"
    Const CMD_UI_MACRO = "19"
    Const CMD_GET_UI_LOC = "1A"
    Const CMD_GET_COMPLETE_STATUS = "1B"
    Const CMD_PREP_MOVE = "60"
    Const CMD_EXEC_MOVE = "61"
    Const CMD_STOP = "62"
    Const CMD_STATUS = "63"
    Const CMD_PATH_INIT = "64"
    Const CMD_PATH_ADD = "65"
    Const CMD_PATH_RUN = "66"
    Const CMD_EXIT = "80"

    Const USER_INPUT_MACRO_RUNPRESET0 = "32"
    Const USER_INPUT_MACRO_RUNPRESET1 = "33"
    Const USER_INPUT_MACRO_RUNPRESET2 = "34"
    Const USER_INPUT_MACRO_RUNPRESET3 = "35"
    Const USER_INPUT_MACRO_RUNPRESET4 = "36"
    Const USER_INPUT_MACRO_ORBITMODE = "37"
    Const USER_INPUT_MACRO_WAYMODE = "38"
    Const USER_INPUT_MACRO_REALMODE = "39"
    Const USER_INPUT_MACRO_EXTMODE = "3A"
    Const USER_INPUT_MACRO_SLEEP = "3B"
    Const USER_INPUT_MACRO_WAKE = "3C"


    Private ResourceLocker As New Object
    Private Serial As IO.Ports.SerialPort
    Private MyID As Integer

    Public Sub New(ByVal S As IO.Ports.SerialPort, ID As Integer)
        Serial = S
        Serial.Encoding = System.Text.Encoding.ASCII
        Serial.NewLine = "#"
        Serial.ReadTimeout = 1000
        MyID = ID
    End Sub

    Private Function SendCommandWithReturnData(cmd As String, Parameters() As Byte) As Byte()
        Dim data As String
        SyncLock ResourceLocker
            Try
                Dim sb As New StringBuilder
                Dim SendChkSum As Integer = 0
                sb.AppendFormat("@{0,2:X2}{1}", MyID, cmd)
                If IsNothing(Parameters) = False Then
                    For Each param In Parameters
                        sb.AppendFormat("{0,2:X2}", param)
                        SendChkSum += param
                        If SendChkSum > &HFF Then SendChkSum -= &HFF
                    Next
                End If
                sb.AppendFormat("{0,2:X2}", SendChkSum)
                sb.Append("#")
                Serial.ReadExisting()
                Serial.Write(sb.ToString)
                data = Serial.ReadLine()
            Catch ex As TimeoutException
                Throw New CinefluxOrbitSerialTimeoutException
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

        If data.Substring(0, 1) <> "$" Then Throw CinefluxOrbitNackException.GenerateNew(StringToByteArray(data.Substring(1, 4)))
        If data.Substring(1, 2) <> cmd Then Throw New CinefluxOrbitUnexpectedAckException("Recieved Wrong ACK Command. Expected=" & cmd & "  , Rx=" & data.Substring(1, 2))

        Dim EndPos = InStr(data, "#")
        Dim DataBytes() As Byte = StringToByteArray(data.Substring(3, EndPos - 3))
        Dim ChkSum As Integer = 0
        For idx = 0 To DataBytes.Length - 2
            ChkSum += idx
            If ChkSum > &HFF Then ChkSum -= &HFF
        Next
        If ChkSum <> DataBytes(DataBytes.Length - 1) Then
            Throw New CinefluxOrbitChecksumMismatchException("Calculated Checksum=" & ChkSum & " Expected=" & DataBytes(DataBytes.Length - 1))
        End If
        Return DataBytes
    End Function

    Private Sub SendCommandNoReturnData(Cmd As String, Parameters() As Byte)
        Dim data As String
        SyncLock ResourceLocker
            Try
                Dim sb As New StringBuilder
                Dim SendChkSum As Integer = 0
                sb.AppendFormat("@{0,2:X2}{1}", MyID, Cmd)
                If IsNothing(Parameters) = False Then
                    For Each param In Parameters
                        sb.AppendFormat("{0,2:X2}", param)
                        SendChkSum += param
                        If SendChkSum > &HFF Then SendChkSum -= &HFF
                    Next
                End If
                sb.AppendFormat("{0,2:X2}", SendChkSum)
                sb.Append("#")
                Serial.ReadExisting()
                Serial.Write(sb.ToString)
                data = Serial.ReadLine()
            Catch ex As TimeoutException
                Throw New CinefluxOrbitSerialTimeoutException
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
        If data.Substring(0, 1) <> "$" Then Throw CinefluxOrbitNackException.GenerateNew(StringToByteArray(data.Substring(1, 4)))
        If data.Substring(1, 2) <> Cmd Then Throw New CinefluxOrbitUnexpectedAckException("Recieved Wrong ACK Command. Expected=" & Cmd & "  , Rx=" & data.Substring(1, 2))
    End Sub

    Private Function StringToByteArray(ByVal hex As String) As Byte()
        Dim NumberChars As Integer = hex.Length
        Dim bytes((NumberChars >> 1) - 1) As Byte
        Try
            For i As Integer = 0 To NumberChars - 1 Step 2
                bytes(i >> 1) = Convert.ToByte(hex.Substring(i, 2), 16)
            Next
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return bytes
    End Function

    Public Function GetDisplay() As String
        Return System.Text.Encoding.ASCII.GetString(SendCommandWithReturnData(CMD_GETDISPLAY, Nothing), 0, 80)
    End Function

    Public Function ReadPreset(ByVal PresetNumber) As Object
        Dim DataBytes As Byte() = SendCommandWithReturnData(CMD_GET_PRESET, New Byte() {PresetNumber})
        If DataBytes(0) = 1 Then
            Return OrbitPreset.Deserialize(DataBytes)
        End If
        If DataBytes(0) = 2 Then
            Return WaypointPreset.Deserialize(DataBytes)
        End If
        Return Nothing
    End Function

    Public Sub WritePreset(PresetNumber As Byte, Preset As Object)
        Dim ms As New MemoryStream
        ms.WriteByte(PresetNumber)
        If TypeOf (Preset) Is WaypointPreset Then
            Dim dat As Byte() = CType(Preset, WaypointPreset).Serialize
            ms.Write(dat, 0, dat.Length)
        End If
        If TypeOf (Preset) Is OrbitPreset Then
            Dim dat As Byte() = CType(Preset, OrbitPreset).Serialize
            ms.Write(dat, 0, dat.Length)
        End If
        SendCommandNoReturnData(CMD_SET_PRESET, ms.ToArray)
    End Sub

    Public Sub UI_Click()
        SendCommandNoReturnData(CMD_UI_CLICK, Nothing)
    End Sub
    Public Sub UI_Back()
        SendCommandNoReturnData(CMD_UI_BACK, Nothing)
    End Sub
    Public Sub UI_Cancel()
        SendCommandNoReturnData(CMD_UI_CANCEL, Nothing)
    End Sub
    Public Sub UI_INC()
        SendCommandNoReturnData(CMD_UI_INC, Nothing)
    End Sub
    Public Sub UI_DEC()
        SendCommandNoReturnData(CMD_UI_DEC, Nothing)
    End Sub

    Public Function GetPosition() As Single
        Return BitConverter.ToSingle(SendCommandWithReturnData(CMD_GET_POS, Nothing), 0)
    End Function

    Public Function GetSpeed() As Single
        Return BitConverter.ToSingle(SendCommandWithReturnData(CMD_GET_SPEED, Nothing), 0)
    End Function

    Public Function GetBattery() As Single
        Return BitConverter.ToSingle(SendCommandWithReturnData(CMD_GET_BATTERY, Nothing), 0)

    End Function

    Public Function GetStatus() As UI_ControllerStatus
        Dim DataBytes As Byte() = SendCommandWithReturnData(CMD_GET_COMPLETE_STATUS, Nothing)
        Dim rdIdx As Integer = 0
        Dim D = System.Text.Encoding.ASCII.GetString(DataBytes, rdIdx, 40) : rdIdx += 40
        Dim P = BitConverter.ToSingle(DataBytes, rdIdx) : rdIdx += 4
        Dim S = BitConverter.ToSingle(DataBytes, rdIdx) : rdIdx += 4
        Dim B = BitConverter.ToSingle(DataBytes, rdIdx) : rdIdx += 4
        Return New UI_ControllerStatus(P, S, B, D)
    End Function

    Public Sub RunPreset(ByVal PresetNumber As Integer)
        Select Case PresetNumber
            Case 0
                SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_RUNPRESET0})
            Case 1
                SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_RUNPRESET1})
            Case 2
                SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_RUNPRESET2})
            Case 3
                SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_RUNPRESET3})
            Case 4
                SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_RUNPRESET4})
        End Select
    End Sub

    Public Sub PushToOrbitSetup()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_ORBITMODE})
    End Sub

    Public Sub PushToWaypointSetup()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_WAYMODE})
    End Sub

    Public Sub PushToRealtimeMode()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_REALMODE})
    End Sub

    Public Sub PushToExternalMode()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_EXTMODE})
    End Sub

    Public Sub PushToSleep()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_SLEEP})
    End Sub

    Public Sub Wake()
        SendCommandNoReturnData(CMD_UI_MACRO, New Byte() {USER_INPUT_MACRO_WAKE})
    End Sub

    Public Sub ECMD_PrepareMove(Distance_deg As Single, Speed_deg_sec As Single, Acceleration_deg_sec_sec As Single)
        Dim MoveData As New MemoryStream
        MoveData.Write(BitConverter.GetBytes(Distance_deg), 0, 4)
        MoveData.Write(BitConverter.GetBytes(Speed_deg_sec), 0, 4)
        MoveData.Write(BitConverter.GetBytes(Acceleration_deg_sec_sec), 0, 4)
        SendCommandNoReturnData(CMD_PREP_MOVE, MoveData.ToArray)
    End Sub

    Public Sub ECMD_ExecuteMove()
        SendCommandNoReturnData(CMD_EXEC_MOVE, Nothing)
    End Sub

    Public Sub ECMD_Stop()
        SendCommandNoReturnData(CMD_STOP, Nothing)
    End Sub

    Public Function ECMD_Status() As ECMD_ControllerStatus
        Dim data As Byte() = SendCommandWithReturnData(CMD_STATUS, Nothing)
        Dim RdIdx As Integer = 0
        Dim State As Byte = data(RdIdx) : RdIdx += 1
        Dim PrepMoveReady As Byte = data(RdIdx) : RdIdx += 1
        Dim Position As Single = BitConverter.ToSingle(data, RdIdx) : RdIdx += 4
        Dim Speed As Single = BitConverter.ToSingle(data, RdIdx) : RdIdx += 4
        Dim Time As Single = BitConverter.ToSingle(data, RdIdx) : RdIdx += 4
        Dim Battery As Single = BitConverter.ToSingle(data, RdIdx) : RdIdx += 4
        Return New ECMD_ControllerStatus(State, PrepMoveReady, Speed, Battery, Position, Time)
    End Function

    Public Sub ECMD_Waypoint_Init()
        SendCommandNoReturnData(CMD_PATH_INIT, Nothing)
    End Sub

    Public Sub ECMD_Waypoint_Add(Distance_deg As Int16, TravelTime_sec As UInt16, DwellTime_sec As UInt16)
        Dim MoveData As New MemoryStream
        MoveData.Write(BitConverter.GetBytes(Distance_deg), 0, 2)
        MoveData.Write(BitConverter.GetBytes(TravelTime_sec), 0, 2)
        MoveData.Write(BitConverter.GetBytes(DwellTime_sec), 0, 2)
        SendCommandNoReturnData(CMD_PATH_ADD, MoveData.ToArray)
    End Sub

    Public Sub ECMD_Waypoint_Run()
        SendCommandNoReturnData(CMD_PATH_RUN, Nothing)
    End Sub

    Public Sub ECMD_ReturnToUI()
        SendCommandNoReturnData(CMD_EXIT, Nothing)
    End Sub

    Public Function GetUILocation() As UI_Location
        Dim ret As UI_Location
        ret = SendCommandWithReturnData(CMD_GET_UI_LOC, Nothing)(0)
        Return ret
    End Function
End Class


