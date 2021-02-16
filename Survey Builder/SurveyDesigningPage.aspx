<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SurveyDesigningPage.aspx.vb" Inherits="Survey_Builder.SurveyDesigningPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JavaScript/SureveyDesigningPageJavaScript.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >    
        <main class="CurrentPageMainBody">           
            <section>
                <telerik:RadImageButton ID="GoBackButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" runat="server" Text="Back" OnClick="GoBackButton_Click2">
                    <ContentTemplate>
                        <i class="fas fa-arrow-left"></i>Back
                    </ContentTemplate>
                </telerik:RadImageButton>
            </section> 
            <section class="SurveyHeaderSection">                
                <asp:UpdatePanel ID="SurveyNameUpdatePanel" runat="server">
                    <ContentTemplate>
                        <div class="SurveyHeaderDiv" onmouseover="DisplayEdit()" onmouseout="RemoveEdit()">
                            <div class="LabelTextBox" style="display: inline-block;">
                                <telerik:RadLabel ID="SurveyNameLabel" runat="server" Text="" Style="padding-top: 10px; color: #333; font-weight: 500; font-size: x-large; letter-spacing: 1px;"></telerik:RadLabel>
                                <telerik:RadTextBox ID="SurveyNameTextbox"  ClientEvents-OnKeyPress="ValidateKeyPress"   runat="server" Visible="false"></telerik:RadTextBox>
                            </div>
                            <div class="EditButtonDiv">
                                <telerik:RadButton ID="SurveyNameEditButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyNameEdit" runat="server" Text="Edit" Style="margin-top: 5px;" OnClick="SurveyNameEditButton_Click"></telerik:RadButton>
                            </div>
                            <div class="SurveySaveCancelDiv">
                                <telerik:RadButton ID="SurveyNameSaveButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover " runat="server" Text="Save" Visible="false" OnClick="SurveyNameSaveButton_Click"></telerik:RadButton>
                                <telerik:RadButton ID="SurveyNameCancelButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover" runat="server" Text="Cancel" Visible="false" OnClick="SurveyNameCancelButton_Click1"></telerik:RadButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
            <section class="SurveyDesigningsection" style="padding: 20px;">
                <div class="SurveyDesigningDiv">
                    <asp:UpdatePanel ID="QuestionsUpdatePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadListView ID="QuestionsListView" ClientSettings-ClientEvents-OnListViewCreated="LoadQuestionListView" runat="server" DataKeyNames="QuestionId , DisplayId" ItemPlaceholderID="QuestionsContainer">
                                <LayoutTemplate>
                                    <asp:PlaceHolder ID="QuestionsContainer" runat="server"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="IndividualQuestionUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="EditPanel" CssClass="NewQuestionSection" runat="server" Visible="false">
                                                <telerik:RadLabel ID="HiddenResponsesLabel" runat="server" Style="display: none;"></telerik:RadLabel>
                                                <div class="NewQuesitonDiv">
                                                    <div class="NewQuesitonHeaderDiv">
                                                        <telerik:RadButton ID="CancelEditQuestionButton" Text="X" runat="server" CssClass="SurveyBuilderButton SurveyBuilderButtonHover" OnCommand="CancelQuestionButton_Command" CommandArgument="EditQuestion"></telerik:RadButton>
                                                    </div>
                                                    <div class="NewQuesitonBodyDiv">
                                                        <telerik:RadLabel ID="EditQuestionNumeberLabel" runat="server"></telerik:RadLabel>
                                                        <telerik:RadTextBox ID="EditQuestionTextBox" ClientEvents-OnKeyPress="ValidateKeyPress" ClientEvents-OnLoad="LoadNewQuestionTextBox" runat="server" EmptyMessage="Enter your question" Width="50%" WrapperCssClass="NewQuestionTextBox"></telerik:RadTextBox>
                                                        <telerik:RadComboBox ID="EditControlsComboBox" runat="server" OnSelectedIndexChanged="ControlsCombobox_SelectedIndexChanged" CssClass="ControlComboBoxCss" Style="margin-left: 10px;" EmptyMessage="Select any one" Width="30%" DataTextField="ControlName" DataValueField="ControlId" OnItemDataBound="ControlsCombobox_ItemDataBound" AutoPostBack="true"></telerik:RadComboBox>
                                                        <div>
                                                            <telerik:RadLabel ID="EditQuestionTextBoxValidatorLabel" CssClass="QuestionTextBoxValidatorLabel" Text="Question Field Empty !" runat="server" ForeColor="Red" Style="margin-left: 23px; margin-top: 10px; display: none; font-weight: 100;"></telerik:RadLabel>
                                                        </div>
                                                        <telerik:RadTextBox ID="EditQuestionDescriptionTextBox" ClientEvents-OnKeyPress="ValidateKeyPress" EmptyMessage="Describe your question" runat="server" Width="50%" Style="margin-left: 20px; margin-top: 10px;"></telerik:RadTextBox>
                                                        <telerik:RadCheckBox ID="EditMandatoryQuestionCheck" runat="server" Text="Mandatory" AutoPostBack="false" CssClass="MandatoryQuestionCheck"></telerik:RadCheckBox>
                                                    </div>
                                                    <div id="ControlPanelsEdit" runat="server" class="OptionsDiv">
                                                        <asp:Panel ID="RadioButtonPanelEdit" CssClass="RadioButtonPanelCss OptionsPanel" runat="server" Visible="false">
                                                            <div id="RadioResponses" runat="server" class="AllResponses">
                                                            </div>
                                                            <div>
                                                                <telerik:RadLabel ID="MinimumResponsesLabelRBEdit" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="font-weight: 100; display: none;"></telerik:RadLabel>
                                                            </div>
                                                            <telerik:RadButton ID="SaveRadioQuestionsButtonEdit" OnClientClicking="SaveResponsesRadioAndMultipleBoxes" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                        </asp:Panel>
                                                        <asp:Panel ID="MultipleCheckBoxPanelEdit" runat="server" CssClass="OptionsPanel MultipleCheckBoxPanelCss" Visible="false">
                                                            <div class="AllResponses" id="MultipleResponses" runat="server">
                                                            </div>
                                                            <div>
                                                                <telerik:RadLabel ID="MinimumResponsesLabelMBEdit" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="font-weight: 100; display: none;"></telerik:RadLabel>
                                                            </div>
                                                            <telerik:RadButton ID="SaveMultipleQuestionsEdit" OnClientClicking="SaveResponsesRadioAndMultipleBoxes" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                        </asp:Panel>
                                                        <asp:Panel ID="SliderPanelEdit" runat="server" CssClass="OptionsPanel SliderPanelCss" Visible="false">
                                                            <telerik:RadDropDownList ID="SliderOptionDropdownEdit" runat="server" EnableViewState="false" DefaultMessage="Select any one" Width="15em" OnClientItemSelected="SliderOptionSeleted">
                                                                <Items>
                                                                    <telerik:DropDownListItem Text="Slider Items" Value="1" />
                                                                    <telerik:DropDownListItem Text="Slider Numbers" Value="2" />
                                                                </Items>
                                                            </telerik:RadDropDownList>
                                                            <asp:Panel ID="SliderItemsPanelEdit" CssClass="SliderPanel" runat="server" Style="display: none;">
                                                                <div class="AllResponses" id="SliderItemsResponses" runat="server">
                                                                </div>
                                                                <div>
                                                                    <telerik:RadLabel ID="MinimumSliderResponsesLabelEdit" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="display: none;"></telerik:RadLabel>
                                                                    <telerik:RadLabel ID="MaximumSliderResponsesLabelEdit" CssClass="MaximumResponsesLabel" Text="Maximum five repsonses !" runat="server" ForeColor="Red" Style="display: none;"></telerik:RadLabel>
                                                                </div>
                                                                <telerik:RadButton ID="SliderSaveQuestionsItemsEdit" OnClientClicking="SaveSliderResponses" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                            </asp:Panel>
                                                            <asp:Panel runat="server" ID="SliderRangePanelEdit" Style="display: none;">
                                                                <div class="SliderRangeOptions" id="SliderRangePanelResponses" runat="server">
                                                                    <div>
                                                                        <telerik:RadLabel ID="ScaleRangeLabelEdit" Text="Scale Range" runat="server" Style="margin-bottom: 3px; display: block;"></telerik:RadLabel>
                                                                        <telerik:RadNumericTextBox ID="StartingRangeBoxEdit" ClientEvents-OnKeyPress="ValidateKeyPress" CssClass="StartingRangeBox" EnableViewState="false" NumberFormat-DecimalDigits="0" MinValue="-1000000" MaxValue="1000000" runat="server" Width="4em" Style="margin: 0px;"></telerik:RadNumericTextBox>
                                                                        <span>to</span>
                                                                        <telerik:RadNumericTextBox ID="EndingRangeBoxEdit" ClientEvents-OnKeyPress="ValidateKeyPress" CssClass="EndingRangeBox" EnableViewState="false" NumberFormat-DecimalDigits="0" MinValue="-1000000" MaxValue="1000000" runat="server" Width="4em" Style="margin: 0px;" CausesValidation="True"></telerik:RadNumericTextBox>
                                                                    </div>
                                                                    <div>
                                                                        <telerik:RadLabel ID="StepSizeLabelEdit" CssClass="StepSizeLabel" Text="Step Size" runat="server" Style="display: block; margin-bottom: 3px;"></telerik:RadLabel>
                                                                        <telerik:RadNumericTextBox NumberFormat-DecimalDigits="0" ClientEvents-OnKeyPress="ValidateKeyPress" CssClass="StepSizeBox" EnableViewState="false" ID="StepSizeBoxEdit" runat="server" Width="5em" Style="margin: 0px;"></telerik:RadNumericTextBox>
                                                                    </div>
                                                                    <div>
                                                                        <telerik:RadCheckBox ID="NumericalCheckBoxEdit" EnableViewState="false" AutoPostBack="false" runat="server" Text="Show Numerical Text Box" CssClass="MandatoryQuestionCheck" Style="margin-left: -1px; display: block;"></telerik:RadCheckBox>
                                                                    </div>
                                                                </div>
                                                                <telerik:RadLabel ID="IncorrectSliderRangeLabelEdit" CssClass="IncorrectSliderRangeLabel" Text="Scale range must be whole numbers between -1,000,000 and 1,000,000" ForeColor="Red" Style="font-weight: 100; display: none;" runat="server"></telerik:RadLabel>
                                                                <telerik:RadLabel ID="IncorrectStepRangeLabelEdit" CssClass="IncorrectStepRangeLabel" Text="" ForeColor="Red" Style="font-weight: 100; display: none;" runat="server"></telerik:RadLabel>
                                                                <div>
                                                                    <telerik:RadButton ID="SliderSaveQuestionsRangesEdit" OnClientClicking="SaveSliderResponses" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                                </div>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                        <asp:Panel ID="SingleTextBoxPanelEdit" runat="server" CssClass="OptionsPanel " Visible="false">
                                                            <telerik:RadButton ID="SaveSingleTextBoxButtonEdit" OnClientClicking="IsNewQuestionTextBoxEmpty" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                        </asp:Panel>
                                                        <asp:Panel ID="CommentBoxPanelEdit" runat="server" CssClass="OptionsPanel " Visible="false">
                                                            <telerik:RadButton ID="SaveCommmentBoxButtonEdit" OnClientClicking="IsNewQuestionTextBoxEmpty" OnClick="UpdateQuestion" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="IndividualQuestionPanel" CssClass="IndividualQuestionPanel IndividualQuestionPanelhover " runat="server" onmouseover="DisplayReorder(this)" onmouseout="HideReorder(this)">
                                                <div class="QuestionHeader">
                                                    <div class="MoveUpDiv">
                                                        <telerik:RadButton ID="EditQuestionButton" runat="server" OnClick="EditQuestionButton_Click1" Text="Edit" CssClass="SurveyBuilderButton SurveyBuilderButtonHover "></telerik:RadButton>
                                                        <telerik:RadButton ID="DeleteQuestionsButton" runat="server" OnClick="DeleteQuestionsButton_Click" Text="Delete" CssClass="SurveyBuilderButton SurveyBuilderButtonHover DeleteQuestionButton"></telerik:RadButton>
                                                        <telerik:RadImageButton ID="MoveUpButton" runat="server" OnCommand="RearrangeQuestion" CommandArgument="MoveUp" CssClass="SurveyBuilderButton SurveyBuilderButtonHover">
                                                            <ContentTemplate>
                                                                <i class="fas fa-chevron-up"></i>Move Up
                                                            </ContentTemplate>
                                                        </telerik:RadImageButton>
                                                    </div>
                                                    <telerik:RadLabel ID="QuestionNumberLabel" EnableViewState="true" CssClass="QuestionNumberLabel" runat="server" Text='<%# Eval("DisplayId")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="QuestionLabel" CssClass='<%#Eval("MandatoryCss") %>' runat="server" Text='<%# Eval("Question")%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="QuestionDescriptionLabel" CssClass="QuestionDescriptionLabel" runat="server" Text='<%# Eval("QuestionDescription")%>'></telerik:RadLabel>
                                                </div>
                                                <telerik:RadLabel ID="QuestionIdLabel" runat="server" Visible="false" Text='<%# Eval("QuestionId")%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="ControlIdLabel" runat="server" Visible="false" Text='<%# Eval("Control.ControlId")%>'></telerik:RadLabel>
                                                <asp:Panel ID="ResponsesControlPanel" CssClass="ResponsesControlPanel " runat="server">
                                                </asp:Panel>
                                                <div class="MoveDownDiv">
                                                    <telerik:RadImageButton ID="MoveDownButton" runat="server" OnCommand="RearrangeQuestion" CommandArgument="MoveDown" CssClass="SurveyBuilderButton SurveyBuilderButtonHover">
                                                        <ContentTemplate>
                                                            <i class="fas fa-chevron-down"></i>Move Down
                                                        </ContentTemplate>
                                                    </telerik:RadImageButton>
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </telerik:RadListView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="NewQuestionUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="NewQuestionButtonDiv" style="text-align: center; margin: 20px 0px;">
                                <telerik:RadImageButton ID="NewQuestionButton" runat="server" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" Style="padding: 15px;" OnClick="NewQuestionButton_Click">
                                    <ContentTemplate>
                                        <i class="fas fa-plus-circle" style="padding: 8px;"></i>NEW QUESTION
                                    </ContentTemplate>
                                </telerik:RadImageButton>
                            </div>
                            <asp:Panel ID="NewQuestionPanel" runat="server" Visible="false" CssClass="NewQuestionSection">
                                <div class="NewQuesitonDiv">
                                    <div class="NewQuesitonHeaderDiv">
                                        <telerik:RadButton ID="CancelQuestionButton" Text="X" runat="server" CssClass="SurveyBuilderButton SurveyBuilderButtonHover" OnCommand="CancelQuestionButton_Command" CommandArgument="NewQuestion"></telerik:RadButton>
                                    </div>
                                    <div class="NewQuesitonBodyDiv">
                                        <telerik:RadLabel ID="NewQuestionNumeberLabel" runat="server" Width="40px"></telerik:RadLabel>
                                        <telerik:RadTextBox ID="NewQuestionTextBox"  ClientEvents-OnKeyPress="ValidateKeyPress" ClientEvents-OnLoad="LoadNewQuestionTextBox" EmptyMessage="Enter your question" runat="server" Width="50%" WrapperCssClass="NewQuestionTextBox"></telerik:RadTextBox>
                                        <telerik:RadComboBox ID="ControlsCombobox" CssClass="ControlComboBoxCss" Style="margin-left: 10px;" EmptyMessage="Select any one" runat="server" Width="30%" DataTextField="ControlName" DataValueField="ControlId" OnItemDataBound="ControlsCombobox_ItemDataBound" OnSelectedIndexChanged="ControlsCombobox_SelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
                                        <div>
                                            <telerik:RadLabel ID="NewQuestionTextBoxValidatorLabel" CssClass="QuestionTextBoxValidatorLabel" Text="Question Field Empty !" runat="server" ForeColor="Red" Style="margin-left: 65px; margin-top: 10px; display: none; font-weight: 100;"></telerik:RadLabel>
                                        </div>
                                        <telerik:RadTextBox ID="NewQuestionDescriptionTextBox" ClientEvents-OnKeyPress="ValidateKeyPress" EmptyMessage="Describe your question" runat="server" Width="50%" Style="margin-left: 60px; margin-top: 10px;"></telerik:RadTextBox>
                                        <telerik:RadCheckBox ID="MandatoryQuestionCheck" runat="server" Text="Mandatory" AutoPostBack="false" CssClass="MandatoryQuestionCheck"></telerik:RadCheckBox>
                                    </div>
                                    <div id="ControlPanels" runat="server" class="OptionsDiv">
                                        <asp:Panel ID="RadioButtonPanel" CssClass="RadioButtonPanelCss OptionsPanel" runat="server" Visible="false">
                                            <div class="AllResponses">
                                                <div>
                                                    <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="1" class="AnswerChoiceTextBoxRB AnswerChoiceTextBox" placeholder="Please enter a choice" />
                                                    <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                    <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                </div>
                                                <div>
                                                    <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="2" placeholder="Please enter a choice" class="AnswerChoiceTextBox AnswerChoiceTextBoxRB" />
                                                    <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                    <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                </div>
                                            </div>
                                            <div>
                                                <telerik:RadLabel ID="MinimumResponsesLabelRB" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="font-weight: 100; display: none;"></telerik:RadLabel>
                                            </div>
                                            <telerik:RadButton ID="SaveRadioQuestionsButton" OnClientClicking="SaveResponsesRadioAndMultipleBoxes" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                        </asp:Panel>
                                        <asp:Panel ID="MultipleCheckBoxPanel" runat="server" CssClass="OptionsPanel MultipleCheckBoxPanelCss" Visible="false">
                                            <div class="AllResponses">
                                                <div>
                                                    <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="1" class="AnswerChoiceTextBox AnswerChoiceTextBoxMB" placeholder="Please enter a choice" />
                                                    <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                    <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                </div>
                                                <div>
                                                    <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="2" placeholder="Please enter a choice" class="AnswerChoiceTextBox AnswerChoiceTextBoxMB" />
                                                    <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                    <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                </div>
                                            </div>
                                            <div>
                                                <telerik:RadLabel ID="MinimumResponsesLabelMB" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="font-weight: 100; display: none;"></telerik:RadLabel>
                                            </div>
                                            <telerik:RadButton ID="SaveMultipleQuestions" OnClientClicking="SaveResponsesRadioAndMultipleBoxes" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                        </asp:Panel>
                                        <asp:Panel ID="SliderPanel" runat="server" CssClass="OptionsPanel SliderPanelCss" Visible="false">
                                            <telerik:RadDropDownList ID="SliderOptionDropdown" runat="server" EnableViewState="false" DefaultMessage="Select any one" Width="15em" OnClientItemSelected="SliderOptionSeleted">
                                                <Items>
                                                    <telerik:DropDownListItem Text="Slider Items" Value="1" />
                                                    <telerik:DropDownListItem Text="Slider Numbers" Value="2" />
                                                </Items>
                                            </telerik:RadDropDownList>
                                            <asp:Panel ID="SliderItemsPanel" CssClass="SliderPanel" runat="server" Style="display: none;">
                                                <div class="AllResponses">
                                                    <div>
                                                        <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="1" class="AnswerChoiceTextBoxSlider AnswerChoiceTextBox" placeholder="Please enter a choice" />
                                                        <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                        <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                    </div>
                                                    <div>
                                                        <input type="text" onkeypress="return ValidateKeyPressInput(event);" tabindex="2" placeholder="Please enter a choice" class="AnswerChoiceTextBox AnswerChoiceTextBoxSlider" />
                                                        <button type="button" onclick="AddAnotherAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceAddButton"><i class="fas fa-plus-circle"></i></button>
                                                        <button type="button" onclick="RemoveAnswerChoice(this)" class="AnswerChoiceButton AnswerChoiceButtonHover AnswerChoiceRemoveButton"><i class="fas fa-minus"></i></button>
                                                    </div>
                                                </div>
                                                <div>
                                                    <telerik:RadLabel ID="MinimumSliderResponsesLabel" CssClass="MinimumResponsesLabel" Text="Minimum two repsonses !" runat="server" ForeColor="Red" Style="display: none;"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="MaximumSliderResponsesLabel" CssClass="MaximumResponsesLabel" Text="Maximum five repsonses !" runat="server" ForeColor="Red" Style="display: none;"></telerik:RadLabel>
                                                </div>
                                                <telerik:RadButton ID="SliderSaveQuestionsItems" OnClientClicking="SaveSliderResponses" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="SliderRangePanel" Style="display: none;">
                                                <div class="SliderRangeOptions">
                                                    <div>
                                                        <telerik:RadLabel ID="ScaleRangeLabel" Text="Scale Range" runat="server" Style="margin-bottom: 3px; display: block;"></telerik:RadLabel>
                                                        <telerik:RadNumericTextBox ClientEvents-OnKeyPress="ValidateKeyPress" ID="StartingRangeBox" CssClass="StartingRangeBox" EnableViewState="false" NumberFormat-DecimalDigits="0" MinValue="-1000000" MaxValue="1000000" runat="server" Width="4em" Style="margin: 0px;"></telerik:RadNumericTextBox>
                                                        <span>to</span>
                                                        <telerik:RadNumericTextBox ClientEvents-OnKeyPress="ValidateKeyPress" ID="EndingRangeBox" CssClass="EndingRangeBox" EnableViewState="false" NumberFormat-DecimalDigits="0" MinValue="-1000000" MaxValue="1000000" runat="server" Width="4em" Style="margin: 0px;" CausesValidation="True"></telerik:RadNumericTextBox>
                                                    </div>
                                                    <div>
                                                        <telerik:RadLabel ID="StepSizeLabel" Text="Step Size" runat="server" Style="display: block; margin-bottom: 3px;"></telerik:RadLabel>
                                                        <telerik:RadNumericTextBox ClientEvents-OnKeyPress="ValidateKeyPress" NumberFormat-DecimalDigits="0" CssClass="StepSizeBox" EnableViewState="false" ID="StepSizeBox" runat="server" Width="5em" Style="margin: 0px;"></telerik:RadNumericTextBox>
                                                    </div>
                                                    <div>
                                                        <telerik:RadCheckBox ID="NumericalCheckBox" EnableViewState="false" AutoPostBack="false" runat="server" Text="Show Numerical Text Box" CssClass="MandatoryQuestionCheck" Style="margin-left: -1px; display: block;"></telerik:RadCheckBox>
                                                    </div>
                                                </div>
                                                <telerik:RadLabel ID="IncorrectSliderRangeLabel" CssClass="IncorrectSliderRangeLabel" Text="Scale range must be whole numbers between -1,000,000 and 1,000,000" ForeColor="Red" Style="font-weight: 100; display: none;" runat="server"></telerik:RadLabel>
                                                <telerik:RadLabel ID="IncorrectStepRangeLabel" CssClass="IncorrectStepRangeLabel" Text="" ForeColor="Red" Style="font-weight: 100; display: none;" runat="server"></telerik:RadLabel>
                                                <div>
                                                    <telerik:RadButton ID="SliderSaveQuestionsRanges" OnClientClicking="SaveSliderResponses" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                        <asp:Panel ID="SingleTextBoxPanel" runat="server" CssClass="OptionsPanel " Visible="false">
                                            <telerik:RadButton ID="SaveSingleTextBoxButton" OnClientClicking="IsNewQuestionTextBoxEmpty" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                        </asp:Panel>
                                        <asp:Panel ID="CommentBoxPanel" runat="server" CssClass="OptionsPanel" Visible="false">
                                            <telerik:RadButton ID="SaveCommmentBoxButton" OnClientClicking="IsNewQuestionTextBoxEmpty" OnClick="SaveResponses" CssClass=" SaveButton SurveyBuilderButton SaveButtonHover" runat="server" Text="Save" Style="width: 5em !important;"></telerik:RadButton>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </section>
        </main>
</asp:Content>

