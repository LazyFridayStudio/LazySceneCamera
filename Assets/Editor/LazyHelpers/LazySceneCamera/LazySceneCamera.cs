//================================================================
//================================================================
//							IMPORTANT
//================================================================
//				 Copyright LazyFridayStudio
// DO NOT SELL THIS CODE OR REDISTRIBUTE WITH INTENT TO SELL.
//
// Send an email to our support line for any questions or inquirys
// CONTACT: admin@lazyfridaystudio.com
//
// Alternatively join our Discord
// DISCORD: https://discord.gg/Z3tpMG
//
// Hope you enjoy the simple lazy Scene Camera 
// designed and made by lazyFridayStudio
//================================================================

#region namespaces

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace LazyHelper.LazySceneCamera
{
    internal enum eDisplayType
    {
        CurrentScene,
        AllScenes
    }

    public class LazySceneCamera : EditorWindow
    {
        private static LazySceneCamera currentWindow;
        private Vector2 scrollArea = Vector2.zero;

        private LazySceneCameraInfo viewInfos;

        private int viewModeSelected = 0;
        private string viewToSaveName = String.Empty;
        private string errorInfo = String.Empty;

        [MenuItem("Window/LazyHelper/Lazy Scene Camera")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            currentWindow = (LazySceneCamera) GetWindow(typeof(LazySceneCamera));
            currentWindow.titleContent.text = "Lazy Scene Camera";
            currentWindow.position = new Rect(0, 0, 600, 800);
            currentWindow.autoRepaintOnSceneChange = false;
        }

        #region Textures

        Texture2D headerBackground;
        Texture2D headerSeperator;

        Texture2D submenuBackground;

        Texture2D itemBackground;

        Texture2D itemOddBackground;
        Texture2D itemEvenBackground;

        #endregion

        #region Styles

        //Heading Style
        public GUIStyle headerStyleText = new GUIStyle();
        public GUIStyle subHeaderStyle = new GUIStyle();
        public GUIStyle generalStyle = new GUIStyle();

        //Padding style
        public GUIStyle stylePadding = new GUIStyle();

        //Background Styles
        public GUIStyle evenBoxStyle = new GUIStyle();
        public GUIStyle oddBoxStyle = new GUIStyle();

        //Font Styles
        public GUIStyle itemTitleStyle = new GUIStyle();
        public GUIStyle errorTitleStyle = new GUIStyle();

        #endregion

        #region Sections

        Rect headerSection;
        Rect subMenuSection;
        Rect itemSection;

        #endregion

        #region Init Methods

        private void OnEnable()
        {
            InitTextures();
            InitStyles();
            CreateAndCheckResources();
        }

        // Draw the textures
        private void InitTextures()
        {
            headerBackground = new Texture2D(1, 1);
            headerBackground.SetPixel(0, 0, new Color32(26, 26, 26, 255));
            headerBackground.Apply();

            headerSeperator = new Texture2D(1, 1);
            headerSeperator.SetPixel(0, 0, new Color32(242, 242, 242, 255));
            headerSeperator.Apply();

            submenuBackground = new Texture2D(1, 1);
            submenuBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            submenuBackground.Apply();

            itemBackground = new Texture2D(1, 1);
            itemBackground.SetPixel(0, 0, new Color32(22, 22, 22, 255));
            itemBackground.Apply();

            itemEvenBackground = new Texture2D(1, 1);
            itemEvenBackground.SetPixel(0, 0, new Color32(44, 44, 44, 255));
            itemEvenBackground.Apply();

            itemOddBackground = new Texture2D(1, 1);
            itemOddBackground.SetPixel(0, 0, new Color32(33, 33, 33, 255));
            itemOddBackground.Apply();
        }

        // Create the styles
        private void InitStyles()
        {
            oddBoxStyle.normal.background = itemOddBackground;
            oddBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            oddBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            evenBoxStyle.normal.background = itemEvenBackground;
            evenBoxStyle.border = new RectOffset(0, 0, 5, 5);
            evenBoxStyle.padding = new RectOffset(3, 3, 3, 3);
            evenBoxStyle.normal.textColor = new Color32(255, 255, 255, 255);

            itemTitleStyle.normal.textColor = new Color32(218, 124, 40, 255);
            itemTitleStyle.fontSize = 14;
            itemTitleStyle.fontStyle = FontStyle.Bold;
            itemTitleStyle.alignment = TextAnchor.MiddleLeft;

            headerStyleText.normal.textColor = new Color32(218, 124, 40, 255);
            headerStyleText.fontSize = 16;
            headerStyleText.alignment = TextAnchor.LowerCenter;

            subHeaderStyle.normal.textColor = new Color32(218, 124, 40, 255);
            subHeaderStyle.fontSize = 14;
            subHeaderStyle.alignment = TextAnchor.LowerCenter;

            generalStyle.normal.textColor = new Color32(255, 255, 255, 255);
            generalStyle.fontSize = 11;

            stylePadding.margin = new RectOffset(2, 2, 4, 4);

            errorTitleStyle.normal.textColor = new Color32(255, 0, 0, 255);
            errorTitleStyle.fontSize = 12;
            errorTitleStyle.alignment = TextAnchor.MiddleLeft;
        }

        #endregion

        #region Drawing Methods

        private void OnGUI()
        {
            if (headerBackground == null)
            {
                OnEnable();
            }

            DrawLayout();
            DrawHeader();
            DrawSubHeading();
            DrawItemAreas();
        }

        private void DrawLayout()
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 25;

            subMenuSection.x = 0;
            subMenuSection.y = headerSection.height;
            subMenuSection.width = Screen.width;
            subMenuSection.height = 27;

            itemSection.x = 0;
            itemSection.y = headerSection.height + subMenuSection.height;
            itemSection.width = Screen.width;
            itemSection.height = Screen.height - (headerSection.height + subMenuSection.height + 15);

            GUI.DrawTexture(headerSection, headerBackground);
            GUI.DrawTexture(subMenuSection, submenuBackground);
            GUI.DrawTexture(itemSection, headerBackground);

            //Draw Seperators
            //GUI.DrawTexture(new Rect(0 - 2, headerSection.height + subMenuSection.height, 2, itemSection.h), headerSeperator);
            GUI.DrawTexture(new Rect(headerSection.x, headerSection.height - 2, headerSection.width, 2), headerSeperator);
            GUI.DrawTexture(new Rect(subMenuSection.x, (subMenuSection.height + headerSection.height) - 2, subMenuSection.width, 2), headerSeperator);
        }

        private void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.BeginHorizontal();
            GUILayout.Label("LAZY SCENE CAMERA", headerStyleText);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawSubHeading()
        {
            GUILayout.BeginArea(subMenuSection);
            GUILayout.BeginHorizontal(stylePadding);

            viewToSaveName = GUILayout.TextField(viewToSaveName, 20, GUILayout.MinWidth(160));

            if (GUILayout.Button("Add View", GUILayout.MaxWidth(100)))
            {
                for (int i = 0; i < viewInfos.viewItems.Count; i++)
                {
                    if (viewToSaveName.Equals(viewInfos.viewItems[i].viewName))
                    {
                        errorInfo = "Name Already Exists";
                        return;
                    }   
                }
                
                errorInfo = String.Empty;
                
                EditorUtility.SetDirty(viewInfos);
                var lastActiveScnee = SceneView.lastActiveSceneView;
                
                var NewViewItem = new LazySceneCameraInfo.ViewItems();
                NewViewItem.viewPosition = lastActiveScnee.pivot;
                NewViewItem.viewRotation = lastActiveScnee.rotation;
                NewViewItem.viewName = viewToSaveName;
                NewViewItem.sceneName = SceneManager.GetActiveScene().name;
                
                viewInfos.viewItems.Add(NewViewItem);
                
                viewToSaveName = String.Empty;
                AssetDatabase.SaveAssets();
            }

            if (!string.IsNullOrEmpty(errorInfo))
            {
                GUILayout.Label(errorInfo, errorTitleStyle);
            }

            GUILayout.FlexibleSpace();

            viewModeSelected = EditorGUILayout.Popup(viewModeSelected, new[] {"Scene Only", "All Scenes"});

            if (GUILayout.Button("?", GUILayout.MaxWidth(25)))
            {
                Application.OpenURL("https://www.lazyfridaystudio.com/lazysceneloader");
            }

            
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawItemAreas()
        {
            GUILayout.Space(headerSection.height + subMenuSection.height);
            GUILayout.BeginArea(itemSection);
            scrollArea = GUILayout.BeginScrollView(scrollArea);
            var currentSceneName = SceneManager.GetActiveScene().name;

            for (int i = 0; i < viewInfos.viewItems.Count; i++)
            {
                if (viewModeSelected == 0)
                {
                    if (currentSceneName.Equals(viewInfos.viewItems[i].sceneName))
                    {
                        DrawItem(i % 2 == 0, viewInfos.viewItems[i]);
                    }
                }
                else
                {
                    DrawItem(i % 2 == 0, viewInfos.viewItems[i]);
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawItem(bool isEven, LazySceneCameraInfo.ViewItems item)
        {
            var itemStyle = new GUIStyle();
            itemStyle = isEven ? evenBoxStyle : oddBoxStyle;
            
            GUILayout.BeginHorizontal(itemStyle);
            // Draw the view Name
            
            GUILayout.Label(item.sceneName + " - " + item.viewName, itemTitleStyle, GUILayout.MaxWidth(100));
            
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Set", GUILayout.MaxHeight(20), GUILayout.MaxWidth(50)))
            {
                errorInfo = String.Empty;
                
                var scene = SceneView.lastActiveSceneView;

                scene.pivot = item.viewPosition;
                scene.rotation = item.viewRotation;
                scene.Repaint();
            }
            
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                EditorUtility.SetDirty(viewInfos);
                
                for (int i = 0; i < viewInfos.viewItems.Count; i++)
                {
                    if (viewInfos.viewItems[i].viewName.Equals(item.viewName))
                    {
                        viewInfos.viewItems.Remove(viewInfos.viewItems[i]);
                    }
                }
                
                AssetDatabase.SaveAssets();
            }
            
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Generation Methods

        private void CreateAndCheckResources()
        {
            if (AssetDatabase.IsValidFolder("Assets/Editor/LazyHelpers/LazySceneLoader/Resources"))
            {
                viewInfos = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneCamera/Resources/ViewItems.asset",
                    typeof(LazySceneCameraInfo)) as LazySceneCameraInfo;

                if (viewInfos != null) return;
                viewInfos = CreateInstance(typeof(LazySceneCameraInfo)) as LazySceneCameraInfo;
                AssetDatabase.CreateAsset(viewInfos, "Assets/Editor/LazyHelpers/LazySceneCamera/Resources/ViewItems.asset");
                GUI.changed = true;
            }
            else
            {
                AssetDatabase.CreateFolder("Assets/Editor/LazyHelpers/LazySceneCamera", "Resources");
                viewInfos = AssetDatabase.LoadAssetAtPath("Assets/Editor/LazyHelpers/LazySceneCamera/Resources/ViewItems.asset",
                    typeof(LazySceneCameraInfo)) as LazySceneCameraInfo;

                if (viewInfos != null) return;
                viewInfos = CreateInstance(typeof(LazySceneCameraInfo)) as LazySceneCameraInfo;
                AssetDatabase.CreateAsset(viewInfos, "Assets/Editor/LazyHelpers/LazySceneCamera/Resources/ViewItems.asset");
                GUI.changed = true;
            }
        }

        #endregion
    }
}