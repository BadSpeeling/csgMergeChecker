using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker
{
    public class JsonExtractor
    {

        public static string? ExtractAssignmentFromWorkItem(string workItemResponse)
        {

            var jObject = JObject.Parse(workItemResponse);
            var extractedDisplayName = jObject.SelectToken("$.fields['System.AssignedTo'].displayName")?.Value<string>();
            return extractedDisplayName;

        }

        public static IEnumerable<string> ExtractWorkItemIDsFromSprintQuery (string sprintQueryResponse) 
        {

            var jObject = JObject.Parse(sprintQueryResponse);

            var jObjectTicketNumbers = jObject.SelectTokens("$.workItems..id");
            return jObjectTicketNumbers
                .Select(t => t.Value<string>() ?? "0")
                .Where(t => t != "0");
        }

    }
}
