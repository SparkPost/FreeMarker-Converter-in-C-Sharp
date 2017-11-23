using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace dirlister
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Display Application info
            string introDoc = @"
This application converts FreeMarker templates into SparkPost templates. 
Place any existing FreeMarker templates into the application INBOX, 
then run the application.  Converted templates will appear in teh OUTBOX.

             ";
            Console.WriteLine("{0}", introDoc);


            // Find out where we are installed
            string appPath = Directory.GetCurrentDirectory();
            Console.WriteLine("Application directory is {0}", appPath.ToString());

            // Check to see if the INBOX and OUTBOX directories exist already
            try
            {
                string[] dirs = Directory.GetDirectories(@appPath, "*", SearchOption.TopDirectoryOnly);
              //  Console.WriteLine("There are {0} directories here.", dirs.Length);
                foreach (string dir in dirs)
                {
                   // Console.WriteLine(dir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            // If the needed directories dont exist, create them
            string targetIN = appPath + "/INBOX";
            if (!Directory.Exists(targetIN))
            {
                Directory.CreateDirectory(targetIN);
                Console.WriteLine("Creating the INBOX directory");
            }
            string targetOUT = appPath + "/OUTBOX";
            if (!Directory.Exists(targetOUT))
            {
                Directory.CreateDirectory(targetOUT);
                Console.WriteLine("Creating the OUTBOX directory");
            }


            // Get a list of all the files in INBOX
            // Loop through all the files in the INBOX and convert them
            foreach (string path in targetIN.ToString().Split(new[] { '\n' }))
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path, appPath);
                }
                 else if(Directory.Exists(path)) 
                {
                // This path is a directory
                  ProcessDirectory(path, appPath);
                }
                 else 
                 {
                     Console.WriteLine("{0} is not a valid file or directory.", path);
                 }        
            }
        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory, string appPath)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, appPath);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, appPath);
        }

        // Logic for processing template files
        public static void ProcessFile(string path, string appPath)
        {

            // Get the actual template name
            string result = Path.GetFileName(path);

            // Check to make sure we only read unprocessed files
            string ext = result.Substring(result.Length - 4);
            if (ext != "read")
            {
                // Read the template into memory
                string text = File.ReadAllText(path);

                /**************************************************************/
                // Here is the real meat of the application
                // Run the regex rules on it
                // Add all regex replacement rules here
                //text = text.Replace("test", "solution");

                // Replace plain Variables ()
                Regex rgx = new Regex("\\$\\{(.*)\\}");
                text = rgx.Replace(text, "{{$1}}");


                /**************************************************************/

                // Save it to the OUTBOX
                File.WriteAllText(appPath + "/OUTBOX/" + result, text);

                // Mark original file as "read"
                File.Move(path, path + ".read");

                // Report / Log the activity
                Console.WriteLine("Processed file '{0}' into the OUTBOX.", result);
            }
            else { 
                Console.WriteLine("Already processed file '{0}'.", result);
            }

            // Go to the next file
        }

    }
}
