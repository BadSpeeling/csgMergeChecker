using Microsoft.TeamFoundation.Build.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using AzureMergeChecker.AzureDevopsTypes;
using AzureMergeChecker.CheckinVerification;
using System.Net;
using Newtonsoft.Json.Linq;

namespace AzureMergeChecker
{
    public class AzureDevopsClient
    {

        public AzureDevopsClient()
        {
        }

        public HttpClientHandler GetHttpClientHandler ()
        {
            return new HttpClientHandler() { UseDefaultCredentials = true };
        }

        public async Task<string> SendHttpRequest(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient(GetHttpClientHandler()))
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not send a request to {url}.  Error: {ex.ToString()}");
            }
        }

        public async Task<string?> GetDevOpsFile(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient(GetHttpClientHandler()))
                {

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }
                        
                        return await response.Content.ReadAsStringAsync();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not send a request to {url}.  Error: {ex.ToString()}");
            }

        }

    }

}
