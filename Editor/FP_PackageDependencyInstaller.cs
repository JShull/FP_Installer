namespace FuzzPhyte.Installer.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.PackageManager;
    using UnityEditor.PackageManager.Requests;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Linq;

    public static class FP_PackageDependencyInstaller
    {
        private static List<Request> installRequests = new();
        public static bool IsInstalling { get; private set; } = false;

        public static void TryToInstallPackage(string packageGitURL)
        {
            //need the url to the git package here
            installRequests.Add(Client.Add(packageGitURL));
            SessionState.SetBool("FP_InstalledDeps_" + packageGitURL, true);
            IsInstalling = true;
            if (installRequests.Count > 0)
            {
                EditorApplication.update += MonitorInstallRequests;
            }
        }
        /// <summary>
        /// package name needs to be in the format of "com.fuzzphyte.package.name" 
        /// </summary>
        /// <param name="packageName"></param>
        public static void TryInstallDependencies(string packageName)
        {
            var allPackages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            var package = System.Array.Find(allPackages, p => p.name == packageName);

            if (package == null)
            {
                Debug.LogError($"Package {packageName} not found.");
                return;
            }
            string filePath = Path.Combine(package.resolvedPath, "Installation~/Dependencies.md");
            if (!File.Exists(filePath))
            {
                return;
            }
            if (SessionState.GetBool("FP_InstalledDeps_" + package.name, false))
            {
                return;
            }

            var urls = ParseMarkdownGitUrls(filePath);
            foreach (var url in urls)
            {
                Debug.Log($"[FuzzPhyte Installer] Installing {url} for {package.name}");
                installRequests.Add(Client.Add(url));
            }

            if (urls.Count > 0)
            {
                SessionState.SetBool("FP_InstalledDeps_" + package.name, true);
                IsInstalling = true;
            }

            

            if (installRequests.Count > 0)
            {
                EditorApplication.update += MonitorInstallRequests;
            }
               
        }
        /// <summary>
        /// Update package URL
        /// </summary>
        /// <param name="packageGitURL"></param>
        public static void PackageUpdateURL(string packageGitURL)
        {
            var request = Client.Add(packageGitURL);
            if (request != null) 
            {
                installRequests.Add(request);
                SessionState.SetBool("FP_InstalledDeps_" + packageGitURL, true);
                IsInstalling = true;
                if (installRequests.Count > 0)
                {
                    EditorApplication.update += MonitorInstallRequests;
                }
            }
            else
            {
                Debug.LogError($"FuzzPhyte Installer] Failed to create request for {packageGitURL}. It may already be installed or the URL is invalid.");
            }
        }
        private static void MonitorInstallRequests()
        {
            bool allDone = true;
            foreach (var req in installRequests)
            {
                if (!req.IsCompleted)
                {
                    allDone = false;
                    continue;
                }

                if (req.Status == StatusCode.Success)
                {
                    Debug.Log("[FuzzPhyte Installer] Successfully installed: " + req.ToString());
                }
                else
                {
                    Debug.LogError("[FuzzPhyte Installer] Failed to install: " + req.Error.message);
                }  
            }

            if (allDone)
            {
                installRequests.Clear();
                EditorApplication.update -= MonitorInstallRequests;
                IsInstalling = false;
            }
        }

        private static List<string> ParseMarkdownGitUrls(string filePath)
        {
            var urls = new List<string>();
            var lines = File.ReadAllLines(filePath);
            var regex = new Regex(@"\[.*?\]\((https:\/\/github\.com\/[^)]+\.git)\)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);
                if (match.Success)
                    urls.Add(match.Groups[1].Value);
            }

            return urls;
        }
    }
}
