<%@ Page Title="" Language="vb"  AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SurveyPage.aspx.vb" Inherits="Survey_Builder.SurveyPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="CurrentPageMainBody">
        <section>
            <asp:UpdatePanel runat="server" ID="SurveyListViewUpdatePanel" >
                <ContentTemplate>
                    <telerik:RadListView ID="SurveyListView" OnLayoutCreated="SurveyListView_LayoutCreated" runat="server" ItemPlaceholderID="SurveyListViewPlaceholder">
                        <LayoutTemplate>
                            <div style="text-align: center; padding: 10px;">
                                <telerik:RadLabel ID="SurveyNameLabel" Style="font-size: x-large; color: #005eb8" runat="server"></telerik:RadLabel>
                            </div>
                            <asp:PlaceHolder ID="SurveyListViewPlaceholder" runat="server"></asp:PlaceHolder>
                            <div style="text-align: center; padding: 10px;">
                                <telerik:RadButton ID="SubmitButton" Style="font-size: larger; padding: 10px; width: 7em !important;" runat="server" CssClass="SurveyBuilderButton  SurveyBuilderIconButton SaveButton SaveButtonHover" Text="Submit" OnClick="SubmitButton_Click"></telerik:RadButton>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <asp:Panel ID="IndividualQuestionPanel" CssClass="IndividualQuestionPanel IndividualQuestionPanelhover " runat="server">
                                <div class="QuestionHeader">
                                    <telerik:RadLabel ID="QuestionNumberLabel" EnableViewState="true" CssClass="QuestionNumberLabel" runat="server" Text='<%# Eval("DisplayId")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="QuestionLabel" CssClass='<%#Eval("MandatoryCss") %>' runat="server" Text='<%# Eval("Question")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="QuestionDescriptionLabel" CssClass="QuestionDescriptionLabel" runat="server" Text='<%# Eval("QuestionDescription")%>'></telerik:RadLabel>
                                </div>
                                <telerik:RadLabel ID="QuestionIdLabel" runat="server" Visible="false" Text='<%# Eval("QuestionId")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="ControlIdLabel" runat="server" Visible="false" Text='<%# Eval("Control.ControlId")%>'></telerik:RadLabel>
                                <asp:Panel ID="ResponsesControlPanel" CssClass="ResponsesControlPanel " runat="server">
                                </asp:Panel>
                            </asp:Panel>
                        </ItemTemplate>
                    </telerik:RadListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </section>
    </main>
</asp:Content>
