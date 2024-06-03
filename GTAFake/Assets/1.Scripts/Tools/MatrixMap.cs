using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MatrixMap : MonoBehaviour
{
    #region Variables
    //pure data
    public float AreaSize = 13;
    [HideInInspector] public int column = 1;
    [HideInInspector] public int row = 1;
    [HideInInspector]
    public Vector3[] c = new Vector3[4] {
        new Vector3(0,0,0),
        new Vector3(0,0,13),
        new Vector3(13,0,13),
        new Vector3(13,0,0)
    };
    int MAXCEL = 100;

    //data using when setup
    private string data;
    Dictionary<string, bool> CellMarked = new Dictionary<string, bool>();
    private Vector3 lastdownleft = Vector3.one * 99;
    private float lastSize = -1;

    //data using to save file
    private string RealData;
    private string filePath;
    #endregion

    #region Mono
    private void OnEnable()
    {
        filePath = System.IO.Path.Combine(Application.dataPath, "data.txt");
    }
    #endregion

    #region public event called to setup
    public void UpdateSquare(Vector3[] _c, bool recall = false)
    {
        LoadCellMark();

        int i;
        row = Mathf.Abs(Mathf.RoundToInt((Getdownleft().x - GetupRight().x) / AreaSize));
        column = Mathf.Abs(Mathf.RoundToInt((Getdownleft().z - GetupRight().z) / AreaSize));
        for (i = 0; i < c.Length; i++)
        {
            _c[i] = new Vector3(_c[i].x, 0, _c[i].z);
            if (Vector3.Distance(_c[i], c[i]) > 0.01f)
            {


                if (row > MAXCEL)
                {
                    AreaSize = (_c[i].x - c[(i + 2) % 4].x) / MAXCEL;
                    row = MAXCEL;
                }
                if (column > MAXCEL)
                {
                    AreaSize = (_c[i].z - c[(i + 2) % 4].z) / MAXCEL;
                    column = MAXCEL;
                }
                c[i].x = c[(i + 2) % 4].x + Mathf.RoundToInt((_c[i].x - c[(i + 2) % 4].x) / AreaSize) * AreaSize;
                c[i].z = c[(i + 2) % 4].z + Mathf.RoundToInt((_c[i].z - c[(i + 2) % 4].z) / AreaSize) * AreaSize;


                if (c[(i + 1) % 4].z != c[(i + 2) % 4].z)
                {
                    c[(i + 1) % 4].z = c[i].z;
                    c[(i + 3) % 4].x = c[i].x;
                }
                else
                {
                    c[(i + 1) % 4].x = c[i].x;
                    c[(i + 3) % 4].z = c[i].z;
                }
                return;
            }
        }

        i = 0;
        if (recall)
        {
            if (row > MAXCEL)
            {
                AreaSize = (_c[i].x - c[(i + 2) % 4].x) / MAXCEL;
                row = MAXCEL;
            }
            if (column > MAXCEL)
            {
                AreaSize = (_c[i].z - c[(i + 2) % 4].z) / MAXCEL;
                column = MAXCEL;
            }
            c[i].x = c[(i + 2) % 4].x + Mathf.RoundToInt((_c[i].x - c[(i + 2) % 4].x) / AreaSize) * AreaSize;
            c[i].z = c[(i + 2) % 4].z + Mathf.RoundToInt((_c[i].z - c[(i + 2) % 4].z) / AreaSize) * AreaSize;


            if (c[(i + 1) % 4].z != c[(i + 2) % 4].z)
            {
                c[(i + 1) % 4].z = c[i].z;
                c[(i + 3) % 4].x = c[i].x;
            }
            else
            {
                c[(i + 1) % 4].x = c[i].x;
                c[(i + 3) % 4].z = c[i].z;
            }
        }
    }
    public bool isMarked(int i, int j)
    {
        return CellMarked.ContainsKey(getKey(i, j));
    }
    public void MarkCell(int i, int j)
    {
        if (CellMarked.ContainsKey(getKey(i, j)))
        {
            CellMarked.Remove(getKey(i, j));
        }
        else
        {
            CellMarked.Add(getKey(i, j), true);
        }
        SaveCellMark();
    }

    #endregion

    #region Get Infomation about grid
    private string getKey(int i, int j) => i + "_" + j;
    private Vector2Int getKey(string i)
    {
        string[] a = i.Split('_');
        return new Vector2Int(int.Parse(a[0]), int.Parse(a[1]));
    }
    private Vector2 GetCenter(int i, int j) => new Vector2(Getdownleft().x + (i + 0.5f) * AreaSize, Getdownleft().z + (j + 0.5f) * AreaSize);
    public Vector3 Getupleft() => new Vector3(Mathf.Max(c[1].x, c[3].x), 0, Mathf.Min(c[1].z, c[3].z));
    public Vector3 GetupRight() => new Vector3(Mathf.Max(c[1].x, c[3].x), 0, Mathf.Max(c[1].z, c[3].z));
    public Vector3 GetdownRight() => new Vector3(Mathf.Min(c[1].x, c[3].x), 0, Mathf.Max(c[1].z, c[3].z));
    public Vector3 Getdownleft() => new Vector3(Mathf.Min(c[1].x, c[3].x), 0, Mathf.Min(c[1].z, c[3].z));
    #endregion

    #region save and load using on seting up
    private void SaveCellMark()
    {
        string result = "";

        foreach (var key in CellMarked.Keys)
        {
            SquareInfo sq = new SquareInfo()
            {
                center = GetCenter(getKey(key).x, getKey(key).y),
                size = AreaSize
            };
            result += sq.ToString() + "_";
        }

        //PlayerPrefs.SetString("Marked", result);
        data = result;

    }
    private void LoadCellMark()
    {
        if (Vector3.Distance(lastdownleft, Getdownleft()) < 0.1f && Mathf.Abs(lastSize - AreaSize) < 0.1f) return;
        lastdownleft = Getdownleft();
        lastSize = AreaSize;
        CellMarked = new Dictionary<string, bool>();
        //string result = PlayerPrefs.GetString("Marked", "");
        string result = data;
        if (data != null)
        {
            string[] keys = result.Split('_');
            SquareInfo newSq;
            if (keys != null && keys.Length > 0)
                for (int i = 0; i < keys.Length; i++)
                {
                    newSq = SquareInfo.GetInfoByString(keys[i]);
                    if (newSq != null)
                    {
                        List<Vector2Int> IndexMArked = newSq.getSqsWrapedInBigSquare(new Vector2(Getdownleft().x, Getdownleft().z), AreaSize);
                        foreach (var index in IndexMArked)
                        {
                            if (CellMarked.ContainsKey(getKey(index.x, index.y)) == false)
                                CellMarked.Add(getKey(index.x, index.y), true);
                        }
                    }
                }
        }

    }
    public void ClearAllCellMarked()
    {
        data = "";
        CellMarked.Clear();
        //PlayerPrefs.SetString("Marked", "");
    }
    #endregion

    #region Save and laod file
    public void GetDataFromFile()
    {
        MapInfo inf = new MapInfo();
        inf.Size = AreaSize;
        inf.MaxX = GetupRight().x;
        inf.MaxZ = GetupRight().z;
        inf.MinX = Getdownleft().x;
        inf.MinZ = Getdownleft().z;
        inf.SpawnableCellx = new List<int>();
        inf.SpawnableCelly = new List<int>();
        int x, y;
        foreach (var key in CellMarked.Keys)
        {
            x = getKey(key).x;
            y = getKey(key).y;
            if (y < 0 || x < 0 || x >= row || y >= column) continue;
            inf.SpawnableCellx.Add(getKey(key).x);
            inf.SpawnableCelly.Add(getKey(key).y);
        }
        RealData = inf.ToString();
        SaveStringToFile(RealData);
    }
    public MapInfo LoadMapSaved()
    {
        return MapInfo.ParseData(RealData);
    }
    public void LoadDataFromFile()
    {
        RealData = ReadStringFromFile();
        MapInfo mapInfo = MapInfo.ParseData(RealData);

        c[0] = new Vector3(mapInfo.MinX, 0, mapInfo.MaxZ);
        c[1] = new Vector3(mapInfo.MaxX, 0, mapInfo.MaxZ);
        c[2] = new Vector3(mapInfo.MaxX, 0, mapInfo.MinZ);
        c[3] = new Vector3(mapInfo.MinX, 0, mapInfo.MinZ);
        AreaSize = mapInfo.Size;

        CellMarked = new Dictionary<string, bool>();
        for (int i = 0; i < mapInfo.SpawnableCellx.Count; i++)
        {
            if (CellMarked.ContainsKey(getKey(mapInfo.SpawnableCellx[i], mapInfo.SpawnableCelly[i])))
            {
                CellMarked.Remove(getKey(mapInfo.SpawnableCellx[i], mapInfo.SpawnableCelly[i]));
            }
            else
            {
                CellMarked.Add(getKey(mapInfo.SpawnableCellx[i], mapInfo.SpawnableCelly[i]), true);
            }
        }
        SaveCellMark();
        UpdateSquare(c, true);
    }
    private void SaveStringToFile(string content)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false)) // false để ghi đè lên file nếu tồn tại
            {
                writer.WriteLine(content);
                Debug.Log("File written successfully.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing to file: " + ex.Message);
        }
    }
    private string ReadStringFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string content = reader.ReadToEnd();
                    Debug.Log("File read successfully.");
                    return content;
                }
            }
            else
            {
                Debug.LogWarning("File does not exist.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error reading from file: " + ex.Message);
            return null;
        }
    }
    #endregion
}
public class SquareInfo
{
    public Vector2 center;
    public float size;
    public override string ToString()
    {
        return center.x + "," + center.y + "," + size;
    }

    public static SquareInfo GetInfoByString(string data)
    {
        string[] pieces = data.Split(',');
        if (pieces.Length >= 3)
        {
            float centerx = float.Parse(pieces[0]);
            float centery = float.Parse(pieces[1]);
            float sizea = float.Parse(pieces[2]);
            return new SquareInfo()
            {
                center = new Vector2(centerx, centery),
                size = sizea
            };
        }
        return null;
    }
    public List<Vector2Int> getSqsWrapedInBigSquare(Vector2 topleftBig, float sizePiece)
    {
        int maxX = Mathf.CeilToInt((center.x + size / 2 - topleftBig.x) / sizePiece - 0.1f);
        int minX = Mathf.FloorToInt((center.x - size / 2 - topleftBig.x) / sizePiece + 0.1f);

        int maxY = Mathf.CeilToInt((center.y + size / 2 - topleftBig.y) / sizePiece - 0.1f);
        int minY = Mathf.FloorToInt((center.y - size / 2 - topleftBig.y) / sizePiece + 0.1f);

        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
                result.Add(new Vector2Int(i, j));
        }
        return result;
    }
}

[Serializable]
public class MapInfo
{
    public float MaxX;
    public float MinX;
    public float MaxZ;
    public float MinZ;
    public float Size;

    public List<int> SpawnableCellx;
    public List<int> SpawnableCelly;
    public void SaveData()
    {

    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
    public static MapInfo ParseData(string data)
    {
        MapInfo info = JsonConvert.DeserializeObject<MapInfo>(data);
        return info;
    }
    public int GetRow() => Mathf.RoundToInt((MaxX - MinX) / Size);
    public int GetColumn() => Mathf.RoundToInt((MaxZ - MinZ) / Size);
    public string GetIllusiveGrid()
    {
        string result = "";
        List<string> marked= new List<string>();

        for (int i = 0; i < GetRow(); i++)
        {
            for (int j = 0; j < GetColumn(); j++)
            {

            }
        }
        return "";
    }
}