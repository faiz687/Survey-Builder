Imports System.Globalization
Imports Telerik.Web.UI

Public Class ViewAllSurveysPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                LoadAllSurveys("")
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Else
        End If
    End Sub

    Private Sub LoadAllSurveys(SurveyName)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim SurveyQuestionsList As List(Of cSurvey) = Survey.GetAllSurveyEnabledDisabled(SurveyName)
        SurveysListView.DataSource = SurveyQuestionsList
        SurveysListView.DataBind()
    End Sub

    Protected Sub GoBackButton_Click(sender As Object, e As Telerik.Web.UI.ImageButtonClickEventArgs)
        Response.Redirect("SurveysHomePage.aspx", False)
        Context.ApplicationInstance.CompleteRequest()

    End Sub

    Protected Sub EditSurveyButton_Click(sender As Object, e As EventArgs)
        Dim EditSurveyButton As RadButton = sender
        Dim SurveyDataItem As RadListViewDataItem = EditSurveyButton.NamingContainer
        Dim SurveyId As String = SurveyDataItem.GetDataKeyValue("SurveyId")
        Dim SurveyName As RadLabel = TryCast(SurveyDataItem.FindControl("SurveyNameLabel"), RadLabel)
        Response.Redirect(String.Format("SurveyDesigningPage.aspx?SurveyId={0}&SurveyName={1}&PreviousPage={2}", SurveyId, SurveyName.Text, "ViewAllSurveysPage"), False)
        Context.ApplicationInstance.CompleteRequest()

    End Sub

    Protected Sub SurveysListView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs)
        If TypeOf e.Item Is RadListViewDataItem Then
            Dim SurveyContainer As RadListViewDataItem = e.Item
            Dim StatusDropDownList As RadDropDownList = SurveyContainer.FindControl("StatusDropDownList")
            Dim StatusLabel As RadLabel = SurveyContainer.FindControl("StatusLabel")
            If StatusLabel.Text = True Then
                StatusDropDownList.CssClass = StatusDropDownList.CssClass + " GreenBorder"
                StatusDropDownList.Items.Item(0).Selected = True
            Else
                StatusDropDownList.CssClass = StatusDropDownList.CssClass + " RedBorder"
                StatusDropDownList.Items.Item(1).Selected = True
            End If
        End If
    End Sub

    Protected Sub StatusDropDownList_SelectedIndexChanged1(sender As Object, e As DropDownListEventArgs)
        Dim SurveyContainer As RadListViewDataItem = CType(CType(sender, RadDropDownList).NamingContainer, RadListViewDataItem)
        Dim SurveyId As Integer = SurveyContainer.GetDataKeyValue("SurveyId")
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim SurveyStatus As Boolean
        If e.Value = 0 Then
            SurveyStatus = True
        Else
            SurveyStatus = False
        End If
        If Survey.UpdateSurveyStatus(New cSurvey With {.SurveyId = SurveyId, .Enabled = SurveyStatus}) = True Then
            LoadAllSurveys("")
            Notification.ShowSuccessNotification(Page, "Survey Updated")
        Else
            Notification.ShowFailNotification(Page, "Survey cannot be updated now , try again later")
        End If
    End Sub

    Protected Sub ConfirmSurveyDeleteButton_Click(sender As Object, e As EventArgs)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim SurveyQuestion As ISurveyQuestion = New SurveyQuestionsHandler
        Dim QuestionResponses As IResponsesHandler = New ResponsesHandler

        Dim SurveyId As Integer = SurveyIdHidden.Value

        If Survey.DeleteSurveyById(New cSurvey With {.SurveyId = SurveyId}) And SurveyQuestion.DeleteAllQuestionById(SurveyId) AndAlso QuestionResponses.DeleteAllQuestionResponsesBySurveyId(SurveyId) Then
            OverlayPageSection.Visible = False
            LoadAllSurveys("")
            SurveysListViewUpdatePanel.Update()
            Notification.ShowSuccessNotification(Page, "Survey Deleted")
        Else
            Notification.ShowFailNotification(Page, "Survey cannot be deleted , contact administrator")
        End If
    End Sub

    Protected Sub ShowOverlayButton_Click(sender As Object, e As EventArgs)
        Dim SurveyContainer As RadListViewDataItem = CType(CType(sender, RadButton).NamingContainer, RadListViewDataItem)
        Dim SurveyId As Integer = SurveyContainer.GetDataKeyValue("SurveyId")
        OverlayPageSection.Visible = True
        SurveyIdHidden.Value = SurveyId
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "DisableMainBodyScroll();", True)
    End Sub

    Protected Sub CancelOverlayButton_Click(sender As Object, e As EventArgs)
        OverlayPageSection.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "EnableMainBodyScroll();", True)
    End Sub
End Class
