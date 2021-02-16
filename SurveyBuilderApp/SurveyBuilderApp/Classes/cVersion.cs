using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class cVersion
    {
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
    }
}

