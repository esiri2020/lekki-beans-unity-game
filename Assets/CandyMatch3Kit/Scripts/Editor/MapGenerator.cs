using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using GameVanilla.Game.UI; 

namespace GameVanilla.Editor
{
    public class MapGenerator : EditorWindow
    {
        private GameObject levelMapParent;
        private GameObject buttonParent;
        private GameObject buttonPrefab;
        
        private List<Sprite> backgroundSprites = new List<Sprite>();
        private List<GameObject> cloudPrefabs = new List<GameObject>(); // New: Cloud Prefabs
        private Sprite bottomSprite; 

        private int startLevel = 1;
        private float backgroundHeight = 3500f;
        
        // Exact Reconstruction Constants
        private float originalMapParentY = -7961f;
        private float originalMap1LocalY = 755f;
        private float originalBottomLocalY = -2114f;

        private int repeatCount = 1;
        private Vector2 scrollPos;
        
        private string diagnosticFilename = "LevelMap10.png";
        private bool forceReimportAll = false;

        // EXACT BUTTON COORDINATES (Levels 1-100)
        private Vector2[] levelPositions = new Vector2[] {
            new Vector2(166.0f, -8820.0f), // Level 1
            new Vector2(-24.0f, -8612.0f), // Level 2
            new Vector2(-152.0f, -8380.0f), // Level 3
            new Vector2(-186.0f, -8119.0f), // Level 4
            new Vector2(-22.0f, -7931.0f), // Level 5
            new Vector2(246.0f, -7876.0f), // Level 6
            new Vector2(328.0f, -7636.0f), // Level 7
            new Vector2(150.0f, -7482.0f), // Level 8
            new Vector2(-133.0f, -7510.0f), // Level 9
            new Vector2(-327.0f, -7372.0f), // Level 10
            new Vector2(-300.0f, -7156.0f), // Level 11
            new Vector2(-197.0f, -6954.0f), // Level 12
            new Vector2(-16.0f, -6826.0f), // Level 13
            new Vector2(222.0f, -6770.0f), // Level 14
            new Vector2(377.0f, -6582.0f), // Level 15
            new Vector2(269.0f, -6373.0f), // Level 16
            new Vector2(65.0f, -6252.0f), // Level 17
            new Vector2(-52.0f, -6053.0f), // Level 18
            new Vector2(12.0f, -5842.0f), // Level 19
            new Vector2(1.0f, -5598.0f), // Level 20
            new Vector2(-6.0f, -5284.0f), // Level 21
            new Vector2(242.0f, -5166.0f), // Level 22
            new Vector2(384.0f, -4963.0f), // Level 23
            new Vector2(314.0f, -4747.0f), // Level 24
            new Vector2(68.0f, -4670.0f), // Level 25
            new Vector2(-200.0f, -4653.0f), // Level 26
            new Vector2(-407.0f, -4516.0f), // Level 27
            new Vector2(-413.0f, -4269.0f), // Level 28
            new Vector2(-211.0f, -4120.0f), // Level 29
            new Vector2(70.0f, -4139.0f), // Level 30
            new Vector2(345.0f, -4102.0f), // Level 31
            new Vector2(457.0f, -3892.0f), // Level 32
            new Vector2(420.0f, -3647.0f), // Level 33
            new Vector2(226.0f, -3488.0f), // Level 34
            new Vector2(16.0f, -3349.0f), // Level 35
            new Vector2(-126.0f, -3144.0f), // Level 36
            new Vector2(-174.0f, -2829.0f), // Level 37
            new Vector2(-163.0f, -2588.0f), // Level 38
            new Vector2(-39.0f, -2394.0f), // Level 39
            new Vector2(-3.0f, -2118.0f), // Level 40
            new Vector2(11.0f, -1752.0f), // Level 41
            new Vector2(242.0f, -1651.0f), // Level 42
            new Vector2(403.0f, -1466.0f), // Level 43
            new Vector2(423.0f, -1221.0f), // Level 44
            new Vector2(277.0f, -1013.0f), // Level 45
            new Vector2(-17.0f, -942.0f), // Level 46
            new Vector2(-307.0f, -871.0f), // Level 47
            new Vector2(-414.0f, -655.0f), // Level 48
            new Vector2(-248.0f, -474.0f), // Level 49
            new Vector2(42.0f, -418.0f), // Level 50
            new Vector2(243.0f, -257.0f), // Level 51
            new Vector2(424.0f, -105.0f), // Level 52
            new Vector2(409.0f, 130.0f), // Level 53
            new Vector2(228.0f, 302.0f), // Level 54
            new Vector2(-51.0f, 342.0f), // Level 55
            new Vector2(-263.0f, 487.0f), // Level 56
            new Vector2(-275.0f, 736.0f), // Level 57
            new Vector2(-132.0f, 933.0f), // Level 58
            new Vector2(16.0f, 1092.0f), // Level 59
            new Vector2(1.0f, 1373.0f), // Level 60
            new Vector2(-2.0f, 1635.0f), // Level 61
            new Vector2(301.0f, 1788.0f), // Level 62
            new Vector2(61.0f, 2035.0f), // Level 63
            new Vector2(-93.0f, 2246.0f), // Level 64
            new Vector2(-322.0f, 2575.0f), // Level 65
            new Vector2(-407.0f, 2827.0f), // Level 66
            new Vector2(-246.0f, 3016.0f), // Level 67
            new Vector2(6.0f, 2946.0f), // Level 68
            new Vector2(287.0f, 2928.0f), // Level 69
            new Vector2(416.0f, 3122.0f), // Level 70
            new Vector2(343.0f, 3346.0f), // Level 71
            new Vector2(97.0f, 3434.0f), // Level 72
            new Vector2(-144.0f, 3542.0f), // Level 73
            new Vector2(-224.0f, 3775.0f), // Level 74
            new Vector2(-104.0f, 3984.0f), // Level 75
            new Vector2(153.0f, 4061.0f), // Level 76
            new Vector2(302.0f, 4249.0f), // Level 77
            new Vector2(150.0f, 4442.0f), // Level 78
            new Vector2(-25.0f, 4614.0f), // Level 79
            new Vector2(0.0f, 4894.0f), // Level 80
            new Vector2(-8.0f, 5153.0f), // Level 81
            new Vector2(208.0f, 5387.0f), // Level 82
            new Vector2(336.0f, 5592.0f), // Level 83
            new Vector2(138.0f, 5720.0f), // Level 84
            new Vector2(-131.0f, 5703.0f), // Level 85
            new Vector2(-337.0f, 5853.0f), // Level 86
            new Vector2(-248.0f, 6085.0f), // Level 87
            new Vector2(24.0f, 6075.0f), // Level 88
            new Vector2(302.0f, 6064.0f), // Level 89
            new Vector2(385.0f, 6289.0f), // Level 90
            new Vector2(206.0f, 6496.0f), // Level 91
            new Vector2(34.0f, 6831.0f), // Level 92
            new Vector2(-173.0f, 7130.0f), // Level 93
            new Vector2(-242.0f, 7372.0f), // Level 94
            new Vector2(-59.0f, 7520.0f), // Level 95
            new Vector2(198.0f, 7543.0f), // Level 96
            new Vector2(242.0f, 7768.0f), // Level 97
            new Vector2(19.0f, 7896.0f), // Level 98
            new Vector2(-126.0f, 8074.0f), // Level 99
            new Vector2(-42.0f, 8359.0f), // Level 100
        };

        // EXACT CLOUD COORDINATES (11 unique clouds)
        private struct CloudData {
            public string name;
            public Vector2 pos;
            public CloudData(string n, float x, float y) { name = n; pos = new Vector2(x, y); }
        }
        
        private CloudData[] cloudPositions = new CloudData[] {
            new CloudData("Cloud1Particle", -1063.334f, -267.77295f),
            new CloudData("Cloud2Particle", 1023.33356f, 725.5605f),
            new CloudData("Cloud3Particle", -1124.4446f, 2120.005f),
            new CloudData("Cloud4Particle", 990.00024f, 3741.1162f),
            new CloudData("Cloud5Particle", -1124.4448f, 5221.1167f),
            new CloudData("Cloud6Particle", 990.00024f, 7384.45f),
            new CloudData("Cloud7Particle", -1124.4448f, 9007.783f),
            new CloudData("Cloud8Particle", 990.00024f, 10495.561f),
            new CloudData("Cloud9Particle", -1124.4448f, 12399.0f),
            new CloudData("Cloud10Particle", 990.00024f, 13886.777f),
            new CloudData("Cloud11Particle", -1124.4448f, 15476.222f),
        };

        [MenuItem("Candy Match 3 Kit/Map Generator")]
        public static void ShowWindow()
        {
            GetWindow<MapGenerator>("Map Generator");
        }

        private string GetAbsoluteFolderPath()
        {
            return Path.Combine(Application.dataPath, "CandyMatch3Kit/Sprites/LevelScene/LevelMap").Replace('\\', '/');
        }
        
        private string GetCloudFolderPath()
        {
            return Path.Combine(Application.dataPath, "CandyMatch3Kit/Prefabs/Particles/Level").Replace('\\', '/');
        }

        private string ToRelativePath(string absolutePath)
        {
            string dataPath = Application.dataPath.Replace('\\', '/');
            string path = absolutePath.Replace('\\', '/');
            if (path.StartsWith(dataPath, System.StringComparison.OrdinalIgnoreCase)) 
            {
                string relative = "Assets" + path.Substring(dataPath.Length);
                return relative.Replace("//", "/");
            }
            return null;
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("Map Generation Settings", EditorStyles.boldLabel);
            levelMapParent = (GameObject)EditorGUILayout.ObjectField("Level Map Parent", levelMapParent, typeof(GameObject), true);
            buttonParent = (GameObject)EditorGUILayout.ObjectField("Button Parent (Optional)", buttonParent, typeof(GameObject), true);
            buttonPrefab = (GameObject)EditorGUILayout.ObjectField("Button Prefab", buttonPrefab, typeof(GameObject), false);
            
            startLevel = EditorGUILayout.IntField("Start Level Number", startLevel);
            
            GUILayout.Space(5);
            repeatCount = EditorGUILayout.IntField("Repeat Pattern", repeatCount);

            GUILayout.Space(10);
            GUILayout.Label("File Tools", EditorStyles.boldLabel);
            
            forceReimportAll = EditorGUILayout.Toggle("Force Reimport ALL", forceReimportAll);

            if (GUILayout.Button("1. Scan & Fix Imports"))
            {
                FixImports();
            }
            if (GUILayout.Button("2. Load Sprites & Clouds"))
            {
                LoadSprites();
                LoadClouds();
            }

            // --- DIAGNOSTIC ---
            GUILayout.Space(10);
            GUILayout.Label("Deep Diagnostic", EditorStyles.boldLabel);
            diagnosticFilename = EditorGUILayout.TextField("Test File", diagnosticFilename);
            if (GUILayout.Button("Run Diagnostics on File"))
            {
                RunDiagnostics(diagnosticFilename);
            }
            // ------------------

            GUILayout.Space(10);
            GUILayout.Label("Loaded Assets", EditorStyles.boldLabel);
            bottomSprite = (Sprite)EditorGUILayout.ObjectField("Bottom (Rectangle)", bottomSprite, typeof(Sprite), false);
            
            GUILayout.Label("Clouds (" + cloudPrefabs.Count + ")", EditorStyles.boldLabel);
            // Allow user to see all clouds
            int cloudShowCount = EditorGUILayout.IntField("Preview Size", cloudPrefabs.Count);
            // We generally don't want to add nulls via this simple UI for auto-loaded stuff, but purely for viewing:
            int loopCount = Mathf.Min(cloudShowCount, cloudPrefabs.Count);
            
            for(int i=0; i<loopCount; i++) {
                 EditorGUILayout.ObjectField("Cloud " + (i+1), cloudPrefabs[i], typeof(GameObject), false);
            }
            
            GUILayout.Space(5);
            int newCount = Mathf.Max(0, EditorGUILayout.IntField("Preview Size", backgroundSprites.Count));
            while (newCount < backgroundSprites.Count) backgroundSprites.RemoveAt(backgroundSprites.Count - 1);
            while (newCount > backgroundSprites.Count) backgroundSprites.Add(null);

            for (int i = 0; i < backgroundSprites.Count; i++)
            {
                backgroundSprites[i] = (Sprite)EditorGUILayout.ObjectField($"Sprite {i + 1}", backgroundSprites[i], typeof(Sprite), false);
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Clear Existing Map"))
            {
                if (EditorUtility.DisplayDialog("Confirm", "Delete all existing backgrounds, buttons, AND CLOUDS?", "Yes", "No"))
                {
                    ClearMap();
                }
            }

            if (GUILayout.Button("Generate Map (Matched to Backup)"))
            {
                GenerateMap();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void RunDiagnostics(string filename)
        {
            try
            {
                string folder = GetAbsoluteFolderPath();
                string absPath = Path.Combine(folder, filename).Replace('\\', '/');
                Debug.Log($"--- DIAGNOSTIC: {filename} ---");
                if (!File.Exists(absPath)) { Debug.LogError("Missing: " + absPath); return; }
                FileInfo fi = new FileInfo(absPath);
                Debug.Log($"Size: {fi.Length}");
                string relPath = ToRelativePath(absPath);
                Debug.Log($"Relative: {relPath}");
                AssetImporter imp = AssetImporter.GetAtPath(relPath);
                if (imp != null && imp is TextureImporter ti) 
                {
                    Debug.Log($"Mode: {ti.spriteImportMode}"); 
                }
                Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(relPath);
                Debug.Log($"Sprite Load: {(s!=null?"OK":"FAIL")}");
            }
            catch (System.Exception e) { Debug.LogError("Diag error: " + e.Message); }
        }

        private void FixImports()
        {
            string fPath = GetAbsoluteFolderPath();
            if (!Directory.Exists(fPath)) { Debug.LogError("Folder missing"); return; }
            string[] allFiles = Directory.GetFiles(fPath);
            int fixedCount = 0;
            foreach (string filePathRaw in allFiles)
            {
                string fileName = Path.GetFileName(filePathRaw);
                if (fileName.EndsWith(".meta")) continue;
                string ext = Path.GetExtension(fileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg") continue;
                if (!fileName.StartsWith("LevelMap") && !fileName.StartsWith("Rectangle")) continue; 

                string relativePath = ToRelativePath(filePathRaw);
                if (relativePath == null) continue;

                AssetImporter importer = AssetImporter.GetAtPath(relativePath);
                if (forceReimportAll || importer == null)
                {
                    AssetDatabase.ImportAsset(relativePath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                    importer = AssetImporter.GetAtPath(relativePath);
                }

                if (importer is TextureImporter texImporter)
                {
                    bool dirty = false;
                    if (texImporter.textureType != TextureImporterType.Sprite) { texImporter.textureType = TextureImporterType.Sprite; dirty = true; }
                    if (texImporter.spriteImportMode != SpriteImportMode.Single) { texImporter.spriteImportMode = SpriteImportMode.Single; dirty = true; }
                    if (dirty) { texImporter.SaveAndReimport(); fixedCount++; }
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Scan Complete", $"Updated {fixedCount} files.", "OK");
        }

       private void LoadClouds()
        {
            cloudPrefabs.Clear();
            string cPath = GetCloudFolderPath();
             if (!Directory.Exists(cPath)) { Debug.LogError("Cloud Folder missing: " + cPath); return; }
             
             // Look for Cloud1Particle.prefab to Cloud11Particle.prefab
             for (int i = 1; i <= 11; i++)
             {
                 string fname = "Cloud" + i + "Particle.prefab";
                 string absPath = Path.Combine(cPath, fname).Replace('\\', '/');
                 string relPath = ToRelativePath(absPath);
                 if (!string.IsNullOrEmpty(relPath))
                 {
                     GameObject fab = AssetDatabase.LoadAssetAtPath<GameObject>(relPath);
                     if (fab != null) cloudPrefabs.Add(fab);
                     else Debug.LogWarning("Could not load cloud: " + fname);
                 }
             }
             Debug.Log($"Loaded {cloudPrefabs.Count} Cloud Prefabs.");
        }

        private void LoadSprites()
        {
            backgroundSprites.Clear();
            bottomSprite = null;
            
            string fPath = GetAbsoluteFolderPath();
            string[] allFiles = Directory.GetFiles(fPath);
            List<Sprite> loadedSprites = new List<Sprite>();
            List<string> failedFiles = new List<string>();

            foreach (string filePathRaw in allFiles)
            {
                string fileName = Path.GetFileName(filePathRaw);
                if (fileName.EndsWith(".meta")) continue;
                string ext = Path.GetExtension(fileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg") continue;
                
                bool isLevelMap = fileName.StartsWith("LevelMap") && fileName != "LevelMapEnd.png";
                bool isBottom = fileName.Equals("Rectangle.png", System.StringComparison.OrdinalIgnoreCase);

                if (!isLevelMap && !isBottom) continue;

                string relativePath = ToRelativePath(filePathRaw);
                if (relativePath == null) continue;

                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
                if (sprite == null)
                {
                    Object[] assets = AssetDatabase.LoadAllAssetsAtPath(relativePath);
                    foreach (Object obj in assets) { if (obj is Sprite s) { sprite = s; break; } }
                }

                if (sprite != null)
                {
                    if (isBottom) bottomSprite = sprite;
                    else loadedSprites.Add(sprite);
                }
                else failedFiles.Add(fileName);
            }

            backgroundSprites = loadedSprites.Distinct().OrderBy(s => 
            {
                string numberString = System.Text.RegularExpressions.Regex.Match(s.name, @"\d+").Value;
                return int.TryParse(numberString, out int number) ? number : int.MaxValue;
            }).ToList();

            string report = $"Loaded {backgroundSprites.Count} unique sprites.";
            if (bottomSprite != null) report += "\nLoaded Bottom Sprite (Rectangle).";
            else report += "\nWarning: Bottom Sprite (Rectangle) NOT found.";
            
            if (failedFiles.Count > 0) report += $"\nFailed: {failedFiles.Count} files.";
            Debug.Log($"[MapGenerator] {report}");
            EditorUtility.DisplayDialog("Result", report, "OK");
        }

        private void ClearMap()
        {
            if (levelMapParent != null)
            {
                var children = new List<GameObject>();
                foreach (Transform child in levelMapParent.transform) children.Add(child.gameObject);
                foreach (var child in children)
                {
                     // Aggressively clear all generated or template objects
                     if (child.name.StartsWith("LevelMap") || 
                         child.name.StartsWith("LevelButton") || 
                         child.name == "MapHolder" || 
                         child.name == "ButtonHolder" || 
                         child.name == "Bottom" ||
                         child.name == "LevelMapButton" ||
                         child.name == "Animations") 
                        DestroyImmediate(child);
                }
            }
            if (buttonParent != null)
            {
                 var bChildren = new List<GameObject>();
                 foreach (Transform child in buttonParent.transform) bChildren.Add(child.gameObject);
                 foreach (var child in bChildren) { if (child.name.StartsWith("LevelButton") || child.name == "LevelMapButton") DestroyImmediate(child); }
            }
        }

        private void GenerateMap()
        {
            if (levelMapParent == null || buttonPrefab == null || backgroundSprites.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "Please assign Parent, Button Prefab, and at least one Sprite.", "OK");
                return;
            }

            // 0. AUTO-DETECT CORRECT PARENT (Ignore User Selection to prevent errors)
            // The user often selects the wrong "LevelMap". We need the one specific ScrollRect uses.
            ScrollRect sr = GameObject.FindObjectOfType<ScrollRect>();
            if (sr != null && sr.content != null)
            {
                levelMapParent = sr.content.gameObject;
                buttonParent = null; // Reset to force internal generation
                Debug.Log($"Auto-Detected correct Map Parent: {levelMapParent.name} (ScrollRect Content)");
            }
            else
            {
                 // Fallback: If passed object has ScrollRect
                 sr = levelMapParent.GetComponent<ScrollRect>();
                 if (sr != null && sr.content != null) levelMapParent = sr.content.gameObject;
            }

            // 1. Setup Hierarchy
            List<Sprite> fullSequence = new List<Sprite>();
            for (int r = 0; r < repeatCount; r++) fullSequence.AddRange(backgroundSprites);

            // CALCULATE DYNAMIC HEIGHT & OFFSETS
            // The original coordinates are centered for a map of ~17500 height (5 maps).
            // When we expand to 500 levels, the height grows, and the "Bottom" moves down.
            // We must shift everything down to keep Level 1 at the bottom.
            
            float referenceHeight = 17500f; // Height of 5 maps (Original Backup Size)
            float totalHeight = fullSequence.Count * backgroundHeight;
            // Add some padding
            float finalContentHeight = totalHeight + 2000f; 
            
            // If new map is taller, we shift content down by half the growth to stick to the bottom edge
            // relative to the center pivot.
            float heightGrowth = finalContentHeight - referenceHeight;
            float verticalOffset = heightGrowth / 2f; 

            // OLD OBJECT CLEANUP (Safety First)
            RemoveDuplicateNamedObjects("MapHolder");
            RemoveDuplicateNamedObjects("Animations");
            RemoveDuplicateNamedObjects("ButtonHolder");
            RemoveDuplicateNamedObjects("LevelmapButtons"); // Old name
            RemoveDuplicateNamedObjects("Bottom");
            
            // A. Map Holder (Backgrounds)
            GameObject mapHolder = new GameObject("MapHolder");
            mapHolder.transform.SetParent(levelMapParent.transform, false);
            RectTransform mapHolderRect = mapHolder.AddComponent<RectTransform>();
            // Original: -7961. New: Shift down by offset.
            mapHolderRect.anchoredPosition = new Vector2(0, originalMapParentY - verticalOffset); 
            
            // B. Animations Holder
            GameObject animHolder = new GameObject("Animations");
            animHolder.transform.SetParent(levelMapParent.transform, false);
            RectTransform animRect = animHolder.AddComponent<RectTransform>();
            animRect.anchoredPosition = new Vector2(0, originalMapParentY - verticalOffset);
            
            // C. Button Holder
            GameObject buttonHolder = new GameObject("ButtonHolder");
            GameObject targetButtonParent = buttonParent != null ? buttonParent : buttonHolder; 
            
            if (buttonParent == null)
            {
                 buttonHolder.transform.SetParent(levelMapParent.transform, false);
                 RectTransform bhRect = buttonHolder.AddComponent<RectTransform>();
                 // Original: 62. New: Shift down.
                 bhRect.anchoredPosition = new Vector2(0, 62f - verticalOffset); 
                 targetButtonParent = buttonHolder;
            }

            // 2. Place BOTTOM Sprite
            if (bottomSprite != null)
            {
                GameObject bottomObj = new GameObject("Bottom");
                bottomObj.transform.SetParent(mapHolder.transform, false);
                RectTransform botRect = bottomObj.AddComponent<RectTransform>();
                botRect.anchoredPosition = new Vector2(0, originalBottomLocalY); 
                botRect.sizeDelta = new Vector2(1440, 2382);
                Image botImg = bottomObj.AddComponent<Image>();
                botImg.sprite = bottomSprite;
            }

            // 3. Place Level Maps
            float currentLocalY = originalMap1LocalY; 
            foreach (var sprite in fullSequence)
            {
                if (sprite == null) continue;
                GameObject bgObj = new GameObject(sprite.name);
                bgObj.transform.SetParent(mapHolder.transform, false);
                RectTransform bgRect = bgObj.AddComponent<RectTransform>();
                bgRect.sizeDelta = new Vector2(1440, backgroundHeight); 
                bgRect.anchoredPosition = new Vector2(0, currentLocalY);
                Image bgImage = bgObj.AddComponent<Image>();
                bgImage.sprite = sprite;
                currentLocalY += backgroundHeight;
            }
            
            // 4. Generate Clouds
            int cyclesNeeded = Mathf.CeilToInt(fullSequence.Count / 5.0f);
            float cycleHeight = 17500f; 

            for (int c = 0; c < cyclesNeeded; c++)
            {
                float cycleOffsetY = c * cycleHeight;
                if (cloudPrefabs.Count > 0)
                {
                    foreach(var cData in cloudPositions)
                    {
                        GameObject prefab = cloudPrefabs.FirstOrDefault(p => p.name == cData.name);
                        if (prefab == null) continue;
                        GameObject cloudObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        cloudObj.transform.SetParent(animHolder.transform, false);
                        RectTransform rt = cloudObj.GetComponent<RectTransform>();
                        if (rt != null) rt.anchoredPosition = new Vector2(cData.pos.x, cData.pos.y + cycleOffsetY);
                    }
                }
            }
            
            // 5. Resize CONTENT
            RectTransform parentRect = levelMapParent.GetComponent<RectTransform>();
            if (parentRect != null)
            {
                parentRect.anchorMin = new Vector2(0.5f, 0.5f);
                parentRect.anchorMax = new Vector2(0.5f, 0.5f);
                parentRect.pivot = new Vector2(0.5f, 0.5f);
                parentRect.sizeDelta = new Vector2(1440f, finalContentHeight);
            }

            // 6. Generate Buttons
            int levelsToCreate = fullSequence.Count * 20;

            for (int i = 0; i < levelsToCreate; i++)
            {
                int currentLevel = startLevel + i;
                int normalizedIndex = (currentLevel - 1);
                int templateIndex = normalizedIndex % 100;
                int cycle = normalizedIndex / 100;

                if (templateIndex >= levelPositions.Length) continue;

                Vector2 templatePos = levelPositions[templateIndex];
                float finalY = templatePos.y + (cycle * cycleHeight);
        
                GameObject btn = (GameObject)PrefabUtility.InstantiatePrefab(buttonPrefab);
                btn.transform.SetParent(targetButtonParent.transform, false);
                btn.name = "LevelMapButton"; 

                LevelButton lb = btn.GetComponent<LevelButton>();
                if (lb != null)
                {
                    lb.numLevel = currentLevel;
                    var textComponent = lb.GetComponentInChildren<Text>();
                    if (textComponent != null) textComponent.text = currentLevel.ToString();
                }

                RectTransform btnRect = btn.GetComponent<RectTransform>();
                btnRect.anchoredPosition = new Vector2(templatePos.x, finalY);
            }

            RemoveDuplicateNamedObjects("LevelMapEnd");
            
            // RESET VIEW TO BOTTOM (Level 1)
            // Center is 0. Bottom is -Height/2. To see bottom, we move Content UP by +Height/2.
            if (parentRect != null)
            {
               parentRect.anchoredPosition = new Vector2(0, finalContentHeight / 2f);
            }

            EditorUtility.DisplayDialog("Success", $"Generated EXACT REPLICA for levels {startLevel} to {startLevel + levelsToCreate - 1}.\nAligned to Bottom.", "OK");
        }

        private void RemoveDuplicateNamedObjects(string name)
        {
             if (levelMapParent == null) return;
             var children = new List<Transform>();
             foreach (Transform child in levelMapParent.transform) { if (child.name == name) children.Add(child); }
             if (children.Count > 1) { for (int i = 0; i < children.Count - 1; i++) DestroyImmediate(children[i].gameObject); }
        }

        private void MoveElementToEnd(string name, float yPos)
        {
            if (levelMapParent == null) return;
            Transform t = levelMapParent.transform.Find(name);
            if (t != null)
            {
                RectTransform rt = t.GetComponent<RectTransform>();
                if (rt != null) { rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yPos); t.SetAsLastSibling(); }
            }
        }
    }
}
