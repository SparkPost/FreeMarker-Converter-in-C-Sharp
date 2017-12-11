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
then run the application.  Converted templates will appear in the OUTBOX.

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
                else if (Directory.Exists(path))
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



                // Collect all the in-line assignments into one array
                string @assignments = "substitution_data\": {\r\n";
                string pattern = @"<#(assign.*)/>";
                foreach (Match match in Regex.Matches(text, pattern))
                {
                    //Console.WriteLine("Found '{0}' at position {1}", match.Value, match.Index);
                    assignments = assignments + match.Value;
                }
                assignments = assignments.Replace("<#assign ", "\"");
                assignments = assignments.Replace(" />", "\",\r\n");
                assignments = assignments.Replace("/>", "\",\r\n");
                assignments = assignments.Replace("[0]!\"\"", "");
                assignments = assignments.Replace("=", "\":\"");

                //remove last comma
                assignments = assignments.Trim(',');

                // finish off the substitutions stanza
                assignments = assignments + "},";


                Regex rgx;

                // Remove all in-line assignments
                rgx = new Regex("<#(assign.*)/>");
                text = rgx.Replace(text, "");

                // Replace if conditions
                rgx = new Regex(@"<#if(.*)>([\s\n\r]*)(.*)([\s\n\r]*)</#if>");
                text = rgx.Replace(text, "\r\n{{if $1}}\r\n  $3 \r\n{{end}");

                // Replace foreach iterations
                rgx = new Regex(@"<#(.*) as (.*)>([\s\n\r]*)(.*)([\s\n\r]*)</#(.*)>");
                text = rgx.Replace(text, "\r\n{{each $1}}\r\n  $4 \r\n{{end}");

                // Replace empty variable condition
                rgx = new Regex("(\\[0\\]\\!\"\")");
                text = rgx.Replace(text, "");

                // Replace plain Variables (Interpolations)
                rgx = new Regex("\\$\\{(\\w*)");
                text = rgx.Replace(text, "{{$1}");

                // Replace comments
                rgx = new Regex(@"<#--(\w*)");
                text = rgx.Replace(text, "<!-- $1");


                string rawtext = "Use an HTML reader to view this message";
                string newTemplate = @"{
'options': {
    'open_tracking': true,
    'click_tracking': true,
    'transactional': false,
    'sandbox': false,
    'ip_pool': 'sp_shared',
    'inline_css': false
  },
  'description': 'n.description',
  'campaign_id': 'n.campaign',
  'metadata': {
            },
" + assignments + @"
  'recipients': [
    {
      'address': {
        'email': 'n.address.email',
        'name': 'n.address.fullname'
      },
      'tags': [],
      'metadata': {},
      'substitution_data': {}
    }
  ],
  'content': {
    'from': {
      'name': 'r.fromName',
      'email': 'r.fromEmail'
    },
    'subject': 'r.subject',
    'reply_to': 'r.replyAddress',
     'text': '" + rawtext + @"',
    'html': '" + text + @"'
  }
}";

                /**************************************************************/

                // Save it to the OUTBOX
                File.WriteAllText(appPath + "/OUTBOX/" + result + ".json", newTemplate);

                // Mark original file as "read"
                // commeted temporarily for debugging
          //      File.Move(path, path + ".read");

                // Report / Log the activity
                Console.WriteLine("Processed file '{0}' into the OUTBOX.", result + ".json");
            //    Console.WriteLine("{0}", @assignments);

            }
            else { 
                Console.WriteLine("Already processed file '{0}'.", result);
            }

            // Go to the next file
        }

    }
}
