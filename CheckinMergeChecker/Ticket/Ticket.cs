namespace CheckinMergeDetector.Types
{
    public class Ticket
    {
        public int TicketNumber { get; set; }
        public IEnumerable<Checkin> Checkins { get; set; }

        public Ticket ()
        {
            Checkins = new List<Checkin>();
        }
    }
}
