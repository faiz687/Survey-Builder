Public Class WebForm1
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            DisableNavigationPanel()
        End If

    End Sub

    Private Sub DisableNavigationPanel()
        Dim NavigationSection As Control = Me.Master.FindControl("Nav1")
        NavigationSection.Visible = False
    End Sub

    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click
        If ValidateAllFields() Then

            Dim LoginHandler As ILoginHandler = New LoginHandler()
            Dim UserAccountStatus As cAccountStatus = LoginHandler.ValidateADCredentials(UserNameTextBox.Text, PasswordTextBox.Text, ConfigurationManager.AppSettings("Domain").ToString())

            If UserAccountStatus.Code = cAccountStatus.Status.Authorised Then
                Response.Redirect("UserAccountPage.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowIncorrectPasswordOrUsernameLabel", "ShowIncorrectPasswordOrUsernameLabel('" + UserAccountStatus.Message + "');", True)
            End If
        End If
    End Sub
    Private Function ValidateAllFields() As Boolean
        If UserNameTextBoxValidator.IsValid And PassswordTextBoxFieldValidator.IsValid Then
            If (Not String.IsNullOrWhiteSpace(UserNameTextBox.Text.ToString())) And (Not String.IsNullOrWhiteSpace(PasswordTextBox.Text.ToString())) Then
                Return True
            End If
        End If
        Return False
    End Function

End Class