using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
   public class cSurveyAnswer
    {
        [DataMember]
        public int AnswerId { get; set; }
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public string Platform { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public int SurveyVersion { get; set; }
    }
}
