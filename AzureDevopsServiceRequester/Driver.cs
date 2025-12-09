using AzureDevopsServiceRequester.CheckinVerification;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Common.CommandLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester
{
    public class Driver
    {

        private static AzureDevopsClient client = new AzureDevopsClient();

        public async static Task Run ()
        {

            //2691267
            //await HandleWorkItem("2632460");
            //await HandleWorkItem("2691267");

            //await HandleWorkItem("2730470");

        }

        public async static Task HandleSprint (string iterationID, AzureDevOpsTeam azureDevOpsTeam)
        {

            Console.WriteLine("Getting work items for team...");
            
            var workItems = await client.GetIterationWorkItems(iterationID, azureDevOpsTeam.ParentID);

            if (workItems == null)
            {
                Console.WriteLine($"Could not get work items for iteration {iterationID} and team {azureDevOpsTeam.TeamName}");
                return;
            }

            var teamMemberAssignedWork = new Dictionary<string, TeamMember>();

            foreach (var teamMemberName in azureDevOpsTeam.TeamMemberNames)
            {
                teamMemberAssignedWork.Add(teamMemberName, new TeamMember(teamMemberName));
            }

            Console.WriteLine("Getting metadata for work items...");

            foreach (var workItem in workItems)
            {

                var assignedTeamMemberName = await client.GetTicketAssignment(workItem);

                if (assignedTeamMemberName == null)
                {
                    Console.WriteLine($"Could not get assigned team member for ticket {workItem}");
                    continue;
                }

                if (teamMemberAssignedWork.ContainsKey(assignedTeamMemberName))
                {
                    teamMemberAssignedWork[assignedTeamMemberName].AddAssignedTicket(workItem);
                }

            }

            Console.WriteLine("Starting merge verification!");

            foreach (TeamMember teamMember in teamMemberAssignedWork.Values)
            {
                Console.WriteLine($"\nTickets for: {teamMember.DisplayName}\n");
                await VerifyWorkItems(teamMember.AssignedTickets);
            }

        }

        public async static Task VerifyWorkItems(List<string> workItemIds)
        {
            foreach (string workItem in workItemIds)
            {
                var workItemVerifyResult = await VerifyWorkItem(workItem);
                Console.WriteLine(workItemVerifyResult);
            }
        }

        private async static Task<string> VerifyWorkItem (string workItemId)
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
            var mergeVerifier = new MergeVerifier();

            foreach (var changeset in changesets)
            {
                await LoadChangesetIntoVerifier(mergeVerifier, changeset);
            }

            await mergeVerifier.VerifyDevFilesAreMerged();
            workItemVerifyResult += mergeVerifier.GetOutput(baseTabs: 1);

            return workItemVerifyResult;

        }

        public async static Task LoadChangesetIntoVerifier(MergeVerifier mergeVerifier, string changesetId)
        {

            var client = new AzureDevopsClient();
            var changeset = await client.GetChangeset(changesetId);

            var devFileChanges = AzureDevopsResponseHelper.ExtractDevChangesOnTicket(changeset);

            devFileChanges.ForEach(devFileChange =>
            {

                var devSourceControlPath = devFileChange.GetSourceControlPath();

                if (devSourceControlPath != null)
                {
                    mergeVerifier.ProcessDevFile(devSourceControlPath);
                }

            });

        }

    }
}
