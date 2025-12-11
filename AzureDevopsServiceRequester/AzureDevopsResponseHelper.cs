using AzureMergeChecker.AzureDevopsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker
{
    public class AzureDevopsResponseHelper
    {

        public static IEnumerable<string>? ExtractChangesetIdsFromWorkItemUpdate (WorkItemUpdate workItemUpdateResponse)
        {

            var changesets = workItemUpdateResponse.value?.Where(v =>
                {
                    return v.relations?.added?.Where(a => a.IsChangeset()).FirstOrDefault() != null;
                });

            var added = changesets?.SelectMany<WorkItemUpdateValue, WorkItemUpdateAdded>(changeset =>
            {
                return changeset.relations?.added ?? new WorkItemUpdateAdded[] { };
            });

            var changesetIds = added?.Select(a => a.GetChangesetId());

            return changesetIds;

        }

        public static IEnumerable<Change>? ExtractDevChangesOnTicket (Changesets changesetsResponse)
        {

            var devChangesMadeOnTicket = changesetsResponse.changes?
                .Where(change => !change?.changeType?.Contains("merge") == true && (change?.changeType?.Contains("edit") == true || change?.changeType?.Contains("add") == true));

            return devChangesMadeOnTicket;

        }

    }
}
