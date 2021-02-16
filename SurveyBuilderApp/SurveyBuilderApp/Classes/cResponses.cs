using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
  public class cResponses
    {
        [DataMember]
        public int ResponseId { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public int QuestionId { get; set; }

    }
}
