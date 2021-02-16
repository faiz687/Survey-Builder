using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cSurveyQuestions
    {
        [DataMember]
        public int QuestionId { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string QuestionDescription { get; set; }           
        [DataMember]
        public bool Mandatory { get; set; }
        [DataMember]
        public int Control { get; set; }
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public DateTime QuestionCreatedDateTime { get; set; }
        [DataMember]
        public DateTime QuestionLastModifiedDateTime { get; set; }
        [DataMember]
        public int DisplayId { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        public string  QuestionAndNumber
        {
            get
            {
                return string.Format("Q{0}. {1}", DisplayId, Question);
            }
        }
    }
}
