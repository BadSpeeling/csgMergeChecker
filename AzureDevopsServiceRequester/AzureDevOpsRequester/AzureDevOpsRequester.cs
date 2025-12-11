using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AzureMergeChecker.AzureDevOpsRequester
{
    public class AzureDevOpsRequester : Requester
    {

        private string Instance { get; set; }
        private string Project { get; set; }
        private AzureDevopsClient Client { get; set; }

        public AzureDevOpsRequester(string Instance, string Project) 
        {
            this.Instance = Instance;
            this.Project = Project;
            this.Client = new AzureDevopsClient();
        }

        private string GetURLRoot ()
        {
            return $"https://tfs.prd.costargroup.com/{Instance}/{Project}";
        }

        private async Task<string> SendRequest (string url)
        {
            return await Client.SendHttpRequest(url);
        }

        public async Task<string> GetChangesetAsync(string changesetNumber)
        {
            var url = $"{GetURLRoot()}/_apis/tfvc/changesets/{changesetNumber}?maxChangeCount=100&api-version=7.2-preview.3";
            return await SendRequest(url);                
        }

        public async Task<string?> GetFileContentsAsync(string sourceControlPath)
        {
            var url = $"{GetURLRoot()}/_apis/tfvc/items?path={sourceControlPath}&api-version=7.2-preview.1";
            return await Client.GetDevOpsFile(url);
        }

        public async Task<string> GetIterationWorkItemsAsync(string iterationID, string teamID)
        {
            var url = $"{GetURLRoot()}/{teamID}/_apis/work/teamsettings/iterations/{iterationID}/workitems?api-version=7.1";
            return await SendRequest(url);
        }

        public async Task<string> GetTicketAssignmentAsync(string tfsTicketNumber)
        {
            var url = $"{GetURLRoot()}/_apis/wit/workitems/{tfsTicketNumber}?api-version=7.2-preview.3&$expand=links";
            return await SendRequest(url);
        }

        public async Task<string> GetWorkItemsByQuery(string queryID)
        {
            var url = $"{GetURLRoot()}/_apis/wit/wiql/{queryID}?api-Version=1.0";
            return await SendRequest(url);
        }

        public async Task<string> GetWorkItemUpdateAsync(string tfsTicketNumber)
        {
            var url = $"{GetURLRoot()}/_apis/wit/workItems/{tfsTicketNumber}/updates";
            return await SendRequest(url);
        }
    }
}
