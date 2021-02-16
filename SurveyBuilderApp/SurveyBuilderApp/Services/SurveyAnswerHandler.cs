using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using System.IO;
using System.Net;

namespace SurveyBuilderApp.Services
{
    public class SurveyAnswerHandler : ISurveyAnswerHandler
    {
        public string InsertSurveyAnswer(cSurveyAnswer SurveyAnswer)
        {
            string mStatusResponse = "";
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                try
                {
                    var mRequest = HttpWebRequest.Create(""); // removed for security.
                    mRequest.ContentType = "application/json";
                    mRequest.Method = "POST";
                    mRequest.Headers[""] = ""; // removed for security.

                    var mJSON = JsonConvert.SerializeObject(SurveyAnswer);
                    using (StreamWriter sw = new StreamWriter(mRequest.GetRequestStream()))
                    {
                        sw.Write(mJSON);
                        sw.Flush();
                        sw.Close();
                    }
                    using (HttpWebResponse mResponse = mRequest.GetResponse() as HttpWebResponse)
                    {
                        mStatusResponse = ((HttpWebResponse)mResponse).StatusDescription;
                        if (mResponse.StatusCode != HttpStatusCode.OK)
                        {
                            return "Error";
                        }
                        else
                        {
                            using (StreamReader sr = new StreamReader(mResponse.GetResponseStream()))
                            {
                                var content = sr.ReadToEnd();
                                if (string.IsNullOrWhiteSpace(content))
                                {
                                    return "No Content";
                                }
                                else
                                {
                                    string SurveyAnswerId;
                                    SurveyAnswerId = JsonConvert.DeserializeObject<string>(content);
                                    return SurveyAnswerId;
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    string mStatus = (ex.Response as HttpWebResponse)?.StatusCode.ToString();
                    return mStatus;
                }
            }
            else
            {
                return "No internet connectiuon";
            }
        }
    }
}
