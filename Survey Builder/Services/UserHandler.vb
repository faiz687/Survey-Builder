Imports System.Data.SqlClient

Public Class Userhandler
    Implements IUserHandler

    Private DbActiveDirectoryConnection As String = ConfigurationManager.ConnectionStrings("DBAD").ToString

    Private DbSurveyBuilderConnection As String = ConfigurationManager.ConnectionStrings("DBConnection").ToString
    Public Function GetAllUsers(Name As cUser_SearchOptions) As List(Of cUser) Implements IUserHandler.GetAllUsers
        Dim dt As New DataTable
        Dim mUser As New List(Of cUser)
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = ""
                If Not String.IsNullOrWhiteSpace(Name.Forename) And Not String.IsNullOrWhiteSpace(Name.Surname) Then
                    SQL = "Select Case UserId,UserName,Forename,Surname,Email, UsersTbl.PermissionId , PermissionName ,CreatedDateTime ,Locked,Deleted, SID from UsersTbl INNER JOIN  PermissionTbl On UsersTbl.PermissionId = PermissionTbl.PermissionId where Forename Like @FN + '%' and  Surname LIKE @SN + '%' order by Forename ASC"
                ElseIf Not String.IsNullOrWhiteSpace(Name.Forename) Then
                    SQL = "select UserId,UserName,Forename,Surname,Email, UsersTbl.PermissionId ,PermissionName ,CreatedDateTime ,Locked,Deleted, SID from UsersTbl INNER JOIN  PermissionTbl on UsersTbl.PermissionId = PermissionTbl.PermissionId where Forename LIKE  @FN + '%'  order by Forename ASC"
                ElseIf Not String.IsNullOrWhiteSpace(Name.Surname) Then
                    SQL = "select UserId,UserName,Forename,Surname,Email,UsersTbl.PermissionId  , PermissionName ,CreatedDateTime ,Locked,Deleted, SID from UsersTbl INNER JOIN  PermissionTbl On UsersTbl.PermissionId = PermissionTbl.PermissionId where Surname Like @SN + '%'  order by Surname ASC"
                Else
                    SQL = "select UserId,UserName,Forename,Surname,Email,UsersTbl.PermissionId  ,PermissionName ,CreatedDateTime ,Locked,Deleted, SID from UsersTbl INNER JOIN  PermissionTbl on UsersTbl.PermissionId = PermissionTbl.PermissionId order by Forename ASC"
                End If
                Dim myCommand As New SqlCommand(SQL, cn)
                If Not String.IsNullOrWhiteSpace(Name.Forename) Then
                    myCommand.Parameters.Add(New SqlParameter("@FN", Name.Forename))
                End If
                If Not String.IsNullOrWhiteSpace(Name.Surname) Then
                    myCommand.Parameters.Add(New SqlParameter("@SN", Name.Surname))
                End If
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using
            For Each dr As DataRow In dt.Rows
                mUser.Add(New cUser With {.UserId = dr.Item("UserId"), .UserName = dr.Item("UserName"), .Forename = dr.Item("Forename"), .Surname = dr.Item("Surname"), .Permission = New cPermission With {.PermissionID = dr.Item("PermissionId"), .PermissionName = dr.Item("PermissionName")}, .CreatedDateTime = dr.Item("CreatedDateTime"), .Deleted = dr.Item("Deleted"), .Locked = dr.Item("Locked"), .Email = dr.Item("Email"), .SID = dr.Item("SID")})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return mUser
    End Function

    Public Function GetUserById(UserId As Integer) As cUser Implements IUserHandler.GetUserById
        Dim dt As New DataTable
        Dim mUser As New cUser
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim SQL As String = "select UserId , UserName , Forename , Surname , Email , UsersTbl.PermissionId , PermissionName , CreatedDateTime , Locked , Deleted    from UsersTbl inner join PermissionTbl On UsersTbl.PermissionId = PermissionTbl.PermissionId where UserId=@UID;"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@UID", UserId))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using
            If dt.Rows.Count > 0 Then
                mUser = New cUser With {.UserId = dt.Rows(0).Item("UserId"), .UserName = dt.Rows(0).Item("UserName"), .Forename = dt.Rows(0).Item("Forename"), .Surname = dt.Rows(0).Item("Surname"), .Email = dt.Rows(0).Item("Email"), .Permission = New cPermission With {.PermissionID = dt.Rows(0).Item("PermissionId"), .PermissionName = dt.Rows(0).Item("PermissionName")}, .CreatedDateTime = dt.Rows(0).Item("CreatedDateTime"), .Locked = dt.Rows(0).Item("Locked"), .Deleted = dt.Rows(0).Item("Deleted")}
                Return mUser
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        'Return mUser
    End Function

    Public Function DeleteUser(UserId As Integer) As Boolean Implements IUserHandler.DeleteUser
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim queryString As String = "DELETE FROM UsersTbl WHERE UserId=@UID;"
                myCommand = New SqlCommand(queryString, cn)
                myCommand.Parameters.Add(New SqlParameter("@UID", UserId))
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

    Public Function GetActiveDirecotryUsers() As List(Of cUser) Implements IUserHandler.GetActiveDirecotryUsers
        Dim dt As New DataTable
        Dim mList As New List(Of cUser)

        Try
            Using cn As New SqlConnection(DbActiveDirectoryConnection)
                Dim SQL As String = "SELECT [SID], [Username], [Firstname], [Surname], [Email] FROM tblUserInformation WHERE ([HideFlag]=0 OR [HideFlag] IS NULL) AND ([Organisation] LIKE '%Coventry & Warwickshire%' OR [Organisation] LIKE '%coventry and warkshire%' OR [Organisation] LIKE '%Coventry and Warwickshire Partnership%' OR [Organisation] LIKE '%CWPT%') AND ([Email] IS NOT NULL AND [Firstname] IS NOT NULL AND [Surname] IS NOT NULL) ORDER BY [Firstname] ASC"
                Dim myCommand As New SqlCommand(SQL, cn)
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using

            For Each dr As DataRow In dt.Rows
                mList.Add(New cUser With {.SID = dr.Item("SID"), .Forename = dr.Item("Firstname"), .Surname = dr.Item("Surname"), .Email = dr.Item("Email"), .UserName = dr.Item("Email")})
            Next
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
        Return mList
    End Function

    Public Function GetActiveDirectoryUserBySID(SID As String) As cUser Implements IUserHandler.GetActiveDirectoryUserBySID
        Dim dt As New DataTable
        Dim mUser As New cUser

        Try
            Using cn As New SqlConnection(DbActiveDirectoryConnection)
                Dim SQL As String = "SELECT [SID], [Username], [Firstname], [Surname], [Email] FROM tblUserInformation WHERE [SID]=@SID"
                Dim myCommand As New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@SID", SID))
                Dim da As SqlDataAdapter = New SqlDataAdapter With {.SelectCommand = myCommand}
                da.Fill(dt)
                da.Dispose()
            End Using

            If dt.Rows.Count > 0 Then
                mUser = New cUser With {.SID = dt.Rows(0).Item("SID"), .Forename = dt.Rows(0).Item("Firstname"), .Surname = dt.Rows(0).Item("Surname"), .Email = dt.Rows(0).Item("Email"), .UserName = dt.Rows(0).Item("Username")}
            End If
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try

        Return mUser
    End Function

    Public Function UpdateUser(User As cUser) As Boolean Implements IUserHandler.UpdateUser
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "update UsersTbl set PermissionId=@PID, Locked=@LK where UserId=@UID"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@PID", User.Permission.PermissionID))
                myCommand.Parameters.Add(New SqlParameter("@LK", User.Locked))
                myCommand.Parameters.Add(New SqlParameter("@UID", User.UserId))
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

    Public Function AddUser(User As cUser) As Boolean Implements IUserHandler.AddUser
        Try
            Using cn As New SqlConnection(DbSurveyBuilderConnection)
                Dim myCommand As SqlCommand
                Dim SQL As String = "INSERT INTO UsersTbl ( UserName, Forename, Surname, Email, PermissionId, CreatedDateTime, Locked, Deleted, SID) VALUES ( @UN, @FN, @SN, @EM, @PID , @CDT , @LK , @DT, @SID)"
                myCommand = New SqlCommand(SQL, cn)
                myCommand.Parameters.Add(New SqlParameter("@UN", User.UserName))
                myCommand.Parameters.Add(New SqlParameter("@FN", User.Forename))
                myCommand.Parameters.Add(New SqlParameter("@SN", User.Surname))
                myCommand.Parameters.Add(New SqlParameter("@EM", User.Email))
                myCommand.Parameters.Add(New SqlParameter("@PID", User.Permission.PermissionID))
                myCommand.Parameters.Add(New SqlParameter("@CDT", User.CreatedDateTime))
                myCommand.Parameters.Add(New SqlParameter("@LK", User.Locked))
                myCommand.Parameters.Add(New SqlParameter("@DT", User.Deleted))
                myCommand.Parameters.Add(New SqlParameter("@SID", User.SID))
                myCommand.Connection.Open()
                myCommand.ExecuteNonQuery()
                myCommand.Connection.Close()
            End Using
        Catch ex As Exception
            'Dim ErrorHandler As IErrorHandler = New ErrorHandler
            'ErrorHandler.Add(New cError With {.FunctionDetail = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString, .Page = Me.GetType().BaseType.FullName, .Code = ex.HResult, .Description = ex.Message.ToString, .StackTrace = New StackTrace(ex).GetFrame(0).GetMethod().Name.ToString})
        End Try
    End Function
End Class
