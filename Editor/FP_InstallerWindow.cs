namespace FuzzPhyte.Installer.Editor
{
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;

    public class FP_InstallerWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private string manualURL = "";
        private static readonly Color InstallActiveColor = new Color(1f, 0.9f, 0.6f);  // light yellow
        private static readonly Color InstallDefaultColor = Color.cyan;
        private bool useSSH = false;


        [MenuItem("FuzzPhyte/Installer/FP Installer", priority = 0)]
        public static void Open()
        {
            GetWindow<FP_InstallerWindow>("FP Installer");
        }
        private void OnEnable()
        {
            EditorApplication.update += RepaintOnUpdate;
        }
        private void OnDisable()
        {
            EditorApplication.update -= RepaintOnUpdate;
        }
        private void RepaintOnUpdate()
        {
            Repaint();
        }
        private void OnGUI()
        {
            Color prevColor = GUI.backgroundColor;
            GUI.backgroundColor = FP_PackageDependencyInstaller.IsInstalling ? InstallActiveColor : InstallDefaultColor;
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("FuzzPhyte Package Installer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Use this to install package dependencies either for all packages or a single GitHub URL/package name.", MessageType.Info);
            EditorGUILayout.Space(8);


            if (GUILayout.Button("🔍 Install Dependencies for All Packages"))
            {
                InstallAllFuzzPhytePackages();
            }

            GUILayout.Space(10);
            GUILayout.Label("➕ Install Manually", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("GitHub URL or Package Name", GUILayout.Width(180)); // 👈 tweak width as needed
            manualURL = GUILayout.TextField(manualURL);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Install"))
            {
                if (manualURL.StartsWith("http")||manualURL.StartsWith("git@"))
                {
                    FP_PackageDependencyInstaller.TryToInstallPackage(manualURL);
                }else if(manualURL.StartsWith("com.fuzzphyte"))
                {
                    FP_PackageDependencyInstaller.TryInstallDependencies(manualURL);
                }
                else
                {
                    Debug.LogWarning("Enter either a valid GitHub .git URL or a FuzzPhyte package name.");
                } 
            }
            GUILayout.Space(10);
            GUILayout.Label("🔄 Update All FuzzPhyte Packages", EditorStyles.boldLabel);
            if(GUILayout.Button("Update FuzzPhyte Packages"))
            {
                CheckForPackageUpdates("com.fuzzphyte.");
            }
            
            GUILayout.EndVertical();
            GUI.backgroundColor = prevColor;
            GUILayout.Space(8);
            string status = FP_PackageDependencyInstaller.IsInstalling ? "Installing..." : "Idle";
            EditorGUILayout.LabelField($"Status: {status}", EditorStyles.miniBoldLabel);
        }

        private void InstallAllFuzzPhytePackages()
        {
            var allPackages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            var fuzzPackages = allPackages.Where(p => p.name.StartsWith("com.fuzzphyte."));

            foreach (var pkg in fuzzPackages)
            {
                FP_PackageDependencyInstaller.TryInstallDependencies(pkg.name);
            }
        }
        
        private void CheckForPackageUpdates(string packageNameConvention)
        {
            var projectRoot = new DirectoryInfo(Application.dataPath).Parent.FullName;

            string manifestPath = Path.Combine(projectRoot, "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                UnityEngine.Debug.LogError("Could not find manifest.json file.");
                return;
            }
            FPManifest manifest = FPManifest.LoadFromFile(manifestPath, packageNameConvention);
            if (manifest == null)
            {
                UnityEngine.Debug.LogError("Null on Manifest");
                return;
            }
            
            for (int i = 0; i < manifest.dependencyUrls.Count; i++)
            {
                var dependency = manifest.dependencyUrls[i];
                //UnityEngine.Debug.Log($"Fetching latest for package: {dependency}");
                FP_PackageDependencyInstaller.PackageUpdateURL(dependency);
            }
        }
    }
}
