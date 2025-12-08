using CheckinMergeDetector.Types;

namespace CheckinMergeCheckerTest
{
    public class UnitTest1
    {

        private FileChange file1 = new FileChange() { FileID = 1, SourceControlPath = "$/path/to/file/1" };
        private FileChange file2 = new FileChange() { FileID = 2, SourceControlPath = "$/path/to/different/file/2" };
        private FileChange file3 = new FileChange() { FileID = 3, SourceControlPath = "$/path/to/file/3" };

        [Fact]
        public void Test1()
        {
            Ticket ticketFullyMerged = new Ticket()
            {
                TicketNumber = 1,
                Checkins = new List<Checkin>()
                {
                    new CheckinForCodeChanges (new List<FileChange>() { file1, file2 }),
                    new CheckinForMerges (new List<FileChange>() { file1, file2 })
                }
            };

            var simpleTest = new CheckinMergeDetector.CheckinMergeDetector(ticketFullyMerged);
            simpleTest.Run();

            Assert.True(simpleTest.AreAllMergesDoneFlag(), "Checkin then merge should pass");

        }

        [Fact]
        public void Test2()
        {
            Ticket ticketCheckinMissingOneMerge = new Ticket()
            {
                TicketNumber = 2,
                Checkins = new List<Checkin>()
                {
                    new CheckinForCodeChanges (new List<FileChange>() { file1, file2, file3 }),
                    new CheckinForMerges (new List<FileChange>() { file1, file2 })
                }
            };

            var oneFileMissingTest = new CheckinMergeDetector.CheckinMergeDetector(ticketCheckinMissingOneMerge);
            oneFileMissingTest.Run();

            Assert.False(oneFileMissingTest.AreAllMergesDoneFlag(), "Missed a file in merge, expected to fail");

        }

        [Fact]
        public void Test3()
        {
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

            Assert.True(multipleChangesetMergesTest.AreAllMergesDoneFlag(), "Multiple merges test expected to pass");
            
        }

        [Fact]
        public void Test4()
        {
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

            Assert.True(partialMergesTest.AreAllMergesDoneFlag(), "Partial merges are expected to work");
            
        }

    }
}
