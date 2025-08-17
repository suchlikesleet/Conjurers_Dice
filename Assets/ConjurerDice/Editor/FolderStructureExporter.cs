using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ConjurerDice
{
    public class FolderStructureExporter : EditorWindow
    {
        private DefaultAsset rootFolder;
        private bool includeSecondaryClasses = false;
        private bool includeMethodNames = false;
        private bool ignoreLifecycleMethods = false;

        private readonly string[] unityLifecycleMethods =
        {
            "Awake","Start","Update","FixedUpdate","LateUpdate",
            "OnEnable","OnDisable","OnDestroy"
        };

        [MenuItem("Tools/Folder Structure Exporter")]
        public static void Open()
        {
            GetWindow<FolderStructureExporter>("Folder Structure Exporter");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Root Folder", EditorStyles.boldLabel);
            rootFolder = (DefaultAsset)EditorGUILayout.ObjectField("Root Folder", rootFolder, typeof(DefaultAsset), false);

            includeSecondaryClasses = EditorGUILayout.Toggle("Include nested/secondary classes", includeSecondaryClasses);
            includeMethodNames      = EditorGUILayout.Toggle("Include method names", includeMethodNames);
            ignoreLifecycleMethods  = EditorGUILayout.Toggle("Ignore Unity lifecycle methods", ignoreLifecycleMethods);

            if (GUILayout.Button("Export to Markdown"))
            {
                if (rootFolder == null)
                {
                    EditorUtility.DisplayDialog("Export", "Please select a root folder.", "OK");
                    return;
                }
                var rootPath = AssetDatabase.GetAssetPath(rootFolder);
                if (!AssetDatabase.IsValidFolder(rootPath))
                {
                    EditorUtility.DisplayDialog("Export", "Selected asset is not a folder.", "OK");
                    return;
                }

                string markdown = BuildTree(rootPath, 0);

                var savePath = EditorUtility.SaveFilePanel("Save Markdown", Application.dataPath, "FolderStructure", "md");
                if (!string.IsNullOrEmpty(savePath))
                {
                    File.WriteAllText(savePath, markdown);
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Export", $"Exported:\n{savePath}", "OK");
                }
            }
        }

        private string BuildTree(string folderPath, int depth)
        {
            var sb = new StringBuilder();
            string indent = new string(' ', depth * 2);
            sb.AppendLine($"{indent}├── {Path.GetFileName(folderPath)}");

            foreach (var dir in Directory.GetDirectories(folderPath))
                sb.Append(BuildTree(dir, depth + 1));

            foreach (var file in Directory.GetFiles(folderPath))
            {
                if (file.EndsWith(".meta")) continue;

                string line = $"{indent}│   ├── {Path.GetFileName(file)}";
                if (file.EndsWith(".cs"))
                {
                    var classes = GetClassNames(file);
                    if (classes.Any())
                    {
                        var selectedClasses = includeSecondaryClasses ? classes : new[] { classes.First() };
                        var names = selectedClasses.Select(cls =>
                        {
                            if (!includeMethodNames) return cls;

                            var methods = GetMethods(file, cls);
                            if (ignoreLifecycleMethods)
                            {
                                methods = methods.Where(m => !unityLifecycleMethods.Contains(m.Split('(')[0])).ToArray();
                            }

                            return methods.Any() ? $"{cls} ({string.Join(", ", methods)})" : cls;
                        });

                        line += $" ({string.Join(", ", names)})";
                    }
                }

                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        private static string[] GetClassNames(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var matches = Regex.Matches(content, @"\bclass\s+([A-Za-z0-9_]+)");
            return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
        }

        private static string[] GetMethods(string filePath, string className)
        {
            var content = File.ReadAllText(filePath);

            // --- Find class block using brace counting ---
            int idx = content.IndexOf("class " + className);
            if (idx < 0) return new string[0];

            // Move to first "{"
            int braceStart = content.IndexOf('{', idx);
            if (braceStart < 0) return new string[0];

            int braceCount = 0;
            int end = braceStart;
            for (int i = braceStart; i < content.Length; i++)
            {
                if (content[i] == '{') braceCount++;
                if (content[i] == '}') braceCount--;
                if (braceCount == 0) { end = i; break; }
            }

            if (braceCount != 0) end = content.Length - 1; // fallback

            string body = content.Substring(braceStart + 1, end - braceStart - 1);

            // --- Regex to find method declarations (also works with attributes above) ---
            var regex = new Regex(@"(^|\n)\s*(\[[^\]]*\]\s*)*(?:public|private|protected|internal|static|\s)+\s+[A-Za-z0-9_<>]+\s+([A-Za-z0-9_]+)\s*\(([^\)]*)\)",
                                  RegexOptions.Multiline);

            var matches = regex.Matches(body);

            return matches.Cast<Match>().Select(m =>
            {
                var name = m.Groups[3].Value;
                var p = m.Groups[4].Value.Trim();
                if (string.IsNullOrEmpty(p)) return $"{name}()";

                var types = string.Join(", ",
                    p.Split(',')
                     .Select(param => param.Trim().Split(' ')[0]));

                return $"{name}({types})";
            }).ToArray();
        }
    }
}
