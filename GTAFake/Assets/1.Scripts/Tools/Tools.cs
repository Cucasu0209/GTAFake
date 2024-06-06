
using AmplifyImpostors;
using System;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;



public class Testeditor : EditorWindow
{
    public GameObject Parent;
    public GameObject Parent2;
    private List<GameObject> children;
    AmplifyImpostor m_instance;
    private AmplifyImpostorAsset m_currentData;
    private List<AmplifyImpostor> m_children;


    public GameObject prefabA;
    public GameObject prefabB;

    public int c;
    public int r;
    [MenuItem("Tools/MyTool")]
    public static void OpenWindow()
    {
        Testeditor window = (Testeditor)GetWindow(typeof(Testeditor));
        window.minSize = new UnityEngine.Vector2(600, 1000);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Add children");
        Parent = (GameObject)EditorGUILayout.ObjectField(Parent, typeof(GameObject), true);

        // Hiển thị danh sách GameObjects
        if (GUILayout.Button("Add"))
        {
            children = new List<GameObject>();
            foreach (var chil in Parent.GetComponentsInChildren<Transform>())
            {
                if (chil.parent == Parent.transform)
                    children.Add(chil.gameObject);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Parent 2");
        Parent2 = (GameObject)EditorGUILayout.ObjectField(Parent2, typeof(GameObject), true);
        GUILayout.EndHorizontal();


        var buttonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold, fixedHeight = 30 };






        if (GUILayout.Button("Change Name ANd Add", buttonStyle))
        {
            foreach (var obj in children)
            {
                Debug.Log("???????");
                AmplifyImpostor instance = obj.GetComponent<AmplifyImpostor>();
                if (instance == null)
                {
                    obj.AddComponent<AmplifyImpostor>();
                }
                if (obj.GetComponent<Animator>() != null)
                {
                    DestroyImmediate(obj.GetComponent<Animator>());
                }

                GameObject child;
                if (obj.transform.childCount <= 0)
                {
                    child = Instantiate(new GameObject(obj.name), obj.transform);
                    //Debug.Log("???????");
                }
                else
                {
                    child = obj.transform.GetChild(0).gameObject;
                    child.name = obj.name;
                }

                if (child.GetComponent<LODGroup>() == null)
                {
                    child.AddComponent<LODGroup>();
                }
                child.transform.parent = obj.transform.parent;
                child.transform.localScale = Vector3.one;
                obj.transform.parent = child.transform;
                child.name = obj.name;
            }
        }

        if (GUILayout.Button("Rename", buttonStyle))
        {
            foreach (var obj in children)
            {
                obj.transform.GetChild(0).gameObject.name = obj.name + "_LOD0";
                obj.transform.GetChild(1).gameObject.name = obj.name + "_LOD1";
            }
        }


        if (GUILayout.Button("ReBake", buttonStyle))
        {
            if (children.Count > 0)
            {
                AmplifyImpostor impostor = children[0].transform.GetChild(0).GetComponent<AmplifyImpostor>();
                Debug.Log(impostor != null);
                impostor.Data.TexSize = new Vector2(2048, 2048);

                try
                {
                    impostor.RenderAllDeferredGroups(impostor.Data);
                }
                catch (Exception e)
                {
                    EditorUtility.ClearProgressBar();
                    Debug.LogWarning("[AmplifyImpostors] Something went wrong with the baking process, please contact support@amplify.pt with this log message.\n" + e.Message + e.StackTrace);
                }
                children.RemoveAt(0);
            }
        }

        if (GUILayout.Button("GetString", buttonStyle))
        {
            string a = "";
            foreach (var obj in children)
            {

                LODGroup Lg = obj.GetComponent<LODGroup>();
                if (Lg != null)
                {
                    int[] tris = new int[2] { 0, 0 };
                    int[] verts = new int[2] { 0, 0 };
                    for (int i = 0; i <= 1; i++)
                    {
                        MeshRenderer meshRenderer = ((MeshRenderer)Lg.GetLODs()[i].renderers[0]);

                        // Kiểm tra nếu có MeshRenderer
                        if (meshRenderer != null)
                        {
                            // Lấy MeshFilter từ GameObject
                            MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();

                            // Kiểm tra nếu có MeshFilter
                            if (meshFilter != null)
                            {
                                // Lấy Mesh từ MeshFilter
                                Mesh mesh = meshFilter.mesh;

                                // Lấy vertices từ Mesh
                                Vector3[] vertices = mesh.vertices;

                                // Lấy triangles từ Mesh
                                int[] triangles = mesh.triangles;

                                // In thông tin vertices và triangles ra console
                                tris[i] = triangles.Length / 3;
                                verts[i] = vertices.Length;

                            }
                            else
                            {
                                Debug.LogError("MeshFilter không tồn tại trên GameObject này.");
                            }
                        }
                    }


                    a += $"{obj.name}\t{tris[0]} - {verts[0]}\t{tris[1]} - {verts[1]}\t{(int)Lg.size}\t_{Lg.GetLODs()[0].screenRelativeTransitionHeight * 100}%\t_{Lg.GetLODs()[1].screenRelativeTransitionHeight * 100}%\n";
                }
                else if (obj.GetComponent<MeshRenderer>() != null)
                {
                    MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                    int tris = 0;
                    int verts = 0;
                    // Kiểm tra nếu có MeshRenderer
                    if (meshRenderer != null)
                    {
                        // Lấy MeshFilter từ GameObject
                        MeshFilter meshFilter = meshRenderer.GetComponent<MeshFilter>();

                        // Kiểm tra nếu có MeshFilter
                        if (meshFilter != null)
                        {
                            // Lấy Mesh từ MeshFilter
                            Mesh mesh = meshFilter.mesh;

                            // Lấy vertices từ Mesh
                            Vector3[] vertices = mesh.vertices;

                            // Lấy triangles từ Mesh
                            int[] triangles = mesh.triangles;

                            // In thông tin vertices và triangles ra console
                            tris = triangles.Length / 3;
                            verts = vertices.Length;

                        }
                        else
                        {
                            Debug.LogError("MeshFilter không tồn tại trên GameObject này.");
                        }
                    }
                    a += $"{obj.name}\t{tris} - {verts}\n";
                }
            }
            Debug.Log(a);
        }










        //if (GUILayout.Button("1.Add Component Amplify imsposter", buttonStyle))
        //{
        //    foreach (var obj in children)
        //    {
        //        AmplifyImpostor instance = obj.GetComponent<AmplifyImpostor>();
        //        if (instance == null)
        //        {
        //            obj.AddComponent<AmplifyImpostor>();
        //        }
        //    }
        //}
        //if (GUILayout.Button("2.Create folder", buttonStyle))
        //{
        //    foreach (var obj in children)
        //    {
        //        AmplifyImpostor instance = obj.GetComponent<AmplifyImpostor>();

        //        string folderPath = Application.dataPath + "/AmplifyImposter/" + obj.name + "_Imposter";
        //        string fileName = instance.name + "_Impostor";
        //        if (!string.IsNullOrEmpty(instance.m_impostorName))
        //            fileName = instance.m_impostorName;
        //        if (!Directory.Exists(folderPath))
        //        {
        //            Directory.CreateDirectory(folderPath);
        //            Debug.Log("Đã tạo thư mục mới: " + folderPath);
        //            if (!string.IsNullOrEmpty(fileName))
        //            {
        //                folderPath = Path.GetDirectoryName(folderPath).Replace("\\", "/");
        //                if (!string.IsNullOrEmpty(folderPath))
        //                {
        //                    if (!Preferences.GlobalDefaultMode)
        //                    {
        //                        instance.m_folderPath = folderPath;
        //                    }
        //                    else
        //                    {
        //                        Preferences.GlobalFolder = folderPath;
        //                        EditorPrefs.SetString(Preferences.PrefGlobalFolder, Preferences.GlobalFolder);
        //                    }
        //                    instance.m_impostorName = fileName;
        //                }
        //            }

        //        }
        //    }

        //}

        //if (GUILayout.Button("3.Create Imposter Data", buttonStyle))
        //{
        //    foreach (var obj in children)
        //    {

        //        AmplifyImpostor instance = obj.GetComponent<AmplifyImpostor>();
        //        if (instance.Data != null)
        //        {
        //            instance.Data.ImpostorType = ImpostorType.Spherical;
        //        }
        //        else
        //        {
        //            string folderPath = "Assets/AmplifyImposter/" + obj.name + "_Imposter/";
        //            instance.m_impostorName = obj.name + "_Imposter";
        //            instance.RootTransform = obj.transform;
        //            //instance.CreateAssetFile(null, folderPath);
        //            instance.Data.ImpostorType = ImpostorType.Spherical;
        //        }

        //    }
        //}
        //if (GUILayout.Button("4.Bake", buttonStyle))
        //{

        //    Debug.Log(m_children.Count);
        //    DelayedBake();
        //}
        //if (GUILayout.Button("5.Create Prefab", buttonStyle))
        //{
        //    foreach (var obj in Parent2.GetComponentsInChildren<LODGroup>())
        //    {
        //        PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(obj);

        //        if (prefabType != PrefabAssetType.Regular)
        //        {
        //            // Chọn đường dẫn lưu Prefab
        //            string prefabPath = Application.dataPath + "/AmplifyImposter/" + obj.name + "_Imposter/" + obj.name + ".prefab";

        //            // Kiểm tra xem Prefab đã tồn tại chưa
        //            GameObject existingPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        //            if (existingPrefab == null)
        //            {
        //                // Tạo Prefab từ GameObject được chọn
        //                PrefabUtility.SaveAsPrefabAsset(obj.gameObject, prefabPath);

        //                Debug.Log("Prefab đã được tạo tại đường dẫn: " + prefabPath);
        //            }
        //            else
        //            {
        //                Debug.LogWarning("Prefab với tên " + obj.name + " đã tồn tại tại đường dẫn: " + prefabPath);
        //            }
        //        }
        //    }

        //}

        GUILayout.BeginHorizontal();
        GUILayout.Label("PrefabA");
        prefabA = (GameObject)EditorGUILayout.ObjectField(prefabA, typeof(GameObject), true);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("PrefabB");
        prefabB = (GameObject)EditorGUILayout.ObjectField(prefabB, typeof(GameObject), true);
        GUILayout.EndHorizontal();


        if (GUILayout.Button("Replace", buttonStyle))
        {
            ReplacePrefab();
        }
        if (GUILayout.Button("Create Square", buttonStyle))
        {
            int number = 100;
            foreach (var a in Parent2.GetComponentsInChildren<Transform>())
            {
                if (a.parent == Parent2.transform)
                {
                    DestroyImmediate(a.gameObject);
                }
            }

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < number; j++)
                {
                    GameObject a = Instantiate(((i + j) % 2 == 0) ? prefabA : prefabB, Parent2.transform);
                    a.transform.localScale = Vector3.one / number;
                    a.transform.localPosition = new Vector3((i + 0.5f) / number - 0.5f, 1, (j + 0.5f) / number - 0.5f);
                }
            }
        }
    }
    void DelayedBake()
    {
        children = new List<GameObject>();
        foreach (var chil in Parent.GetComponentsInChildren<Transform>())
        {
            if (chil.parent == Parent.transform)
                children.Add(chil.gameObject);
            break;
        }
        m_children = new List<AmplifyImpostor>();

        foreach (var obj in children)
        {

            AmplifyImpostor instance = obj.GetComponent<AmplifyImpostor>();
            if (instance.Data != null)
            {
                m_children
                    .Add(instance);
            }
        }
        if (m_children.Count <= 0)
        {
            return;
        }

        m_instance = m_children[0];
        m_currentData = m_instance.Data;


        try
        {
            m_instance.RenderAllDeferredGroups(m_currentData);
        }
        catch (Exception e)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogWarning("[AmplifyImpostors] Something went wrong with the baking process, please contact support@amplify.pt with this log message.\n" + e.Message + e.StackTrace);
        }

        bool createLodGroup = true;
        //group = m_instance.RootTransform.GetComponentInChildren<LODGroup>();
        //if (Preferences.GlobalCreateLodGroup)
        //{
        //    LODGroup group = m_instance.RootTransform.GetComponentInParent<LODGroup>();
        //    if (group == null)
        //        group = m_instance.RootTransform.GetComponentInChildren<LODGroup>();
        //    if (group == null && m_instance.LodGroup == null)
        //        createLodGroup = true;
        //}

        if (createLodGroup && m_instance.m_lastImpostor != null)
        {
            GameObject lodgo = new GameObject(m_instance.name);
            LODGroup lodGroup = lodgo.AddComponent<LODGroup>();
            lodGroup.transform.position = m_instance.transform.position;
            int hierIndex = m_instance.transform.GetSiblingIndex();

            m_instance.transform.SetParent(lodGroup.transform, true);
            m_instance.m_lastImpostor.transform.SetParent(lodGroup.transform, true);
            LOD[] lods = lodGroup.GetLODs();
            ArrayUtility.RemoveAt<LOD>(ref lods, 2);
            lods[0].fadeTransitionWidth = 0.5f;
            lods[0].screenRelativeTransitionHeight = 0.15f;
            lods[0].renderers = m_instance.RootTransform.GetComponentsInChildren<Renderer>();
            lods[1].fadeTransitionWidth = 0.5f;
            lods[1].screenRelativeTransitionHeight = 0.01f;
            lods[1].renderers = m_instance.m_lastImpostor.GetComponentsInChildren<Renderer>();
            lodGroup.fadeMode = LODFadeMode.CrossFade;
            lodGroup.animateCrossFading = true;
            lodGroup.SetLODs(lods);
            lodgo.transform.SetSiblingIndex(hierIndex);
            lodgo.transform.parent = Parent2.transform;
            //if (m_children.Count > 0)
            //{

            //    DelayedBake();
            //}
        }




    }


    void ReplacePrefab()
    {

        if (prefabA != null && prefabB != null)
        {
            // Tìm tất cả các GameObjects trong Scene có Prefab A như là Prefab Instance
            GameObject[] sceneObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in sceneObjects)
            {
                if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.Connected &&
                    PrefabUtility.GetCorrespondingObjectFromSource(obj) == prefabA)
                {
                    // Lấy thông tin vị trí, quay, tỷ lệ và cha mẹ của GameObject A
                    Transform objTransform = obj.transform;
                    Vector3 position = objTransform.position;
                    Quaternion rotation = objTransform.rotation;
                    Vector3 scale = objTransform.localScale;
                    Transform parent = objTransform.parent;

                    // Xóa GameObject A khỏi Scene
                    DestroyImmediate(obj);

                    // Tạo một Prefab Instance mới từ Prefab B với thông tin của GameObject A
                    GameObject newObject = PrefabUtility.InstantiatePrefab(prefabB) as GameObject;
                    newObject.transform.position = position;
                    newObject.transform.rotation = rotation;
                    newObject.transform.localScale = scale;
                    newObject.transform.SetParent(parent);

                    Debug.Log("Prefab A đã được thay thế bằng Prefab B trong Scene!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Vui lòng chọn cả Prefab A và Prefab B để thực hiện thay thế!");
        }
    }

}
#endif