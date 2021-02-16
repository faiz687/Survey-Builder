using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cControl
    {
        [DataMember]
        public int ControlId { get; set; }
        [DataMember]
        public string ControlName { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
    }
}
