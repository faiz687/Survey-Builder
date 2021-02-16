using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
  public class cSliderNumbersRange
    {
        [DataMember]
        public int SliderId { get; set; }
        [DataMember]
        public int StartRange { get; set; }
        [DataMember]
        public int EndRange { get; set; }
        [DataMember]
        public int StepRange { get; set; }
        [DataMember]
        public bool NumericalTextBox { get; set; }
        [DataMember]
        public int QuestionId { get; set; }

    }
}
