Imports System.Data.SqlClient

Public Class AnswerHandler
    Implements IAnswersHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString

    Public Function InsertAnswer(Answer As cAnswer) As Boolean Implements IAnswersHandler.InsertAnswer
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "insert into AnswersTbl (AnswerId,QuestionId,Answer,AnswerDateTime) values (@ANSID,@QID,@ANS,@ANSDT);"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@ANSID", Answer.AnswerId))
                myCommand.Parameters.Add(New SqlParameter("@QID", Answer.QuestionId))
                myCommand.Parameters.Add(New SqlParameter("@ANS", Answer.Answer))
                myCommand.Parameters.Add(New SqlParameter("@ANSDT", Answer.AnswerDateTime))
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
