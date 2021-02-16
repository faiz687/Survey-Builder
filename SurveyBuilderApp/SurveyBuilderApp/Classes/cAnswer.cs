using System;
using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cAnswer
    {
        [DataMember]
        public int AnswerId { get; set; }
        [DataMember]
        public int QuestionId { get; set; }
        [DataMember]
        public string Answer { get; set; }
        [DataMember]
        public DateTime AnswerDateTime { get; set; }

    }
}
