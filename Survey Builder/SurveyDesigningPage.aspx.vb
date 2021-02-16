Imports System.CodeDom
Imports Microsoft.Ajax.Utilities
Imports Telerik.Web.UI

Public Class SurveyDesigningPage
    Inherits System.Web.UI.Page
    Public Shared ResponsesChoiceList As List(Of String) = New List(Of String)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                LoadSurveyName()
                LoadAllQuestions()
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Private Sub LoadSurveyName()
        SurveyNameLabel.Text = Request.QueryString("SurveyName")
    End Sub

    Private Sub LoadAllQuestions()
        Dim SurveyQuestions As ISurveyQuestion = New SurveyQuestionsHandler
        Dim SurveyQuestionsList As List(Of cSurveyQuestions) = SurveyQuestions.GetAllQuesitons(Request.QueryString("SurveyId"))
        QuestionsListView.DataSource = SurveyQuestionsList
        QuestionsListView.DataBind()
    End Sub

    Protected Sub SurveyNameEditButton_Click(sender As Object, e As EventArgs)
        SurveyNameLabel.Visible = False
        SurveyNameTextbox.Text = SurveyNameLabel.Text
        SurveyNameTextbox.Visible = True
        SurveyNameCancelButton.Visible = True
        SurveyNameSaveButton.Visible = True

    End Sub

    Protected Sub SurveyNameSaveButton_Click(sender As Object, e As EventArgs)
        If Not String.IsNullOrWhiteSpace(SurveyNameTextbox.Text) Then
            Dim Surveys As ISurveyHandler = New SurveyHandler
            If Surveys.UpdateSurveyName(New cSurvey With {.SurveyName = SurveyNameTextbox.Text.Trim(), .SurveyId = Request.QueryString("SurveyId")}) = True Then
                Notification.ShowSuccessNotification(Page, "Survey name updated")
                Surveys.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                SurveyNameLabel.Visible = True
                SurveyNameLabel.Text = SurveyNameTextbox.Text
                SurveyNameCancelButton.Visible = False
                SurveyNameSaveButton.Visible = False
                SurveyNameTextbox.Visible = False
            End If
        Else
            SurveyNameLabel.Visible = True
            SurveyNameLabel.Text = SurveyNameTextbox.Text
            SurveyNameCancelButton.Visible = False
            SurveyNameSaveButton.Visible = False
            SurveyNameTextbox.Visible = False
        End If
    End Sub

    Protected Sub SurveyNameCancelButton_Click1(sender As Object, e As EventArgs)
        SurveyNameLabel.Visible = True
        SurveyNameCancelButton.Visible = False
        SurveyNameSaveButton.Visible = False
        SurveyNameTextbox.Visible = False
    End Sub

    Private Sub QuestionsListView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles QuestionsListView.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then
            Dim EditControlBox As RadComboBox = CType(e.Item.FindControl("EditControlsComboBox"), RadComboBox)
            LoadControlsDropDown(EditControlBox)
            Dim ControlIdLabel As RadLabel = CType(e.Item.FindControl("ControlIdLabel"), RadLabel)
            Dim QuestionId As RadLabel = CType(e.Item.FindControl("QuestionIdLabel"), RadLabel)
            Dim Responsepanel As Panel = CType(e.Item.FindControl("ResponsesControlPanel"), Panel)
            Dim QuestionNumberLabel As RadLabel = CType(e.Item.FindControl("QuestionNumberLabel"), RadLabel)
            If QuestionNumberLabel.Text = 1 Then
                Dim MoveUpButton As RadImageButton = CType(e.Item.FindControl("MoveUpButton"), RadImageButton)
                MoveUpButton.Visible = False
            End If
            GenerateControlsForEachQuestion(Integer.Parse(ControlIdLabel.Text), Integer.Parse(QuestionId.Text), Responsepanel)
        Else
        End If
    End Sub

    Private Sub GenerateControlsForEachQuestion(TypeOfControl As Integer, QuesitonId As Integer, Responsepanel As Panel)
        Dim Responses As IResponsesHandler = New ResponsesHandler
        Select Case TypeOfControl
            Case 1
                'textbox
                Dim Text_Box As New RadTextBox With {.Width = Unit.Parse("30em"), .Height = Unit.Parse("2.3em"), .CssClass = "ResponsesTextBox SingleTextBox", .TextMode = TextBoxMode.SingleLine}
                Text_Box.ClientEvents.OnKeyPress = "ValidateKeyPress"
                Responsepanel.Controls.Add(Text_Box)
            Case 2
                'commentbox
                Dim Text_Box As New RadTextBox With {.Width = Unit.Parse("30em"), .EnableViewState = True, .Height = Unit.Parse("6em"), .CssClass = "ResponsesTextBox", .TextMode = TextBoxMode.MultiLine}
                Text_Box.ClientEvents.OnKeyPress = "ValidateKeyPress"
                Responsepanel.Controls.Add(Text_Box)
            Case 3
                'Slider 
                Dim Slider As New RadSlider With {.ID = "RadSlider", .TrackPosition = SliderTrackPosition.BottomRight, .CssClass = "SliderCssClass", .ShowDragHandle = True, .RenderMode = RenderMode.Lightweight, .Width = Unit.Parse("800px"), .Height = Unit.Parse("60px"), .ShowIncreaseHandle = False, .ShowDecreaseHandle = False}
                Dim ResponsesList As List(Of cResponses) = Responses.GetAllResponses(QuesitonId)
                If Not IsNothing(ResponsesList) Then
                    Slider.Height = Unit.Parse("80px")
                    Slider.ItemType = SliderItemType.Item
                    Slider.TrackPosition = SliderTrackPosition.Center
                    For Each SliderItemResponse As cResponses In ResponsesList
                        Dim SliderItem As New RadSliderItem With {.Text = SliderItemResponse.Response, .Value = SliderItemResponse.ResponseId.ToString()}
                        Slider.Items.Add(SliderItem)
                    Next
                Else
                    Dim SliderPropertiesHandler As ISliderNumbersRange = New SliderNumbersRangeHandler
                    Dim SliderNumberRange As cSliderNumbersRange = SliderPropertiesHandler.GetPropertiesForSlider(QuesitonId)
                    If Not IsNothing(SliderNumberRange) Then
                        With Slider
                            .ItemType = SliderItemType.Tick
                            .MinimumValue = SliderNumberRange.StartRange
                            .MaximumValue = SliderNumberRange.EndRange
                            .SmallChange = SliderNumberRange.StepRange
                            .LargeChange = SliderNumberRange.EndRange - SliderNumberRange.StartRange
                            If SliderNumberRange.NumericalTextBox = True Then
                                Dim NumericalTextBox As New RadTextBox With {.ID = "NumericalTextBox", .WrapperCssClass = "SliderNumericalTextBoxOuter", .BackColor = System.Drawing.Color.FromArgb(192, 192, 192), .CssClass = "SliderNumericalTextBox", .Width = Unit.Parse("3em")}
                                Slider.OnClientValueChanged = "ShowSliderValueOnNumericalTextBox"
                                Responsepanel.Controls.Add(NumericalTextBox)
                            End If
                        End With
                    End If
                End If
                Responsepanel.Controls.Add(Slider)
            Case 4
                'CheckBoxList
                Dim CheckBoxList As New RadCheckBoxList With {.ID = "RadCheckBoxList", .AutoPostBack = False, .Width = Unit.Percentage(100)}
                Dim ResponsesList As List(Of cResponses) = Responses.GetAllResponses(QuesitonId)
                If Not IsNothing(ResponsesList) Then
                    For Index As Integer = 0 To ResponsesList.Count - 1
                        Dim RadCheckBoxItem As New ButtonListItem With {.Text = ResponsesList(Index).Response, .Value = ResponsesList(Index).ResponseId}
                        CheckBoxList.Items.Add(RadCheckBoxItem)
                        Responsepanel.Controls.Add(CheckBoxList)
                    Next
                End If
            Case 5
                'RadioButtonList
                Dim RadioButtonList1 As New RadRadioButtonList With {.ID = "RadRadioButtonList", .AutoPostBack = False, .Width = Unit.Percentage(100)}
                Dim ResponsesList As List(Of cResponses) = Responses.GetAllResponses(QuesitonId)
                If Not IsNothing(ResponsesList) Then
                    For Index As Integer = 0 To ResponsesList.Count - 1
                        Dim RadRadioButtonItem As New ButtonListItem With {.Text = ResponsesList(Index).Response, .Value = ResponsesList(Index).ResponseId}
                        RadioButtonList1.Items.Add(RadRadioButtonItem)
                        Responsepanel.Controls.Add(RadioButtonList1)
                    Next
                End If
        End Select
    End Sub

    Protected Sub RearrangeQuestion(sender As Object, e As CommandEventArgs)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim RearrangeButton As RadImageButton = CType(sender, RadImageButton)
        Dim RearrangeButtonContainer As RadListViewDataItem = RearrangeButton.NamingContainer
        Dim QuestionId As Integer = RearrangeButtonContainer.GetDataKeyValue("QuestionId")

        Dim QuestionIdSwappingQuestion As Integer
        Dim NewDisplayIdOfQuestionClickedOn As Integer
        Dim NewDisplayIdOfQuestionToBeSwappedWith As Integer = RearrangeButtonContainer.GetDataKeyValue("DisplayId")

        If e.CommandArgument.ToString = "MoveUp" Then
            NewDisplayIdOfQuestionClickedOn = RearrangeButtonContainer.GetDataKeyValue("DisplayId") - 1
            QuestionIdSwappingQuestion = QuestionsListView.Items.Item(RearrangeButtonContainer.GetDataKeyValue("DisplayId") - 2).GetDataKeyValue("QuestionId")
        Else
            NewDisplayIdOfQuestionClickedOn = RearrangeButtonContainer.GetDataKeyValue("DisplayId") + 1
            QuestionIdSwappingQuestion = QuestionsListView.Items.Item(RearrangeButtonContainer.GetDataKeyValue("DisplayId")).GetDataKeyValue("QuestionId")
        End If

        Dim Question As ISurveyQuestion = New SurveyQuestionsHandler()
        Question.UpdateQuestionDisplayID(New cSurveyQuestions With {.QuestionId = QuestionId, .DisplayId = NewDisplayIdOfQuestionClickedOn})
        Question.UpdateQuestionDisplayID(New cSurveyQuestions With {.QuestionId = QuestionIdSwappingQuestion, .DisplayId = NewDisplayIdOfQuestionToBeSwappedWith})
        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
        LoadAllQuestions()
        QuestionsUpdatePanel.Update()
    End Sub

    Private Sub QuestionsListView_PreRender(sender As Object, e As EventArgs) Handles QuestionsListView.PreRender
        If QuestionsListView.Items.Count >= 1 Then
            QuestionsListView.Items.Item(QuestionsListView.Items.Count - 1).FindControl("MoveDownButton").Visible = False
        End If
    End Sub

    Protected Sub GoBackButton_Click2(sender As Object, e As ImageButtonClickEventArgs)
        Dim PreviousPage As String = Request.QueryString("PreviousPage")
        If PreviousPage = "SurveysPage" Then
            Response.Redirect("SurveysHomePage.aspx", True)
            Context.ApplicationInstance.CompleteRequest()
        Else
            Response.Redirect("EditSurveysPage.aspx", True)
            Context.ApplicationInstance.CompleteRequest()
        End If

    End Sub

    Private Sub LoadControlsDropDown(ControlCombobox As RadComboBox)
        Dim Control As IControlHandler = New ControlHandler
        Dim ControlsList As List(Of cControl) = Control.GetAllControls()

        ControlCombobox.DataSource = ControlsList
        ControlCombobox.DataBind()
    End Sub

    Protected Sub ControlsCombobox_ItemDataBound(sender As Object, e As RadComboBoxItemEventArgs)
        Select Case e.Item.Text
            Case "Radio button"
                e.Item.ImageUrl = "\Icons\ControlIcons\RadioButtonsIcon.PNG"
            Case "Multiple check boxes"
                e.Item.ImageUrl = "Icons\ControlIcons\CheckBoxIcon.PNG"
            Case "Comment box"
                e.Item.ImageUrl = "Icons\ControlIcons\CommentBoxIcon.PNG"
            Case "Slider"
                e.Item.ImageUrl = "\Icons\ControlIcons\SliderIcon.PNG"
            Case "Single Text box"
                e.Item.ImageUrl = "\Icons\ControlIcons\SingleTextBoxIcon.PNG"
        End Select
    End Sub

    Protected Sub NewQuestionButton_Click(sender As Object, e As ImageButtonClickEventArgs)
        LoadAllQuestions()
        QuestionsUpdatePanel.Update()
        NewQuestionPanel.Visible = True
        NewQuestionNumeberLabel.Text = String.Format("Q{0}.", QuestionsListView.Items.Count + 1)
        Dim ControlDropDownBox As RadComboBox = CType(NewQuestionPanel.FindControl("ControlsCombobox"), RadComboBox)
        LoadControlsDropDown(ControlDropDownBox)
    End Sub

    Protected Sub ControlsCombobox_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        Dim ControlsCombobox As RadComboBox = CType(sender, RadComboBox)
        Dim AllControlsPanels As HtmlGenericControl
        Dim IndividualQuestionUpdatePanel As UpdatePanel
        If ControlsCombobox.ID = "EditControlsComboBox" Then
            Dim ControlsComboboxContainer As RadListViewDataItem
            ControlsComboboxContainer = ControlsCombobox.NamingContainer
            IndividualQuestionUpdatePanel = CType(ControlsComboboxContainer.FindControl("IndividualQuestionUpdatePanel"), UpdatePanel)
            AllControlsPanels = ControlsComboboxContainer.FindControl("ControlPanelsEdit")
            If e.Value = 3 Or e.Value = 4 Or e.Value = 5 Then
                ShowResponsesInEditingPanel(ControlsComboboxContainer, e.Value)
            End If
        Else
            AllControlsPanels = ControlPanels
        End If
        For Each Control As Control In AllControlsPanels.Controls
            If TypeOf Control Is Panel Then
                Dim ControlPanel As Panel = DirectCast(Control, Panel)
                ControlPanel.Visible = False
            End If
        Next
        Select Case e.Value
            Case 1
                If ControlsCombobox.ID = "EditControlsComboBox" Then
                    Dim SingleTextBoxPanel As Panel = CType(AllControlsPanels.FindControl("SingleTextBoxPanelEdit"), Panel)
                    SingleTextBoxPanel.Visible = True
                Else
                    SingleTextBoxPanel.Visible = True
                End If
            Case 2
                If ControlsCombobox.ID = "EditControlsComboBox" Then
                    Dim CommentBoxPanel As Panel = CType(AllControlsPanels.FindControl("CommentBoxPanelEdit"), Panel)
                    CommentBoxPanel.Visible = True
                Else
                    CommentBoxPanel.Visible = True
                End If
            Case 3
                If ControlsCombobox.ID = "EditControlsComboBox" Then
                    Dim SliderPanel As Panel = CType(AllControlsPanels.FindControl("SliderPanelEdit"), Panel)
                    SliderPanel.Visible = True
                Else
                    SliderPanel.Visible = True
                End If
            Case 4
                If ControlsCombobox.ID = "EditControlsComboBox" Then
                    Dim MultipleCheckBoxPanel As Panel = CType(AllControlsPanels.FindControl("MultipleCheckBoxPanelEdit"), Panel)
                    MultipleCheckBoxPanel.Visible = True
                Else
                    MultipleCheckBoxPanel.Visible = True
                End If
            Case 5
                If ControlsCombobox.ID = "EditControlsComboBox" Then
                    Dim RadioButtonPanel As Panel = CType(AllControlsPanels.FindControl("RadioButtonPanelEdit"), Panel)
                    RadioButtonPanel.Visible = True

                Else
                    RadioButtonPanel.Visible = True
                End If
        End Select
        If ControlsCombobox.ID = "EditControlsComboBox" Then
            IndividualQuestionUpdatePanel.Update()
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function SaveAllResponses(ByVal ResponsesList)
        For Each AnswerChoices As String In ResponsesList
            If Not String.IsNullOrWhiteSpace(AnswerChoices.ToString()) Then
                Debug.WriteLine(AnswerChoices)
                SurveyDesigningPage.ResponsesChoiceList.Add(AnswerChoices)
            End If
        Next
    End Function

    Protected Sub SaveResponses()
        If Not String.IsNullOrWhiteSpace(NewQuestionTextBox.Text) Then
            Dim Survey As ISurveyHandler = New SurveyHandler
            Dim NewSurveyQuestion As ISurveyQuestion = New SurveyQuestionsHandler
            Dim NewQuestionResponses As IResponsesHandler = New ResponsesHandler
            If ControlsCombobox.SelectedValue = 4 Or ControlsCombobox.SelectedValue = 5 Then
                If ResponsesChoiceList.Count >= 2 Then
                    Dim NewSurveyQuestionId As Integer = NewSurveyQuestion.AddQuestionToSurvey(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(ControlsCombobox.SelectedValue)}, .Mandatory = IIf(MandatoryQuestionCheck.Checked, 1, 0), .Question = NewQuestionTextBox.Text.Trim(), .DisplayId = QuestionsListView.Items.Count + 1, .Survey = New cSurvey With {.SurveyId = Integer.Parse(Request.QueryString("SurveyId"))}, .QuestionDescription = NewQuestionDescriptionTextBox.Text.Trim(), .Enabled = True})
                    For Each Response As String In ResponsesChoiceList
                        NewQuestionResponses.AddResponseForQuestion(New cResponses With {.Question = New cSurveyQuestions With {.QuestionId = NewSurveyQuestionId}, .Response = Response.Trim()})
                    Next
                    Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                    RefreshAllQuestions()
                    Notification.ShowSuccessNotification(Page, "Question Added")
                Else
                    ShowMinimumTwoRepsonsesLabel()
                End If
            ElseIf ControlsCombobox.SelectedValue = 3 Then
                If SliderOptionDropdown.SelectedValue = "1" Then
                    If ResponsesChoiceList.Count >= 2 Then
                        Dim newsurveyquestionid As Integer = NewSurveyQuestion.AddQuestionToSurvey(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(ControlsCombobox.SelectedValue)}, .Mandatory = IIf(MandatoryQuestionCheck.Checked, 1, 0), .Question = NewQuestionTextBox.Text.Trim(), .DisplayId = QuestionsListView.Items.Count + 1, .Survey = New cSurvey With {.SurveyId = Integer.Parse(Request.QueryString("surveyid"))}, .QuestionDescription = NewQuestionDescriptionTextBox.Text, .Enabled = True})
                        For Each Response As String In ResponsesChoiceList
                            NewQuestionResponses.AddResponseForQuestion(New cResponses With {.Question = New cSurveyQuestions With {.QuestionId = newsurveyquestionid}, .Response = Response.Trim()})
                        Next
                        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                        RefreshAllQuestions()
                        ResponsesChoiceList.Clear()
                        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                    End If
                Else
                    If SliderValue.IsSliderRangeAccurate(StartingRangeBox.Text, EndingRangeBox.Text) And SliderValue.IsSliderStepsAccurate(StartingRangeBox.Text, EndingRangeBox.Text, StepSizeBox.Text) Then
                        Dim NewSurveyQuestionId As Integer = NewSurveyQuestion.AddQuestionToSurvey(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(ControlsCombobox.SelectedValue)}, .Mandatory = IIf(MandatoryQuestionCheck.Checked, 1, 0), .Question = NewQuestionTextBox.Text.Trim(), .DisplayId = QuestionsListView.Items.Count + 1, .Survey = New cSurvey With {.SurveyId = Integer.Parse(Request.QueryString("SurveyId"))}, .QuestionDescription = NewQuestionDescriptionTextBox.Text, .Enabled = True})
                        Dim SliderProperties As ISliderNumbersRange = New SliderNumbersRangeHandler
                        SliderProperties.AddSliderProperties(New cSliderNumbersRange With {.NumericalTextBox = NumericalCheckBox.Checked, .QuestionId = New cSurveyQuestions With {.QuestionId = NewSurveyQuestionId}, .StepRange = Integer.Parse(StepSizeBox.Text), .EndRange = Integer.Parse(EndingRangeBox.Text), .StartRange = Integer.Parse(StartingRangeBox.Text)})
                        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                        RefreshAllQuestions()
                        ResponsesChoiceList.Clear()
                        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                    End If
                End If
            Else
                NewSurveyQuestion.AddQuestionToSurvey(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(ControlsCombobox.SelectedValue)}, .Mandatory = IIf(MandatoryQuestionCheck.Checked, 1, 0), .Question = NewQuestionTextBox.Text.Trim(), .DisplayId = QuestionsListView.Items.Count + 1, .Survey = New cSurvey With {.SurveyId = Integer.Parse(Request.QueryString("SurveyId"))}, .QuestionDescription = NewQuestionDescriptionTextBox.Text, .Enabled = True})
                Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                RefreshAllQuestions()
                Notification.ShowSuccessNotification(Page, "Question Added")
            End If
        Else
            ShowQuestionBoxEmptyLabel()
        End If
        ResponsesChoiceList.Clear()
    End Sub

    Private Sub ShowQuestionBoxEmptyLabel()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "ShowQuestionBoxEmptyLabel();", True)
    End Sub

    Private Sub ShowMinimumTwoRepsonsesLabel()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "", "ShowMinimumTwoRepsonsesLabelMB();", True)
    End Sub

    Private Sub RefreshAllQuestions()
        LoadAllQuestions()
        QuestionsUpdatePanel.Update()
        ResetNewQuestionPanel()
    End Sub

    Private Sub ResetNewQuestionPanel()
        RadioButtonPanel.Visible = False
        MultipleCheckBoxPanel.Visible = False
        SliderPanel.Visible = False
        NewQuestionPanel.Visible = False
        SingleTextBoxPanel.Visible = False
        CommentBoxPanel.Visible = False
        NewQuestionTextBox.Text = ""
        NewQuestionDescriptionTextBox.Text = ""
        ControlsCombobox.ClearSelection()
        MandatoryQuestionCheck.Checked = False
    End Sub

    Protected Sub DeleteQuestionsButton_Click(sender As Object, e As EventArgs)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim DeleteButton As RadButton = CType(sender, RadButton)
        Dim DeleteQuestionContainer As RadListViewDataItem = DeleteButton.NamingContainer
        Dim DisplayId As Integer = DeleteQuestionContainer.GetDataKeyValue("DisplayId")
        Dim QuestionId As Integer = DeleteQuestionContainer.GetDataKeyValue("QuestionId")

        Dim Question As ISurveyQuestion = New SurveyQuestionsHandler()
        Dim Responses As IResponsesHandler = New ResponsesHandler()
        If Question.UpdateAllDisplayIdsFromQuestionId(DisplayId, Request.QueryString("SurveyId")) = True Then
            If Question.DeleteQuestionById(QuestionId) = True Then
                If Responses.DeleteQuestionResponsesByQuestionID(QuestionId) = True Then
                    RefreshAllQuestions()
                    Notification.ShowSuccessNotification(Page, "Question Deleted")
                Else
                    Notification.ShowFailNotification(Page, "Question Failed to be Deleted")
                End If
                DeleteSliderPropertiesIfExits(QuestionId)
                Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
            Else
                Notification.ShowFailNotification(Page, "Question Failed to be Deleted")
            End If
        Else
            Notification.ShowFailNotification(Page, "Question Failed to be Deleted")
        End If
    End Sub

    Protected Sub EditQuestionButton_Click1(sender As Object, e As EventArgs)
        ResetNewQuestionPanel()
        NewQuestionUpdatePanel.Update()
        Dim EditButton As RadButton = CType(sender, RadButton)
        Dim QuestionContainer As RadListViewDataItem = EditButton.NamingContainer
        Dim QuestionNumberToEdit As RadLabel = CType(QuestionContainer.FindControl("QuestionNumberLabel"), RadLabel)

        LoadAllQuestions()
        QuestionsUpdatePanel.Update()


        Dim QuestionDataItem As RadListViewDataItem = QuestionsListView.Items.Item(Integer.Parse(QuestionNumberToEdit.Text) - 1)
        Dim EditingPanel As Panel = CType(QuestionDataItem.FindControl("EditPanel"), Panel)
        Dim IndividualQuestionPanel As Panel = CType(QuestionDataItem.FindControl("IndividualQuestionPanel"), Panel)
        Dim IndividualQuestionUpdatePanel As UpdatePanel = CType(QuestionDataItem.FindControl("IndividualQuestionUpdatePanel"), UpdatePanel)
        EditingPanel.Visible = True

        LoadTextBoxesAndSelectedControlInEditingPanel(QuestionDataItem)
        IndividualQuestionPanel.Visible = False
        IndividualQuestionUpdatePanel.Update()
    End Sub

    Private Sub LoadTextBoxesAndSelectedControlInEditingPanel(QuestionDataItem As RadListViewDataItem)
        Dim EditingPanel As Panel = CType(QuestionDataItem.FindControl("EditPanel"), Panel)
        Dim QuestionNumber As RadLabel = CType(QuestionDataItem.FindControl("QuestionNumberLabel"), RadLabel)
        Dim QuestionLabel As RadLabel = CType(QuestionDataItem.FindControl("QuestionLabel"), RadLabel)
        Dim EditQuestionTextBox As RadTextBox = CType(EditingPanel.FindControl("EditQuestionTextBox"), RadTextBox)
        EditQuestionTextBox.Text = QuestionLabel.Text

        Dim QuestionDescriptionLabel As RadLabel = CType(QuestionDataItem.FindControl("QuestionDescriptionLabel"), RadLabel)
        Dim EditQuestionDescriptionTextBox As RadTextBox = CType(EditingPanel.FindControl("EditQuestionDescriptionTextBox"), RadTextBox)
        EditQuestionDescriptionTextBox.Text = QuestionDescriptionLabel.Text

        Dim MandatoryCheck As Boolean = False
        If QuestionLabel.CssClass.Contains("QuestionMandatory") Then
            MandatoryCheck = True
        End If
        Dim EditMandatoryQuestionCheck As RadCheckBox = CType(EditingPanel.FindControl("EditMandatoryQuestionCheck"), RadCheckBox)
        EditMandatoryQuestionCheck.Checked = MandatoryCheck

        Dim EditControlsComboBox As RadComboBox = CType(EditingPanel.FindControl("EditControlsComboBox"), RadComboBox)
        Dim ControlIdLabel As RadLabel = CType(QuestionDataItem.FindControl("ControlIdLabel"), RadLabel)
        EditControlsComboBox.SelectedValue = Integer.Parse(ControlIdLabel.Text)

        Dim e As New RadComboBoxSelectedIndexChangedEventArgs("", "", ControlIdLabel.Text, "")
        ControlsCombobox_SelectedIndexChanged(EditControlsComboBox, e)
    End Sub

    Private Sub ShowResponsesInEditingPanel(QuestionDataItem As RadListViewDataItem, SelectedControlId As Integer)
        Dim QuestionId As Integer = QuestionDataItem.GetDataKeyValue("QuestionId")
        Dim ComboBoxControlId As Integer = SelectedControlId
        Dim ResponsesDiv As HtmlGenericControl
        Select Case ComboBoxControlId
            Case 3
                ResponsesDiv = CType(QuestionDataItem.FindControl("SliderItemsResponses"), HtmlGenericControl)
            Case 4
                ResponsesDiv = CType(QuestionDataItem.FindControl("MultipleResponses"), HtmlGenericControl)
            Case 5
                ResponsesDiv = CType(QuestionDataItem.FindControl("RadioResponses"), HtmlGenericControl)
        End Select
        If ComboBoxControlId = 3 Or ComboBoxControlId = 4 Or ComboBoxControlId = 5 Then
            Dim Responses As IResponsesHandler = New ResponsesHandler
            Dim ResponsesList As List(Of cResponses) = Responses.GetResponsesByQuestionIdAndControlId(QuestionId, ComboBoxControlId)
            Dim ResponseListCount As Integer
            If ResponsesList Is Nothing Then
                ResponseListCount = 1
            Else
                ResponseListCount = ResponsesList.Count - 1
            End If
            For Index As Integer = 0 To ResponseListCount
                Dim NewResponseDiv As HtmlGenericControl
                If ResponsesList Is Nothing Then
                    NewResponseDiv = CreateNewResponseDivWithValues("")
                Else
                    NewResponseDiv = CreateNewResponseDivWithValues(ResponsesList(Index).Response.ToString())
                End If
                ResponsesDiv.Controls.Add(NewResponseDiv)
            Next
        End If
        If ComboBoxControlId = 3 Then
            Dim SliderPropertiesHandler As ISliderNumbersRange = New SliderNumbersRangeHandler
            Dim SliderNumberRange As cSliderNumbersRange = SliderPropertiesHandler.GetPropertiesForSlider(QuestionId)
            If Not SliderNumberRange Is Nothing Then
                Dim StartingRangeBoxEdit As RadNumericTextBox = CType(QuestionDataItem.FindControl("StartingRangeBoxEdit"), RadNumericTextBox)
                StartingRangeBoxEdit.Text = SliderNumberRange.StartRange
                Dim EndingRangeBoxEdit As RadNumericTextBox = CType(QuestionDataItem.FindControl("EndingRangeBoxEdit"), RadNumericTextBox)
                EndingRangeBoxEdit.Text = SliderNumberRange.EndRange
                Dim StepSizeBoxEdit As RadNumericTextBox = CType(QuestionDataItem.FindControl("StepSizeBoxEdit"), RadNumericTextBox)
                StepSizeBoxEdit.Text = SliderNumberRange.StepRange
                Dim NumericalCheckBoxEdit As RadCheckBox = CType(QuestionDataItem.FindControl("NumericalCheckBoxEdit"), RadCheckBox)
                NumericalCheckBoxEdit.Checked = SliderNumberRange.NumericalTextBox
            End If
        End If
    End Sub

    Private Function CreateNewResponseDivWithValues(ResponseValue As String) As HtmlGenericControl
        Dim NewResponseDiv As HtmlGenericControl = New HtmlGenericControl("div")

        Dim NewResponseTextBox As TextBox = New TextBox
        With NewResponseTextBox
            .TabIndex = 1
            .Attributes.Add("onkeypress", "return ValidateKeyPressInput(event);")
            .Attributes.Add("placeholder", "Please enter a choice")
            .Attributes.Add("Class", "AnswerChoiceTextBox")
            If Not String.IsNullOrWhiteSpace(ResponseValue) Then
                .Text = ResponseValue
            End If
        End With
        NewResponseDiv.Controls.Add(NewResponseTextBox)

        Dim NewAddResponseButton As HtmlButton = New HtmlButton
        With NewAddResponseButton

            .Attributes.Add("onclick", "AddAnotherAnswerChoice(this);return false;")
            .Attributes.Add("Class", "AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton")
            .InnerHtml = "<i Class=""fas fa-plus-circle""></i>"
        End With
        NewResponseDiv.Controls.Add(NewAddResponseButton)

        Dim NewRemoveResponseButton As HtmlButton = New HtmlButton
        With NewRemoveResponseButton
            .Attributes.Add("onclick", "RemoveAnswerChoice(this);return false;")
            .Attributes.Add("Class", "AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton")
            .InnerHtml = "<i Class=""fas fa-minus""></i>"
        End With
        NewResponseDiv.Controls.Add(NewRemoveResponseButton)

        Return NewResponseDiv
    End Function

    Protected Sub CancelQuestionButton_Command(sender As Object, e As CommandEventArgs)
        If e.CommandArgument.ToString() = "NewQuestion" Then
            ResetNewQuestionPanel()
        Else
            LoadAllQuestions()
            QuestionsUpdatePanel.Update()
        End If
    End Sub

    Protected Sub UpdateQuestion(sender As Object, e As EventArgs)

        Dim SaveButton As RadButton = CType(sender, RadButton)
        Dim QuestionContainer As RadListViewDataItem = SaveButton.NamingContainer

        Dim EditQuestion As String = CType(QuestionContainer.FindControl("EditQuestionTextBox"), RadTextBox).Text
        Dim EditQuestionDescription As String = CType(QuestionContainer.FindControl("EditQuestionDescriptionTextBox"), RadTextBox).Text
        Dim EditMandatoryQuestionCheck As Boolean = CType(QuestionContainer.FindControl("EditMandatoryQuestionCheck"), RadCheckBox).Checked
        Dim EditControlsComboBox As RadComboBox = CType(QuestionContainer.FindControl("EditControlsComboBox"), RadComboBox)
        Dim SliderOptionDropdownEdit As RadDropDownList = CType(QuestionContainer.FindControl("SliderOptionDropdownEdit"), RadDropDownList)
        Dim DisplayId As Integer = Integer.Parse(QuestionContainer.GetDataKeyValue("DisplayId"))
        Dim QuestionId As Integer = Integer.Parse(QuestionContainer.GetDataKeyValue("QuestionId"))
        Dim SurveyId As Integer = Integer.Parse(Request.QueryString("SurveyId"))

        If Not String.IsNullOrWhiteSpace(EditQuestion) Then
            Dim Survey As ISurveyHandler = New SurveyHandler
            Dim SurveyQuestion As ISurveyQuestion = New SurveyQuestionsHandler
            Dim QuestionResponses As IResponsesHandler = New ResponsesHandler
            If EditControlsComboBox.SelectedValue = 4 Or EditControlsComboBox.SelectedValue = 5 Then
                If ResponsesChoiceList.Count >= 2 Then
                    SurveyQuestion.UpdateQuestionById(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(EditControlsComboBox.SelectedValue)}, .Mandatory = EditMandatoryQuestionCheck, .Question = EditQuestion.Trim(), .DisplayId = DisplayId, .Survey = New cSurvey With {.SurveyId = SurveyId}, .QuestionDescription = EditQuestionDescription.Trim(), .Enabled = True, .QuestionId = QuestionId})
                    Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                    QuestionResponses.DeleteQuestionResponsesByQuestionID(QuestionId)
                    DeleteSliderPropertiesIfExits(QuestionId)
                    For Each Response As String In ResponsesChoiceList
                        QuestionResponses.AddResponseForQuestion(New cResponses With {.Question = New cSurveyQuestions With {.QuestionId = QuestionId}, .Response = Response.Trim()})
                    Next
                    Notification.ShowSuccessNotification(Page, "Question Updated")
                    RefreshAllQuestions()
                Else
                    ShowMinimumTwoRepsonsesLabel()
                End If
            ElseIf EditControlsComboBox.SelectedValue = 3 Then
                If SliderOptionDropdownEdit.SelectedValue = "1" Then
                    If ResponsesChoiceList.Count >= 2 Then
                        SurveyQuestion.UpdateQuestionById(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(EditControlsComboBox.SelectedValue)}, .Mandatory = EditMandatoryQuestionCheck, .Question = EditQuestion.Trim(), .DisplayId = DisplayId, .Survey = New cSurvey With {.SurveyId = SurveyId}, .QuestionDescription = EditQuestionDescription.Trim(), .Enabled = True, .QuestionId = QuestionId})
                        QuestionResponses.DeleteQuestionResponsesByQuestionID(QuestionId)
                        DeleteSliderPropertiesIfExits(QuestionId)
                        For Each Response As String In ResponsesChoiceList
                            QuestionResponses.AddResponseForQuestion(New cResponses With {.Question = New cSurveyQuestions With {.QuestionId = QuestionId}, .Response = Response.Trim()})
                        Next
                        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                        RefreshAllQuestions()
                        ResponsesChoiceList.Clear()
                        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                    Else
                        ShowMinimumTwoRepsonsesLabel()
                    End If
                Else
                    Dim StartingRange As String = CType(QuestionContainer.FindControl("StartingRangeBoxEdit"), RadNumericTextBox).Text
                    Dim EndingRange As String = CType(QuestionContainer.FindControl("EndingRangeBoxEdit"), RadNumericTextBox).Text
                    Dim StepSize As String = CType(QuestionContainer.FindControl("StepSizeBoxEdit"), RadNumericTextBox).Text
                    Dim NumericalCheckBox As Boolean = CType(QuestionContainer.FindControl("NumericalCheckBoxEdit"), RadCheckBox).Checked

                    If SliderValue.IsSliderRangeAccurate(StartingRange, EndingRange) And SliderValue.IsSliderStepsAccurate(StartingRange, EndingRange, StepSize) Then
                        SurveyQuestion.UpdateQuestionById(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(EditControlsComboBox.SelectedValue)}, .Mandatory = EditMandatoryQuestionCheck, .Question = EditQuestion.Trim(), .DisplayId = DisplayId, .Survey = New cSurvey With {.SurveyId = SurveyId}, .QuestionDescription = EditQuestionDescription.Trim(), .Enabled = True, .QuestionId = QuestionId})
                        Dim SliderProperties As ISliderNumbersRange = New SliderNumbersRangeHandler
                        QuestionResponses.DeleteQuestionResponsesByQuestionID(QuestionId)
                        DeleteSliderPropertiesIfExits(QuestionId)
                        SliderProperties.AddSliderProperties(New cSliderNumbersRange With {.NumericalTextBox = NumericalCheckBox, .QuestionId = New cSurveyQuestions With {.QuestionId = QuestionId}, .StepRange = Integer.Parse(StepSize), .EndRange = Integer.Parse(EndingRange), .StartRange = Integer.Parse(StartingRange)})
                        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                        RefreshAllQuestions()
                        ResponsesChoiceList.Clear()
                        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                    End If
                End If
            Else
                SurveyQuestion.UpdateQuestionById(New cSurveyQuestions With {.Control = New cControl With {.ControlId = Integer.Parse(EditControlsComboBox.SelectedValue)}, .Mandatory = EditMandatoryQuestionCheck, .Question = EditQuestion.Trim(), .DisplayId = DisplayId, .Survey = New cSurvey With {.SurveyId = SurveyId}, .QuestionDescription = EditQuestionDescription.Trim(), .Enabled = True, .QuestionId = QuestionId})
                QuestionResponses.DeleteQuestionResponsesByQuestionID(QuestionId)
                Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
                DeleteSliderPropertiesIfExits(QuestionId)
                RefreshAllQuestions()
                Notification.ShowSuccessNotification(Page, "Question Updated")
            End If
        Else
            ShowQuestionBoxEmptyLabel()
        End If
        ResponsesChoiceList.Clear()
    End Sub

    Private Sub DeleteSliderPropertiesIfExits(questionId As Integer)
        Dim Survey As ISurveyHandler = New SurveyHandler
        Survey.UpdateSurveyLastModifiedDate(New cSurvey With {.SurveyId = Request.QueryString("SurveyId")})
        Dim SliderPropertiesHandler As ISliderNumbersRange = New SliderNumbersRangeHandler
        Dim SliderNumberRange As cSliderNumbersRange = SliderPropertiesHandler.GetPropertiesForSlider(questionId)
        If Not SliderNumberRange Is Nothing Then
            SliderPropertiesHandler.DeleteSliderPropertiesById(questionId)
        End If
    End Sub
End Class
