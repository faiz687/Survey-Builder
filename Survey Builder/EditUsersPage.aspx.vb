Public Class EditUsersPage
    Inherits System.Web.UI.Page
    Dim WithEvents UserHandler As IUserHandler
    Dim WithEvents Permssionhandler As IPermissionHandler

    Private UserId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                UserId = Integer.Parse(Request.QueryString("UserId"))
                LoadDropDownOptions()
                LoadUserDetails(UserId)
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Private Sub LoadDropDownOptions()
        Dim User_Permission As New PermissionHandler
        Me.Permssionhandler = User_Permission
        Dim PermissionList As List(Of cPermission) = Permssionhandler.GetAllPermission()
        PermissionDropDownList.DataSource = PermissionList
        PermissionDropDownList.DataBind()
    End Sub

    Private Sub LoadUserDetails(UserId As Integer)
        Dim User As New Userhandler
        UserHandler = User
        Dim UserDetails As cUser = UserHandler.GetUserById(UserId)
        UserIdLabel.Text = UserDetails.UserId
        UsernameTextBox.Text = UserDetails.UserName
        ForenameTextBox.Text = UserDetails.Forename
        SurnameTextBox.Text = UserDetails.Surname
        EmailTextBox.Text = UserDetails.Email
        For Index As Integer = 0 To PermissionDropDownList.Items.Count
            Dim a As cPermission = UserDetails.Permission
            If PermissionDropDownList.Items(Index).Text = UserDetails.Permission.PermissionName Then
                PermissionDropDownList.SelectedIndex = Index
                Exit For
            End If
        Next
        If UserDetails.Locked = "True" Then
            AccountStatusDropDownList.SelectedIndex = 1
        Else
            AccountStatusDropDownList.SelectedIndex = 0
        End If
    End Sub
    Protected Sub GoBackButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("AddOrEditUsersPage.aspx", True)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
    Protected Sub UpdateUserButton_Click(sender As Object, e As EventArgs)
        Dim User As New Userhandler
        UserHandler = User
        If UserHandler.UpdateUser(New cUser With {.UserId = UserIdLabel.Text, .Permission = New cPermission With {.PermissionID = PermissionDropDownList.SelectedValue}, .Locked = AccountStatusDropDownList.SelectedValue}) = True Then
            Notification.ShowSuccessNotification(Page, "User Updated")
        Else
            Notification.ShowFailNotification(Page, "User details cannot be updated now")
        End If
    End Sub
End Class