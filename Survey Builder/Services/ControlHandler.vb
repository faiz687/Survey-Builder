Imports System.Data.SqlClient

Public Class ControlHandler
    Implements IControlHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString
    Public Function GetAllControls() As List(Of cControl) Implements IControlHandler.GetAllControls
        Dim ControlsDatatable As New DataTable
        Dim ControlsList As New List(Of cControl)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from ControlTbl order by ControlName asc;"
                Dim myCommand As New SqlCommand(SQL, cn)
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(ControlsDatatable)
                da.Dispose()
            End Using
            For Each ControlRow As DataRow In ControlsDatatable.Rows
                ControlsList.Add(New cControl With {.ControlId = ControlRow.Item("ControlId"), .ControlName = ControlRow.Item("ControlName")})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return ControlsList
    End Function
End Class
