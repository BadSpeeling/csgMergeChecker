using System.Numerics;

namespace CheckinMergeDetector.Types
{
    public class FileChange : IEquatable<FileChange>
    {
        public int FileID { get; set; }
        public string SourceControlPath { get; set; }
        public string FileContentsLookupUrl { get; set; }
        public DateTime FileChangeDateTime { get; set; }

        public bool Equals(FileChange? other)
        {
            return this.FileID == other?.FileID;
        }

        public override string ToString ()
        {
            return $"FileChange = [{SourceControlPath}]";
        }
    }
}
