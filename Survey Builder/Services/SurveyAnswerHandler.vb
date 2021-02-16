Imports System.Data.SqlClient

Public Class SurveyAnswerHandler
    Implements ISurveyAnswerHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString

    Public Function InsertSurveyAnswer(SurveyAnswer As cSurveyAnswer) As Integer Implements ISurveyAnswerHandler.InsertSurveyAnswer
        Dim SurveyId As Integer = 0
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "insert into SurveyAnswer (SurveyId,SurveyAnswer.Platform, AppVersion ,SurveyVersion ) values (@SID,@PLT,@AVER,@SVER);SELECT SCOPE_IDENTITY();"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SurveyAnswer.SurveyId))
                myCommand.Parameters.Add(New SqlParameter("@PLT", SurveyAnswer.Platform))
                myCommand.Parameters.Add(New SqlParameter("@AVER", SurveyAnswer.AppVersion))
                myCommand.Parameters.Add(New SqlParameter("@SVER", SurveyAnswer.AppVersion))
                myCommand.Connection.Open()
                SurveyId = myCommand.ExecuteScalar()
                myCommand.Connection.Close()
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return SurveyId
    End Function
End Class
