using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckinMergeDetector.Types
{
    public class CheckinForCodeChanges : Checkin
    {

        public CheckinForCodeChanges(IEnumerable<FileChange> fileChanges) : base(fileChanges)
        {

        }        

        public override void UpdateFilesWaitingOnMerge(ICollection<FileChange> filesWaitingOnMerge)
        {
            foreach (var fileChange in this.FileChanges)
            {
                //Add a file to the collection if it is not already in our "WaitingOnMerge" collection
                //If a file is already waiting on a merge, no need to add it again, so just ignore                
                if (!filesWaitingOnMerge.Contains(fileChange))
                {
                    filesWaitingOnMerge.Add(fileChange);
                }
            }
        }
    }
}
