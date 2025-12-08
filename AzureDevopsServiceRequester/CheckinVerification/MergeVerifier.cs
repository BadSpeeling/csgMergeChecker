using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester.CheckinVerification
{
    public class MergeVerifier
    {

        private ISet<string> DevFilesPath;

        private IList<string> MergedDevFiles;
        private IList<string> MergeMissingDevFiles;

        public MergeVerifier ()
        {
            this.DevFilesPath = new HashSet<string>();
            this.MergedDevFiles = new List<string>();
            this.MergeMissingDevFiles = new List<string>();
        }

        public void ProcessDevFile (string file)
        {
            if (!DevFilesPath.Contains(file))
            {
                DevFilesPath.Add(file);
            }
        }

        public async Task VerifyDevFilesAreMerged ()
        {

            foreach (var devFile in DevFilesPath)
            {

                var modifiedFile = new ModifiedFile(devFile);

                if (await IsModifiedFileMerged(modifiedFile))
                {
                    MergedDevFiles.Add(devFile);
                }
                else
                {
                    MergeMissingDevFiles.Add(devFile);
                }

            }

        }

        private async Task<bool> IsModifiedFileMerged(ModifiedFile modifiedFile)
        {

            var client = new AzureDevopsClient();
            var devFileContents = await client.GetFileContents(modifiedFile.GetDevSourceControlPath());
            var mainFileContents = await client.GetFileContents(modifiedFile.GetMainSourceControlPath());

            return devFileContents.Equals(mainFileContents);

        }

        public string GetOutput(int baseTabs)
        {

            string sep = "---------------------------------\n";
            string output = "";
            var baseSpacing = new String('\t', baseTabs);

            output += baseSpacing + sep;
            output += baseSpacing + "| Files not fully merged\n";
            output += baseSpacing + sep;

            if (MergeMissingDevFiles.Count() > 0) {

                foreach (var unmergedFile in MergeMissingDevFiles)
                {
                    output += baseSpacing + "\t" + unmergedFile + "\n";
                }

            }
            else
            {
                output += baseSpacing + "\tNone!\n";
            }

            return output;

        }

    }
}
