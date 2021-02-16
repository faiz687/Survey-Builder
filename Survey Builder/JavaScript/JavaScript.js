var Radgrid;
var PasswordTextBox;

function CalulateScreenSize() {
    var ScreenWidth = window.outerWidth;
    if (ScreenWidth < 700) {
        HideRadGridColumn()
    }
    else {
        ShowRadGridColumn()
    }
}
function DisableMainBodyScroll() {
    var MainBody = document.getElementById("SurveyBuilderBody");
    MainBody.style.overflow = "hidden";
}

function EnableMainBodyScroll() {
    var MainBody = document.getElementById("SurveyBuilderBody");
    MainBody.style.overflow = "initial";
}

function ShowRadGridColumn() {
    Radgrid.get_masterTableView().get_columns().length - 1;
    Radgrid.get_masterTableView().showColumn(2);
}

function HideRadGridColumn() {
    Radgrid.get_masterTableView().get_columns().length - 1;
    Radgrid.get_masterTableView().hideColumn(2);    
}

function GridCreated(sender, eventArgs) {
    Radgrid = sender;
    CalulateScreenSize();
}

function LoadPasswordtextbox(sender, args) {
    PasswordTextBox = sender;
}

function LoadUsernametextbox(sender, args) {
    UserTextBox = sender;
}

function ShowOrHidePassword(sender, args) {
    var PasswordEyeShow = document.getElementById("ShowPasswordEye");
    var PasswordEyeHide = document.getElementById("HidePasswordEye");
    if (getComputedStyle(PasswordEyeShow, null).display === "inline-block") {
        PasswordTextBox.get_element().setAttribute("type", "text");
        PasswordEyeShow.style.display = "none";
        PasswordEyeHide.style.display = "inline-block";
    } else {
        PasswordTextBox.get_element().setAttribute("type", "password");
        PasswordEyeShow.style.display = "inline-block";
        PasswordEyeHide.style.display = "none";
    }
}

function LoadUserAccountsButton(sender, args) {
    UserAccountsButton = sender;
}

function MinimizeOrExpandNavigation() {
    var NavigationButtons = document.getElementsByClassName("NavigationButton");
    for (var i = 0; i < NavigationButtons.length; i++) {
        if (NavigationButtons[i].classList.contains("DropMenu")) {
            NavigationButtons[i].classList.remove("DropMenu");
        }
        else {
            NavigationButtons[i].classList.add("DropMenu");
        }
    }
}

function ShowOverlayPage() {
    var OverlayPage = document.getElementsByClassName("OverlayPage");
    var Bodytag = document.getElementsByTagName("body");
    OverlayPage[0].style.display = "block";
    Bodytag[0].style.overflow = "hidden";
}

function TurnOffOverlayPage(sender, args) {
    var OverlayPage = document.getElementsByClassName("OverlayPage");
    var Bodytag = document.getElementsByTagName("body");
    OverlayPage[0].style.display = "none";
    Bodytag[0].style.overflow = "initial";
}

function ShowIncorrectPasswordOrUsernameLabel(Message) {
    $("#ctl00_ContentPlaceHolder1_UsernamePassswordIncorrectLabel")[0].textContent = Message; 
    $("#ctl00_ContentPlaceHolder1_UsernamePassswordIncorrectLabel").show().delay(1000).fadeOut();
}

function ValidateKeyPress(sender, args) {
    if (args.get_keyCode() == 13) {
        args.set_cancel(true);
    }
}
function ShowSliderValueOnNumericalTextBox(slider, args) {
    var ResponsePanel = slider._element.parentElement;
    var NumericalTextBox = ResponsePanel.getElementsByClassName("SliderNumericalTextBox")[0];
    NumericalTextBox.value = slider.get_value();
}

function ValidateKeyPressInput(event) {
    if (event.keyCode == 13) {
        event.stopPropagation();
        event.preventDefault();
        return false;
    }
    else {
        return true;
    }    
}

function ActivateMe(ButtonPressed) {   
    var AllnavigationButtons = $(".NavigationButton");
    for (i = 0; i < AllnavigationButtons.length; i++) {
        if (AllnavigationButtons[i].classList.contains("MenuButtonPressedCss")) {
            AllnavigationButtons[i].classList.remove("MenuButtonPressedCss")
        }
    }
    for (i = 0; i < AllnavigationButtons.length; i++) {
        if (AllnavigationButtons[i].value == ButtonPressed) {
            AllnavigationButtons[i].classList.add("MenuButtonPressedCss")
        }
    }
}

function HighlightIncompleteQuestion(QuestionId) {
    setTimeout(function () {
        $('#' + QuestionId)[0].scrollIntoViewIfNeeded();
        $('#' + QuestionId)[0].style.border = "2px solid red";
    }, 100);
}

function SuccessNotification(Message) {
    $.notify(Message, {
        position: "bottom left",
        className: 'success',
    });
}
function FailedNotification(Message) {
    $.notify(Message, {
        position: "bottom left",
        className: 'error',
    });
}
