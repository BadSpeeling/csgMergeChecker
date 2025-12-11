using AzureMergeChecker.AzureDevOpsRequester;
using AzureMergeChecker.AzureDevopsTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker
{
    public class AzureDevOpsOperation
    {

        private Requester Requester { get; set; }

        public AzureDevOpsOperation(Requester Requester)
        {
            this.Requester = Requester;
        }

        public async Task<(string WorkItemID, string AssignedTeamMember)> GetTicketAssignment(string tfsTicketNumber)
        {

            var responseBody = await Requester.GetTicketAssignmentAsync(tfsTicketNumber);
            var assignedTeamMember = JsonExtractor.ExtractAssignmentFromWorkItem(responseBody);

            return (tfsTicketNumber, assignedTeamMember ?? "Unassigned");

        }
    
        public async Task<WorkItemUpdate?> GetWorkItemUpdate(string tfsTicketNumber)
        {

            var responseBody = await Requester.GetWorkItemUpdateAsync(tfsTicketNumber);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<WorkItemUpdate>(responseBody);
    
        }

        public async Task<IEnumerable<string>?> GetIterationWorkItems(string iterationID, string teamID)
        {

            var responseBody = await Requester.GetIterationWorkItemsAsync(iterationID, teamID);

            var jObject = JObject.Parse(responseBody);
            var jObjectTicketNumbers = jObject.SelectTokens("$.workItemRelations..target.id");
            
            return jObjectTicketNumbers.Select(t => t.Value<string>() ?? "0");

        }

        public async Task<Changesets?> GetChangeset(string changesetNumber)
        {
          
            string responseBody = await Requester.GetChangesetAsync(changesetNumber);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Changesets>(responseBody);

        }

        public async Task<string?> GetFileContents(string sourceControlPath)
        {
            return await Requester.GetFileContentsAsync(sourceControlPath);            
        }

        public async Task<IEnumerable<string>?> GetWorkItemsByQuery(string queryID)
        {

            string responseBody = await Requester.GetWorkItemsByQuery(queryID);
            return JsonExtractor.ExtractWorkItemIDsFromSprintQuery(responseBody);


        }

    }

}
