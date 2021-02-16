using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SurveyBuilderApp.Classes
{
    [Foundation.Preserve(AllMembers = true)]
    [DataContract]
    public class SurveyVersions
    {
        [DataMember]
        public int SurveyId { get; set; }
        [DataMember]
        public int SurveyVersion { get; set; }
    }
}