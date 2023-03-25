using UnityEngine;
using UnityEditor;

public class ObjectCreator : EditorWindow
{
    private GameObject newObject;
    
    [MenuItem("Tools/Object Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectCreator));
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Create a new object", EditorStyles.boldLabel);
        
        newObject = EditorGUILayout.ObjectField("Prefab", newObject, typeof(GameObject), false) as GameObject;
        
        if (GUILayout.Button("Create Object"))
        {
            CreateNewObject();
        }
    }
    
    private void CreateNewObject()
    {
        if (newObject != null)
        {
            Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            GameObject obj = PrefabUtility.InstantiatePrefab(newObject) as GameObject;
            obj.transform.position = spawnPosition;
            Selection.activeObject = obj;
        }
    }
    
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            CreateNewObject();
            e.Use();
        }
    }
}