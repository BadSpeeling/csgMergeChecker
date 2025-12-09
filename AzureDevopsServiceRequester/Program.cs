using AzureDevopsServiceRequester;

string teamName = args[0];
string iterationID = args[1];

var teams = new AzureDevOpsTeamMappings();
var team = teams.AzureDevOpsTeams[teamName];

await Driver.HandleSprint(iterationID, team);