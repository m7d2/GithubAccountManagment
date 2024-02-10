/**
 *
 *  (C) Mohammad Rashed 2024
 *  This tool was created to solve my problem with dealing with two github accounts
 *  
 */

using Info;
using System.Diagnostics;
using System.IO;

namespace GithubAccountManager
{
    internal class Program
    {
        #region variables
        const string FILENAME = "save.txt";
        const string SAVEDFILEPATH = @"./" + FILENAME;
        #endregion

        #region Main App Function

        static void Main(string[] args)
        {
            
            string? uInput;
            do
            {
                try
                {
                    if(IsSaveFileExists(SAVEDFILEPATH))
                    {
                        StartInstance();
                    }
                    else
                    {
                        Console.WriteLine("Welcome New User :) \n" +
                                          "Let's get started\r\n");
                        StartNewInstance();
                    }

                    Console.WriteLine();
                    Console.WriteLine("Want to Clone a new repository? Y/N");
                    uInput = Console.ReadLine().Trim();
                    switch(uInput.ToLower())
                    {
                        case "y":
                            StartInstance();
                            break;
                        case "n":
                            Console.WriteLine("Have a nice day :)");
                            Console.WriteLine($"Github Account Manager | {CopyRight()} ");
                            uInput = "quit";
                            break;
                        default:
                            Console.WriteLine("Want to Clone a new repository? Y/N");
                            uInput = Console.ReadLine().Trim();
                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                    uInput = null;
                }
            } while (uInput != "quit");
            
        }

        #endregion

        #region Methods
        static void StartNewInstance()
        {
            Console.WriteLine($"******* Github Account Manager | {CopyRight()} *******");

            Console.Write("Github Username: ");
            string? username = Console.ReadLine().Trim();

            Console.Write("Github Email: ");
            string? email = Console.ReadLine().Trim();

            Console.Write("SSH File Name In Your .ssh/ Directory: ");
            string? sshpath = Console.ReadLine().Trim();

            Console.Write("Prent Directory Path Where You Clone All Your Repo: ");
            string? parentDirectory = Console.ReadLine().Trim();

            Console.Write("Repo URL: ");
            string? repoURL = Console.ReadLine().Trim();

            // Extract the directory name from the github repo url
            string repositoryName = repoURL.Substring(repoURL.LastIndexOf('/') + 1).Replace(".git", "");

          
            // Create Account instance
            Account acc = new Account(username, email, sshpath);

            // Create Save File
            string info = $"Username:{acc.UserName}\n" +
                          $"Email:{acc.Email}\n" +
                          $"SshPath:{acc.SSHFilePath}\n" +
                          $"ParentDirectory:{parentDirectory}\n";
            CreateSaveFile(info);

            string targetDirectory = Path.Combine(parentDirectory, repositoryName);

            // Create and configure the git clone process
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {repoURL} {targetDirectory} --config core.sshCommand=\"ssh -i ~/.ssh/{acc.SSHFilePath}\"",
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

        // Start an instace if user already signed in
        static void StartInstance()
        {
            string[] lines = File.ReadAllLines(SAVEDFILEPATH);

            string userName = GetValueFromLine(lines[0]);
            string email = GetValueFromLine(lines[1]);
            string sshPath = GetValueFromLine(lines[2]);
            string parentDirectory = GetValueFromLine(lines[3]);

            Account acc = new Account(userName, email, sshPath);
            Console.WriteLine($"*********** Welcome Back {userName} ***********");
            Console.WriteLine($"******* Github Account Manager | {userName} *******");

            Console.Write("Repo URL: ");
            string? repoURL = Console.ReadLine().Trim();

            // Extract the directory name from the github repo url
            string repositoryName = repoURL.Substring(repoURL.LastIndexOf('/') + 1).Replace(".git", "");

            string targetDirectory = Path.Combine(parentDirectory, repositoryName);

            // Create and configure the git clone process
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {repoURL} {targetDirectory} --config core.sshCommand=\"ssh -i ~/.ssh/{acc.SSHFilePath}\"",
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
            Console.WriteLine($"Github Account Manager | {CopyRight()} ");
        }

        // Checks For Existing Save File
        static bool IsSaveFileExists(string path)
        {
            return File.Exists(path);
        }

        // Create Save File
        static void CreateSaveFile(string content)
        {
            if (IsSaveFileExists(SAVEDFILEPATH))
            {
                Console.WriteLine("Save File Already Exists: " + SAVEDFILEPATH);
                if (File.GetLastWriteTime(SAVEDFILEPATH) < DateTime.Now)
                {
                    Console.Write("Do you want to override it? Y/N ");
                    string? userInput = Console.ReadLine().Trim();

                    switch (userInput.ToLower()){
                        case "y":
                            File.WriteAllText(SAVEDFILEPATH, content);
                            Console.WriteLine("File overwritten successfully.\r\n");
                            break;
                        case "n":
                            Console.WriteLine("Operation skipped successfully.\r\n");
                            break;
                        default:
                            Console.WriteLine("Please enter valid option.\r\n");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Save File not exists: " + SAVEDFILEPATH);
                File.WriteAllText(SAVEDFILEPATH, content);
                Console.WriteLine("Save File Created successfuly.\r\n");
            }
        }

        // Return values from inside text file
        static string GetValueFromLine(string line)
        {
            // Split the line based on the first occurrence of colon (':') character
            int colonIndex = line.IndexOf(':');

            // Ensure that the colon exists in the line
            if (colonIndex != -1)
            {
                // Extract the value part after the first colon and trim it
                string value = line.Substring(colonIndex + 1).Trim();
                return value;
            }
            else
            {
                // If the colon is not found, return an empty string
                return string.Empty;
            }
        }

        // CopyRight string
        static string CopyRight()
        {
            return $"© Mohammad Rashed {DateTime.Now.Year}";
        }
        #endregion

    }
}
