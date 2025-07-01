namespace FuzzPhyte.Installer.Editor
{
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    public class FP_InstallerWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private string manualURL = "";
        

        [MenuItem("FuzzPhyte/Installer/Dependency Installer")]
        public static void Open()
        {
            GetWindow<FP_InstallerWindow>("FP Installer");
        }

        private void OnGUI()
        {
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
                if (manualURL.StartsWith("http"))
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

        
    }
}
