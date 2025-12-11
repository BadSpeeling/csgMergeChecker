using AzureMergeChecker.AzureDevopsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker.AzureDevOpsRequester
{
    public interface Requester
    {

        public Task<string> GetTicketAssignmentAsync(string tfsTicketNumber);
        public Task<string> GetWorkItemUpdateAsync(string tfsTicketNumber);
        public Task<string> GetIterationWorkItemsAsync(string iterationID, string teamID);
        public Task<string> GetChangesetAsync(string changesetNumber);
        public Task<string?> GetFileContentsAsync(string sourceControlPath);
        public Task<string> GetWorkItemsByQuery(string queryID);

    }
}
