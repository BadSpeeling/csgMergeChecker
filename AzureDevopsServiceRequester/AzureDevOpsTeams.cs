using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMergeChecker
{
    public class AzureDevOpsTeamMappings
    {

        public Dictionary<string, AzureDevOpsTeam> AzureDevOpsTeams { get; set; }

        public AzureDevOpsTeamMappings()
        {

            AzureDevOpsTeams = new Dictionary<string, AzureDevOpsTeam> ();

            var teamAnastasia = "Team Anastasia";
            AzureDevOpsTeams.Add(teamAnastasia, new AzureDevOpsTeam()
            {
                TeamName = teamAnastasia,
                ParentID = "0c514242-d539-41bd-a110-dbebf07f870e",
                TeamMemberNames = new List<string> () 
                {
                    "Anastasia Reshetnyak",
                    "Eric Frye",
                    "Thomas Brickhouse"
                }
            });

        }

    }

    public class AzureDevOpsTeam
    {

        public required string TeamName { get; set; }
        public required string ParentID { get; set; }
        public required List<string> TeamMemberNames { get; set; }

    }

}
