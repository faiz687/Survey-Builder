Imports System.Data.SqlClient

Public Class SurveyQuestionsHandler
    Implements ISurveyQuestion

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString



    Public Function GetAllQuesitons(SurveyID As Integer) As List(Of cSurveyQuestions) Implements ISurveyQuestion.GetAllQuesitons
        Dim QuestionsDatatable As New DataTable
        Dim QuestionsList As New List(Of cSurveyQuestions)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from QuestionsTbl where SurveyId=@SID ORDER BY DisplayId asc;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SurveyID))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(QuestionsDatatable)
                da.Dispose()
            End Using
            For Each QuestionsRow As DataRow In QuestionsDatatable.Rows
                QuestionsList.Add(New cSurveyQuestions With {.QuestionId = QuestionsRow.Item("QuestionId"), .Question = QuestionsRow.Item("Question"), .QuestionDescription = IIf(QuestionsRow.Item("QuestionDescription").Equals(DBNull.Value), "", QuestionsRow.Item("QuestionDescription")), .Mandatory = QuestionsRow.Item("Mandatory"), .Control = New cControl With {.ControlId = QuestionsRow.Item("ControlId")}, .QuestionCreatedDateTime = QuestionsRow.Item("QuestionCreatedDateTime"), .QuestionLastModifiedDateTime = QuestionsRow.Item("QuestionLastModifiedDateTime"), .Survey = New cSurvey With {.SurveyId = QuestionsRow.Item("SurveyId")}, .DisplayId = QuestionsRow.Item("DisplayId"), .Enabled = QuestionsRow.Item("Enabled")})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return QuestionsList
    End Function

    Public Function UpdateQuestionDisplayID(Question As cSurveyQuestions) As Boolean Implements ISurveyQuestion.UpdateQuestionDisplayID
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update QuestionsTbl set DisplayId=@DID where QuestionId =@QID"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@DID", Question.DisplayId))
                myCommand.Parameters.Add(New SqlParameter("@QID", Question.QuestionId))
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

    Public Function AddQuestionToSurvey(NewQuestion As cSurveyQuestions) As Integer Implements ISurveyQuestion.AddQuestionToSurvey
        Dim QuestionId As Integer = 0
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "insert into QuestionsTbl (Question,QuestionDescription,Mandatory,ControlId,QuestionCreatedDateTime,QuestionLastModifiedDateTime,SurveyId,DisplayId,Enabled) VALUES (@QT,@QTD,@MDT,@QCID,@QCDT,@QMDT,@SID,@DID,@ENL);Select SCOPE_IDENTITY()"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@QT", NewQuestion.Question))
                myCommand.Parameters.Add(New SqlParameter("@QTD", NewQuestion.QuestionDescription))
                myCommand.Parameters.Add(New SqlParameter("@MDT", NewQuestion.Mandatory))
                myCommand.Parameters.Add(New SqlParameter("@QCID", NewQuestion.Control.ControlId))
                myCommand.Parameters.Add(New SqlParameter("@QCDT", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@QMDT", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@SID", NewQuestion.Survey.SurveyId))
                myCommand.Parameters.Add(New SqlParameter("@DID", NewQuestion.DisplayId))
                myCommand.Parameters.Add(New SqlParameter("@ENL", NewQuestion.Enabled))
                myCommand.Connection.Open()
                QuestionId = myCommand.ExecuteScalar()
                myCommand.Connection.Close()
            End Using
            Return QuestionId
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
    End Function

    Public Function DeleteQuestionById(QuestionId As Integer) As Boolean Implements ISurveyQuestion.DeleteQuestionById
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from QuestionsTbl where QuestionId  = @QID "
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

    Public Function UpdateAllDisplayIdsFromQuestionId(DisplayId As Integer, SurveyId As Integer) As Boolean Implements ISurveyQuestion.UpdateAllDisplayIdsFromQuestionId
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update QuestionsTbl set DisplayId =  DisplayId - 1 where DisplayId > @DID and  SurveyId = @SID"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@DID", DisplayId))
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

    Public Function UpdateQuestionById(Question As cSurveyQuestions) As Boolean Implements ISurveyQuestion.UpdateQuestionById
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update QuestionsTbl set Question = @QT , QuestionDescription = @QTD , Mandatory = @MDT ,  ControlId = @QCID ,  QuestionLastModifiedDateTime = @QMDT ,SurveyId = @SID , DisplayId = @DID , Enabled =  @ENL  where QuestionId = @QID ;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@QT", Question.Question))
                myCommand.Parameters.Add(New SqlParameter("@QTD", Question.QuestionDescription))
                myCommand.Parameters.Add(New SqlParameter("@MDT", Question.Mandatory))
                myCommand.Parameters.Add(New SqlParameter("@QCID", Question.Control.ControlId))
                myCommand.Parameters.Add(New SqlParameter("@QMDT", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@SID", Question.Survey.SurveyId))
                myCommand.Parameters.Add(New SqlParameter("@DID", Question.DisplayId))
                myCommand.Parameters.Add(New SqlParameter("@ENL", Question.Enabled))
                myCommand.Parameters.Add(New SqlParameter("@QID", Question.QuestionId))
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

    Public Function DeleteAllQuestionById(SurveyId As Integer) As Boolean Implements ISurveyQuestion.DeleteAllQuestionById
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from QuestionsTbl where SurveyId =@SID  "
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID ", SurveyId))
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
