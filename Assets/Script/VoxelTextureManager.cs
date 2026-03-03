using System.IO;
using UnityEngine;

public class VoxelTextureManager : MonoBehaviour
{
    public Texture2D[] availableTextures = new Texture2D[10];

    private int currentTextureIndex = 0;
    private bool isTextureMode = false;

    string FolderPath => Path.Combine(Application.persistentDataPath, "voxel_textures");

    public bool IsTextureMode() => isTextureMode;
    public void ToggleMode() => isTextureMode = !isTextureMode;

    public Texture2D GetCurrentTexture()
    {
        if (currentTextureIndex >= 0 && currentTextureIndex < availableTextures.Length)
            return availableTextures[currentTextureIndex];
        return null;
    }

    public void SetTextureIndex(int index)
    {
        currentTextureIndex = Mathf.Clamp(index, 0, availableTextures.Length - 1);

        if (availableTextures[currentTextureIndex] != null)
            isTextureMode = true;
    }

    public void NextTexture()
    {
        currentTextureIndex = (currentTextureIndex + 1) % availableTextures.Length;
    }

    public void PreviousTexture()
    {
        currentTextureIndex--;
        if (currentTextureIndex < 0)
            currentTextureIndex = availableTextures.Length - 1;
    }

    void Awake()
    {
        LoadAllSlotsFromDisk();
    }

    void OnApplicationQuit()
    {
        SaveAllSlotsToDisk();
    }

    public void LoadTextureFromFile(string filePath, int slot)
    {
        if (slot < 0 || slot >= availableTextures.Length || !File.Exists(filePath)) return;

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (texture.LoadImage(fileData))
        {
            texture.name = Path.GetFileNameWithoutExtension(filePath);
            availableTextures[slot] = texture;
            SaveSlotToDisk(slot);
            currentTextureIndex = slot;
            isTextureMode = true;
        }
    }

    public void SaveAllSlotsToDisk()
    {
        Directory.CreateDirectory(FolderPath);
        for (int i = 0; i < availableTextures.Length; i++)
            SaveSlotToDisk(i);
    }

    public void SaveSlotToDisk(int slot)
    {
        Directory.CreateDirectory(FolderPath);

        Texture2D tex = availableTextures[slot];
        string path = Path.Combine(FolderPath, $"slot_{slot}.png");

        if (tex == null)
        {
            if (File.Exists(path))
                File.Delete(path);
            return;
        }

        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(path, png);
    }

    public void LoadAllSlotsFromDisk()
    {
        Directory.CreateDirectory(FolderPath);

        for (int slot = 0; slot < availableTextures.Length; slot++)
        {
            string path = Path.Combine(FolderPath, $"slot_{slot}.png");
            if (!File.Exists(path)) continue;

            byte[] png = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            if (tex.LoadImage(png))
            {
                tex.name = $"slot_{slot}";
                availableTextures[slot] = tex;
            }
        }
    }

#if UNITY_EDITOR
    public void OpenTextureDialog(int slot)
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Load Texture", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
            LoadTextureFromFile(path, slot);
    }
#endif
}