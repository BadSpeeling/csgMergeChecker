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
using Newtonsoft.Json.Linq;

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

        public async Task<(string WorkItemID, string AssignedTeamMember)?> GetTicketAssignment(string tfsTicketNumber)
        {

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(
                            $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/wit/workitems/{tfsTicketNumber}?api-version=7.2-preview.3&$expand=links"))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jObject = JObject.Parse(responseBody);

                    var ticketAssignedToName = jObject.SelectToken("$.fields['System.AssignedTo'].displayName")?.Value<string>();
                    return (WorkItemID: tfsTicketNumber, AssignedTeamMember: ticketAssignedToName ?? "Unassigned");
                    
                }
            }


        }

        public async Task<WorkItemUpdate?> GetWorkItemUpdate(string tfsTicketNumber)
        {

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(
                            $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/wit/workItems/{tfsTicketNumber}/updates"))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<WorkItemUpdate>(responseBody);

                }
            }

        }

        public async Task<IEnumerable<string>?> GetIterationWorkItems(string iterationID, string teamID)
        {

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                using (HttpResponseMessage response = await client.GetAsync(
                    $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/{teamID}/_apis/work/teamsettings/iterations/{iterationID}/workitems?api-version=7.1"))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }
                    
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jObject = JObject.Parse(responseBody);

                    var jObjectTicketNumbers = jObject.SelectTokens("$.workItemRelations..target.id");
                    return jObjectTicketNumbers
                        .Select(t => t.Value<string>() ?? "0");
                    
                }

            }

        }

        public async Task<Changesets?> GetChangeset(string changesetNumber)
        {

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                using (HttpResponseMessage response = await client.GetAsync(
                    $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/tfvc/changesets/{changesetNumber}?maxChangeCount=100&api-version=7.2-preview.3"))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }
             
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Changesets>(responseBody);

                }

            }

        }

        public async Task<string?> GetFileContents(string sourceControlPath)
        {            

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                var fileContentsApiUrl = $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/tfvc/items?path={sourceControlPath}&api-version=7.2-preview.1";

                using (HttpResponseMessage response = await client.GetAsync(
                    fileContentsApiUrl))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;
                    }

                    return await response.Content.ReadAsStringAsync();                    

                }

            }

        }

        public async Task<IEnumerable<string>?> GetWorkItemsByQuery(string queryID)
        {

            using (HttpClient client = new HttpClient(GetHttpClientHandler()))
            {

                var wiqlQueryURL = $"https://tfs.prd.costargroup.com/CoStarCollection/CoStarInternalApps/_apis/wit/wiql/{queryID}?api-Version=1.0";

                using (HttpResponseMessage response = await client.GetAsync(
                    wiqlQueryURL))
                {

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return null;                        
                    }

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseBody);

                    var jObjectTicketNumbers = jObject.SelectTokens("$.workItems..id");
                    return jObjectTicketNumbers
                        .Select(t => t.Value<string>() ?? "0");

                }

            }

        }
    }

}
