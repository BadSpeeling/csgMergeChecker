using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker.CheckinVerification
{
    public class ModifiedFile
    {
        private static string DevSourceControlPrefix = "$/CoStarInternalApps/Dev/Ent-US";
        private static string MainSourceControlPrefix = "$/CoStarInternalApps/Main/Enterprise";

        private string DevSourceControlPath;
        private string MainSourceControlPath;

        public ModifiedFile (string DevSourceControlPath)
        {
            
            this.DevSourceControlPath = DevSourceControlPath;
            this.MainSourceControlPath = this.DevSourceControlPath.Replace(DevSourceControlPrefix, MainSourceControlPrefix);

        }

        public string GetDevSourceControlPath ()
        {
            return DevSourceControlPath;
        }

        public string GetMainSourceControlPath ()
        {
            return MainSourceControlPath;
        }


    }
}
