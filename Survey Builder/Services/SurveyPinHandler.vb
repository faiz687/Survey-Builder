Imports System.Data.SqlClient

Public Class SurveyPinHandler
    Implements ISurveyPinHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString
    Public Function InsertPin(Pin As cSurveysPin) As Boolean Implements ISurveyPinHandler.InsertPin
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "INSERT INTO SurveysPinTbl (PinNumber , SurveyId ,  PinGeneratedDatetime)  VALUES (@PN,@SID,@PGDT);"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@PN", Pin.PinNumber))
                myCommand.Parameters.Add(New SqlParameter("@SID", Pin.SurveyId))
                myCommand.Parameters.Add(New SqlParameter("@PGDT", DateTime.Now))
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

    Public Function GetSurveyPin(Pin As cSurveysPin) As cSurveysPin Implements ISurveyPinHandler.GetSurveyPin
        Dim SurveysPinDatatable As New DataTable
        Dim SurveyPin As cSurveysPin
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from SurveysPinTbl where PinNumber = @PIN and SurveyId = @SID;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@PIN", Pin.PinNumber))
                myCommand.Parameters.Add(New SqlParameter("@SID", Pin.SurveyId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(SurveysPinDatatable)
                da.Dispose()
            End Using
            If SurveysPinDatatable.Rows.Count > 0 Then
                Dim SurveysPinDataRow As DataRow = SurveysPinDatatable.Rows(0)
                SurveyPin = New cSurveysPin With {.PinId = SurveysPinDataRow.Item("PinId"), .PinNumber = SurveysPinDataRow.Item("PinNumber"), .SurveyId = SurveysPinDataRow.Item("SurveyId"), .PinGeneratedDatetime = SurveysPinDataRow.Item("PinGeneratedDatetime")}
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return SurveyPin
    End Function

    Public Function UpdatePinUsedGeneratedDateTime(Pin As cSurveysPin) As Object Implements ISurveyPinHandler.UpdatePinUsedGeneratedDateTime
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update SurveysPinTbl set PinUsedDateTime = @PUDT  where PinId = @PID; "
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@PUDT", Pin.PinUsedDateTime))
                myCommand.Parameters.Add(New SqlParameter("@PID", Pin.PinId))
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

    Public Function DeleteSurveyPin(Pin As cSurveysPin) As Object Implements ISurveyPinHandler.DeleteSurveyPin
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "delete from SurveysPinTbl where PinId=@PIN "
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@PIN", Pin.PinId))
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
