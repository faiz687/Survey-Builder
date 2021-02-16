using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using SurveyBuilderApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SurveyBuilderApp.AppPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurveyPage : ContentPage
    {
        private int SurveyVersion;
        public int SurveyId { get; set; }
        public IEnumerable<cSurveyQuestions> AllSurveys { get; set; }

        bool Online = false;

        IDatabaseService dbHandler = DependencyService.Get<IDatabaseService>();
        public SurveyPage()
        {
            InitializeComponent();
            SurveyVersion = (int)App.Current.Properties["SelectedSurveyVersion"];
            SurveyId = (int)App.Current.Properties["SelectedSurveyId"];
            SurveyNameLabel.Text = (string)App.Current.Properties["SelectedSurveyName"];
        }
        protected async override void OnAppearing()
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                Online = true;
                ActivityIndicatorSurveyPage.IsRunning = true;
                string Response = "";
                ISurveyQuestion Survey = new SurveyQuestionsHandler();
                string AllSurveyQuestions = await Task.Run(() => Response = Survey.GetAllQuesitons(SurveyId));
                ActivityIndicatorSurveyPage.IsRunning = false;
                MainStackLayout.IsVisible = true;
                if (!(AllSurveyQuestions == "Unauthorized" || AllSurveyQuestions == "No Content" || AllSurveyQuestions == "No Internet Connection"))
                {
                    AllSurveys = JsonConvert.DeserializeObject<IEnumerable<cSurveyQuestions>>(AllSurveyQuestions.ToString());
                    CreateAllQuestions(AllSurveys);
                    FullStackLayout.IsVisible = true;
                }
                else
                {
                    SurveyEmptyStacklayout.IsVisible = true;
                }
            }
            else
            {
                var ListOfSurveyQuestions = dbHandler.GetAllQuestionBySurveyId(SurveyId);
                if (ListOfSurveyQuestions.Count > 0)
                {
                    CreateAllQuestions(ListOfSurveyQuestions);
                    FullStackLayout.IsVisible = true;
                    MainStackLayout.IsVisible = true;
                }
                else
                {
                    SurveyEmptyStacklayout.IsVisible = true;
                }
            }
        }
        private void CreateAllQuestions(IEnumerable<cSurveyQuestions> AllSurveys)
        {
            foreach (var Question in AllSurveys)
            {
                SurveyQuestionsStackLayout.Children.Add(CreateQuestionlayout(Question));
            }
        }
        private async void ChevronDownIconButton_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (AreAllMandatoryQuestionsCompleted())
            {
                Dictionary<int, string> AllAnswers = GetAllAnswers();
                ISurveyAnswerHandler SurveyAnswer = new SurveyAnswerHandler();
                IAnswersHandler _QuestionAnswer = new AnswerHandler();

                if (Reachability.IsHostReachable("http://www.google.co.uk"))
                {
                    string SurveyAnswerId = "";
                    await Task.Run(() => SurveyAnswerId = SurveyAnswer.InsertSurveyAnswer(new cSurveyAnswer { Platform = Device.Idiom.ToString(), SurveyId = SurveyId, AppVersion = App.Version, SurveyVersion = SurveyVersion }));
                    foreach (var QuestionAnswer in AllAnswers)
                    {
                        await Task.Run(() => _QuestionAnswer.InsertAnswer(new cAnswer { AnswerId = int.Parse(SurveyAnswerId), Answer = QuestionAnswer.Value, QuestionId = QuestionAnswer.Key, AnswerDateTime = DateTime.Now }));
                    }
                    await DisplayAlert("Message", "Thank You For Completing This Survey.", "OK");
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
            }
        }
        private Dictionary<int, string> GetAllAnswers()
        {
            Dictionary<int, string> AllAnswers = new Dictionary<int, string>();
            int count = 0;
            foreach (cSurveyQuestions Question in AllSurveys)
            {
                var View = (((((SurveyQuestionsStackLayout.Children[count] as StackLayout).Children[0] as Frame).Content as StackLayout).Children[1] as StackLayout).Children[0]);
                if (View.GetType() == typeof(Frame))
                {
                    if ((View as Frame).Content.GetType() == typeof(Editor))
                    {
                        Editor CommentBox = (View as Frame).Content as Editor;
                        if (!(CommentBox.Text == null))
                        {
                            AllAnswers.Add(Question.QuestionId, CommentBox.Text);
                        }
                    }
                    else
                    {
                        Entry SingleTextBox = (View as Frame).Content as Entry;
                        if (!(SingleTextBox.Text == null))
                        {
                            AllAnswers.Add(Question.QuestionId, SingleTextBox.Text);
                        }
                    }
                }
                else
                {
                    if ((View as StackLayout).Children[0].GetType() == typeof(RadioButton))
                    {
                        bool IsChecked = false;
                        for (int i = 0; i < (View as StackLayout).Children.Count; i++)
                        {
                            RadioButton radioButton = ((View as StackLayout).Children[i] as RadioButton);

                            if (radioButton.IsChecked)
                            {
                                IsChecked = true;
                                AllAnswers.Add(Question.QuestionId, radioButton.Text.ToString());
                                break;
                            }
                        }
                        if (IsChecked == false)
                        {
                            AllAnswers.Add(Question.QuestionId, "");
                        }
                    }
                    else if ((View as StackLayout).Children[0].GetType() == typeof(StackLayout))
                    {
                        int MainCheckBoxStackLayout = (View as StackLayout).Children.Count;
                        bool IsChecked = false;
                        string MultipleCheckAnswers = "";
                        for (int i = 0; i < ((View as StackLayout).Children.Count); i++)
                        {
                            CheckBox checkBox = ((View as StackLayout).Children[i] as StackLayout).Children[0] as CheckBox;

                            if (checkBox.IsChecked)
                            {
                                IsChecked = true;
                                MultipleCheckAnswers += string.Format("{0} ", (((View as StackLayout).Children[i] as StackLayout).Children[1] as Label).Text);
                                if (AllAnswers.ContainsKey(Question.QuestionId))
                                {
                                    AllAnswers[Question.QuestionId] = MultipleCheckAnswers;
                                }
                                else
                                {
                                    AllAnswers.Add(Question.QuestionId, MultipleCheckAnswers);
                                }
                            }
                        }
                        if (IsChecked == false)
                        {
                            AllAnswers.Add(Question.QuestionId, "");
                        }
                    }
                    else
                    {
                        if ((View as StackLayout).Children[0].GetType() == typeof(Grid))
                        {
                          Slider slider = (View as StackLayout).Children[1] as Slider;

                          int SliderValue = Convert.ToInt32(slider.Value);

                          Label SliderLabel = ((View as StackLayout).Children[0] as Grid).Children[SliderValue] as Label;

                          AllAnswers.Add(Question.QuestionId, SliderLabel.Text);

                        }
                        else
                        {
                            Slider slider = (View as StackLayout).Children[1] as Slider;
                            AllAnswers.Add(Question.QuestionId, slider.Value.ToString());
                        }
                    }         
                }
                count += 1;
            }
            return AllAnswers;
        }
        private bool AreAllMandatoryQuestionsCompleted()
        {
            int count = 0;
            foreach (cSurveyQuestions Question in AllSurveys)
            {
                if (Question.Mandatory == true)
                {
                    var View = (((((SurveyQuestionsStackLayout.Children[count] as StackLayout).Children[0] as Frame).Content as StackLayout).Children[1] as StackLayout).Children[0]);
                    if (View.GetType() == typeof(Frame))
                    {
                        if ((View as Frame).Content.GetType() == typeof(Editor))
                        {
                            Editor CommentBox = (View as Frame).Content as Editor;
                            if (CommentBox.Text == null)
                            {
                                HighLightAndFocus(Question);
                                return false;
                            }
                            else
                            {
                                if (CommentBox.Text.Length == 0)
                                {
                                    HighLightAndFocus(Question);
                                    return false;
                                }
                            }

                        }
                        else
                        {
                            Entry SingleTextBox = (View as Frame).Content as Entry;
                            if (SingleTextBox.Text == null)
                            {
                                HighLightAndFocus(Question);
                                return false;
                            }
                            else
                            {
                                if (SingleTextBox.Text.Length == 0)
                                {
                                    HighLightAndFocus(Question);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((View as StackLayout).Children[0].GetType() == typeof(RadioButton))
                        {
                            bool IsChecked = false;
                            for (int i = 0; i < (View as StackLayout).Children.Count; i++)
                            {
                                RadioButton radioButton = ((View as StackLayout).Children[i] as RadioButton);

                                if (radioButton.IsChecked)
                                {
                                    IsChecked = true;
                                    break;
                                }
                            }
                            if (IsChecked == false)
                            {
                                HighLightAndFocus(Question);
                                return false;
                            }
                        }
                        else if ((View as StackLayout).Children[0].GetType() == typeof(StackLayout))
                        {
                            int MainCheckBoxStackLayout = (View as StackLayout).Children.Count;
                            bool IsChecked = false;
                            for (int i = 0; i < ((View as StackLayout).Children.Count) ; i++)
                            {
                                CheckBox checkBox = ((View as StackLayout).Children[i] as StackLayout).Children[0] as CheckBox;

                                if (checkBox.IsChecked)
                                {
                                    IsChecked = true;
                                    break;
                                }
                            }
                            if (IsChecked == false)
                            {
                                CheckBox checkBox = ((View as StackLayout).Children[0] as StackLayout).Children[0] as CheckBox;
                                HighLightAndFocus(Question);
                                return false;
                            }
                        }
                    }
                }
                count += 1;
            }
            return true;
        }
        private async void HighLightAndFocus(cSurveyQuestions Question)
        {            
            await DisplayAlert("Message",string.Format("Question {0} is mandatory",Question.DisplayId), "OK");
        }
        private StackLayout CreateQuestionlayout(cSurveyQuestions Question)
        {
            StackLayout MainStackLayout = new StackLayout() { Padding = 5 };

            Frame MainFramelayout = new Frame() { HasShadow = true, CornerRadius = 0, Padding = 7 };

            StackLayout WrapperQuestionLayout = new StackLayout();

            StackLayout QuestionAndNumberlayout = CreateBasicQuestionLayout(Question);

            StackLayout QuestionControlsLayout = CreateQuestionControls(Question);

            WrapperQuestionLayout.Children.Add(QuestionAndNumberlayout);

            WrapperQuestionLayout.Children.Add(QuestionControlsLayout);

            MainFramelayout.Content = WrapperQuestionLayout;

            MainStackLayout.Children.Add(MainFramelayout);

            return MainStackLayout;
        }
        private StackLayout CreateQuestionControls(cSurveyQuestions Question)
        {
            StackLayout QuestionControlLayout = new StackLayout();
            View Control = ConstructControl(Question);
            QuestionControlLayout.Children.Add(Control);
            return QuestionControlLayout;
        }
        private View ConstructControl(cSurveyQuestions Question)
        {
            View Control;
            switch (Question.Control)
            {
                case 1:
                    Control = ContructSingleTextBox();
                    break;
                case 2:
                    Control = ConstructCommentBox();
                    break;
                case 3:
                    Control = ConstructSlider(Question);
                    break;
                case 4:
                    Control = ConstructMultipleCheckBoxes(Question);
                    break;
                case 5:
                    Control = ConstructRadioButtons(Question);
                    break;
                default:
                    Control = new Editor();
                    break;
            }
            return Control;
        }
        private Frame ContructSingleTextBox()
        {
            Frame EntryFrame = new Frame() { Padding = 2, HasShadow = false, BorderColor = Color.FromHex("#005eb8") ,BackgroundColor = Color.FromHex("#005eb8") };

            Entry SingleTextBox = new Entry();

            EntryFrame.Content = SingleTextBox;

            return EntryFrame;

        }
        private StackLayout ConstructMultipleCheckBoxes(cSurveyQuestions Question)
        {
            IResponsesHandler Responses = new ResponsesHandler();

            List<cResponses> AllResponses = new List<cResponses>();

            if (Online == true)
            {
                string _Responses = Responses.GetAllResponses(Question.QuestionId);

                if (!(_Responses == "NoContent"))
                {
                    AllResponses = JsonConvert.DeserializeObject<List<cResponses>>(_Responses.ToString());
                }
            }
            else
            {
                AllResponses =  dbHandler.GetAllResponsesByQuestionId(Question.QuestionId);
            }
            StackLayout MainCheckBoxesStackLayout = new StackLayout() { Spacing = 0 };

            for (int i = 0; i < AllResponses.Count - 1; i++)
            {
                StackLayout LabelCheckBoxesStackLayout = new StackLayout() { Spacing = 0, Orientation = StackOrientation.Horizontal, Margin = 0, Padding = 0 };

                Label CheckBoxLabel = new Label { TextColor = Color.FromHex("#005eb8"), Text = AllResponses[i].Response, Margin = new Thickness(-8, 0, 0, 0), FontSize = 15, VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold };

                CheckBox _CheckBox = new CheckBox() { Margin = new Thickness(-8, 0, 0, 0), Visual = VisualMarker.Material, Color = Color.FromHex("#005eb8") };

                LabelCheckBoxesStackLayout.Children.Add(_CheckBox);

                LabelCheckBoxesStackLayout.Children.Add(CheckBoxLabel);

                MainCheckBoxesStackLayout.Children.Add(LabelCheckBoxesStackLayout);
            }

            return MainCheckBoxesStackLayout;
        }
        private StackLayout ConstructRadioButtons(cSurveyQuestions Question)
        {
            IResponsesHandler Responses = new ResponsesHandler();

            List<cResponses> AllResponses = new List<cResponses>();

            if (Online == true)
            {
                string _ResponsesRadio = Responses.GetAllResponses(Question.QuestionId);

                if (!(_ResponsesRadio == "NoContent"))
                {
                    AllResponses = JsonConvert.DeserializeObject<List<cResponses>>(_ResponsesRadio.ToString());
                }
            }
            else
            {
                AllResponses = dbHandler.GetAllResponsesByQuestionId(Question.QuestionId);
            }
            StackLayout RadioButtonsStackLayout = new StackLayout();

            foreach (cResponses Response in AllResponses)
            {
                RadioButton radioButton = new RadioButton { Text = Response.Response.ToString(), BorderColor = Color.FromHex("#005eb8"), TextColor = Color.FromHex("#005eb8"), FontAttributes = FontAttributes.Bold };

                RadioButtonsStackLayout.Children.Add(radioButton);
            }
            return RadioButtonsStackLayout;
        }
        private StackLayout ConstructSlider(cSurveyQuestions Question)
        {
            IResponsesHandler Responses = new ResponsesHandler();

            List<cResponses> AllResponses = new List<cResponses>();

            bool DoeSliderHasNumber = false;

            if (Online == true)
            {
                string _ResponsesSlider = Responses.GetAllResponses(Question.QuestionId);

                if (!(_ResponsesSlider == "NoContent"))
                {
                    AllResponses = JsonConvert.DeserializeObject<List<cResponses>>(_ResponsesSlider.ToString());
                }
                else
                {
                    DoeSliderHasNumber = true;
                }
            }
            else
            {
                AllResponses = dbHandler.GetAllResponsesByQuestionId(Question.QuestionId);
                if (!(AllResponses.Count > 0))
                {
                    DoeSliderHasNumber = true;
                }
            }
            if (DoeSliderHasNumber == false)
            {
                StackLayout SliderAndLabelsStack = new StackLayout() { VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true), HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true) };

                Grid LabelsGrid = new Grid() { VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true), HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true) };

                int NumberOfColumns = AllResponses.Count;

                int ColumnWidth = 100 / AllResponses.Count;

                for (int i = 0; i < NumberOfColumns; i++)
                {
                    ColumnDefinition LabelColumn = new ColumnDefinition() { Width = new GridLength(ColumnWidth, GridUnitType.Star) };

                    LabelsGrid.ColumnDefinitions.Add(LabelColumn);
                }

                for (int i = 0; i < AllResponses.Count; i++)
                {
                    Label SliderItemLabel = new Label()
                    {
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                        Text = AllResponses[i].Response
                    };
                    if (i == 0 || AllResponses.Count - 1 == i)
                    {
                        if (i == 0)
                        {
                            SliderItemLabel.HorizontalTextAlignment = TextAlignment.Start;
                        }
                        else
                        {
                            SliderItemLabel.HorizontalTextAlignment = TextAlignment.End;
                        }
                    }
                    else
                    {
                        SliderItemLabel.HorizontalTextAlignment = TextAlignment.Center;
                    }
                    LabelsGrid.Children.Add(SliderItemLabel, i, LabelsGrid.RowDefinitions.Count);
                }
                Slider _Slider = new Slider()
                {
                    Minimum = 0,

                    Maximum = AllResponses.Count - 1,

                    VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                    MaximumTrackColor = Color.FromHex("#005eb8")

                };
                _Slider.ValueChanged += new SurveyPage()._Slider_ValueChanged;

                SliderAndLabelsStack.Children.Add(LabelsGrid);

                SliderAndLabelsStack.Children.Add(_Slider);

                return SliderAndLabelsStack;
            }         
            else
            {
                cSliderNumbersRange SliderProperties = new cSliderNumbersRange();

                if (Online == true)
                {
                    ISliderNumbersRange _SliderProperties = new SliderNumbersRangeHandler();

                    string SliderResponses = _SliderProperties.GetPropertiesForSlider(Question.QuestionId);

                    SliderProperties = JsonConvert.DeserializeObject<cSliderNumbersRange>(SliderResponses.ToString());
                }
                else
                {
                   var ListSliderNumbers =  dbHandler.GetSliderProperties(Question.QuestionId);

                    if (ListSliderNumbers.Count > 0)
                    {
                        SliderProperties = ListSliderNumbers[0];
                    }
                }

                StackLayout SliderAndValueLayout = new StackLayout();

                Slider _Slider = new Slider()
                {
                    Maximum = SliderProperties.EndRange,

                    Minimum = SliderProperties.StartRange,

                    VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),

                    MaximumTrackColor = Color.FromHex("#005eb8")
                };

                _Slider.ValueChanged += new SurveyPage()._Slider_ValueChanged;

                Label SliderStepLabel = new Label { Text = SliderProperties.StepRange.ToString(), IsVisible = false };

                SliderAndValueLayout.Children.Add(SliderStepLabel);

                SliderAndValueLayout.Children.Add(_Slider);

                if (SliderProperties.NumericalTextBox)
                {
                    Label NumericalTextBox = new Label() { BindingContext = _Slider };

                    NumericalTextBox.SetBinding(Label.TextProperty, "Value");

                    SliderAndValueLayout.Children.Add(NumericalTextBox);

                }
                return SliderAndValueLayout;
            }
        }
        private void _Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var Slider = sender as Xamarin.Forms.Slider;

            var SliderStackLayout = Slider.Parent as StackLayout;

            if (SliderStackLayout.Children[0].GetType() == typeof(Label))
            {
                Label StepLabel = (SliderStackLayout.Children[0] as Label);
                int StepValue = int.Parse(StepLabel.Text);
                Slider.Value = (Math.Round((e.NewValue) / StepValue)) * StepValue;
            }
            else
            {
                Slider.Value = Math.Round(e.NewValue);
            }
        }
        private Frame ConstructCommentBox()
        {
            Frame EditorFrame = new Frame() { Padding = 2, HasShadow = false, BorderColor = Color.FromHex("#005eb8"), BackgroundColor = Color.FromHex("#005eb8") };

            Editor CommentBox = new Editor() { HeightRequest = 80, WidthRequest = 20 };

            EditorFrame.Content = CommentBox;

            return EditorFrame;

        }
        private StackLayout CreateBasicQuestionLayout(cSurveyQuestions Question)
        {
            StackLayout QuestionAndNumberlayout = new StackLayout() { Margin = new Thickness { Bottom = 5  } };

            Label QuestionLabel = new Label();

            FormattedString QuestionFormattedString = new FormattedString();

            Span QuestionAndNumber = new Span() { Text = string.Format("Q{0}. {1}", Question.DisplayId, Question.Question), CharacterSpacing = 0.3 };

            QuestionFormattedString.Spans.Add(QuestionAndNumber);

            if (!string.IsNullOrEmpty(Question.QuestionDescription))
            {
                Span QuestionDescription = new Span() { Text = string.Format("\n{0}", Question.QuestionDescription), CharacterSpacing = 0.3, FontAttributes = FontAttributes.Italic };
                QuestionFormattedString.Spans.Add(QuestionDescription);
            }

            if (Question.Mandatory == true)
            {
                Span QuestionMandatory = new Span() { Text = " *", TextColor = Color.Red };
                QuestionFormattedString.Spans.Add(QuestionMandatory);
            }

            QuestionLabel.FormattedText = QuestionFormattedString;

            QuestionAndNumberlayout.Children.Add(QuestionLabel);

            return QuestionAndNumberlayout;
        }
    }
}