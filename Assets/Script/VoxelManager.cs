using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SFB;
public class VoxelManager : MonoBehaviour
{
    public Material voxelMaterial;
    private HashSet<Vector3Int> voxelData = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> voxelObjects = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, Color> voxelColors = new Dictionary<Vector3Int, Color>();
    private Dictionary<Vector3Int, Texture2D> voxelTextures = new Dictionary<Vector3Int, Texture2D>();
    private Mesh cubeMesh;
    void Start()
    {
        cubeMesh = CreateCubeMesh();
    }
    public void AddVoxel(Vector3Int gridPos, Color color, Texture2D texture = null, bool force = false)
    {
        if (voxelData.Contains(gridPos)) return;
        if (!force && gridPos.y != 0 && !HasNeighbor(gridPos)) return;
        voxelData.Add(gridPos);
        voxelColors[gridPos] = color;
        if (texture != null)
            voxelTextures[gridPos] = texture;
        GameObject voxel = new GameObject("Voxel");
        voxel.transform.position = GridToWorld(gridPos);
        voxel.transform.parent = transform;
        MeshFilter meshFilter = voxel.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = voxel.AddComponent<MeshRenderer>();
        BoxCollider boxCollider = voxel.AddComponent<BoxCollider>();
        boxCollider.size = Vector3.one;
        meshFilter.sharedMesh = cubeMesh;
        Material instanceMaterial = new Material(voxelMaterial);
        ApplyColorTexture(instanceMaterial, color, texture);
        meshRenderer.material = instanceMaterial;
        voxelObjects[gridPos] = voxel;
    }
    public void ChangeVoxelMaterial(Vector3Int gridPos, Color newColor, Texture2D newTexture = null)
    {
        if (!voxelData.Contains(gridPos)) return;
        voxelColors[gridPos] = newColor;
        if (newTexture != null)
            voxelTextures[gridPos] = newTexture;
        else
            voxelTextures.Remove(gridPos);
        if (voxelObjects.ContainsKey(gridPos))
        {
            MeshRenderer renderer = voxelObjects[gridPos].GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material newMat = new Material(voxelMaterial);
                ApplyColorTexture(newMat, newColor, newTexture);
                renderer.material = newMat;
            }
        }
    }
    public void RemoveVoxel(Vector3Int gridPos)
    {
        if (!voxelData.Contains(gridPos)) return;
        voxelData.Remove(gridPos);
        voxelColors.Remove(gridPos);
        voxelTextures.Remove(gridPos);
        if (voxelObjects.ContainsKey(gridPos))
        {
            Destroy(voxelObjects[gridPos]);
            voxelObjects.Remove(gridPos);
        }
    }
    bool HasNeighbor(Vector3Int pos)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.right, Vector3Int.left,
            Vector3Int.up, Vector3Int.down,
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };
        foreach (var dir in directions)
        {
            if (voxelData.Contains(pos + dir))
                return true;
        }
        return false;
    }

    public Vector3Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector3Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y - 0.5f),
            Mathf.RoundToInt(worldPos.z)
        );
    }
    public Vector3 GridToWorld(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y + 0.5f, gridPos.z);
    }
    Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] v = new Vector3[]
        {
            new Vector3(-0.5f,-0.5f, 0.5f), new Vector3( 0.5f,-0.5f, 0.5f),
            new Vector3( 0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3( 0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f,-0.5f),
            new Vector3(-0.5f, 0.5f,-0.5f), new Vector3( 0.5f, 0.5f,-0.5f),
            new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f,-0.5f),
            new Vector3( 0.5f,-0.5f, 0.5f), new Vector3( 0.5f,-0.5f,-0.5f),
            new Vector3( 0.5f, 0.5f,-0.5f), new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f), new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3( 0.5f, 0.5f,-0.5f), new Vector3(-0.5f, 0.5f,-0.5f),
            new Vector3(-0.5f,-0.5f,-0.5f), new Vector3( 0.5f,-0.5f,-0.5f),
            new Vector3( 0.5f,-0.5f, 0.5f), new Vector3(-0.5f,-0.5f, 0.5f),
        };
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
            new Vector2(0,0), new Vector2(1,0), new Vector2(1,1), new Vector2(0,1),
        };
        int[] t = new int[]
        {
            0,2,1, 0,3,2,
            4,6,5, 4,7,6,
            8,10,9, 8,11,10,
            12,14,13, 12,15,14,
            16,18,17, 16,19,18,
            20,22,21, 20,23,22
        };
        mesh.vertices = v;
        mesh.uv = uv;
        mesh.triangles = t;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
    [System.Serializable]
    public class VoxelSaveData
    {
        public List<Vector3Int> voxels;
        public List<SerializableColor> colors;
        public List<string> textures;
    }
    [System.Serializable]
    public class SerializableColor
    {
        public float r, g, b, a;

        public SerializableColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }
        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
    public void SaveModel()
    {
    #if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.SaveFilePanel("Save Voxel Model", "", "voxelSave.json", "json");
    #else
    var extensions = new[] { new SFB.ExtensionFilter("JSON", "json") };
    string path = SFB.StandaloneFileBrowser.SaveFilePanel("Save Voxel Model", "", "voxelSave.json", extensions);
    #endif
        if (string.IsNullOrEmpty(path)) return;
        VoxelSaveData saveData = new VoxelSaveData();
        saveData.voxels = new List<Vector3Int>(voxelData);
        saveData.colors = new List<SerializableColor>();
        saveData.textures = new List<string>();
        foreach (Vector3Int pos in saveData.voxels)
        {
            Color color;
            if (voxelColors.ContainsKey(pos))
            {
                color = voxelColors[pos];
            }
            else
            {
                color = Color.white;
            }
            saveData.colors.Add(new SerializableColor(color));

            Texture2D tex;
            if (voxelTextures.ContainsKey(pos))
            {
                tex = voxelTextures[pos];
            }
            else
            {
                tex = null;
            }
            if (tex != null)
            {
                byte[] pngBytes = tex.EncodeToPNG();
                string base64 = System.Convert.ToBase64String(pngBytes);
                saveData.textures.Add(base64);
            }
            else
            {
                saveData.textures.Add("");
            }
        }
        File.WriteAllText(path, JsonUtility.ToJson(saveData, true));
    }

    public void LoadModel()
    {
    #if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.OpenFilePanel("Load Voxel Model", "", "json");
    #else
        var extensions = new[] { new SFB.ExtensionFilter("JSON", "json") };
        string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel("Load Voxel Model", "", extensions, false);
        string path;
        if (paths.Length > 0)
        {
            path = paths[0];
        }
        else
        {
            path = "";
        }
    #endif
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;
        string json = File.ReadAllText(path);
        VoxelSaveData saveData = JsonUtility.FromJson<VoxelSaveData>(json);
        for (int i = 0; i < saveData.voxels.Count; i++)
        {
            Vector3Int pos = saveData.voxels[i];
            Color color;
            if (i < saveData.colors.Count)
            {
                color = saveData.colors[i].ToColor();
            }
            else
            {
                color = Color.white;
            }
            Texture2D tex = null;
            if (i < saveData.textures.Count)
            {
                string texString = saveData.textures[i];
                if (!string.IsNullOrEmpty(texString))
                {
                    byte[] bytes = System.Convert.FromBase64String(texString);
                    tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    tex.LoadImage(bytes);
                    tex.filterMode = FilterMode.Point;
                }
            }
            AddVoxel(pos, color, tex, true);
        }
    }
    void ApplyColorTexture(Material mat, Color color, Texture2D tex)
    {
        if (mat == null) return;
        if (mat.HasProperty("_BaseColor"))
            mat.SetColor("_BaseColor", color);
        else if (mat.HasProperty("_Color"))
            mat.SetColor("_Color", color);
        else
            mat.color = color;
        if (tex != null)
        {
            if (mat.HasProperty("_BaseMap"))
                mat.SetTexture("_BaseMap", tex);
            else if (mat.HasProperty("_MainTex"))
                mat.SetTexture("_MainTex", tex);
            else
                mat.mainTexture = tex;
        }
        else
        {
            if (mat.HasProperty("_BaseMap"))
                mat.SetTexture("_BaseMap", null);
            else if (mat.HasProperty("_MainTex"))
                mat.SetTexture("_MainTex", null);
            else
                mat.mainTexture = null;
        }
    }
}