Imports System.Data.SqlClient

Public Class SurveyHandler
    Implements ISurveyHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString
    Public Function DoesSurveyExists(Survey As cSurvey) As Boolean Implements ISurveyHandler.DoesSurveyExists
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "select SurveyId from SurveysTbl where SurveyName=@SN"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SN", Survey.SurveyName))
                cn.Open()
                Dim Reader As SqlDataReader = myCommand.ExecuteReader()
                If Reader.HasRows Then
                    Return True
                Else
                    Return False
                End If
                cn.Close()
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
    End Function
    Public Function CreateSurvey(Survey As cSurvey) As Integer Implements ISurveyHandler.CreateSurvey
        Dim SurveyId As Integer = 0
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "  INSERT INTO SurveysTbl(SurveyName  , SurveyCreatedDate, LastModifiedDate, UserId, Enabled ,SurveyVersion)
                 VALUES (@SN,@CDT,@LDT,@UID,@ENL,@VSN);SELECT SCOPE_IDENTITY()"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SN", Survey.SurveyName))
                myCommand.Parameters.Add(New SqlParameter("@CDT", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@LDT", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@UID", Survey.UserId.UserId))
                myCommand.Parameters.Add(New SqlParameter("@ENL", 1))
                myCommand.Parameters.Add(New SqlParameter("@VSN", Survey.SurveyVersion))
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
    Public Function GetAllSurvey(Survey As String) As List(Of cSurvey) Implements ISurveyHandler.GetAllSurvey
        Dim dt As New DataTable
        Dim AllSurveyList As New List(Of cSurvey)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = ""
                If Not String.IsNullOrWhiteSpace(Survey) Then
                    SQL = "Select * From SurveysTbl where SurveyName Like @SN +'%' and where Enabled=@ENL;"
                Else
                    SQL = "Select * From SurveysTbl where Enabled=@ENL;"
                End If
                Dim myCommand As New SqlCommand(SQL, cn)
                If Not String.IsNullOrWhiteSpace(Survey) Then
                    myCommand.Parameters.Add(New SqlParameter("@SN", Survey))
                End If
                myCommand.Parameters.Add(New SqlParameter("@ENL", 1))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using
            For Each dr As DataRow In dt.Rows
                AllSurveyList.Add(New cSurvey With {.SurveyId = dr.Item("SurveyId"), .SurveyName = dr.Item("SurveyName"), .SurveyCreatedDateTime = dr.Item("SurveyCreatedDate"), .LastModifiedDateTime = dr.Item("LastModifiedDate"), .Enabled = dr.Item("Enabled"), .UserId = New cUser With {.UserId = dr.Item("UserId")}})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return AllSurveyList
    End Function
    Public Function GetSurveyById(SurveyId As Integer) As cSurvey Implements ISurveyHandler.GetSurveyById
        Dim SurveyDatatable As New DataTable
        Dim Survey As cSurvey
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from SurveysTbl where SurveyId = @SID;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SurveyId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(SurveyDatatable)
                da.Dispose()
            End Using
            If SurveyDatatable.Rows.Count > 0 Then
                Dim SurveyDataRow As DataRow = SurveyDatatable.Rows(0)
                Survey = New cSurvey With {.SurveyId = SurveyDataRow.Item("SurveyId"), .UserId = SurveyDataRow.Item("UserId"), .Enabled = SurveyDataRow.Item("Enabled"), .SurveyName = SurveyDataRow.Item("SurveyName"), .SurveyCreatedDateTime = SurveyDataRow.Item("SurveyCreatedDate"), .LastModifiedDateTime = SurveyDataRow.Item("LastModifiedDate")}
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return Survey
    End Function
    Public Function UpdateSurveyName(Survey As cSurvey) As Boolean Implements ISurveyHandler.UpdateSurveyName
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update SurveysTbl set SurveyName=@SYN , LastModifiedDate=@LMD where SurveyId=@SID ;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SYN", Survey.SurveyName))
                myCommand.Parameters.Add(New SqlParameter("@LMD", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@SID", Survey.SurveyId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
                UpdateSurveyVersion(Survey)
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
        Return True
    End Function
    Public Function UpdateSurveyLastModifiedDate(Survey As cSurvey) As Boolean Implements ISurveyHandler.UpdateSurveyLastModifiedDate
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update SurveysTbl set LastModifiedDate = @LMD where SurveyId = @SID;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@LMD", DateTime.Now))
                myCommand.Parameters.Add(New SqlParameter("@SID", Survey.SurveyId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
                UpdateSurveyVersion(Survey)
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
        Return True
    End Function
    Public Function UpdateSurveyStatus(Survey As cSurvey) As Boolean Implements ISurveyHandler.UpdateSurveyStatus
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update SurveysTbl set Enabled=@EN where SurveyId=@SID"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@EN", Survey.Enabled))
                myCommand.Parameters.Add(New SqlParameter("@SID", Survey.SurveyId))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
                UpdateSurveyVersion(Survey)
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
            Return False
        End Try
        Return True
    End Function
    Public Function DeleteSurveyById(Survey As cSurvey) As Boolean Implements ISurveyHandler.DeleteSurveyById
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "  delete from SurveysTbl where SurveyId=@SID;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", Survey.SurveyId))
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
    Public Function UpdateSurveyVersion(Survey As cSurvey) As Boolean Implements ISurveyHandler.UpdateSurveyVersion
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update SurveysTbl  set  Version = (select SurveyVersion from SurveysTbl where SurveyId = @SID) + 1 where SurveyId = @SID;"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", Survey.SurveyId))
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
    Public Function GetSurveyVersion(SurveyId As Integer) As Integer Implements ISurveyHandler.GetSurveyVersion
        Dim SurveyDatatable As New DataTable
        Dim SurveyVersion As Integer = 0
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select SurveyVersion from SurveysTbl where SurveyId = @SID;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SurveyId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(SurveyDatatable)
                da.Dispose()
            End Using
            If SurveyDatatable.Rows.Count > 0 Then
                Dim SurveyDataRow As DataRow = SurveyDatatable.Rows(0)
                SurveyVersion = SurveyDataRow("SurveyVersion")
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return SurveyVersion
    End Function

    Public Function GetAllSurveyEnabledDisabled(SurveyName As String) As List(Of cSurvey) Implements ISurveyHandler.GetAllSurveyEnabledDisabled
        Dim dt As New DataTable
        Dim AllSurveyList As New List(Of cSurvey)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = ""
                If Not String.IsNullOrWhiteSpace(SurveyName) Then
                    SQL = "Select * From SurveysTbl where SurveyName Like @SN +'%';"
                Else
                    SQL = "Select * From SurveysTbl ;"
                End If
                Dim myCommand As New SqlCommand(SQL, cn)
                If Not String.IsNullOrWhiteSpace(SurveyName) Then
                    myCommand.Parameters.Add(New SqlParameter("@SN", SurveyName))
                End If
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using
            For Each dr As DataRow In dt.Rows
                AllSurveyList.Add(New cSurvey With {.SurveyId = dr.Item("SurveyId"), .SurveyName = dr.Item("SurveyName"), .SurveyCreatedDateTime = dr.Item("SurveyCreatedDate"), .LastModifiedDateTime = dr.Item("LastModifiedDate"), .Enabled = dr.Item("Enabled"), .UserId = New cUser With {.UserId = dr.Item("UserId")}})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return AllSurveyList
    End Function
End Class
