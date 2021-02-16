using Newtonsoft.Json;
using SurveyBuilderApp.Classes;
using SurveyBuilderApp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBuilderApp.Services
{
    public class VersionHandler : IVersionHandler
    {
        public string GetLatestVersion()
        {
            if (Reachability.IsHostReachable("http://www.google.co.uk"))
            {
                var request = HttpWebRequest.Create(""); // removed for security.
                request.ContentType = "application/json";
                request.Method = "GET";
                request.Headers[""] = "";  // removed for security.                                             
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
