using UnityEngine;
using UnityEditor;

public class ObjectCreator : EditorWindow
{
    private GameObject dotObject;
    private int dotCount = -1;

    [MenuItem("Tools/Object Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create a new object", EditorStyles.boldLabel);

        dotObject = EditorGUILayout.ObjectField("dotObject", dotObject, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Create Object"))
        {
            CreateDotObject();
        }
    }
    private void CreateDotObject()
    {
        if (dotObject != null)
        {
            Vector3 spawnPosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            GameObject temp = PrefabUtility.InstantiatePrefab(dotObject) as GameObject;
            temp.name = dotCount.ToString();
            temp.GetComponent<Dot>().Index = dotCount;
            temp.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            Selection.activeObject = temp;
            dotCount++;
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
            CreateDotObject();
            e.Use();
        }
    }
}