namespace FuzzPhyte.Installer.Editor
{
    using UnityEditor;
    using UnityEngine;
    using System.Diagnostics;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor.PackageManager;
    using UnityEditor.PackageManager.Requests;
    using System.Text.RegularExpressions;
    public class FPManifest
    {
        public List<string> dependencyUrls = new List<string>();

        // Load and parse manifest.json
        public static FPManifest LoadFromFile(string manifestPath, string packageNameConvention)
        {
            var manifest = new FPManifest();

            if (!File.Exists(manifestPath))
            {
                UnityEngine.Debug.LogError("Could not find manifest.json file.");
                return null;
            }

            // Read all lines from the file
            string[] lines = File.ReadAllLines(manifestPath);
            bool inDependenciesSection = false;

            // Regular expression to capture URLs
            Regex urlPattern = new Regex(@"https?://[^\s""]+");

            foreach (string line in lines)
            {
                // Start processing once we reach "dependencies"
                if (line.Trim().StartsWith("\"dependencies\":"))
                {
                    inDependenciesSection = true;
                    continue;
                }

                // Stop processing if we exit the dependencies section
                if (inDependenciesSection && line.Trim().StartsWith("}"))
                {
                    break;
                }

                // Process lines only within the dependencies section
                // assuming package convention is something like com.<core>.PACKAGETOUPDATE = "com.fuzzphyte."
                if (inDependenciesSection && line.Contains(packageNameConvention))
                {
                    // Extract the URL using the regex pattern
                    Match match = urlPattern.Match(line);
                    if (match.Success)
                    {
                        manifest.dependencyUrls.Add(match.Value);
                    }
                }
            }

            return manifest;
        }
    }
}
