Public NotInheritable Class SliderValue
    Public Shared Function IsSliderRangeAccurate(StartingRange As String, EndingRange As String) As Boolean
        If (Not String.IsNullOrWhiteSpace(StartingRange)) And (Not String.IsNullOrWhiteSpace(EndingRange)) Then
            If Integer.Parse(EndingRange) <= Integer.Parse(EndingRange) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function IsSliderStepsAccurate(StartingRange As String, EndingRange As String, StepSize As String) As Boolean
        Dim CorrectStepsRange As Integer
        If (Not String.IsNullOrWhiteSpace(StartingRange)) And (Not String.IsNullOrWhiteSpace(EndingRange)) Then
            CorrectStepsRange = Integer.Parse(EndingRange) - Integer.Parse(StartingRange)
            If Not String.IsNullOrWhiteSpace(StepSize) Then
                If Integer.Parse(StepSize) >= 1 And Integer.Parse(StepSize) <= CorrectStepsRange Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function
End Class
