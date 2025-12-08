using Microsoft.TeamFoundation.Build.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using AzureDevopsServiceRequester.AzureDevopsTypes;
using AzureDevopsServiceRequester.CheckinVerification;
using System.Net;

namespace AzureDevopsServiceRequester
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

        public async Task GetWorkItems(string tfsTicketNumber)
        {

            try
            {

                using (HttpClient client = new HttpClient(GetHttpClientHandler()))
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    //    Convert.ToBase64String(
                    //        System.Text.ASCIIEncoding.ASCII.GetBytes(
                    //            string.Format("{0}:{1}", "", personalaccesstoken))));

                    using (HttpResponseMessage response = await client.GetAsync(
                                //"https://dev.azure.com/{organization}/_apis/projects"))
                                $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/wit/workitems/{tfsTicketNumber}?api-version=7.2-preview.3&$expand=links"))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseBody);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task<WorkItemUpdate?> GetWorkItemUpdate(string tfsTicketNumber)
        {

            WorkItemUpdate? workItemUpdateResponse = null;

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                //    Convert.ToBase64String(
                //        System.Text.ASCIIEncoding.ASCII.GetBytes(
                //            string.Format("{0}:{1}", "", personalaccesstoken))));

                using (HttpResponseMessage response = await client.GetAsync(
                            $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/wit/workItems/{tfsTicketNumber}/updates"))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        workItemUpdateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkItemUpdate>(responseBody);
                    }
                }
            }

            if (workItemUpdateResponse == null)
            {
                //log?
                return null;                
            }

            return workItemUpdateResponse;

        }

        //public async Task GetIterationWorkItems(string iterationID, string teamID)
        //{

        //    using (HttpClient client = new HttpClient(GetHttpClientHandler()))
        //    {

        //        using (HttpResponseMessage response = await client.GetAsync(
        //            $"https://tfs.prd.costargroup.com/CoStarCollection/{teamID}/_apis/work/teamsettings/iterations/{iterationID}/workitems?api-version=7.1"))
        //        {

        //            response.EnsureSuccessStatusCode();
        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            Console.WriteLine(responseBody);

        //        }

        //    }

        //}

        public async Task<Changesets> GetChangeset(string changesetNumber)
        {

            Changesets? changesetsResponse;

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                using (HttpResponseMessage response = await client.GetAsync(
                    $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/tfvc/changesets/{changesetNumber}?maxChangeCount=100&api-version=7.2-preview.3"))
                {

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    changesetsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Changesets>(responseBody);

                }

            }

            if (changesetsResponse == null)
            {
                throw new Exception("The response from __apis/tfvc/changesets could not be deserialized");
            }

            return changesetsResponse;

        }

        public async Task<string?> GetFileContents(string sourceControlPath)
        {

            string? fileContents = null;

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                var fileContentsApiUrl = $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/tfvc/items?path={sourceControlPath}&api-version=7.2-preview.1";

                using (HttpResponseMessage response = await client.GetAsync(
                    fileContentsApiUrl))
                {

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        fileContents = await response.Content.ReadAsStringAsync();
                    }
                    
                }

            }

            return fileContents;

        }

    }

}
