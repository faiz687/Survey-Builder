using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace SurveyBuilderApp.Services
{
    public class SurveyHandler : ISurveyHandler
    {
        public string GetAllSurvey(string SurveyName)
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                var request = HttpWebRequest.Create(""); // removed for security.
                request.ContentType = "application/json";
                request.Method = "GET";
                request.Headers[""] = ""; // removed for security.
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            return "Unauthorized";
                        }
                        else
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var content = reader.ReadToEnd();

                                if (string.IsNullOrWhiteSpace(content))
                                {
                                    return "No Content";
                                }
                                else
                                {
                                    string Version = JsonConvert.DeserializeObject<string>(content);
                                    return Version;
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
                return "No Internet Connection";
            }
        }
        public string GetSurveyFromServer(UpdatedSurveys Survey)
        {            
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                var request = HttpWebRequest.Create(""); // removed for security.
                request.ContentType = "application/json";
                request.Method = "GET";       
                 request.Headers[""] = ""; // removed for security.
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            return "Unauthorized";
                        }
                        else
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var content = reader.ReadToEnd();

                                if (string.IsNullOrWhiteSpace(content))
                                {
                                    return "No Content";
                                }
                                else
                                {
                                    string SurveyResponse = JsonConvert.DeserializeObject<string>(content);
                                    return SurveyResponse;
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
                return "No Internet Connection";
            }
        }
        public string GetUpdatedSurveyListFromServer(List<SurveyVersions> SurveyIdVersionList)
        {
            string mStatusResponse = "";
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                try
                {
                    var mRequest = HttpWebRequest.Create(""); // removed for security.
                    mRequest.ContentType = "application/json";
                    mRequest.Method = "POST";
                    mRequest.Headers[""] = "";// removed for security.

                    JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };
                    var mJSON = JsonConvert.SerializeObject(SurveyIdVersionList, microsoftDateFormatSettings);
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
                                    string ComparedResultsResponse;
                                    ComparedResultsResponse = JsonConvert.DeserializeObject<string>(content);
                                    return ComparedResultsResponse;
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
