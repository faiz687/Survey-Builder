Imports System.Data.SqlClient

Public Class SliderNumbersRangeHandler
    Implements ISliderNumbersRange

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString

    Public Function AddSliderProperties(SliderProperties As cSliderNumbersRange) As Boolean Implements ISliderNumbersRange.AddSliderProperties
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "insert into SliderNumbersRangeTbl (StartRange,EndRange,StepRange,NumericalTextBox,QuestionId) VALUES (@SR,@ER,@SS,@NTB,@QID);"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SR", SliderProperties.StartRange))
                myCommand.Parameters.Add(New SqlParameter("@ER", SliderProperties.EndRange))
                myCommand.Parameters.Add(New SqlParameter("@SS", SliderProperties.StepRange))
                myCommand.Parameters.Add(New SqlParameter("@NTB", SliderProperties.NumericalTextBox))
                myCommand.Parameters.Add(New SqlParameter("@QID", SliderProperties.QuestionId.QuestionId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
            End Using
            Return True
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
    End Function

    Public Function GetPropertiesForSlider(QuestionId As Integer) As cSliderNumbersRange Implements ISliderNumbersRange.GetPropertiesForSlider
        Dim dt As New DataTable
        Dim SliderProperties As cSliderNumbersRange
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select SliderId , StartRange , EndRange ,  StepRange , NumericalTextBox , QuestionId   from SliderNumbersRangeTbl where QuestionId=@QID ;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@QID", QuestionId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using
            If dt.Rows.Count > 0 Then
                SliderProperties = New cSliderNumbersRange With {.StartRange = dt.Rows(0).Item("StartRange"), .EndRange = dt.Rows(0).Item("EndRange"), .StepRange = dt.Rows(0).Item("StepRange"), .NumericalTextBox = dt.Rows(0).Item("NumericalTextBox"), .QuestionId = New cSurveyQuestions With {.QuestionId = dt.Rows(0).Item("QuestionId")}}
                Return SliderProperties
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return SliderProperties
    End Function

    Public Function DeleteSliderPropertiesById(QuestionId As Integer) As Boolean Implements ISliderNumbersRange.DeleteSliderPropertiesById
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from SliderNumbersRangeTbl where QuestionId=@QID;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@QID", QuestionId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
        Return True
    End Function
End Class
