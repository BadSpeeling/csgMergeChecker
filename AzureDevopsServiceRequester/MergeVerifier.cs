using AzureDevopsServiceRequester.CheckinVerification;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Common.CommandLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester
{
    public class MergeVerifier
    {

        private AzureDevopsClient client = new AzureDevopsClient();
        private Dictionary<string, TeamMember> teamMembers = new Dictionary<string, TeamMember>();

        public async Task HandleSprint(string queryID)
        {

            Console.WriteLine("Getting work items for team...");
            var workItemIds = await GetWorkItemIdsForSprint(queryID);

            if (workItemIds == null)
            {
                Console.WriteLine("Could not load work items");
                return;
            }

            Console.WriteLine("Getting metadata for work items...");
            await foreach (var workItemDetailTask in Task.WhenEach(GetWorkItemDetails(workItemIds)))
            {

                var workItemDetails = await workItemDetailTask;

                if (workItemDetails == null)
                {
                    continue;
                }

                TeamMember teamMember;               

                if (teamMembers.ContainsKey(workItemDetails.Value.AssignedTeamMember))
                {
                    teamMember = teamMembers[workItemDetails.Value.AssignedTeamMember];
                }
                else
                {
                    teamMember = new TeamMember(workItemDetails.Value.AssignedTeamMember);
                    teamMembers.Add(workItemDetails.Value.AssignedTeamMember, teamMember);
                }

                teamMember.AddAssignedTicket(workItemDetails.Value.WorkItemID);

            }

            Console.WriteLine("Starting merge verification!");

            foreach (TeamMember teamMember in teamMembers.Values)
            {
                Console.WriteLine($"\nTickets for: {teamMember.DisplayName}\n");
                await VerifyWorkItems(teamMember.AssignedTickets);
            }

        }

        public async Task<IEnumerable<string>?> GetWorkItemsForSprint(string iterationID, AzureDevOpsTeam azureDevOpsTeam)
        {
            var workItems = await client.GetIterationWorkItems(iterationID, azureDevOpsTeam.ParentID);

            if (workItems == null)
            {
                Console.WriteLine($"Could not get work items for iteration {iterationID} and team {azureDevOpsTeam.TeamName}");
                return null;
            }

            return workItems;

        }

        public async Task<IEnumerable<string>?> GetWorkItemIdsForSprint(string queryID)
        {
            var workItems = await client.GetWorkItemsByQuery(queryID);

            if (workItems == null)
            {
                Console.WriteLine($"Could not get work items for query {queryID}");
                return null;
            }

            return workItems;

        }

        public IEnumerable<Task<(string WorkItemID, string AssignedTeamMember)?>> GetWorkItemDetails(IEnumerable<string> workItems)
        {
            return workItems.Select(w => client.GetTicketAssignment(w));
        }

        public async Task VerifyWorkItems(List<string> workItemIds)
        {
            foreach (string workItem in workItemIds)
            {
                var workItemVerifyResult = await VerifyWorkItem(workItem);
                Console.WriteLine(workItemVerifyResult);
            }
        }

        private async Task<string> VerifyWorkItem(string workItemId)
        {

            var workItemVerifyResult = "";
            workItemVerifyResult += "-----------------------\n| " + workItemId + "\n-----------------------\n";

            var workItemUpdate = await client.GetWorkItemUpdate(workItemId);

            if (workItemUpdate == null)
            {
                workItemVerifyResult += "The provided WorkItemID could not be used to lookup a WorkItem";
                return workItemVerifyResult;
            }

            var changesets = AzureDevopsResponseHelper.ExtractChangesetIdsFromWorkItemUpdate(workItemUpdate);
            var mergeVerifier = new Filechecker();

            foreach (var changeset in changesets)
            {
                await LoadChangesetIntoVerifier(mergeVerifier, changeset);
            }

            await mergeVerifier.VerifyDevFilesAreMerged();
            workItemVerifyResult += mergeVerifier.GetOutput(baseTabs: 1);

            return workItemVerifyResult;

        }

        public async Task LoadChangesetIntoVerifier(Filechecker mergeVerifier, string changesetId)
        {

            var client = new AzureDevopsClient();
            var changeset = await client.GetChangeset(changesetId);

            var devFileChanges = AzureDevopsResponseHelper.ExtractDevChangesOnTicket(changeset);

            devFileChanges.ForEach(devFileChange =>
            {

                var devSourceControlPath = devFileChange.GetSourceControlPath();

                if (devSourceControlPath != null)
                {
                    mergeVerifier.AddDevFile(devSourceControlPath);
                }

            });

        }

    }

}
