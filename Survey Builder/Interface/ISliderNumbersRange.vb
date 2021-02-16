Public Interface ISliderNumbersRange
    Function AddSliderProperties(ByVal SliderProperties As cSliderNumbersRange) As Boolean
    Function GetPropertiesForSlider(ByVal QuestionId As Integer) As cSliderNumbersRange
    Function DeleteSliderPropertiesById(ByVal QuestionId As Integer) As Boolean
End Interface
