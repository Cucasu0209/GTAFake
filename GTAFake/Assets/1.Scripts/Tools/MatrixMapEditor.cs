using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MatrixMap)), CanEditMultipleObjects]
public class MatrixMapEditor : Editor
{
    MatrixMap example;
    protected virtual void OnSceneGUI()
    {


        example = (MatrixMap)target;

        float size = HandleUtility.GetHandleSize(example.c[0]) * 0.1f;
        Vector3 snap = Vector3.one * 0.5f;

        EditorGUI.BeginChangeCheck();
        Vector3 c1 = Handles.FreeMoveHandle(example.c[0], size, snap, Handles.SphereHandleCap);
        Vector3 c2 = Handles.FreeMoveHandle(example.c[1], size, snap, Handles.SphereHandleCap);
        Vector3 c3 = Handles.FreeMoveHandle(example.c[2], size, snap, Handles.SphereHandleCap);
        Vector3 c4 = Handles.FreeMoveHandle(example.c[3], size, snap, Handles.SphereHandleCap);

        for (int i = 0; i <= example.column; i++)
        {
            Handles.color = Color.black;
            Handles.DrawLine((example.Getupleft() * i + example.GetupRight() * (example.column - i)) / example.column,
                        (example.Getdownleft() * i + example.GetdownRight() * (example.column - i)) / example.column);
        }
        for (int i = 0; i <= example.row; i++)
        {
            Handles.DrawLine((example.Getupleft() * i + example.Getdownleft() * (example.row - i)) / example.row,
                (example.GetupRight() * i + example.GetdownRight() * (example.row - i)) / example.row);
        }

        Vector3 downleft = new Vector3(Mathf.Min(example.c[1].x, example.c[3].x), 0, Mathf.Min(example.c[1].z, example.c[3].z));

        for (int i = 0; i < example.row; i++)
        {
            for (int j = 0; j < example.column; j++)
            {
                if (example.isMarked(i, j)) Handles.color = new Color(0, 1f, 0, 0.15f);
                else Handles.color = ((i + j) % 2 == 0) ? new Color(1, 0, 0, 0.08f) : new Color(1, 0.3f, 0f, 0.08f);
                Vector3 position = new Vector3((Mathf.Abs(example.c[3].x - example.c[1].x) / example.row) * (i + 0.5f), 0,
                    (Mathf.Abs(example.c[3].z - example.c[1].z) / example.column) * (j + 0.5f));


                if (Handles.Button(downleft + position, Quaternion.identity, example.AreaSize, example.AreaSize, Handles.CubeHandleCap))
                {
                    example.MarkCell(i, j);
                    Debug.Log("(" + i + ", " + j + ")");
                }
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
           $"Rows: {mapSaved.GetRow()}\n Columns: {mapSaved.GetColumn()}" +
           $"\n Cell Size:{mapSaved.Size}",
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