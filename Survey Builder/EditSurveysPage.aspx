<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="EditSurveysPage.aspx.vb" Inherits="Survey_Builder.ViewAllSurveysPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="CurrentPageMainBody">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <section class="OverlayPage" runat="server" id="OverlayPageSection" visible="false" >
                    <div class="OverlaySecion" style="padding:20px;">
                        <div>
                            <telerik:RadButton ID="CancelOverlayButton" style="margin-bottom:10px" CssClass="SurveyBuilderButton  SurveyBuilderButtonHover" runat="server" Text="X" OnClick="CancelOverlayButton_Click"></telerik:RadButton>
                        </div>
                        <div>
                            <p style="margin-top:0px; font-weight: 500; padding: 10px;background-color: whitesmoke;">Deleting the survey will delete all questions , responses and answers from users. </br> Are you sure you want to proceed.</p>
                        </div>
                        <div class="OverlayFooter" style="text-align: right;">
                            <telerik:RadButton ID="ConfirmSurveyDeleteButton" style="padding:10px;" runat="server" Text="DELETE SURVEY" CssClass="SurveyBuilderButton DeleteButton  DeleteButtonHovered " OnClick="ConfirmSurveyDeleteButton_Click"></telerik:RadButton>
                        </div>
                        <asp:HiddenField ID="SurveyIdHidden" runat="server" /> 
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
        <section>
            <telerik:RadImageButton ID="GoBackButton" CssClass="SurveyBuilderButton  SurveyBuilderButtonHover" runat="server" OnClick="GoBackButton_Click" Text="Back">
                <ContentTemplate>
                    <i class="fas fa-arrow-left"></i>Back
                </ContentTemplate>
            </telerik:RadImageButton>
        </section>
        <section>
            <div class="AllSurveysDiv">
                <asp:UpdatePanel ID="SurveysListViewUpdatePanel"  runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <telerik:RadListView ID="SurveysListView" runat="server" OnItemDataBound="SurveysListView_ItemDataBound" DataKeyNames="SurveyId" ItemPlaceholderID="SurveyHolder">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="SurveyHolder" runat="server"></asp:PlaceHolder>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="SurveyPanel" CssClass="IndividualSurveyPanel IndividualSurveyPanelHover" runat="server">
                                    <telerik:RadPageLayout ID="AddPageTableLayout" runat="server" CssClass="RadPageLayout">
                                        <Rows>
                                            <telerik:LayoutRow>
                                                <Columns>
                                                    <telerik:LayoutColumn Span="4" SpanMd="2" CssClass="SurveyOptionsPanel1">
                                                        <telerik:RadLabel ID="SurveyNameLabel" CssClass="SurveyNamelabelCss" runat="server" Text='<%#Eval("SurveyName") %>'></telerik:RadLabel>
                                                        <div style="color: #D0D2D3;" class="DateDiv">
                                                            <telerik:RadLabel ID="CreatedDateLabel" CssClass="SurveyDateLabelCss" runat="server" Text='<%#  String.Format("Created:{0}", Eval("SurveyCreatedDate")) %>'></telerik:RadLabel>
                                                            |
                                                    <telerik:RadLabel ID="ModifiedDateLabel" CssClass="SurveyDateLabelCss" runat="server" Text='<%# String.Format("Modified:{0}", Eval("LastModifiedDate")) %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="StatusLabel" Text='<%#Eval("Enabled") %>' runat="server" Visible="false"></telerik:RadLabel>
                                                        </div>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="8"  SpanMd="10" CssClass="SurveyOptionsPanel2">
                                                        <div class="SurveyDetailsDiv" style="border: none;">
                                                            <telerik:RadButton ID="EditSurveyButton" runat="server" Text="Edit" CssClass="EditSurveyButtonCss" OnClick="EditSurveyButton_Click">
                                                                <Icon PrimaryIconCssClass="rbEdit" />
                                                            </telerik:RadButton>
                                                        </div>
                                                        <div class="SurveyDetailsDiv">
                                                            <telerik:RadDropDownList ID="StatusDropDownList" Style="text-align: center;" OnSelectedIndexChanged="StatusDropDownList_SelectedIndexChanged1" AutoPostBack="true" runat="server">
                                                                <Items >
                                                                    <telerik:DropDownListItem Text="Enabled" Value="0"  CssClass="StatusDropDownListCssItem" />
                                                                    <telerik:DropDownListItem Text="Disabled" Value="1" CssClass="StatusDropDownListCssItem" />
                                                                </Items>
                                                            </telerik:RadDropDownList>
                                                        </div>
                                                        <div class="SurveyDetailsDiv">
                                                            <telerik:RadButton ID="ShowOverlayButton" runat="server" CssClass="EditSurveyButtonCss" Text="Delete" OnClick="ShowOverlayButton_Click">
                                                                <Icon PrimaryIconCssClass="rbRemove" />
                                                            </telerik:RadButton>
                                                        </div>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </asp:Panel>
                            </ItemTemplate>
                        </telerik:RadListView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </section>
    </main>
</asp:Content>
