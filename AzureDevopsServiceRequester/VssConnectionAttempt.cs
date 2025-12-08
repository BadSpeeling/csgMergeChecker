using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester
{
    class VssConnectionAttempt
    {

        public static void Driver ()
        {

            //============= Config [Edit these with your settings] =====================
            const string azureDevOpsOrganizationUrl = "https://dev.azure.com/costargroup"; //change to the URL of your Azure DevOps account; NOTE: This must use HTTPS
            const string vstsCollectioUrl = "http://myserver:8080/tfs/DefaultCollection"; //alternate URL for a TFS collection

            //Prompt user for credential
            VssConnection connection = new VssConnection(new Uri(azureDevOpsOrganizationUrl), new VssClientCredentials());
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

        }

    }
}
