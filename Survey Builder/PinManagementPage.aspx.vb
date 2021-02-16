Imports Microsoft.Ajax.Utilities

Public Class PinManagementPage
    Inherits System.Web.UI.Page
    Private Sub PinManagementPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Check.IsUserSignedIn(Me.Page) Then
                LoadDropDown()
            Else
                Response.Redirect("Default.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Else
        End If
    End Sub
    Private Sub LoadDropDown()
        Dim Survey As ISurveyHandler = New SurveyHandler
        Dim SurveysList As List(Of cSurvey) = Survey.GetAllSurvey("")
        SurveyNamesDropDownList.DataSource = SurveysList
        SurveyNamesDropDownList.DataBind()
    End Sub
    Protected Sub GeneratePinHereLabel_ServerClick(sender As Object, e As EventArgs)
        If Not SurveyNamesDropDownList.SelectedIndex = -1 Then
            Dim PinNumber As String = GenerateNewGuid()
            Dim SurveyPin As ISurveyPinHandler = New SurveyPinHandler
            If SurveyPin.InsertPin(New cSurveysPin With {.SurveyId = SurveyNamesDropDownList.SelectedValue, .PinNumber = PinNumber}) = True Then
                PinTextBox.Text = PinNumber
                PinMessageLabel.Text = "Generated pin number will expire in 24 hours"
            Else
                PinMessageLabel.Text = "Cannot generate pin number ,  contact Administrator"
            End If
            PinMessageLabel.Visible = True
        Else
            PinMessageLabel.Text = "Select A Survey"
            PinMessageLabel.Visible = True
        End If
    End Sub
    Private Function GenerateNewGuid() As String
        Dim SurveyPin As ISurveyPinHandler = New SurveyPinHandler
        Dim Pinexists As Boolean = True
        Dim NewSurveyPin As String = ""
        While Pinexists
            NewSurveyPin = Guid.NewGuid().ToString().Substring(0, 5)
            If IsNothing(SurveyPin.GetSurveyPin(New cSurveysPin With {.PinNumber = NewSurveyPin, .SurveyId = SurveyNamesDropDownList.SelectedValue})) Then
                Pinexists = False
            End If
        End While
        Return NewSurveyPin
    End Function
End Class
