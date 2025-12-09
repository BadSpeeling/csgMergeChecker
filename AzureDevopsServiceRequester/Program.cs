using AzureDevopsServiceRequester;

string queryID = args[0];

//var teams = new AzureDevOpsTeamMappings();

var mergeVerifier = new MergeVerifier();
await mergeVerifier.HandleSprint(queryID);

