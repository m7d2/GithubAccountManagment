/**
 *
 *  (C) Mohammad Rashed 2024
 *  This tool was created to solve my problem with dealing with two github accounts
 *  
 */

using Info;
using System.Diagnostics;

namespace GithubAccountManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Console.WriteLine($"******* Github Account Manager *******");

                    Console.Write("Github Username: ");
                    string? username = Console.ReadLine().Trim();

                    Console.Write("Github Email: ");
                    string? email = Console.ReadLine().Trim();

                    Console.Write("SSH File Name: ");
                    string? sshpath = Console.ReadLine().Trim();
                    sshpath = $"~/.ssh/" + sshpath;

                    Console.Write("Target Directory: ");
                    string? parentDirectory = Console.ReadLine().Trim();

                    Console.Write("Repo URL: ");
                    string? repoURL = Console.ReadLine().Trim();

                    string repositoryName = repoURL.Substring(repoURL.LastIndexOf('/') + 1).Replace(".git", "");

                    Account acc = new Account(username, email, sshpath);

                    string targetDirectory = System.IO.Path.Combine(parentDirectory, repositoryName);
                    // Testing Log 

                    //Console.WriteLine();
                    //Console.WriteLine($"Username: {acc.UserName}");
                    //Console.WriteLine($"Email: {acc.Email}");
                    //Console.WriteLine($"SSH Filepath: {acc.SSHFilePath}");
                    //Console.WriteLine($"Target Directory: {parentDirectory}");
                    //Console.WriteLine($"Repo name: {repositoryName}");
                    //Console.WriteLine($"Path: {targetDirectory}");
                    //Console.WriteLine();

                    // Create and configure the git clone process
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"clone {repoURL} {targetDirectory} --config core.sshCommand=\"ssh -i {acc.SSHFilePath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = false
                    };
                    process.StartInfo = startInfo;

                    // Start the git clone process
                    process.Start();
                    process.WaitForExit();


                    // Set the username and email for the local repository
                    ProcessStartInfo setUserEmail = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"config user.email \"{acc.Email}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = targetDirectory
                    };
                    ProcessStartInfo setUserName = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"config user.name \"{acc.UserName}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = targetDirectory
                    };

                    // Execute the commands to set the username and email
                    Process.Start(setUserEmail).WaitForExit();
                    Process.Start(setUserName).WaitForExit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            } while (true);
            
        }
    }
}
