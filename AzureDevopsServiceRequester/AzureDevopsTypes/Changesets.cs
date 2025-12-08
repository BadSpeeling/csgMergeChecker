using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester.AzureDevopsTypes
{
    public class Changesets
    {
        public Change[]? changes { get; set; }
    }

    public class Change
    {
        public Item? item { get; set; }
        public string? changeType { get; set; }

        public string? GetSourceControlPath ()
        {
            return item?.path;
        }

    }

    public class Item
    {
        public int? version { get; set; }
        public int? size { get; set; }
        public string? hashValue { get; set; }
        public string? path { get; set; }
        public string? url { get; set; }
    }

}
