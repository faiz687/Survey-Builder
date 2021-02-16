<%@ Page Title="Survey Builder - Surveys" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master"  CodeBehind="SurveysHomePage.aspx.vb" Inherits="Survey_Builder.SurveysHomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="ContentsMainBody">
        <section class="OverlayPage" runat="server" id="OverlayPageSection" visible="false">
            <div class="OverlaySecion NoPadding">
                <div class="OverlayHeader">
                    <telerik:RadLabel ID="NameYourSurveyLabel" Text="Name your Survey" runat="server" Style="width: fit-content;"></telerik:RadLabel>
                    <telerik:RadButton runat="server" ID="CancelOverlay" Text="X" Style="width: fit-content; padding: 0px !important; border: none; outline: none; box-shadow: none;" CausesValidation="false"  OnClick="CancelOverlay_Click" ></telerik:RadButton>
                </div>
                <div class="OverlayBody " style="background-color: whitesmoke; padding: 20px;">
                    <telerik:RadTextBox ID="SurveyNameTextBox" EmptyMessage="Survey name" runat="server" Width="100%"></telerik:RadTextBox>
                    <telerik:RadLabel ID="SurveyNameTakenLabel" Text="Survey name already taken" runat="server" Visible="false" ForeColor="Red"></telerik:RadLabel>
                    <asp:RequiredFieldValidator ID="SurveyNameTextBoxValidator" CssClass="NoPadding SurveyValidator" Display="Dynamic" runat="server" ErrorMessage="Survey name required" ControlToValidate="SurveyNameTextBox" ForeColor="Red" Width="100%"></asp:RequiredFieldValidator>
                </div>
                <div class="OverlayFooter" style="text-align: right;">
                    <telerik:RadButton ID="CreateSurveyButton" runat="server" Text="CREATE SURVEY" CssClass="SurveyBuilderButton SurveyBuilderButtonHover" OnClick="CreateSurveyButton_Click"></telerik:RadButton>
                </div>
            </div>
        </section>
        <a runat="server" style="cursor: pointer;" onserverclick="ShowOverlay">
            <section class="CreateSurveySection">
                <div class="ContentBoxHeaders">
                    <telerik:RadLabel ID="CreateSurveyLabel" CssClass="HeaderLabelCss" runat="server" Text="Create a survey"></telerik:RadLabel>
                    <asp:Image ID="CreateSurveyImage" runat="server" ImageUrl="Images/CreateSurveyImage.PNG" />
                </div>
                <telerik:RadLabel ID="CreateSurveyDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Create a survey"></telerik:RadLabel>
            </section>
        </a>
        <a runat="server" onserverclick="ViewAllsurveys_ServerClick">
            <section class="ViewAllSurveySection">
                <div class="ContentBoxHeaders SecondContentBox">
                    <telerik:RadLabel ID="ViewAllSurveylabel" CssClass="HeaderLabelCss" runat="server" Text="View all Surveys"></telerik:RadLabel>
                    <asp:Image ID="ViewAllSurveyImage" runat="server" ImageUrl="Images/ViewAllSurveysImage.PNG" />
                </div>
                <telerik:RadLabel ID="ViewAllSurveyDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="View all open or active surveys"></telerik:RadLabel>
            </section>
        </a>
        <a runat="server" onserverclick="editSurvey_ServerClick">
            <section class="EditSurveysSection">
                <div class="ContentBoxHeaders">
                    <telerik:RadLabel ID="EditSurveysLabel" CssClass="HeaderLabelCss" runat="server" Text="Edit a survey"></telerik:RadLabel>
                    <asp:Image ID="EditSurveysImage" runat="server" ImageUrl="Images/EditSurveyImage.PNG" />
                </div>
                <telerik:RadLabel ID="EditSurveysDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Modify Existing Surveys"></telerik:RadLabel>
            </section>
        </a>
        <a runat="server" onserverclick="DownloadSurveys_ServerClick">
            <section class="DownloadSurveysSection">
                <div class="ContentBoxHeaders">
                    <telerik:RadLabel ID="DownloadSurveyslabel" CssClass="HeaderLabelCss" runat="server" Text="Download Survey Responses"></telerik:RadLabel>
                    <asp:Image ID="DownloadSurveysImage" runat="server" ImageUrl="Images/DownloadSurveysImage.PNG" />
                </div>
                <telerik:RadLabel ID="DownloadSurveysDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Collect responses from surveys undertaken"></telerik:RadLabel>
            </section>
        </a>
        <a runat="server" onserverclick="ViewDataAnalysis_ServerClick">
            <section class="ViewDataAnalysisSection">
                <div class="ContentBoxHeaders">
                    <telerik:RadLabel ID="ViewDataAnalysisLabel" CssClass="HeaderLabelCss" runat="server" Text="View Data Analysis"></telerik:RadLabel>
                    <asp:Image ID="ViewDataAnalysisImage" runat="server" ImageUrl="Images/ViewDataAnalysisImage.PNG" />
                </div>
                <telerik:RadLabel ID="ViewDataAnalysisDescription" runat="server" CssClass="DescriptionLabelsCssclass" Text="View data of Undertaken surveys"></telerik:RadLabel>
            </section>
        </a>
    </main>
</asp:Content>

