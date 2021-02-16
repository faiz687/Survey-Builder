<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="AllSurveys.aspx.vb" Inherits="Survey_Builder.AllSurveys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <main class="CurrentPageMainBody">
                <section class="OverlayPage" runat="server" id="OverlayPageSection" visible="false">
                    <div class="OverlaySecion NoPadding">
                        <div class="OverlayHeader" style="padding:10px;height: 2em;">
                            <telerik:RadLabel ID="EnterYourPinLabel" Text="Survey Pin" runat="server" Style="width: fit-content;"></telerik:RadLabel>
                            <telerik:RadButton runat="server" ID="CancelOverlay" Text="X" Style="width: fit-content; padding: 0px !important; border: none; outline: none; box-shadow: none;" CausesValidation="false"  OnClick="CancelOverlay_Click" CssClass="SurveyBuilderButton  SurveyBuilderButtonHover"></telerik:RadButton>
                        </div>
                        <div class="OverlayBody " style="background-color: whitesmoke; padding: 20px;">
                            <telerik:RadComboBox   runat="server" DataTextField="SurveyName" EmptyMessage="Select any one" style="margin-bottom:10px;" Width="100%" DataValueField="SurveyId" Filter="Contains" ID="SurveyNameComboBoxPin"></telerik:RadComboBox>
                            <telerik:RadTextBox   ID="PinTextBox" EmptyMessage="Enter Pin Here" runat="server" Width="100%"></telerik:RadTextBox>
                            <telerik:RadLabel  ID="PinMessage" style="margin-top:8px;margin-left:3px;"  runat="server" Visible="false" ForeColor="Red"></telerik:RadLabel>
                        </div>
                        <div class="OverlayFooter" style="text-align: right; padding:10px;">
                            <telerik:RadButton ID="EnterPinButton" runat="server" Text="ENTER PIN" CssClass="SurveyBuilderButton SurveyBuilderButtonHover" OnClick="EnterPinButton_Click"></telerik:RadButton>
                        </div>
                    </div>
                </section>
                <section>
                    <telerik:RadComboBox runat="server" AutoPostBack="true" OnSelectedIndexChanged="SurveyNameComboBox_SelectedIndexChanged" DataTextField="SurveyName" Width="40%" CssClass="SurveyNameComboBox" EmptyMessage="Search here..." DataValueField="SurveyId" Filter="Contains" ID="SurveyNameComboBox"></telerik:RadComboBox>
                    <telerik:RadButton ID="RefreshSurveyButton" OnClick="RefreshSurveyButton_Click" runat="server" Icon-PrimaryIconCssClass="rbRefresh" Style="margin-top: 2px;" CssClass="SurveyBuilderButton  SurveyBuilderButtonHover"></telerik:RadButton>
                    <telerik:RadImageButton ID="ShowOverlayButton" runat="server" Text="Enter Pin" Style="float: right;" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton"   OnClick="ShowOverlayButton_Click">
                        <ContentTemplate>
                            <i class="fas fa-key"></i>Enter Pin
                        </ContentTemplate>
                    </telerik:RadImageButton>
                </section>
                <section>
                    <telerik:RadListView ID="SurveysListView"   OnNeedDataSource="SurveysListView_NeedDataSource" AllowPaging="true" PageSize="10" runat="server" DataKeyNames="SurveyId" ItemPlaceholderID="SurveyNamesHolder">
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="SurveyNamesHolder" runat="server"></asp:PlaceHolder>
                            <div style="text-align: center;">
                                <telerik:RadButton ID="PreviousPageButton" Enabled="<%#Container.CurrentPageIndex > 0 %>" runat="server" Text="Previous" CssClass="SurveyBuilderButton  SurveyBuilderButtonHover" CommandName="Page" CommandArgument="Prev"></telerik:RadButton>
                                <telerik:RadButton ID="NextPageButton" Enabled="<%#Container.CurrentPageIndex + 1 < Container.PageCount %>" CssClass="SurveyBuilderButton  SurveyBuilderButtonHoverrunat" runat="server" Text="Next" CommandName="Page" CommandArgument="Next"></telerik:RadButton>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <a id="SurveyLink" runat="server"  onserverclick="SurveyLink_ServerClick">
                                <asp:Panel ID="SurveyPanel" CssClass="IndividualSurveyPanel IndividualSurveyPanelHover" runat="server">
                                    <telerik:RadLabel ID="SurveyNameLabel" Text='<%#Eval("SurveyName") %>' runat="server"></telerik:RadLabel>
                                </asp:Panel>
                            </a>
                        </ItemTemplate>
                    </telerik:RadListView>
                </section>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

