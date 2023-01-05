# General
This game was originally on another github account.  
The Project has been moved over here for fixes and stuff.  
Mainly getting it working on linux (which is does) and also fixing a lot of bad code.

# Important when Building
I encountered an issue when building on linux.  
The issue was that the MonoGame.Content.Builder.Task was trying to call `dotnet msgb [...]` while it should have called `msgb [...]`.  
Removing the dotnet call for the cammand in the .targets file (of the nuget package) fixed the issue.  
Since the nuget package files are not included in this repo i though i would mention it.