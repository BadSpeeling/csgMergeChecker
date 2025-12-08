using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester
{
    public class WorkItemUpdate
    {
        public WorkItemUpdateValue[]? value { get; set; }
    }

    public class WorkItemUpdateValue
    {
        public string id { get; set; }
        public WorkItemUpdateRelations? relations { get; set; }

    }

    public class WorkItemUpdateRelations
    {
        public WorkItemUpdateAdded[]? added { get; set; }
    }

    public class WorkItemUpdateAdded
    {
        public WorkItemUpdateAttributes? attributes { get; set; }
        public string? url { get; set; }

        public bool IsChangeset()
        {
            return attributes?.name == "Fixed in Changeset";
        }

        public string GetChangesetId()
        {

            if (url == null)
            {
                throw new Exception("Cannot get ChangesetId if url is null");
            }

            Regex regex = new Regex("vstfs:///VersionControl/Changeset/(\\d+)");
            var match = regex.Match(url);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new Exception("Could not parse the changesetId from the url " + url);
            }

        }

    }

    public class WorkItemUpdateAttributes
    {
        public string? name { get; set; }
    }
}
