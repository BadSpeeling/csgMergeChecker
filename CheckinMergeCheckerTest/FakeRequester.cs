using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureMergeChecker.AzureDevOpsRequester;
using System.IO;
using Microsoft.TeamFoundation.Build.WebApi;

namespace CheckinMergeCheckerTest
{
    public class FakeRequester : Requester
    {

        public Task<string> GetChangesetAsync(string changesetNumber)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetFileContentsAsync(string sourceControlPath)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetIterationWorkItemsAsync(string iterationID, string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetTicketAssignmentAsync(string tfsTicketNumber)
        {
            var path = Directory.GetCurrentDirectory() + $"/../../../data/ticket_detail/{tfsTicketNumber}.json";
            return await System.IO.File.ReadAllTextAsync(path);
        }

        public async Task<string> GetWorkItemsByQuery(string queryID)
        {

            var path = Directory.GetCurrentDirectory() + $"/../../../data/query/{queryID}.json";
            return await System.IO.File.ReadAllTextAsync(path);

        }

        public Task<string> GetWorkItemUpdateAsync(string tfsTicketNumber)
        {
            throw new NotImplementedException();
        }
    }
}
