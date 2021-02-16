using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cSurveysPin
    {
        [DataMember]
        public int PinId { get; set; }
        [DataMember]
        public string PinNumber { get; set; }
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public DateTime PinGeneratedDatetime { get; set; }
        [DataMember]
        public DateTime PinUsedDateTime { get; set; }
    }
}
