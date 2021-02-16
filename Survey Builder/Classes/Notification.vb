Public NotInheritable Class Notification
    Public Shared Function ShowSuccessNotification(Page As Page, Message As String) As Boolean
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SuccessNotification", "SuccessNotification('" + Message + "');", True)
    End Function

    Public Shared Function ShowFailNotification(Page As Page, Message As String) As Boolean
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "FailedNotification", "FailedNotification('" + Message + "');", True)
    End Function

End Class
