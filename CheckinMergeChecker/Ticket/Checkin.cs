namespace CheckinMergeDetector.Types
{
    public abstract class Checkin
    {
        protected IEnumerable<FileChange> FileChanges { get; set; }

        public Checkin()
        {
            FileChanges = new List<FileChange>();
        }

        public Checkin(IEnumerable<FileChange> fileChanges)
        {
            FileChanges = fileChanges;
        }

        public abstract void UpdateFilesWaitingOnMerge(ICollection<FileChange> filesWaitingOnMerge);

    }
}
