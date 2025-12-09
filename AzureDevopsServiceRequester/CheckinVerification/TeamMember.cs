using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevopsServiceRequester.CheckinVerification
{
    public class TeamMember
    {

        public string DisplayName { get; set; }
        public List<string> AssignedTickets { get; set; }

        public TeamMember (string DisplayName)
        {
            this.DisplayName = DisplayName;
            this.AssignedTickets = new List<string>();
        }

        public void AddAssignedTicket (string ticket)
        {
            this.AssignedTickets.Add(ticket);
        }

    }
}
