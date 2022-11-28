using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Tree
{
    class Woodpecker
    {
        static void Main()
        {
            Console.WriteLine(@"                         _                 _             
                        | |               | |            
__      _____   ___   __| |_ __   ___  ___| | _____ _ __ 
\ \ /\ / / _ \ / _ \ / _` | '_ \ / _ \/ __| |/ / _ \ '__|
 \ V  V / (_) | (_) | (_| | |_) |  __/ (__|   <  __/ |   
  \_/\_/ \___/ \___/ \__,_| .__/ \___|\___|_|\_\___|_|   
                          | |                            
                          |_|                           ");
            Console.WriteLine("I am going to crawl though the C Drive.\r\nThis tool does not show output in the console to keep it cleen.\r\n\r\nThis takes a while! Sit back, relax and wait for the results in C:\\temp\\result.txt.");

            //To Do: Need to make it based on arguments as it is now hardcoded to C:\
            string dirLocation = @"C:\";
            ApplyAllFiles(dirLocation, ProcessFile);
        }
        static void ProcessFile(FileInfo file)
        {
            try
            {
                FileSecurity fileSec = file.GetAccessControl();


                var authRuleColl = fileSec.GetAccessRules(true, true, typeof(NTAccount));

                foreach (FileSystemAccessRule fsaRule in authRuleColl)
                {
                    var sid = fileSec.GetOwner(typeof(SecurityIdentifier));
                    var ntAccount = sid.Translate(typeof(NTAccount));
                    try
                    {
                        if (ntAccount.ToString() == "BUILTIN\\Administrators" || ntAccount.ToString().Contains(@"Administrators") || ntAccount.ToString() == "NT AUTHORITY\\SYSTEM")
                        {
                            if (fsaRule.IdentityReference.ToString() == "NT AUTHORITY\\Authenticated Users" || fsaRule.IdentityReference.ToString() == "Everyone" || fsaRule.IdentityReference.ToString().Contains(@"Everyone") || fsaRule.IdentityReference.ToString() == "BUILTIN\\Users" || fsaRule.IdentityReference.ToString().Contains(@"Gebruikers"))
                            {
                                if (fsaRule.AccessControlType.ToString() == @"Allow")
                                {

                                    string possibilities = fsaRule.FileSystemRights.ToString();
                                    if (possibilities.Contains(@"Modify") || possibilities.Contains(@"FullControl") || possibilities.Contains(@"Delete"))
                                    {
                                        //Console.WriteLine("Found one: " + file.Directory + "\\" + file.Name);
                                        using StreamWriter test = new("C:\\temp\\result.txt", append: true);
                                        test.WriteLine(file.Directory + "\\" + file.Name);
                                        test.WriteLine("Last edited: " + file.LastWriteTimeUtc);
                                        test.WriteLine("Last accessed: " + file.LastAccessTimeUtc);
                                        test.WriteLine("Owner: " + ntAccount);
                                        test.WriteLine("IdentityReference: {0}", fsaRule.IdentityReference);
                                        test.WriteLine("FileSystemRights: {0}", fsaRule.FileSystemRights);
                                        test.WriteLine("---------------------------");
                                        test.Close();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }
        static void ApplyAllFiles(string folder, Action<FileInfo> fileAction)
        {
            var fileInfo = new DirectoryInfo(folder);
            foreach (FileInfo file in fileInfo.EnumerateFiles())
            {
                fileAction(file);
            }
            foreach (string subDir in Directory.GetDirectories(folder))
            {
                try
                {
                    ApplyAllFiles(subDir, fileAction);
                }
                catch
                {
                    //log?
                }
            }
        }
    }
}
