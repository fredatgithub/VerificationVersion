using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VerificationVersion
{
  class Program
  {
    static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("Verification version can be used to verify version of executable.");
      // checking if ServerListSeparatedBySemicolon is empty
      if (Properties.Settings.Default.ServerListSeparatedBySemicolon.Length == 0)
      {
        display("The server list must not be empty, please enter at least one server, separate all servers with a semicolon ';' in the config file: VerificationVersion.exe.config");
        return;
      }

      // checking if ExecutableName is empty
      if (Properties.Settings.Default.ExecutableName.Length == 0)
      {
        display("The name of the executable cannot be empty, please enter a name of an executable in the config file: VerificationVersion.exe.config");
        return;
      }

      // Checking if ApplicationSharedPath is empty
      if (Properties.Settings.Default.ApplicationSharedPath.Length == 0)
      {
        display("The path of the server cannot be empty, please enter a path like C$ or shareName in the config file: VerificationVersion.exe.config");
        return;
      }

      var listOfServers = new List<string>();
      listOfServers.AddRange(Properties.Settings.Default.ServerListSeparatedBySemicolon.Split(';'));
      string executableName = Properties.Settings.Default.ExecutableName;
      string serverPath = Properties.Settings.Default.ApplicationSharedPath;
      var listOfExecutable = new List<FileInfo>();
      var listOfExecutableDico = new Dictionary<string, string>();
      try
      {
        foreach (string serverName in listOfServers)
        {
          string fullPathName = Path.Combine(serverName, serverPath, executableName);
          fullPathName = @"\\" + fullPathName;
          FileInfo theFile = new FileInfo(fullPathName);
          listOfExecutable.Add(theFile);
          listOfExecutableDico.Add(serverName, FileVersionInfo.GetVersionInfo(fullPathName).FileVersion.ToString());
        }
      }
      catch (Exception exception)
      {
        display($"There was an exception while trying to read the version of the executable: {Properties.Settings.Default.ExecutableName} in the list of servers: {Properties.Settings.Default.ServerListSeparatedBySemicolon}. The exception message is {exception.Message}");
      }

      display(string.Empty);
      foreach (var file in listOfExecutableDico)
      {
        display($"The server {file.Key} has a file {executableName} with a version of {file.Value}");
      }

      display(string.Empty);
      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}
