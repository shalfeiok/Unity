using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Editor
{
    [InitializeOnLoad]
    public static class BootstrapSceneCreator
    {
        private const string ScenePath = "Assets/Scenes/Bootstrap.unity";

        static BootstrapSceneCreator()
        {
            EditorApplication.delayCall += EnsureBootstrapScene;
        }

        private static void EnsureBootstrapScene()
        {
            if (System.IO.File.Exists(ScenePath))
            {
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var bootstrap = new GameObject("Bootstrap");
            var gameRoot = new GameObject("GameRoot");

            var uiRoot = new GameObject("UIRoot");
            var canvas = uiRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiRoot.AddComponent<CanvasScaler>();
            uiRoot.AddComponent<GraphicRaycaster>();

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();

            SceneManager.MoveGameObjectToScene(bootstrap, scene);
            SceneManager.MoveGameObjectToScene(gameRoot, scene);
            SceneManager.MoveGameObjectToScene(uiRoot, scene);
            SceneManager.MoveGameObjectToScene(eventSystemObject, scene);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log("Created Bootstrap scene at Assets/Scenes/Bootstrap.unity");
        }
    }
}
