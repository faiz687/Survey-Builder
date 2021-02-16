Imports System.Linq
Imports Microsoft.Ajax.Utilities
Imports Telerik.Web.UI

Public Class MainMaster
    Inherits System.Web.UI.MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadNewVersionNumber()
            If IsUserSignedIn() Then
                LoadUserName()
            End If
        Else
        End If
        ShowMenuButtonPressed()
    End Sub

    Public Sub DisableUsernameLabel()
        WelcomeNameLabel.Visible = False
    End Sub

    Public Sub DisableAllNavigationButtons()
        For Each NavigationButton As Control In NavigationBarSection.Controls
            If TypeOf NavigationButton Is RadImageButton Then
                Dim Navigation_Button As RadImageButton = CType(NavigationButton, RadImageButton)
                Navigation_Button.Visible = False
            End If
        Next
    End Sub

    Private Sub LoadNewVersionNumber()
        Dim Version As IVersionHandler = New VersionHandler
        Dim Version_ As cVersion = Version.GetVersion()
        VersionLabel.Text = String.Format("Version {0}", Version_.VersionName)
    End Sub

    Private Sub ShowMenuButtonPressed()
        Dim UrlRequested As String = Request.RawUrl
        If UrlRequested.Contains("Survey") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ActivateMe", "ActivateMe('" + "Surveys" + "');", True)
        ElseIf UrlRequested.Contains("User") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ActivateMe", "ActivateMe('" + "User Accounts" + "');", True)
        ElseIf UrlRequested.Contains("Pin") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ActivateMe", "ActivateMe('" + "Pin Management" + "');", True)
        End If
    End Sub

    Private Function IsUserSignedIn() As Boolean
        If (Session("UserId") Is Nothing) Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub LoadUserName()
        If Not IsNothing(HttpContext.Current.Session("Forename")) Then
            WelcomeNameLabel.Text = String.Format("Welcome , <span> {0} </span>", HttpContext.Current.Session("Forename"))
            LoginLabelDiv.Style.Add("max-width", "initial")
            LoginLabelDiv.Style.Add("Text-align", "initial")
            WelcomeNameLabel.Style.Add("margin-left", "20px")
        End If
    End Sub

    Protected Sub UserAccountsButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("UserAccountPage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub SurveysButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("SurveysHomePage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub PinManagementButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("PinManagementPage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub LogoutButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        LogoutUser()
    End Sub

    Private Sub LogoutUser()
        HttpContext.Current.Session.Remove("UserId")
        HttpContext.Current.Session.Remove("PermissionId")
        HttpContext.Current.Session.Remove("Forename")
        HttpContext.Current.Session.Remove("Surname")
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub
End Class
