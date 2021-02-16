Public Class UserAccountPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Else

        End If
    End Sub

    Protected Sub AddorEditUserButtonClicked(sender As Object, e As EventArgs)
        Response.Redirect("AddOrEditUsersPage.aspx?PreviousPage=AddEditUser", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub DeleteUserButtonClicked(sender As Object, e As EventArgs)
        Response.Redirect(String.Format("AddOrEditUsersPage.aspx?PreviousPage=DeleteUser"), False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class