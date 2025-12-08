using CheckinMergeDetector.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckinMergeDetector
{
    public class CheckinMergeDetector
    {

        private int TicketNumber { get; set; }
        private ICollection<FileChange> FilesWaitingOnMerge { get; set; }
        private IEnumerable<Checkin> Checkins { get; set; }
        private bool MergeDetectorRanFlag { get; set; }

        public CheckinMergeDetector (Ticket ticket)
        {
            Checkins = ticket.Checkins;
            FilesWaitingOnMerge = new HashSet<FileChange>();
            MergeDetectorRanFlag = false;
        }

        public bool AreAllMergesDoneFlag ()
        {
            return FilesWaitingOnMerge.Count() == 0;
        }

        public void Run ()
        {

            if (MergeDetectorRanFlag)
            {
                throw new Exception("The CheckinMergeDetector process has already been ran on TFS# " + TicketNumber);
            }

            foreach (var checkin in Checkins)
            {
                checkin.UpdateFilesWaitingOnMerge(FilesWaitingOnMerge);
            }

            MergeDetectorRanFlag = true;

        }

        public IEnumerable<FileChange> GetFilesMissingMerges ()
        {
            return FilesWaitingOnMerge.ToList();
        }

        public override string ToString ()
        {
            return $"Ticket Number {TicketNumber}\n-Files Waiting On Merge: {string.Join(',', FilesWaitingOnMerge)}]";
        }

    }
}
