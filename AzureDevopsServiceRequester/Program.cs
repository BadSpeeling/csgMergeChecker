using AzureMergeChecker;
using AzureMergeChecker.AzureDevOpsRequester;

string queryID = args[0];

//var teams = new AzureDevOpsTeamMappings();

var mergeVerifier = new MergeVerifier(new AzureDevOpsRequester("CoStarCollection", "CoStarInternalApps"));
await mergeVerifier.HandleSprint(queryID);

