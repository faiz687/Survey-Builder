Imports System.Data.SqlClient

Public Class ResponsesHandler
    Implements IResponsesHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString
    Public Function GetAllResponses(QuestionId As Integer) As List(Of cResponses) Implements IResponsesHandler.GetAllResponses
        Dim ResponsesDatatable As New DataTable
        Dim ResponsesList As List(Of cResponses)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from ResponsesTbl where QuestionId=@QID;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@QID", QuestionId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(ResponsesDatatable)
                da.Dispose()
            End Using
            If ResponsesDatatable.Rows.Count > 0 Then
                ResponsesList = New List(Of cResponses)
                For Each ResponsesRow As DataRow In ResponsesDatatable.Rows
                    ResponsesList.Add(New cResponses With {.Response = ResponsesRow.Item("Response"), .ResponseId = ResponsesRow.Item("ResponseId"), .Question = New cSurveyQuestions With {.QuestionId = ResponsesRow.Item("QuestionId")}})
                Next
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return ResponsesList
    End Function

    Public Function AddResponseForQuestion(Response As cResponses) As Boolean Implements IResponsesHandler.AddResponseForQuestion
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "insert into ResponsesTbl (Response, QuestionId) values (@RES,@QID);"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@RES", Response.Response))
                myCommand.Parameters.Add(New SqlParameter("@QID", Response.Question.QuestionId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
            End Using
        Catch ex As Exception
            Return False
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return True
    End Function

    Public Function DeleteQuestionResponsesByQuestionID(QuestionId As Integer) As Boolean Implements IResponsesHandler.DeleteQuestionResponsesByQuestionID
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from ResponsesTbl where QuestionId=@QID ;"
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

    Public Function GetResponsesByQuestionIdAndControlId(QuestionId As Integer, ControlId As Integer) As List(Of cResponses) Implements IResponsesHandler.GetResponsesByQuestionIdAndControlId
        Dim ResponsesDatatable As New DataTable
        Dim ResponsesList As List(Of cResponses)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select Response , ResponsesTbl.Response ,  QuestionsTbl.ControlId from ResponsesTbl inner join QuestionsTbl on ResponsesTbl.QuestionId =  QuestionsTbl.QuestionId where ControlId=@CID and  QuestionsTbl.QuestionId=@QID ;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@CID", ControlId))
                myCommand.Parameters.Add(New SqlParameter("@QID", QuestionId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(ResponsesDatatable)
                da.Dispose()
            End Using
            If ResponsesDatatable.Rows.Count > 0 Then
                ResponsesList = New List(Of cResponses)
                For Each ResponsesRow As DataRow In ResponsesDatatable.Rows
                    ResponsesList.Add(New cResponses With {.Response = ResponsesRow.Item("Response")})
                Next
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return ResponsesList
    End Function

    Public Function DeleteAllQuestionResponsesBySurveyId(SurveyId As Integer) As Boolean Implements IResponsesHandler.DeleteAllQuestionResponsesBySurveyId
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from ResponsesTbl where QuestionId in  (select QuestionId from QuestionsTbl where SurveyId = @SID);;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SurveyId))
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
