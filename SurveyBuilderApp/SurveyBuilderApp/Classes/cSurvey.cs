using System;
using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cSurvey
    {
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public string SurveyName { get; set; }
        [DataMember]
        public DateTime SurveyCreatedDateTime { get; set; }
        [DataMember]
        public DateTime LastModifiedDateTime { get; set; }
        [DataMember]
        public int SurveyVersion  { get; set; }
    }
}
