<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master"  CodeBehind="AddOrEditUsersPage.aspx.vb" Inherits="Survey_Builder.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="CurrentPageMainBody">
        <section class="AddUserSection">
            <telerik:RadImageButton ID="GoBackButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton"  runat="server" OnClick="GoBackButton_Click" Text="Back">
                <ContentTemplate>
                    <i class="fas fa-arrow-left"></i>Back
                </ContentTemplate>
            </telerik:RadImageButton>
            <telerik:RadImageButton ID="AddUserButton" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" runat="server" Text="Add User" OnClick="AddUserButton_Click">
                <ContentTemplate>
                    <i class="fas fa-user-plus"></i>Add Users
                </ContentTemplate>
            </telerik:RadImageButton>
        </section>
        <section class="SearchandEditUserSection">
            <div class="SearchUsersDiv">
                <telerik:RadTextBox ID="ForenameTextBox" EmptyMessage="Forename" ClientEvents-OnKeyPress="ValidateKeyPress" runat="server" ></telerik:RadTextBox>
                <telerik:RadTextBox ID="SurnameTextBox" EmptyMessage="Surname" ClientEvents-OnKeyPress="ValidateKeyPress" runat="server" ></telerik:RadTextBox>
                <telerik:RadImageButton ID="SearchButton" OnClick="SearchButton_Click" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton"  runat="server" Text="Search">
                    <ContentTemplate>
                        <i class="fas fa-search"></i>Search
                    </ContentTemplate>
                </telerik:RadImageButton>
            </div>
            <div class="EditUsersDiv">
                <asp:Panel runat="server" ID="DeletePanel" Visible="false">
                    <section class="DeleteConfirmationSection  DeleteConfirmationPadding">
                        <asp:Panel ID="DeleteConfirmationPanel" runat="server">
                            <asp:Label ID="UserIdLabel" Visible="false" runat="server"></asp:Label>
                            <div class="DeleteConfirmationHeader">
                                <telerik:RadLabel ID="DeleteHeaderMessage" runat="server" Text="Delete the account ?" Style="padding: 10px;"></telerik:RadLabel>
                            </div>
                            <div class="DeleteConfirmationBody">
                                <telerik:RadLabel ID="DeleteMessageDescription" runat="server" Style="font-weight: 100; font-size: large; margin-bottom: 5px;"></telerik:RadLabel>
                                <div class="DeleteCancelButtons">
                                    <telerik:RadButton ID="CancelButton" runat="server" Text="Cancel" CssClass="SurveyBuilderButton SurveyBuilderButtonHover SurveyBuilderIconButton" OnClick="CancelButton_Click"></telerik:RadButton>
                                    <telerik:RadButton ID="ConfirmDeleteButton" runat="server" CssClass="SurveyBuilderButton DeleteButton DeleteButtonHovered" Text="Delete User" OnClick="ConfirmDeleteButton_Click"></telerik:RadButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="DeleteSuccessfullOrFailPanel" runat="server" Visible="false" Style="text-align: center;">
                            <asp:Label ID="DeleteMessagelabel" runat="server" Style="display: block; margin: 10px; font-size: large;"></asp:Label>
                            <telerik:RadButton ID="OkButton" runat="server" CssClass="AddOrEditButtons" HoveredCssClass="AddOrEditButtonsHover" style="margin-bottom:10px;" Text="OK" OnClick="OkButton_Click"></telerik:RadButton>
                        </asp:Panel>
                    </section>
                </asp:Panel>
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" Style="overflow: auto;">
                    <ClientSettings AllowColumnHide="True" ReorderColumnsOnClient="True">
                        <ClientEvents OnGridCreated="GridCreated" />
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" HeaderStyle-Font-Bold="true" DataKeyNames="UserId" EditMode="EditForms">
                        <NoRecordsTemplate>
                            <p style="text-align: center; color: red">
                                No Users Found
                            </p>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridBoundColumn DataField="FullName" HeaderText="Name" UniqueName="FullNameColumn">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Username" HeaderText="Username">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Permission.Permissionname" HeaderText="Permission">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn DataField="Locked" HeaderText="Account Status" UniqueName="LockedColumn">
                                <ItemTemplate>
                                    <div id="StatusDiv" style="display: inline; padding: 10px; font-size: 15px; border-radius: 5px;" runat="server">
                                        <asp:Label ID="StatusLabel" runat="server" Font-Size="Small" ForeColor="White" Text='<%# Eval("Locked") %>' />
                                    </div>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="EditColumn">
                                <ItemTemplate>
                                    <telerik:RadImageButton ID="EditButton" CssClass="EditDetailsButton EditDetailsButtonHovered" OnClick="EditButton_Click1" runat="server" Text="Edit Details">
                                        <ContentTemplate>
                                            <i class="fas fa-edit" style="padding: 10px;"></i></i>Edit
                                        </ContentTemplate>
                                    </telerik:RadImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="DeleteColumn">
                                <ItemTemplate>
                                    <telerik:RadImageButton ID="DeleteButton" CssClass="EditDetailsButton EditDetailsButtonHovered" OnClick="DeleteButton_Click" runat="server" Text="Delete">
                                        <ContentTemplate>
                                            <i class="fas fa-user-times" style="padding: 10px;"></i></i>Delete
                                        </ContentTemplate>
                                    </telerik:RadImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </section>
    </main>
</asp:Content>



