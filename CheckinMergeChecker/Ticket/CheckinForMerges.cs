using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckinMergeDetector.Types
{
    public class CheckinForMerges : Checkin
    {

        public CheckinForMerges(IEnumerable<FileChange> fileChanges) : base(fileChanges)
        {

        }

        public override void UpdateFilesWaitingOnMerge(ICollection<FileChange> filesWaitingOnMerge)
        {
            foreach (var fileChange in this.FileChanges)
            {
                //File was merged, meaning it isn't waiting on a merge anymore.  So, remove it from the collection
                filesWaitingOnMerge.Remove(fileChange);
            }
        }
    }
}
