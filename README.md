# Woodpecker
One of my 'problems' during research can be that I need Administrative rights to assess possible Escalation of Priviliges. The rights are not always available when you do an assessment for a client. This is where Woodpecker comes in. It pecks files (I know, I suck at naming projects) that are owned by "NT AUTHORITY\SYSTEM" of the Administrators group. There is more logic around it though. The interesting part would be that a low privileged user has modify, Delete, Write rights. This could be the start of an escalation.

## Getting started
This is not dificult alt all. I have created a folder in this repo with a published build and all you need to do is run the executable. There is one thing though... For now, the tool expects that the folder "c:\temp" exists on the drive. 

The results will be saved in "c:\temp\results.txt" and will look something like this:

```
---------------------------
C:\ProgramData\testtest\test.txt
Last edited: 5/18/2022 3:08:50 PM
Last accessed: 5/18/2022 3:08:56 PM
Owner: NT AUTHORITY\SYSTEM
IdentityReference: BUILTIN\Users
FileSystemRights: FullControl
---------------------------
C:\ProgramData\Microsoft\EdgeUpdate\Log\MicrosoftEdgeUpdate.log
Last edited: 5/19/2022 8:44:19 AM
Last accessed: 5/19/2022 8:44:19 AM
Owner: NT AUTHORITY\SYSTEM
IdentityReference: BUILTIN\Users
FileSystemRights: Write, Delete, Read, Synchronize
---------------------------
```

## ToDo's
There are a few things that I want to implement in the near future. 
- Users should be able to specify th starting directory (it is now "C:\" by default).
- The tool should be able to identify the current user and add this to the logic as well
- As I am not a developer, I need to find out how to prevent memory leaks and to make it faster


