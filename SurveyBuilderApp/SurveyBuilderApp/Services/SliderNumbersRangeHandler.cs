using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Net;


namespace SurveyBuilderApp.Services
{
    public class SliderNumbersRangeHandler : ISliderNumbersRange
    {
        public string GetPropertiesForSlider(int QuestionId)
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                Debug.WriteLine(QuestionId);
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
    }
}
