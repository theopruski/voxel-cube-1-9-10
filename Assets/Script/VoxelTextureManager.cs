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
        CreateDefaultTextures();
    }

    void OnApplicationQuit()
    {
        SaveAllSlotsToDisk();
    }

    void CreateDefaultTextures()
    {
        if (availableTextures[0] == null)
            availableTextures[0] = CreateWoodTexture();

        if (availableTextures[1] == null)
            availableTextures[1] = CreateStoneTexture();

        if (availableTextures[2] == null)
            availableTextures[2] = CreateBrickTexture();

        if (availableTextures[3] == null)
            availableTextures[3] = CreateGrassTexture();

        if (availableTextures[4] == null)
            availableTextures[4] = CreateWaterTexture();

        if (availableTextures[5] == null)
            availableTextures[5] = CreateSandTexture();

        if (availableTextures[6] == null)
            availableTextures[6] = CreateMetalTexture();

        if (availableTextures[7] == null)
            availableTextures[7] = CreateDirtTexture();

        if (availableTextures[8] == null)
            availableTextures[8] = CreateSnowTexture();

        if (availableTextures[9] == null)
            availableTextures[9] = CreateLavaTexture();
    }

    public void ResetToDefaultTextures()
    {
        availableTextures[0] = CreateWoodTexture();
        availableTextures[1] = CreateStoneTexture();
        availableTextures[2] = CreateBrickTexture();
        availableTextures[3] = CreateGrassTexture();
        availableTextures[4] = CreateWaterTexture();
        availableTextures[5] = CreateSandTexture();
        availableTextures[6] = CreateMetalTexture();
        availableTextures[7] = CreateDirtTexture();
        availableTextures[8] = CreateSnowTexture();
        availableTextures[9] = CreateLavaTexture();

        SaveAllSlotsToDisk();
        Debug.Log("Textures reset to defaults");
    }

    Texture2D CreateWoodTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color woodBase = new Color(0.55f, 0.35f, 0.2f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.5f);
                Color col = woodBase * (0.7f + noise * 0.3f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Wood";
        return tex;
    }

    Texture2D CreateStoneTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color stoneBase = new Color(0.5f, 0.5f, 0.5f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.15f, y * 0.15f);
                Color col = stoneBase * (0.6f + noise * 0.4f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Stone";
        return tex;
    }

    Texture2D CreateBrickTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color brickColor = new Color(0.7f, 0.3f, 0.2f, 1f);
        Color mortarColor = new Color(0.8f, 0.8f, 0.8f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool isMortar = (y % 16 < 2) || (x % 32 < 2);
                if ((y / 16) % 2 == 1)
                    isMortar = isMortar || ((x + 16) % 32 < 2);

                Color col = isMortar ? mortarColor : brickColor;
                float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                col *= (0.9f + noise * 0.1f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Brick";
        return tex;
    }

    Texture2D CreateGrassTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color grassBase = new Color(0.2f, 0.6f, 0.2f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.2f, y * 0.2f);
                Color col = grassBase * (0.7f + noise * 0.3f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Grass";
        return tex;
    }

    Texture2D CreateWaterTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color waterBase = new Color(0.2f, 0.4f, 0.8f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                Color col = waterBase * (0.8f + noise * 0.2f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Water";
        return tex;
    }

    Texture2D CreateSandTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color sandBase = new Color(0.9f, 0.8f, 0.5f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.3f, y * 0.3f);
                Color col = sandBase * (0.85f + noise * 0.15f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Sand";
        return tex;
    }

    Texture2D CreateMetalTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color metalBase = new Color(0.7f, 0.7f, 0.8f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.05f, y * 0.05f);
                Color col = metalBase * (0.9f + noise * 0.1f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Metal";
        return tex;
    }

    Texture2D CreateDirtTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color dirtBase = new Color(0.4f, 0.3f, 0.2f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.2f, y * 0.2f);
                Color col = dirtBase * (0.7f + noise * 0.3f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Dirt";
        return tex;
    }

    Texture2D CreateSnowTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color snowBase = new Color(0.95f, 0.95f, 1f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.25f, y * 0.25f);
                Color col = snowBase * (0.9f + noise * 0.1f);
                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Snow";
        return tex;
    }

    Texture2D CreateLavaTexture()
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color lavaBase = new Color(1f, 0.3f, 0f, 1f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noise = Mathf.PerlinNoise(x * 0.15f, y * 0.15f);
                Color col = lavaBase * (0.7f + noise * 0.3f);

                // Ajouter des zones plus sombres pour l'effet de lave refroidie
                float darkNoise = Mathf.PerlinNoise(x * 0.05f, y * 0.05f);
                if (darkNoise < 0.3f)
                    col *= 0.4f;

                col.a = 1f;
                tex.SetPixel(x, y, col);
            }
        }
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.name = "Lava";
        return tex;
    }

    public void LoadTextureFromFile(string filePath, int slot)
    {
        if (slot < 0 || slot >= availableTextures.Length || !File.Exists(filePath)) return;

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (texture.LoadImage(fileData))
        {
            texture.name = Path.GetFileNameWithoutExtension(filePath);
            texture.filterMode = FilterMode.Point;
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
                tex.filterMode = FilterMode.Point;
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