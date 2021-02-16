using System;
using System.Runtime.Serialization;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class UpdatedSurveys
    {
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public Boolean SurveyUpdatedOrNot { get; set; }
    }
}
