<%@ Page Title="Survey Builder - Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Default.aspx.vb" Inherits="Survey_Builder.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="loginPageContainer" runat="server">
        <asp:Panel runat="server" ID="plLogin" DefaultButton="LoginButton">
            <div class="LoginWrapperDiv">
                <div class="UserNameDiv">
                    <div class="UserNamLabelIconDiv">
                        <telerik:RadLabel ID="UsernameLabel" runat="server" Text="Username"></telerik:RadLabel>
                        <i class="far fa-user"></i>
                    </div>
                    <div class="IconTextBox">
                        <telerik:RadTextBox ID="UserNameTextBox" runat="server"  placeholder="Enter your username" ></telerik:RadTextBox>
                    </div>
                    <div class="LoginValidatorDiv">
                        <asp:RequiredFieldValidator ID="UserNameTextBoxValidator" ForeColor="Red" ControlToValidate="UserNameTextBox" runat="server" ErrorMessage="Please enter a username" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="PasswordDiv">
                    <div class="PasswordLabelIconDiv">
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Password"></telerik:RadLabel>
                        <i class="fas fa-key"></i>
                    </div>
                    <div class="IconTextBox">
                        <telerik:RadTextBox ID="PasswordTextBox" runat="server" placeholder="Enter your password" TextMode="Password" Style="padding-right: 36px;">
                            <ClientEvents OnLoad="LoadPasswordtextbox" />
                        </telerik:RadTextBox>
                        <telerik:RadImageButton ID="PasswordEye" AutoPostBack="false" CausesValidation="false" CssClass="PasswordEyeButton" runat="server" OnClientClicking="ShowOrHidePassword"  >
                            <ContentTemplate>
                                <i id="ShowPasswordEye" class="fas fa-eye"></i>
                                <i id="HidePasswordEye" class="fas fa-eye-slash" style="display: none"></i>
                            </ContentTemplate>
                        </telerik:RadImageButton>
                    </div>
                    <div class="LoginValidatorDiv">
                        <asp:RequiredFieldValidator ID="PassswordTextBoxFieldValidator" ForeColor="Red" ControlToValidate="PasswordTextBox" runat="server" ErrorMessage="Please enter a Password" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="LoginButtonLockedLabel">
                    <telerik:RadButton ID="LoginButton" CssClass="AddOrEditButtons" HoveredCssClass="AddOrEditButtonsHover" runat="server" Text="LOG IN" Width="100%" Style="margin-top: 5px;"></telerik:RadButton>
                    <telerik:RadLabel ID="AccountLockedLabel" Style="text-align: center" Visible="false" runat="server" Text="Account Locked" ForeColor="Red"></telerik:RadLabel>
                    <telerik:RadLabel ID="UsernamePassswordIncorrectLabel" Style="text-align: center; padding:10px; display:none;" runat="server" Text="Incorrect Username or Password" ForeColor="Red"></telerik:RadLabel>
                </div>
            </div>
        </asp:Panel>
    </section>
</asp:Content>
