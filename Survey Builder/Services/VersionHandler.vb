Imports System.Data.SqlClient

Public Class VersionHandler
    Implements IVersionHandler

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString

    Public Function GetVersion() As cVersion Implements IVersionHandler.GetVersion
        Dim VersionsDataTable As New DataTable
        Dim Version As cVersion = New cVersion
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select * from VersionTbl;"
                Dim myCommand As New SqlCommand(SQL, cn)
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(VersionsDataTable)
                da.Dispose()
            End Using
            Dim VersionRow As DataRow = VersionsDataTable.Rows(0)
            Version = New cVersion With {.VersionId = VersionRow.Item("VersionId"), .VersionName = VersionRow.Item("Version")}
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler3
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return Version
    End Function
End Class
