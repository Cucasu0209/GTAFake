using UnityEngine;
using UnityEditor.TerrainTools;
using UnityEditor.PackageManager.UI;



#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MatrixMap)), CanEditMultipleObjects, InitializeOnLoad]
public class MatrixMapEditor : Editor
{

    static MatrixMapEditor()
    {
        if (example != null)
            SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseMove || e.type == EventType.Repaint)
        {
            EditorGUIUtility.AddCursorRect(new Rect(0, 0, sceneView.position.width, sceneView.position.height), MouseCursor.Arrow);
            sceneView.Repaint();
        }
        XArea = new Vector2Int(0, example.row);
        XArea = new Vector2Int(0, example.column);
        if (!sceneView.camera.orthographic)
        {
            return;
        }

        // Lấy kích thước của cửa sổ Scene
        Rect sceneViewRect = sceneView.position;

        // Lấy camera của Scene view
        Camera camera = sceneView.camera;

        // Lấy tọa độ 4 góc của cửa sổ Scene trong screen space
        Vector3 bottomLeftScreen = new Vector3(sceneViewRect.xMin, sceneViewRect.yMin, camera.nearClipPlane);
        Vector3 topRightScreen = new Vector3(sceneViewRect.xMax, sceneViewRect.yMax, camera.nearClipPlane);

        // Chuyển đổi tọa độ từ screen space sang world space
        Vector3 bottomLeftWorld = camera.ScreenToWorldPoint(bottomLeftScreen);
        Vector3 topRightWorld = camera.ScreenToWorldPoint(topRightScreen);

        Vector2Int bottomLeftIndex = example.GetCellIndexByPos(bottomLeftWorld);
        Vector2Int topRightIndex = example.GetCellIndexByPos(topRightWorld);

        XArea = new Vector2Int(bottomLeftIndex.x - 20, topRightIndex.x + 20);
        YArea = new Vector2Int(bottomLeftIndex.y - 20, topRightIndex.y + 20);
    }

    static Vector2Int XArea;
    static Vector2Int YArea;
    static MatrixMap example;
    protected virtual void OnSceneGUI()
    {


        example = (MatrixMap)target;

        float size = HandleUtility.GetHandleSize(example.c[0]) * 0.1f;
        Vector3 snap = Vector3.one * 0.5f;
        int i, j;
        EditorGUI.BeginChangeCheck();
        Vector3 c1 = Handles.FreeMoveHandle(example.c[0], size, snap, Handles.SphereHandleCap);
        Vector3 c2 = Handles.FreeMoveHandle(example.c[1], size, snap, Handles.SphereHandleCap);
        Vector3 c3 = Handles.FreeMoveHandle(example.c[2], size, snap, Handles.SphereHandleCap);
        Vector3 c4 = Handles.FreeMoveHandle(example.c[3], size, snap, Handles.SphereHandleCap);

        for (i = Mathf.Max(0, YArea.x); i <= Mathf.Min(example.column, YArea.y); i++)
        {
            if ((XArea.y - XArea.x) / example.AreaSize > 50 && (i) % 3 != 0) continue;
            if ((XArea.y - XArea.x) / example.AreaSize > 35 && (i) % 2 != 0) continue;
            Handles.color = Color.black;
            Handles.DrawLine(Vector3.up * -200 + (example.Getupleft() * (example.column - i) + example.GetupRight() * i) / example.column,
                      Vector3.up * -200 + (example.Getdownleft() * (example.column - i) + example.GetdownRight() * i) / example.column);
        }
        for (i = Mathf.Max(0, XArea.x); i <= Mathf.Min(example.row, XArea.y); i++)
        {
            if ((XArea.y - XArea.x) / example.AreaSize > 50 && (i) % 3 != 0) continue;
            if ((XArea.y - XArea.x) / example.AreaSize > 35 && (i) % 2 != 0) continue;
            Handles.DrawLine(Vector3.up * -200 + (example.Getupleft() * i + example.Getdownleft() * (example.row - i)) / example.row,
              Vector3.up * -200 + (example.GetupRight() * i + example.GetdownRight() * (example.row - i)) / example.row);
        }

        Vector3 downleft = new Vector3(Mathf.Min(example.c[1].x, example.c[3].x), 0, Mathf.Min(example.c[1].z, example.c[3].z));
        Vector3 position;

        for (i = Mathf.Max(0, XArea.x); i <= Mathf.Min(example.row, XArea.y); i++)
        {
            for (j = Mathf.Max(0, YArea.x); j <= Mathf.Min(example.column, YArea.y); j++)
            {
                if ((XArea.y - XArea.x) / example.AreaSize > 150) continue;
                if ((XArea.y - XArea.x) / example.AreaSize > 100 && (i + j) % 4 != 0) continue;

                if ((XArea.y - XArea.x) / example.AreaSize > 65 && (i + j) % 3 != 0) continue;
                if ((XArea.y - XArea.x) / example.AreaSize > 55 && (i + j) % 3.5f != 0) continue;
                if ((XArea.y - XArea.x) / example.AreaSize > 40 && (i + j) % 2 != 0) continue;
                if ((XArea.y - XArea.x) / example.AreaSize > 25 && (i + j) % 1.5f != 0) continue;


                if (example.isMarked(i, j))
                {
                    position = new Vector3((Mathf.Abs(example.c[3].x - example.c[1].x) / example.row) * (i + 0.5f), 0,
                        (Mathf.Abs(example.c[3].z - example.c[1].z) / example.column) * (j + 0.5f));
                    Handles.color = Color.green;
                    Handles.DrawLine(Vector3.up * -200 + downleft + position + new Vector3(0.5f, 0, 0.5f) * example.AreaSize, Vector3.up * -200 + downleft + position + new Vector3(-0.5f, 0, -0.5f) * example.AreaSize);
                    Handles.DrawLine(Vector3.up * -200 + downleft + position + new Vector3(-0.5f, 0, 0.5f) * example.AreaSize, Vector3.up * -200 + downleft + position + new Vector3(0.5f, 0, -0.5f) * example.AreaSize);
                }
            }
        }

        Event e = Event.current;

        // Kiểm tra nếu sự kiện hiện tại là phím được nhấn xuống
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
        {
            Vector2 mousePosition = e.mousePosition;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePosition);
            Debug.Log("Mouse position in Scene view: " + mousePosition);

            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                Vector3 worldPosition = hitInfo.point;
                Debug.Log("World position: " + worldPosition);
                Vector2Int mouseIndex = example.GetCellIndexByPos(worldPosition);
                example.MarkCell(mouseIndex.x, mouseIndex.y);
            }
        }
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.D)
        {
            Vector2 mousePosition = e.mousePosition;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePosition);
            Debug.Log("Mouse position in Scene view: " + mousePosition);

            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                Vector3 worldPosition = hitInfo.point;
                Debug.Log("World position: " + worldPosition);
                Vector2Int mouseIndex = example.GetCellIndexByPos(worldPosition);
                example.UnmarkCell(mouseIndex.x, mouseIndex.y);
            }
        }



        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            example.UpdateSquare(new Vector3[] { c1, c2, c3, c4 }, true);
        }

    }

    public override void OnInspectorGUI()
    {
        if (example == null) example = (MatrixMap)target;

        DrawDefaultInspector();
        GUILayout.Label("Rows: " + example.row);
        GUILayout.Label("Column: " + example.column);

        if (GUILayout.Button("Show data Saved"))
        {
            MapInfo mapSaved = example.LoadMapSaved();
            EditorUtility.DisplayDialog(
           "Informations",
           $"Rows: {mapSaved.GetRow()}\nColumns: {mapSaved.GetColumn()}" +
           $"\nCell Size:{mapSaved.Size}",
           "Yes");

        }

        if (GUILayout.Button("Clear All"))
        {
            if (EditorUtility.DisplayDialog(
           "Confirmation",
           "Clear all changes? (not save)",
           "Yes",
           "No"))
            {
                example.ClearAllCellMarked();
                example.UpdateSquare(example.c, true);
            }

        }
        if (GUILayout.Button("Load Data"))
        {
            if (EditorUtility.DisplayDialog(
           "Confirmation",
           "Do you want to remove all curent changes and load data from file?",
           "Yes",
           "No"))
            {
                example.LoadDataFromFile();
            }
        }
        if (GUILayout.Button("Save Data"))
        {
            if (EditorUtility.DisplayDialog(
            "Confirmation",
            "Do you want to remove data saved and save new this data?",
            "Yes",
            "No"))
            {
                example.GetDataFromFile();
            }

        }
    }
}



#endif