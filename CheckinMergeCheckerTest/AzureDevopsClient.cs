using AzureMergeChecker;
using Microsoft.TeamFoundation.SourceControl.WebApi.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckinMergeCheckerTest
{
    public class AzureDevopsClient
    {

        [Fact]
        public async Task GetDevFileContents ()
        {

            string? fileContents = await GetFileContents("%24/CoStarInternalApps/Dev/Ent-US/Source/EDS.Financial/Service/EDS-Financial.External/External%20Service%20Operation/BillingBaseService.cs");
            Assert.True(fileContents != null);

        }

        [Fact]
        public async Task GetMainFileContents()
        {

            string? fileContents = await GetFileContents("%24/CoStarInternalApps/Main/Enterprise/Source/EDS.Financial/Service/EDS-Financial.External/External%20Service%20Operation/BillingBaseService.cs");
            Assert.True(fileContents != null);

        }

        [Fact]
        public async Task GetChangeset()
        {

            var client = new AzureMergeChecker.AzureDevopsClient();
            //var changeset = await client.GetChangeset("881731");

            var devChanges = new List<string>();//AzureDevopsResponseHelper.ExtractDevChangesOnTicket(changeset);

            Assert.True(devChanges?.Count() == 4);

        }

        [Fact]
        public async Task ExtractWorkItemIdsFromQueryResult()
        {

            var queryID = "s17_anastasia";

            var requester = new FakeRequester();
            var operation = new AzureDevOpsOperation(requester);

            var results = (await operation.GetWorkItemsByQuery(queryID))?.ToArray<string>();

            Assert.NotNull(results);
            Assert.Equal(4, results.Length);
            Assert.True(results[0] == "2467443");
            Assert.True(results[1] == "2477261");
            Assert.True(results[2] == "2547847");
            Assert.True(results[3] == "2569140");

        }

        [Fact]
        public async Task ExtractTicketAssignment()
        {

            var ticketID = "2645327";

            var requester = new FakeRequester();
            var operation = new AzureDevOpsOperation(requester);

            var assignment = await operation.GetTicketAssignment(ticketID);

            Assert.Equal("Anastasia Reshetnyak", assignment.AssignedTeamMember);

        }

        [Fact]
        public async Task ExtractTicketAssignmentUnassigned()
        {

            var ticketID = "2467443";

            var requester = new FakeRequester();
            var operation = new AzureDevOpsOperation(requester);

            var assignment = await operation.GetTicketAssignment(ticketID);

            Assert.Equal("Unassigned", assignment.AssignedTeamMember);

        }

        private async Task<string?> GetFileContents (string filePath)
        {

            string? fileContents = null;

            try
            {
                var client = new AzureMergeChecker.AzureDevopsClient();
                //fileContents = await client.GetFileContents(filePath);
            }
            catch (Exception ex)
            {

            }

            return fileContents;

        }

    }
}
