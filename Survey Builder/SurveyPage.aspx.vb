Imports Telerik.Web.UI


Public Class SurveyPage
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Check.IsUserSignedIn(Me.Page) Then
                Dim MasterPage As MainMaster = Master
                MasterPage.DisableAllNavigationButtons()
                MasterPage.DisableUsernameLabel()
            End If
        End If
    End Sub
    Private Sub surveypage_init(sender As Object, e As EventArgs) Handles Me.Init
        If IsNothing(Request.QueryString("surveyid")) Then
            Response.Redirect("allsurveys.aspx", True)
            Context.ApplicationInstance.CompleteRequest()
        Else
            LoadAllSurveyQuestions(Request.QueryString("surveyid"))
        End If
    End Sub
    Private Sub LoadAllSurveyQuestions(SurveyId As String)
        Dim SurveyQuestions As ISurveyQuestion = New SurveyQuestionsHandler
        Dim SurveyQuestionsList As List(Of cSurveyQuestions) = SurveyQuestions.GetAllQuesitons(SurveyId)
        SurveyListView.DataSource = SurveyQuestionsList
        SurveyListView.DataBind()
    End Sub
    Protected Sub SurveyListView_LayoutCreated(sender As Object, e As EventArgs)
        If Not IsNothing(Request.QueryString("SurveyName")) Then
            Dim SurveyListView As RadListView = CType(sender, RadListView)
            Dim SurveyNameLabel As RadLabel = CType(SurveyListView.FindControl("SurveyNameLabel"), RadLabel)
            SurveyNameLabel.Text = Request.QueryString("SurveyName")
        End If
    End Sub
    Private Sub SurveyListView_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles SurveyListView.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then
            Dim ControlIdLabel As RadLabel = CType(e.Item.FindControl("ControlIdLabel"), RadLabel)
            Dim QuestionId As RadLabel = CType(e.Item.FindControl("QuestionIdLabel"), RadLabel)
            Dim Responsepanel As Panel = CType(e.Item.FindControl("ResponsesControlPanel"), Panel)
            GenerateControlsForEachQuestion(Integer.Parse(ControlIdLabel.Text), Integer.Parse(QuestionId.Text), Responsepanel)
        Else
        End If
    End Sub
    Private Sub GenerateControlsForEachQuestion(TypeOfControl As Integer, QuesitonId As Integer, Responsepanel As Panel)
        Dim Responses As IResponsesHandler = New ResponsesHandler
        Select Case TypeOfControl
            Case 1
                'textbox
                Dim Text_Box As New RadTextBox With {.Width = Unit.Parse("30em"), .ID = "SingleTextBox", .Height = Unit.Parse("2.3em"), .CssClass = "ResponsesTextBox SingleTextBox", .TextMode = TextBoxMode.SingleLine}
                Text_Box.ClientEvents.OnKeyPress = "ValidateKeyPress"
                Responsepanel.Controls.Add(Text_Box)
            Case 2
                'commentbox
                Dim Text_Box As New RadTextBox With {.Width = Unit.Parse("30em"), .ID = "CommentBox", .EnableViewState = True, .Height = Unit.Parse("6em"), .CssClass = "ResponsesTextBox", .TextMode = TextBoxMode.MultiLine}
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
                            .SelectedRegionStartValue = SliderNumberRange.StartRange
                            If SliderNumberRange.NumericalTextBox = True Then
                                Dim NumericalTextBox As New RadNumericTextBox With {.MinValue = SliderNumberRange.StartRange, .MaxValue = SliderNumberRange.EndRange, .Text = SliderNumberRange.StartRange, .MaxLength = SliderNumberRange.EndRange.ToString.Length, .ID = "NumericalTextBox", .WrapperCssClass = "SliderNumericalTextBoxOuter", .BackColor = System.Drawing.Color.FromArgb(192, 192, 192), .CssClass = "SliderNumericalTextBox", .Width = Unit.Parse("3em")}
                                NumericalTextBox.NumberFormat.DecimalDigits = 0
                                NumericalTextBox.ClientEvents.OnKeyPress = "ValidateKeyPress"
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
    Protected Sub SubmitButton_Click(sender As Object, e As EventArgs)
        If AllMandatoryQuestionsHavebeenCompleted() Then
            Dim QuestionAnswers As Dictionary(Of Integer, String) = GetAllAnswers()
            Dim _Survey As ISurveyHandler = New SurveyHandler
            Dim SurveyAnswer As ISurveyAnswerHandler = New SurveyAnswerHandler
            Dim Answer As IAnswersHandler = New AnswerHandler
            Dim SurveyVersion As Integer = _Survey.GetSurveyVersion(Request.QueryString("surveyid"))
            Dim AnswerId As Integer = SurveyAnswer.InsertSurveyAnswer(New cSurveyAnswer With {.Platform = "FrontEnd", .SurveyId = Request.QueryString("surveyid"), .AppVersion = CType(Master.FindControl("VersionLabel"), RadLabel).Text, .SurveyVersion = SurveyVersion})
            For Each Question As KeyValuePair(Of Integer, String) In QuestionAnswers
                Answer.InsertAnswer(New cAnswer With {.AnswerId = AnswerId, .QuestionId = Question.Key, .Answer = Question.Value, .AnswerDateTime = DateTime.Now})
            Next
        End If
    End Sub
    Private Function GetAllAnswers() As Dictionary(Of Integer, String)
        Dim QuestionAnswers As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)
        For Each SurveyQuestion As RadListViewDataItem In SurveyListView.Items
            Dim QuestionId As Integer = Integer.Parse(CType(SurveyQuestion.FindControl("QuestionIdLabel"), RadLabel).Text)
            Dim ResponsesPanel As Panel = CType(SurveyQuestion.FindControl("ResponsesControlPanel"), Panel)
            For Each QuestionControl As Control In ResponsesPanel.Controls
                If TypeOf QuestionControl Is RadTextBox Then
                    Dim TextBox As RadTextBox = DirectCast(QuestionControl, RadTextBox)
                    If Not String.IsNullOrWhiteSpace(TextBox.Text) Then
                        QuestionAnswers.Add(QuestionId, TextBox.Text)
                    End If
                ElseIf TypeOf QuestionControl Is RadRadioButtonList Then
                    Dim RadioButtonList As RadRadioButtonList = CType(QuestionControl, RadRadioButtonList)
                    If Not RadioButtonList.SelectedIndex = -1 Then
                        QuestionAnswers.Add(QuestionId, RadioButtonList.SelectedValue)
                    End If
                ElseIf TypeOf QuestionControl Is RadSlider Then
                    Dim Slider As RadSlider = CType(QuestionControl, RadSlider)
                    If Slider.ItemType = SliderItemType.Item Then
                        QuestionAnswers.Add(QuestionId, Slider.SelectedValue)
                    Else
                        QuestionAnswers.Add(QuestionId, Slider.Value)
                    End If
                Else
                    If TypeOf QuestionControl Is RadCheckBoxList Then
                        Dim MultipleCheckBoxList As RadCheckBoxList = CType(QuestionControl, RadCheckBoxList)
                        If Not MultipleCheckBoxList.SelectedIndex = -1 Then
                            Dim MultipleCheckBoxAnswers As String = " "
                            For Each CheckBoxSelected As String In MultipleCheckBoxList.SelectedValues
                                MultipleCheckBoxAnswers = MultipleCheckBoxAnswers + " " + CheckBoxSelected
                            Next
                            QuestionAnswers.Add(QuestionId, MultipleCheckBoxAnswers)
                        End If
                    End If
                End If
            Next
        Next
        Return QuestionAnswers
    End Function
    Private Function AllMandatoryQuestionsHavebeenCompleted() As Boolean
        For Each SurveyQuestion As RadListViewDataItem In SurveyListView.Items
            Dim QuestionLabel As RadLabel = CType(SurveyQuestion.FindControl("QuestionLabel"), RadLabel)
            Dim IndividualQuestionPanel As Panel = CType(SurveyQuestion.FindControl("IndividualQuestionPanel"), Panel)
            If QuestionLabel.CssClass.Contains("QuestionMandatory") Then
                Dim ResponsesPanel As Panel = CType(SurveyQuestion.FindControl("ResponsesControlPanel"), Panel)
                For Each QuestionControl As Control In ResponsesPanel.Controls
                    If TypeOf QuestionControl Is RadTextBox Then
                        Dim TextBox As RadTextBox = DirectCast(QuestionControl, RadTextBox)
                        If String.IsNullOrWhiteSpace(TextBox.Text) Then
                            HighlightIncompleteQuestion(IndividualQuestionPanel.ClientID)
                            Return False
                        End If
                    ElseIf TypeOf QuestionControl Is RadRadioButtonList Then
                        Dim RadioButtonList As RadRadioButtonList = CType(QuestionControl, RadRadioButtonList)
                        If RadioButtonList.SelectedIndex = -1 Then
                            HighlightIncompleteQuestion(IndividualQuestionPanel.ClientID)
                            Return False
                        End If
                    Else
                        If TypeOf QuestionControl Is RadCheckBoxList Then
                            Dim MultipleCheckBoxList As RadCheckBoxList = CType(QuestionControl, RadCheckBoxList)
                            If MultipleCheckBoxList.SelectedIndex = -1 Then
                                HighlightIncompleteQuestion(IndividualQuestionPanel.ClientID)
                                Return False
                            End If
                        End If
                    End If
                Next
            End If
        Next
        Return True
    End Function
    Private Sub HighlightIncompleteQuestion(QuestionId)
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HighlightIncompleteQuestion", "HighlightIncompleteQuestion('" + QuestionId + "');", True)
    End Sub
End Class
