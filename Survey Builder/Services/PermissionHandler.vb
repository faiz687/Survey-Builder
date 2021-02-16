Imports System.Data.SqlClient

Public Class PermissionHandler
    Implements IPermissionHandler

    Public Function GetAllPermissions() As List(Of cPermission) Implements IPermissionHandler.GetAllPermission
        Dim PermissionDatabale As New DataTable
        Dim PermissionsList As New List(Of cPermission)
        Try
            Using cn As New SqlConnection(ConfigurationManager.ConnectionStrings("DBConnection").ToString())
                Dim SQL As String = "select * from PermissionTbl order by PermissionName asc;"
                Dim myCommand As New SqlCommand(SQL, cn)
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(PermissionDatabale)
                da.Dispose()
            End Using
            For Each PermissionRow As DataRow In PermissionDatabale.Rows
                PermissionsList.Add(New cPermission With {.PermissionID = PermissionRow.Item("PermissionId"), .PermissionName = PermissionRow.Item("PermissionName")})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return PermissionsList

    End Function
End Class
