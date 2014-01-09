Public Class CinefluxOrbitNackException
    Inherits Exception
    Shared Function GenerateNew(Resp() As Byte) As CinefluxOrbitNackException
        Return GenerateNew(Resp(0), Resp(1))
    End Function
    Shared Function GenerateNew(CmdID As Byte, ReasonCode As Byte) As CinefluxOrbitNackException
        Dim Msg As String
        Select Case CmdID
            Case &H1
                Msg = "CMD SET PRESET Failed:  Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Preset Number is out of range (0-4)"
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H2
                Msg = "CMD GET PRESET Failed: Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Preset Number is out of range (0-4)"
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H10
                Msg = "GET DISPLAY Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H11
                Msg = "Simulate Knob short press (Click) Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H12
                Msg = "Simulate Knob medium press (Back) Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H13
                Msg = "Simulate Knob long press (Cancel) Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H14
                Msg = "Simulate Knob clockwise turn (Increment) Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H15
                Msg = "Simulate Knob counter clockwise turn (Decrement) Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H16
                Msg = "Get Motor Position Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H17
                Msg = "Get Motor Speed Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H18
                Msg = "Get Battery Voltage Failed:  Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H19
                Msg = "Send UI Macro Function Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H1A
                Msg = "Get UI Menu Location Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H1B
                Msg = "Get Complete Status Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H60
                Msg = "Preload (Prep) Move Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H61
                Msg = "Execute Move Failed: Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Preload Move Data is not available.  Pre Load Move Trajectory before calling this function."
                    Case &H2
                        Msg += "The Motion Engine is already performing a move.  Ensure the controler is IDLE before attempting a Move."
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H62
                Msg = "Stop Command Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select

            Case &H63
                Msg = "Get Ext Status Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H64
                Msg = "Initialize (Reset) Waypoint Path Data Failed: Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Engine is currently executing a waypoint path.  Ensure the Engine is not executing a waypoint path.  (It can be executing a move)"
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H65
                Msg = "Add Waypoint Path Point Failed: Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Engine is currently executing a waypoint path.  Ensure the Engine is not executing a waypoint path.  (It can be executing a move)"
                    Case &H2
                        Msg += "The Maxium number of waypoints has been reached (100)."
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H66
                Msg = "Execute Waypoint Path Program Failed: Reason="
                Select Case ReasonCode
                    Case &H1
                        Msg += "Engine is not Idle. Ensure the engine is stopped and idle before executing this."
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case &H80
                Msg = "Exit External Mode Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
            Case Else
                Msg = "Unknown Command (" & Convert.ToString(CmdID, 16) & ") Failed: Reason="
                Select Case ReasonCode
                    Case &HFA
                        Msg += "CheckSum Failed"
                    Case &HFE
                        Msg += "Controller is in External Mode and this command requires User Mode"
                    Case &HFF
                        Msg += "Controller is in User Mode and this command requires External Mode"
                    Case Else
                        Msg += "UNKNOWN REASON(" & Convert.ToString(ReasonCode, 16) & ")"
                End Select
        End Select
        Return New CinefluxOrbitNackException(Msg)
    End Function

    Public Sub New(Msg As String)
        MyBase.New(Msg)
    End Sub

End Class

Public Class CinefluxOrbitSerialTimeoutException
    Inherits TimeoutException
End Class

Public Class CinefluxOrbitUnexpectedAckException
    Inherits Exception
    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub
End Class

Public Class CinefluxOrbitChecksumMismatchException
    Inherits Exception
    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub
End Class