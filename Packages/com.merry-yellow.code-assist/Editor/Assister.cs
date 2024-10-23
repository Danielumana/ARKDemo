using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


#pragma warning disable IDE0005
using Serilog = Meryel.Serilog;
#pragma warning restore IDE0005


#nullable enable


namespace Meryel.UnityCodeAssist.Editor
{
    public class Assister
    {
        public const string Version = "1.3.1"; //do NOT modify this line, except the number value, its being used by VSCode/Typescript for version detection (in exporter.ts.getVersionOfUnitySide())

#if MERYEL_UCA_LITE_VERSION
        public const string Title = "Code Assist Lite";
#else
        public const string Title = "Code Assist";
#endif

        [MenuItem("Tools/" + Title + "/Status", false, 1)]
        static void DisplayStatusWindow()
        {
            StatusWindow.Display();
        }


        [MenuItem("Tools/" + Title + "/Synchronize", false, 2)]
        static void Sync()
        {
            EditorCoroutines.EditorCoroutineUtility.StartCoroutine(SyncAux(), MQTTnetInitializer.Publisher);

            //MQTTnetInitializer.Publisher.SendConnect();
            //Serilog.Log.Information("Code Assist is looking for more IDEs to connect to...");

            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "Synchronize_MenuItem");
        }


        [MenuItem("Tools/" + Title + "/Report error", false, 91)]
        static void DisplayFeedbackWindow()
        {
            FeedbackWindow.Display();
        }

        [MenuItem("Tools/" + Title + "/About", false, 92)]
        static void DisplayAboutWindow()
        {
            AboutWindow.Display();
        }

#if MERYEL_UCA_LITE_VERSION
        [MenuItem("Tools/" + Title + "/Compare versions", false, 31)]
        static void CompareVersions()
        {
            Application.OpenURL("http://unitycodeassist.netlify.app/compare");

            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "CompareVersions_MenuItem");
        }

        [MenuItem("Tools/" + Title + "/Get full version", false, 32)]
        static void GetFullVersion()
        {
            Application.OpenURL("http://u3d.as/2N2H");

            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "FullVersion_MenuItem");
        }
#endif // MERYEL_UCA_LITE_VERSION

        [MenuItem("Tools/" + Title + "/Setup/Upgrade to full version", false, 65)]
        static void Upgrade()
        {
            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "Upgrade_MenuItem");

#if MERYEL_UCA_LITE_VERSION
            Serilog.Log.Information("Purchase <a href=\"http://u3d.as/2N2H\">'Code Assist'</a> from the asset store and download it from the package manager first");
            return;
#else
            var vsixPath = CommonTools.GetInstallerPath("UnityCodeAssist.Full.VisualStudio.Installer.vsix");
            if (!System.IO.File.Exists(vsixPath))
            {
                Serilog.Log.Information($"Installer for Visual Studio couldn't be found at {vsixPath}. Please try re-importing the asset from the package manager");
                return;
            }

            var installerPath = CommonTools.GetToolPath("InstallFullVersionOfVsix.bat");
            Execute(installerPath);
#endif
        }

        /*
        [MenuItem("Tools/" + Title + "/Setup/Update", false, 61)]
        static void Update()
        {
            //UnityEditor.PackageManager.Client.
        }
        */

        [MenuItem("Tools/" + Title + "/Setup/Re-import package", false, 62)]
        static void RepairFiles()
        {
            if (MQTTnetInitializer.Publisher?.clients.Any() != true)
                Serilog.Log.Information("No connected IDE found. Please start up Visual Studio first");

            var cleanupPath = CommonTools.GetToolPath("CleanupObsoleteFiles.bat");
            //var cleanupPath = CommonTools.GetToolPath("HelloWorld.bat");
            Execute(cleanupPath);
            MQTTnetInitializer.Publisher?.SendRequestUpdate("Unity", string.Empty, true);

            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "Reimport_MenuItem");
        }

        [MenuItem("Tools/" + Title + "/Setup/Import files for .NET Standard 2.0", false, 63)]
        static void ImportSystemBinariesForDotNetStandard20()
        {
            if (MQTTnetInitializer.Publisher?.clients.Any() != true)
                Serilog.Log.Information("No connected IDE found. Please start up Visual Studio first");

            MQTTnetInitializer.Publisher?.SendRequestUpdate("SystemBinariesForDotNetStandard20", string.Empty, true);

            MQTTnetInitializer.Publisher?.SendAnalyticsEvent("Gui", "ImportNetStandard20_MenuItem");
        }

        [MenuItem("Tools/" + Title + "/Setup/Regenerate project files", false, 64)]
        public static void RegenerateProjectFiles()
        {
            // other similar approaches
            // https://www.reddit.com/r/Unity3D/comments/s1joc6/help_with_generating_csproj_and_sln_for_github/
            // https://discussions.unity.com/t/manually-generate-sln-and-csproj-files/648686/6
            // https://discussions.unity.com/t/how-can-i-generate-csproj-files-during-continuous-integration-builds/842493/3
            // https://github.com/Unity-Technologies/UnityCsReference/blob/f45f297f342239326ea865a57a1bb8ddf93e38c6/Editor/Mono/CodeEditor/SyncVS.cs#L22
            try
            {
                var t = ScriptFinder.GetType123("Microsoft.Unity.VisualStudio.Editor.Cli");
                var m = t!.GetMethod("GenerateSolution", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                m.Invoke(null, null);
            }
            catch (System.Exception ex)
            {
                Serilog.Log.Error(ex, "Couldn't invoke GenerateSolution");
                Serilog.Log.Information("Please 'Regenerate project files' manually. 'Edit'->'Preferences'->'External Tools'->'Regenerate project files'");
            }
        }

        internal static string Execute(string path)//, string args, string workingDirectoryPath)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                //startInfo.FileName = GetExePath();
                FileName = path,
                //startInfo.Arguments = args;
                UseShellExecute = false,
                RedirectStandardOutput = true
                //startInfo.WorkingDirectory = workingDirectoryPath;
            };
            var process = new System.Diagnostics.Process
            {
                StartInfo = startInfo
            };

            try
            {
                process.Start();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                Serilog.Log.Error(ex, "Error at running bat file {File}", path);
            }

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }


        static IEnumerator SyncAux()
        {
            var clientCount = MQTTnetInitializer.Publisher?.clients.Count ?? 0;
            MQTTnetInitializer.Publisher?.SendConnect();
            Serilog.Log.Information("Code Assist is looking for more IDEs to connect to...");

            //yield return new WaitForSeconds(3);
            yield return new EditorCoroutines.EditorWaitForSeconds(3);

            var newClientCount = MQTTnetInitializer.Publisher?.clients.Count ?? 0;

            var dif = newClientCount - clientCount;

            if (dif <= 0)
                Serilog.Log.Information("Code Assist couldn't find any new IDE to connect to.");
            else
                Serilog.Log.Information("Code Assist is connected to {Dif} new IDE(s).", dif);
        }

#if MERYEL_DEBUG

        [MenuItem("Code Assist/Binary2Text")]
        static void Binary2Text()
        {
            var filePath = CommonTools.GetInputManagerFilePath();
            var hash = Input.UnityInputManager.GetMD5Hash(filePath);
            var convertedPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"UCA_IM_{hash}.txt");
            
            var b = new Input.Binary2TextExec();
            b.Exec(filePath, convertedPath, detailed: false, largeBinaryHashOnly: false, hexFloat: false);
        }

        [MenuItem("Code Assist/Bump InputManager")]
        static void BumpInputManager()
        {
            Input.InputManagerMonitor.Instance.Bump();
        }


        [MenuItem("Code Assist/Layer Check")]
        static void UpdateLayers()
        {
            var names = UnityEditorInternal.InternalEditorUtility.layers;
            var indices = names.Select(l => LayerMask.NameToLayer(l).ToString()).ToArray();
            MQTTnetInitializer.Publisher?.SendLayers(indices, names);

            var sls = SortingLayer.layers;
            var sortingNames = sls.Select(sl => sl.name).ToArray();
            var sortingIds = sls.Select(sl => sl.id.ToString()).ToArray();
            var sortingValues = sls.Select(sl => sl.value.ToString()).ToArray();

            MQTTnetInitializer.Publisher?.SendSortingLayers(sortingNames, sortingIds, sortingValues);

            /*
            for (var i = 0; i < 32; i++)
            {
                var name = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(name))
                {
                    Debug.Log(i + ":" + name);
                }
            }

            if (ScriptFinder.FindGameObjectOfType("Deneme", out var go))
                MQTTnetInitializer.Publisher.SendGameObject(go);
            */
        }

        [MenuItem("Code Assist/Tag Check")]
        static void UpdateTags()
        {
            Serilog.Log.Debug("Listing tags {Count}", UnityEditorInternal.InternalEditorUtility.tags.Length);

            foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    Serilog.Log.Debug("{Tag}", tag);
                }
            }

            MQTTnetInitializer.Publisher?.SendTags(UnityEditorInternal.InternalEditorUtility.tags);

        }

        [MenuItem("Code Assist/GO Check")]

        static void TestGO()
        {

            var go = GameObject.Find("Deneme");
            //var go = MonoBehaviour.FindObjectOfType<Deneme>().gameObject;

            MQTTnetInitializer.Publisher?.SendGameObject(go);
        }

        [MenuItem("Code Assist/Undo Records Test")]
        static void UndoTest()
        {
            var undos = new List<string>();
            var redos = new List<string>();

            var type = typeof(Undo);
            System.Reflection.MethodInfo dynMethod = type.GetMethod("GetRecords",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            dynMethod.Invoke(null, new object[] { undos, redos });

            Serilog.Log.Debug("undos:{UndoCount},redos:{RedoCount}", undos.Count, redos.Count);

            var last = undos.LastOrDefault();
            if (last != null)
            {
                Serilog.Log.Debug("last:{Last}", last);
                Serilog.Log.Debug("group:{UndoCurrentGroup},{UndoCurrentGroupName}",
                    Undo.GetCurrentGroup(), Undo.GetCurrentGroupName());
            }
        }


        [MenuItem("Code Assist/Undo List Test")]
        static void Undo2Test()
        {

            //List<string> undoList, out int undoCursor
            var undoList = new List<string>();
            int undoCursor = int.MaxValue;
            var type = typeof(Undo);
            System.Reflection.MethodInfo dynMethod = type.GetMethod("GetUndoList",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            dynMethod = type.GetMethod("GetUndoList",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static,
                null,
                new System.Type[] { typeof(List<string>), typeof(int).MakeByRefType() },
                null);


            dynMethod.Invoke(null, new object[] { undoList, undoCursor });

            Serilog.Log.Debug("undo count: {Count}", undoList.Count);

        }

        [MenuItem("Code Assist/Reload Domain")]
        static void ReloadDomain()
        {
            EditorUtility.RequestScriptReload();

        }


        /*
        [MenuItem("Code Assist/TEST")]
        static void TEST()
        {
            //if (ScriptFinder.FindGameObjectOfType("Deneme_OtherScene", out var go))
            if (ScriptFinder.FindInstanceOfType("Deneme_SO", out var go, out var so))
            {
                MQTTnetInitializer.Publisher.SendScriptableObject(so);
            }

            ScriptFinder.DENEMEEEE();



        }
        */

#endif // MERYEL_DEBUG


        public static void SendTagsAndLayers()
        {
            Serilog.Log.Debug(nameof(SendTagsAndLayers));

            var tags = UnityEditorInternal.InternalEditorUtility.tags;
            MQTTnetInitializer.Publisher?.SendTags(tags);

            var names = UnityEditorInternal.InternalEditorUtility.layers;
            var indices = names.Select(l => LayerMask.NameToLayer(l).ToString()).ToArray();
            MQTTnetInitializer.Publisher?.SendLayers(indices, names);

            var sls = SortingLayer.layers;
            var sortingNames = sls.Select(sl => sl.name).ToArray();
            var sortingIds = sls.Select(sl => sl.id.ToString()).ToArray();
            var sortingValues = sls.Select(sl => sl.value.ToString()).ToArray();
            MQTTnetInitializer.Publisher?.SendSortingLayers(sortingNames, sortingIds, sortingValues);
        }

    }
}
