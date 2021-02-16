using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBuilderApp.Services
{
    public class SurveyPinHandler : ISurveyPinHandler
    {
        public string DeleteSurveyPin(cSurveysPin Pin)
        {
            throw new NotImplementedException();
        }
        public string GetSurveyPin(cSurveysPin Pin)
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

                    JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };
                    var mJSON = JsonConvert.SerializeObject(Pin, microsoftDateFormatSettings);
                    Debug.WriteLine(mJSON);
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
                                    string SurveyPin;
                                    SurveyPin = JsonConvert.DeserializeObject<string>(content);
                                    return SurveyPin;
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
        public string InsertPin(cSurveysPin Pin)
        {
            string mStatusResponse = "";
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                try
                {
                    var mRequest = HttpWebRequest.Create(""); // removed for security.
                    mRequest.ContentType = "application/json";
                    mRequest.Method = "POST";
                    mRequest.Headers["KeyCode"] = ""; // removed for security.

                    JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };
                    var mJSON = JsonConvert.SerializeObject(Pin, microsoftDateFormatSettings);
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
                                    string SurveyPin;
                                    SurveyPin = JsonConvert.DeserializeObject<string>(content);
                                    return SurveyPin;
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
        public string UpdatePinUsedGeneratedDateTime(cSurveysPin Pin)
        {
            throw new NotImplementedException();
        }
    }
}
