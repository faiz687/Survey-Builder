Imports System.Drawing
Imports Telerik.Web.UI

Public Class Users
    Inherits System.Web.UI.Page
    Private WithEvents UserHandler As IUserHandler
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                LoadUserInformation(ForenameTextBox.Text, SurnameTextBox.Text)
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
        PreviousPageCheck()
    End Sub

    Private Sub PreviousPageCheck()
        Dim Userclciked As String = Request.QueryString("PreviousPage")
        If Userclciked = "DeleteUser" Then
            RadGrid1.MasterTableView.GetColumn("EditColumn").Display = False
        Else
            RadGrid1.MasterTableView.GetColumn("DeleteColumn").Display = False
        End If
    End Sub

    Private Sub LoadUserInformation(ByVal Optional Forename As String = "", ByVal Optional Surname As String = "")
        Dim User_Handler As New Userhandler
        UserHandler = User_Handler

        Dim mUserList As List(Of cUser) = UserHandler.GetAllUsers(New cUser_SearchOptions With {.Forename = Forename, .Surname = Surname})

        RadGrid1.DataSource = mUserList
        RadGrid1.DataBind()
    End Sub

    Public Sub EditButton_Click1(sender As Object, e As ImageButtonClickEventArgs)
        Dim mButton As RadImageButton = sender
        Dim mDataItem As GridDataItem = mButton.NamingContainer
        Dim mUserID As String = mDataItem.GetDataKeyValue("UserId")

        Response.Redirect(String.Format("EditUsersPage.aspx?UserId={0}", mUserID), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Private Sub RadGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadGrid1.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim Item As GridDataItem = CType(e.Item, GridDataItem)
            Dim Cell As TableCell = Item("LockedColumn")
            Dim Statuslabel As Label = CType(Cell.FindControl("StatusLabel"), Label)
            Dim Statusdiv As HtmlGenericControl = CType(Cell.FindControl("StatusDiv"), HtmlGenericControl)
            If Statuslabel.Text = "True" Then
                Statusdiv.Style("background-color") = "red"
                Statuslabel.Text = "Disabled"
            Else
                Statusdiv.Style("background-color") = "#00c900"
                Statuslabel.Text = "Enabled"
            End If
        End If
    End Sub

    Protected Sub GoBackButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        Response.Redirect("UserAccountPage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
    Protected Sub SearchButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        LoadUserInformation(ForenameTextBox.Text, SurnameTextBox.Text)
    End Sub
    Protected Sub DeleteButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        Dim mButton As RadImageButton = sender
        Dim mDataItem As GridDataItem = mButton.NamingContainer
        Dim UserID As String = mDataItem.GetDataKeyValue("UserId")
        Dim Cell As TableCell = mDataItem("FullNameColumn")
        Dim UserFullName As String = Cell.Text.ToString()
        DeleteMessageDescription.Text = "Are you sure you want to delete " + UserFullName.ToString() + ". The user will be permanently removed from the system and would no longer be able to avail its services."
        DeleteSuccessfullOrFailPanel.Visible = False
        DeletePanel.Visible = True
        DeleteConfirmationPanel.Visible = True
        UserIdLabel.Text = UserID
    End Sub

    Private Sub RadGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
        Dim User_Handler As New Userhandler
        UserHandler = User_Handler

        Dim mUserList As List(Of cUser) = UserHandler.GetAllUsers(New cUser_SearchOptions With {.Forename = ForenameTextBox.Text, .Surname = SurnameTextBox.Text})

        RadGrid1.DataSource = mUserList
    End Sub
    Protected Sub CancelButton_Click(sender As Object, e As EventArgs)
        DeletePanel.Visible = False
    End Sub
    Protected Sub ConfirmDeleteButton_Click(sender As Object, e As EventArgs)
        Dim User_Handler As New Userhandler
        UserHandler = User_Handler
        DeleteConfirmationPanel.Visible = False
        DeleteSuccessfullOrFailPanel.Visible = True
        If (UserHandler.DeleteUser(Integer.Parse(UserIdLabel.Text.ToString()))) Then
            DeleteMessagelabel.Text = "User account has been deleted from the system."
            DeleteMessagelabel.ForeColor = Color.Green
            LoadUserInformation()
        Else
            DeleteMessagelabel.Text = "User account could not Be Deleted From the System , try again later."
            DeleteMessagelabel.ForeColor = Color.Red
        End If
    End Sub
    Protected Sub OkButton_Click(sender As Object, e As EventArgs)
        DeleteSuccessfullOrFailPanel.Visible = False
        DeletePanel.Visible = False
    End Sub

    Protected Sub AddUserButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        Response.Redirect("AddUserPage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class