using CheckinMergeDetector;
using CheckinMergeDetector.Types;

FileChange file1 = new FileChange() { FileID = 1, SourceControlPath = "$/path/to/file/1" };
FileChange file2 = new FileChange() { FileID = 2, SourceControlPath = "$/path/to/different/file/2" };
FileChange file3 = new FileChange() { FileID = 3, SourceControlPath = "$/path/to/file/3" };


    Ticket ticketMultipleChangesetsAndMerges = new Ticket()
    {
        TicketNumber = 3,
        Checkins = new List<Checkin>()
    {
        new CheckinForCodeChanges (new List<FileChange>() { file1, file2 }),
        new CheckinForMerges (new List<FileChange>() { file1, file2 }),
        new CheckinForCodeChanges (new List<FileChange>() { file2, file3 }),
        new CheckinForMerges (new List<FileChange>() { file2, file3 }),
    }
    };

var multipleChangesetMergesTest = new CheckinMergeDetector.CheckinMergeDetector(ticketMultipleChangesetsAndMerges);
multipleChangesetMergesTest.Run();

if (!multipleChangesetMergesTest.AreAllMergesDoneFlag())
{
    throw new Exception("Multiple changesets and merges test failed!");
}

Ticket ticketPartialMerges = new Ticket()
{
    TicketNumber = 4,
    Checkins = new List<Checkin>()
    {
        new CheckinForCodeChanges (new List<FileChange>() { file1, file2 }),
        new CheckinForCodeChanges (new List<FileChange>() { file2, file3 }),
        new CheckinForMerges (new List<FileChange>() { file1, file2 }),
        new CheckinForMerges (new List<FileChange>() { file3 }),
    }
};

var partialMergesTest = new CheckinMergeDetector.CheckinMergeDetector(ticketPartialMerges);
partialMergesTest.Run();

if (!partialMergesTest.AreAllMergesDoneFlag())
{
    throw new Exception("Partial merges test failed!");
}