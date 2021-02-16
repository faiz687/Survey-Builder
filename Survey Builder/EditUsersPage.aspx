<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="EditUsersPage.aspx.vb" Inherits="Survey_Builder.EditUsersPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="CurrentPageMainBody">
        <section>
            <telerik:RadImageButton ID="GoBackButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" OnClick="GoBackButton_Click" runat="server" Text="Back">
                <ContentTemplate>
                    <i class="fas fa-arrow-left"></i>Back
                </ContentTemplate>
            </telerik:RadImageButton>
        </section>
        <section class="SearchandEditUserSection">
            <div class="EditUserBody">
                <telerik:RadLabel ID="UsernameLabel" runat="server" CssClass="EditUserBodyLabelCss" Text="Username"></telerik:RadLabel>
                <telerik:RadTextBox ID="UsernameTextBox" WrapperCssClass="EditUserBodyTextBoxWrapper" runat="server" ReadOnly="true" BackColor="#CCCCCC" ></telerik:RadTextBox>
                <telerik:RadLabel ID="ForenameLabel" runat="server" CssClass="EditUserBodyLabelCss" Text="Forename"></telerik:RadLabel>
                <telerik:RadTextBox ID="ForenameTextBox" WrapperCssClass="EditUserBodyTextBoxWrapper" runat="server" ReadOnly="true" BackColor="#CCCCCC"></telerik:RadTextBox>
                <telerik:RadLabel ID="SurnameLabel" runat="server" CssClass="EditUserBodyLabelCss" Text="Surname"></telerik:RadLabel>
                <telerik:RadTextBox ID="SurnameTextBox" WrapperCssClass="EditUserBodyTextBoxWrapper" runat="server" ReadOnly="true" BackColor="#CCCCCC"></telerik:RadTextBox>
                <telerik:RadLabel ID="EmailLabel" runat="server" CssClass="EditUserBodyLabelCss" Text="Email"></telerik:RadLabel>
                <telerik:RadTextBox ID="EmailTextBox" WrapperCssClass="EditUserBodyTextBoxWrapper" runat="server" ReadOnly="true" BackColor="#CCCCCC"></telerik:RadTextBox>
                <telerik:RadLabel ID="PermsissionLabel" runat="server" CssClass="EditUserBodyLabelCss" Text="Permission"></telerik:RadLabel>
                <telerik:RadDropDownList ID="PermissionDropDownList" DataTextField="PermissionName" DataValueField="PermissionId" runat="server" CssClass="EditUserBodyDropDown"></telerik:RadDropDownList>
                <telerik:RadLabel ID="Status" runat="server" CssClass="EditUserBodyLabelCss" Text="Status"></telerik:RadLabel>
                <telerik:RadDropDownList ID="AccountStatusDropDownList" runat="server" CssClass="EditUserBodyDropDown" SelectedText="Enabled" SelectedValue="0">
                    <Items>
                        <telerik:DropDownListItem runat="server" Text="Enabled" Value="0"></telerik:DropDownListItem>
                        <telerik:DropDownListItem runat="server" Text="Disabled" Value="1"></telerik:DropDownListItem>
                    </Items>
                </telerik:RadDropDownList>
                <telerik:RadLabel ID="UserIdLabel" runat="server" Visible="False"></telerik:RadLabel>
            </div>
            <telerik:RadButton ID="UpdateUserButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" runat="server" Text="Save"  OnClick="UpdateUserButton_Click"></telerik:RadButton>
        </section>
    </main>
</asp:Content>
