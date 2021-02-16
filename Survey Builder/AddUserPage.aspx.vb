Imports Telerik.Web.UI

Public Class AddUserPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                LoadDropDownOptions()
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Private Sub LoadDropDownOptions()
        Dim UserPermissions As IPermissionHandler = New PermissionHandler
        Dim UserPermissionList As List(Of cPermission) = UserPermissions.GetAllPermission()

        PermissionDropDownList.DataSource = UserPermissionList
        PermissionDropDownList.DataBind()
    End Sub

    Protected Sub GoBackButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("AddOrEditUsersPage.aspx", True)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub UsernameCombobox_ItemsRequested1(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        Dim UserHandler As IUserHandler = New Userhandler
        Dim ActiveDirectoryUsers As List(Of cUser) = UserHandler.GetActiveDirecotryUsers()
        UsernameCombobox.DataSource = ActiveDirectoryUsers
        UsernameCombobox.DataBind()
    End Sub

    Private Sub UsernameCombobox_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles UsernameCombobox.SelectedIndexChanged
        Dim Userhandler As IUserHandler = New Userhandler
        Dim User As cUser = Userhandler.GetActiveDirectoryUserBySID(e.Value)
        UsernameTextBox.Text = User.UserName
        ForenameTextBox.Text = User.Forename
        SurnameTextBox.Text = User.Surname
        EmailTextBox.Text = User.Email
        UserSIDLabel.Text = User.SID
    End Sub

    Protected Sub SaveButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        Dim Userhandler As IUserHandler = New Userhandler
        If Userhandler.AddUser(New cUser With {.SID = UserSIDLabel.Text, .UserName = UsernameTextBox.Text, .Forename = ForenameTextBox.Text, .Surname = SurnameTextBox.Text, .Email = EmailTextBox.Text, .Permission = New cPermission With {.PermissionID = PermissionDropDownList.SelectedValue}, .CreatedDateTime = DateTime.Now, .Locked = 0, .Deleted = 0}) = True Then
            Notification.ShowSuccessNotification(Page, "User added")
        Else
            Notification.ShowFailNotification(Page, "User cannot be added at this time , try again later")
        End If
    End Sub
End Class