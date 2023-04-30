# General
Originally I only wanted to fix some code but at this point this is basically a new game with a lot of improvements.  
Original repo:
https://github.com/BoiRaigy/NoNameButtonGame

# Play the original
https://raigy.itch.io/nonamebuttongame

# Important when Building
I encountered an issue when building on linux.  
The issue was that the MonoGame.Content.Builder.Task was trying to call `dotnet msgb [...]` while it should have called `msgb [...]`.  
Removing the dotnet call for the command in the .targets file (of the nuget package) fixed the issue.