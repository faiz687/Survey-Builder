Imports Telerik.Web.UI
Public Class AllSurveys
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Check.IsUserSignedIn(Me.Page) Then
                Dim MasterPage As MainMaster = Master
                MasterPage.DisableAllNavigationButtons()
                MasterPage.DisableUsernameLabel()
            End If
            LoadSurveyNamesComboBox()
        End If
    End Sub

    Private Sub LoadSurveyNamesComboBox()
        Dim Survey As ISurveyHandler = New SurveyHandler
        SurveyNameComboBox.DataSource = Survey.GetAllSurvey("")
        SurveyNameComboBox.DataBind()
    End Sub

    Protected Sub SurveysListView_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs)
        Dim Survey As ISurveyHandler = New SurveyHandler
        SurveysListView.DataSource = Survey.GetAllSurvey("")
    End Sub

    Protected Sub SurveyNameComboBox_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim SurveyName As String = e.Text
        SurveysListView.DataSource = Survey.GetAllSurvey(SurveyName)
        SurveysListView.DataBind()
    End Sub

    Protected Sub RefreshSurveyButton_Click(sender As Object, e As EventArgs)
        SurveysListView.Rebind()
        SurveyNameComboBox.ClearSelection()
    End Sub

    Protected Sub CancelOverlay_Click(sender As Object, e As EventArgs)
        OverlayPageSection.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "EnableMainBodyScroll();", True)
        SurveyNameComboBoxPin.ClearSelection()
        PinMessage.Visible = False
        PinTextBox.Text = ""
    End Sub

    Protected Sub EnterPinButton_Click(sender As Object, e As EventArgs)
        If Not SurveyNameComboBoxPin.SelectedIndex = -1 Then
            If Not String.IsNullOrWhiteSpace(PinTextBox.Text) Then
                If Not PinTextBox.Text.Length < 5 Then
                    Dim SurveyPin As ISurveyPinHandler = New SurveyPinHandler
                    Dim Survey_Pin As cSurveysPin = SurveyPin.GetSurveyPin(New cSurveysPin With {.PinNumber = PinTextBox.Text, .SurveyId = SurveyNameComboBoxPin.SelectedValue})
                    If Not IsNothing(Survey_Pin) Then
                        If IsSurveyPinValid(Survey_Pin) Then
                            SurveyPin.UpdatePinUsedGeneratedDateTime(Survey_Pin)
                            Response.Redirect(String.Format("SurveyPage.aspx?SurveyId={0}&SurveyName={1}", Survey_Pin.SurveyId, SurveyNameComboBoxPin.SelectedItem.Text), False)
                            Context.ApplicationInstance.CompleteRequest()
                        Else
                            PinMessage.Text = "Pin number expired"
                            PinMessage.Visible = True
                            Return
                        End If
                    End If
                End If
                PinMessage.Text = "Pin number invalid"
            Else
                PinMessage.Text = "Enter pin number"
            End If
        Else
            PinMessage.Text = "Select a survey"
        End If
        PinMessage.Visible = True
    End Sub

    Private Function IsSurveyPinValid(SurveyPin As cSurveysPin) As Boolean
        Dim SurveyPinGeneratedDateTime As DateTime = SurveyPin.PinGeneratedDatetime
        Dim ExceededGeneratedDatetime As DateTime = SurveyPinGeneratedDateTime.AddHours(24)
        Dim TodaysDateTime As DateTime = DateTime.Now
        If DateTime.Compare(ExceededGeneratedDatetime, TodaysDateTime) = 1 Then Return True
        Return False
    End Function

    Protected Sub ShowOverlayButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        OverlayPageSection.Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "DisableMainBodyScroll();", True)
        LoadSurveyNameComboBoxPin()
        SurveyNameComboBoxPin.Visible = True
    End Sub

    Private Sub LoadSurveyNameComboBoxPin()
        Dim Survey As ISurveyHandler = New SurveyHandler
        SurveyNameComboBoxPin.DataSource = Survey.GetAllSurvey("")
        SurveyNameComboBoxPin.DataBind()
    End Sub

    Protected Sub SurveyLink_ServerClick(sender As Object, e As EventArgs)
        Dim SurveyLinkButton As HtmlAnchor = DirectCast(sender, HtmlAnchor)
        Dim SurveyQuestionContainer As RadListViewDataItem = CType(SurveyLinkButton.NamingContainer, RadListViewDataItem)
        Dim SurveyName As String = CType(SurveyQuestionContainer.FindControl("SurveyNameLabel"), RadLabel).Text
        Response.Redirect(String.Format("SurveyPage.aspx?SurveyId={0}&SurveyName={1}", SurveyQuestionContainer.GetDataKeyValue("SurveyId"), SurveyName), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class
