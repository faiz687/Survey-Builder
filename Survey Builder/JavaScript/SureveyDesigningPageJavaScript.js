var AdjustSliderScale;
var StartinRangeSliderTextBox;
var EndingRangeSliderTextBox;
var SliderDropDownOption;
var QuestionListView;
var NewQuestionTextBox;

function AdjustSliderRangeCheckBoxLoad(sender) {
    AdjustSliderScale = sender;
}

function LoadQuestionListView(sender, args) {
    QuestionListView = sender; 
}

function LoadNewQuestionTextBox(sender) {
    NewQuestionTextBox = sender
}

function AdjustSliderRangeChecked(sender) {
    var SliderRangePanel = $("#ContentPlaceHolder1_SliderRangePanel");
    if (sender.get_checked() == true) {
        SliderRangePanel[0].style.display = "block";
    }
    else {
        SliderRangePanel[0].style.display = "none";
    }
}
function ShowSliderValueOnNumericalTextBox(slider, args) {
    var ResponsePanel = slider._element.parentElement;
    var NumericalTextBox = ResponsePanel.getElementsByClassName("SliderNumericalTextBox")[0];
    NumericalTextBox.value = slider.get_value();
}

function IsSliderScaleRangesValid() {
    var StartingRangeBoxValue = document.getElementsByClassName("StartingRangeBox")[0].value;
    var EndingRangeBoxValue = document.getElementsByClassName("EndingRangeBox")[0].value;
    if (StartingRangeBoxValue.length >= 1 && EndingRangeBoxValue.length >= 1) {
        if (parseInt(StartingRangeBoxValue) < parseInt(EndingRangeBoxValue)) {
            return true;
        }
        return false;
    }
    else {
        return false;
    }
}
function DisplayEdit() {
    var EditButton = document.getElementsByClassName("EditButtonDiv")[0].getElementsByTagName("button");
    EditButton[0].classList.remove("SurveyNameEdit");
}

function RemoveEdit() {
    var EditButton = document.getElementsByClassName("EditButtonDiv")[0].getElementsByTagName("button");
    EditButton[0].classList.add("SurveyNameEdit");
}
function DisplayReorder(x) {
    x.querySelector(".MoveUpDiv").style.visibility = "initial";
    x.querySelector(".MoveDownDiv").style.visibility = "initial";
}
function HideReorder(x) {
    x.querySelector(".MoveUpDiv").style.visibility = "hidden";
    x.querySelector(".MoveDownDiv").style.visibility = "hidden";
}
function RemoveAnswerChoice(sender) {
    if (sender.parentElement.parentElement.childElementCount >= 3)
    {
        sender.parentElement.remove();
    }
    else
    {
        ShowMinimumTwoRepsonsesLabel();
    }
}
function AddAnotherAnswerChoice(sender) {

    var IsSenderSlider = $(sender).parents(':eq(2)')[0].classList.contains("SliderPanel");
    if (IsSenderSlider) {
        if (sender.parentElement.parentElement.childElementCount <= 4) {
            var ClonedElement = sender.parentElement.cloneNode(true);
            ClonedElement.firstElementChild.value = "";
            var ResponsesPanel = sender.parentElement.parentElement;
            ResponsesPanel.insertBefore(ClonedElement, sender.parentElement.nextSibling);
        }
        else {
            ShowMaximumFiveResponsesLabelSlider();
        }
    }
    else {
        var ClonedElement = sender.parentElement.cloneNode(true);
        ClonedElement.firstElementChild.value = "";
        var ResponsesPanel = sender.parentElement.parentElement;
        ResponsesPanel.insertBefore(ClonedElement, sender.parentElement.nextSibling)
    }
}
function IsStepRangeValid() {
    var StartingRangeBoxValue = document.getElementsByClassName("StartingRangeBox")[0].value;
    var EndingRangeBoxValue = document.getElementsByClassName("EndingRangeBox")[0].value;
    var StepsRangeBoxValue = document.getElementsByClassName("StepSizeBox")[0].value;
    var CorrectStepsRange = parseInt(EndingRangeBoxValue) - parseInt(StartingRangeBoxValue);
    if (StepsRangeBoxValue >= 1  && StepsRangeBoxValue <= CorrectStepsRange) {
        return true;
    }
    else {
        ShowIncorrectStepsRangelabel(CorrectStepsRange);
        return false;
    }
}

function SliderOptionSeleted(SliderOptionDropDpown, args) {
    var SliderID = SliderOptionDropDpown.get_id();
    var selectedSliderOptions = args.get_item().get_value();
    SliderDropDownOption = SliderOptionDropDpown;
    if (SliderID == "ctl00_ContentPlaceHolder1_SliderOptionDropdown") {
        if (selectedSliderOptions == 1) {
            $("#ContentPlaceHolder1_SliderItemsPanel")[0].style.display = "block";
            $("#ContentPlaceHolder1_SliderRangePanel")[0].style.display = "none";
        }
        else {
            $("#ContentPlaceHolder1_SliderItemsPanel")[0].style.display = "none";
            $("#ContentPlaceHolder1_SliderRangePanel")[0].style.display = "block";
        }
    }
    else
    {
        var SldierItemsPanelId = SliderOptionDropDpown._element.parentElement.children[1].id;
        var SliderRangePanelId = SliderOptionDropDpown._element.parentElement.children[2].id;
        if (selectedSliderOptions == 1) {
            $("#" + SldierItemsPanelId +"")[0].style.display = "block";
            $("#" + SliderRangePanelId + "")[0].style.display = "none";
        }
        else {
            $("#" + SldierItemsPanelId + "")[0].style.display = "none";
            $("#" + SliderRangePanelId + "")[0].style.display = "block";
        }
    }
}  
function IsNewQuestionTextBoxEmpty(sender, args) {
    if (NewQuestionTextBox._value.length == 0) {
        ShowQuestionBoxEmptyLabel();
        args.set_cancel(true);
        return false;
    }
    else {
        return true;
    }
}
function SaveSliderResponses(sender, args) {
    var SelectedSliderOption = SliderDropDownOption.get_selectedItem().get_value();
    if (!NewQuestionTextBox._value.length == 0) {
        if (SelectedSliderOption == 1) {
            var SliderResponsesList = [];
            var AllSliderResponses = $(".AnswerChoiceTextBox");
            for (var i = 0; i < AllSliderResponses.length; i++) {
                if (AllSliderResponses[i].value) {
                    SliderResponsesList.push(AllSliderResponses[i].value);
                }
            }
            if (SliderResponsesList.length >= 2) {
                PageMethods.SaveAllResponses(SliderResponsesList);
                SliderResponsesList.clear
                return true;
            }
            else {
                ShowMinimumTwoRepsonsesLabel();
                args.set_cancel(true);
                SliderResponsesList.clear
                return false;
            }
            SliderResponsesList.clear;
        }
        else {
            if (IsSliderScaleRangesValid()) {
                if (IsStepRangeValid()) {
                    return true;
                }
                else {
                    args.set_cancel(true);
                    return false;
                }
            }
            else {
                ShowIncorrectSliderRangelabel();
                args.set_cancel(true);
                return false;
            }
        }
    }
    else {
        ShowQuestionBoxEmptyLabel();
        args.set_cancel(true);
        SliderResponsesList.clear
        return false;
    }
}

function SaveResponsesRadioAndMultipleBoxes() {
    var ResponsesList = [];
    var ResponseTextBoxes = $(".AnswerChoiceTextBox");
    if (!NewQuestionTextBox._value.length == 0) {
        for (var i = 0; i < ResponseTextBoxes.length; i++) {
            if (ResponseTextBoxes[i].value) {
                ResponsesList.push(ResponseTextBoxes[i].value);
            }
        }
        if (ResponsesList.length >= 2) {
            PageMethods.SaveAllResponses(ResponsesList);
            ResponsesList.clear
            return true;
        }
        else {
            ShowMinimumTwoRepsonsesLabel();
            args.set_cancel(true);
            ResponsesList.clear
            return false;
        }
    }
    else {
        ShowQuestionBoxEmptyLabel();
        args.set_cancel(true);
        return false;
    }
    ResponsesList.clear;
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
        className: 'error' ,
    });
}
function ShowMinimumTwoRepsonsesLabel() {
    var ShowMinimumTwoRepsonsesLabel = document.getElementsByClassName("MinimumResponsesLabel")[0].id;
    $("#" + ShowMinimumTwoRepsonsesLabel + "").show().delay(3000).fadeOut();
}
function ShowMaximumFiveResponsesLabelSlider() {
    var ShowMaximumFiveResponsesLabelSlider = document.getElementsByClassName("MaximumResponsesLabel")[0].id;
    $("#" + ShowMaximumFiveResponsesLabelSlider + "").show().delay(3000).fadeOut();
}
function ShowQuestionBoxEmptyLabel() {
    var NewTextBoxValidatorLabel = document.getElementsByClassName("QuestionTextBoxValidatorLabel")[0].id;
    $("#"+NewTextBoxValidatorLabel+"").show().delay(3000).fadeOut();
}
function ShowIncorrectSliderRangelabel() {
    var IncorrectSliderRangeLabelEdit = document.getElementsByClassName("IncorrectSliderRangeLabel")[0].id;
    $("#" + IncorrectSliderRangeLabelEdit + "").show().delay(3000).fadeOut();

}
function ShowIncorrectStepsRangelabel(StepsRange) {
    var IncorrectStepRangeLabelEdit = document.getElementsByClassName("IncorrectStepRangeLabel")[0];
    IncorrectStepRangeLabelEdit.innerHTML = "Step size must be a whole number between 1 and " + StepsRange.toString();
    var IncorrectStepRangeLabelEditId = IncorrectStepRangeLabelEdit.id
    $("#" + IncorrectStepRangeLabelEditId + "").show().delay(3000).fadeOut();
}