using AzureDevopsServiceRequester;
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

            var client = new AzureDevopsServiceRequester.AzureDevopsClient();
            var changeset = await client.GetChangeset("881731");

            var devChanges = AzureDevopsResponseHelper.ExtractDevChangesOnTicket(changeset);

            Assert.True(devChanges?.Count() == 4);

        }

        [Fact]
        public async Task GetChangesets()
        {
            
            //AzureDevopsServiceRequester.WorkItemUpdate
        }

        //[Fact]
        //public async Task GetIterationWorkItems ()
        //{

        //    var client = new AzureDevopsServiceRequester.AzureDevopsClient();
        //    await client.GetIterationWorkItems("114c6270-4f70-4736-b797-845e84702ad4", "0c514242-d539-41bd-a110-dbebf07f870e");
          
        //}

        private async Task<string?> GetFileContents (string filePath)
        {

            string? fileContents = null;

            try
            {
                var client = new AzureDevopsServiceRequester.AzureDevopsClient();
                fileContents = await client.GetFileContents(filePath);
            }
            catch (Exception ex)
            {

            }

            return fileContents;

        }

    }
}
