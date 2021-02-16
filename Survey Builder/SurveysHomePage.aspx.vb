Imports Telerik.Web.UI

Public Class SurveysHomePage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadPostBackButton()
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Else
        End If
    End Sub
    Private Sub LoadPostBackButton()
        ScriptManager.GetCurrent(Me).RegisterPostBackControl(CancelOverlay)
    End Sub
    Protected Sub ViewAllsurveys_ServerClick(sender As Object, e As EventArgs)
        Response.Redirect(String.Format("AllSurveys.aspx"), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
    Protected Sub editSurvey_ServerClick(sender As Object, e As EventArgs)
        Response.Redirect(String.Format("EditSurveysPage.aspx"), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
    Protected Sub DownloadSurveys_ServerClick(sender As Object, e As EventArgs)

    End Sub
    Protected Sub ViewDataAnalysis_ServerClick(sender As Object, e As EventArgs)

    End Sub
    Protected Sub CreateSurveyButton_Click(sender As Object, e As EventArgs)
        If (Not String.IsNullOrWhiteSpace(SurveyNameTextBox.Text)) Then
            Dim Surveys As ISurveyHandler = New SurveyHandler
            If Not Surveys.DoesSurveyExists(New cSurvey With {.SurveyName = SurveyNameTextBox.Text}) Then
                SurveyNameTakenLabel.Visible = False
                Dim SurveyId As Integer = Surveys.CreateSurvey(New cSurvey With {.SurveyName = SurveyNameTextBox.Text, .UserId = New cUser With {.UserId = Integer.Parse(HttpContext.Current.Session("UserId"))}, .SurveyVersion = 1})
                If Not SurveyId = 0 Then
                    Response.Redirect(String.Format("SurveyDesigningPage.aspx?SurveyId={0}&SurveyName={1}&PreviousPage={2}", SurveyId, SurveyNameTextBox.Text, "SurveysPage"), False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                End If
            Else
                SurveyNameTakenLabel.Visible = True
            End If
        Else
        End If
    End Sub
    Protected Sub ShowOverlay(sender As Object, e As EventArgs)
        OverlayPageSection.Visible = True
        Dim MainBody As HtmlGenericControl = Master.FindControl("SurveyBuilderBody")
        MainBody.Style.Add("overflow", "hidden")
    End Sub
    Protected Sub CancelOverlay_Click(sender As Object, e As EventArgs)
        OverlayPageSection.Visible = False
        Dim MainBody As HtmlGenericControl = Master.FindControl("SurveyBuilderBody")
        MainBody.Style.Add("overflow", "initial")
    End Sub

End Class
