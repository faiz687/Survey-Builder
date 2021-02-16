<%@ Page Title="Survey Builder - Pin Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="PinManagementPage.aspx.vb" Inherits="Survey_Builder.PinManagementPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="ContentsMainBody">
        <section class="PinManagemenSection PinBox">
            <div class="ContentBoxHeaders PinBoxHeader">
                <telerik:RadLabel ID="PinManagementLabel" CssClass="HeaderLabelCss" runat="server" Text="Generate Pin for a Survey"></telerik:RadLabel>
                <asp:Image ID="Image1" runat="server" ImageUrl="Images/PinManagmentImage.PNG" />
            </div>
            <telerik:RadLabel ID="PinManagementDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Select a Survey"></telerik:RadLabel>
            <telerik:RadDropDownList ID="SurveyNamesDropDownList" DefaultMessage="Select Any One" DataTextField="SurveyName" DataValueField="SurveyId" CssClass="SurveyDropDownMenu" runat="server"></telerik:RadDropDownList>
            <a runat="server" onserverclick="GeneratePinHereLabel_ServerClick">
                <telerik:RadLabel ID="GeneratePinHereLabel" CssClass="DescriptionLabelsCssclass GeneratePin" runat="server">Click <span style="padding: 0px !important; color: #0014ff">here </span>to generate Pin</telerik:RadLabel>
            </a>
            <telerik:RadTextBox ID="PinTextBox" CssClass="PinTextBox" WrapperCssClass="PinTextBoxOuter" runat="server"></telerik:RadTextBox>
            <div>
                <telerik:RadLabel ID="PinMessageLabel" runat="server" Visible="false" Style="padding: 0px;" ForeColor="Red"></telerik:RadLabel>
            </div>
        </section>
    </main>
</asp:Content>
