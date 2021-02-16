<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="AddUserPage.aspx.vb" Inherits="Survey_Builder.AddUserPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="CurrentPageMainBody">
        <section>
            <telerik:RadImageButton ID="GoBackButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" runat="server" Text="Back" OnClick="GoBackButton_Click">
                <ContentTemplate>
                    <i class="fas fa-arrow-left"></i>Back
                </ContentTemplate>
            </telerik:RadImageButton>
            <telerik:RadLabel ID="UserSIDLabel" Visible="false" runat="server"></telerik:RadLabel>
        </section>
        <section>
            <telerik:RadPageLayout ID="AddPageTableLayout" runat="server" CssClass="RadPageLayout">
                <Rows>
                    <telerik:LayoutRow CssClass="DarkWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="FindUserlabel" runat="server" Text="Find User" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadComboBox  RenderMode="Lightweight" Filter="StartsWith" ID="UsernameCombobox" runat="server" Width="100%" AutoPostBack="true" EmptyMessage="Find User" OnItemsRequested="UsernameCombobox_ItemsRequested1" EnableLoadOnDemand="true" 
                                   DataTextField="SearchName"  DataValueField="SID"  ></telerik:RadComboBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow CssClass="LightWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="UsernameLabel" runat="server" Text="Username" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadTextBox ID="UsernameTextBox" runat="server" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow CssClass="DarkWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="ForenameLabel" runat="server" Text="Forename" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadTextBox ID="ForenameTextBox" runat="server" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow CssClass="LightWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="SurnameLabel" runat="server" Text="Surname" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadTextBox ID="SurnameTextBox" runat="server" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow CssClass="DarkWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="EmailLabel" runat="server" Text="Email" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadTextBox ID="EmailTextBox" runat="server" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow CssClass="LightWhiteRow AllRowCss">
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="PermissionLabel" runat="server" Text="Permission" Width="100%"></telerik:RadLabel>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="8">
                                <telerik:RadDropDownList ID="PermissionDropDownList"  DataTextField="PermissionName" DataValueField="PermissionID" runat="server" Width="100%"></telerik:RadDropDownList>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
            <telerik:RadImageButton ID="SaveButton" CssClass="SurveyBuilderButton  SurveyBuilderIconButton SaveButton SaveButtonHover" runat="server" Text="Back" OnClick="SaveButton_Click">
                <ContentTemplate>
                    <i class="fas fa-user-plus"></i>Add User
                </ContentTemplate>
            </telerik:RadImageButton>
        </section>
    </main>
</asp:Content>