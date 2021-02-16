<%@ Page Title="Survey Builder - User Accounts" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="UserAccountPage.aspx.vb" Inherits="Survey_Builder.UserAccountPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="ContentsMainBody">
        <a runat="server" onserverclick="AddorEditUserButtonClicked">
            <section class="AddUsersSection">
                <div class="ContentBoxHeaders">
                    <telerik:RadLabel ID="AddUsersLabel" CssClass="HeaderLabelCss" runat="server" Text="Add or Edit Users"></telerik:RadLabel>
                    <asp:Image ID="AddUsersImage" runat="server" ImageUrl="Images/AddEditUsersImage.PNG" />
                </div>
                <telerik:RadLabel ID="AddUsersDescriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Add or Edit User Details to the system"></telerik:RadLabel>
            </section>
        </a>
        <a runat="server" onserverclick="DeleteUserButtonClicked">
            <section class="DeleteUsersSection">
                <div class="ContentBoxHeaders SecondContentBox">
                    <telerik:RadLabel ID="DeleteUsersLabel" CssClass="HeaderLabelCss" runat="server" Text="Delete Users"></telerik:RadLabel>
                    <asp:Image ID="DeleteUserImage" runat="server" ImageUrl="Images/DeleteUsers.PNG" />
                </div>
                <telerik:RadLabel ID="DeleteUserDecriptionLabel" CssClass="DescriptionLabelsCssclass" runat="server" Text="Delete Users From the system"></telerik:RadLabel>
            </section>
        </a>
    </main>
</asp:Content>
