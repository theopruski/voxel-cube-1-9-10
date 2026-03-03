using UnityEngine;

public class WorkGenerator
{
    public Vector3Int ChunkSize = new Vector3Int(16, 256, 16);
    public Vector2 NoiseScale = Vector2.one;
    public Vector2 NoiseOffset = Vector2.zero;
    [Space]
    public int HeighOffset = 60;
    public float HeightIntensity = 5f;
    private int[,,] TempData;

    void Start() {
        TempData = new int[ChunkSize.x, ChunkSize.y,ChunkSize.z];

        for (int x = 0; x < ChunkSize.x; x++)
        {
            for (int z = 0; z < ChunkSize.z; z++)
            {
                float PerlinCoordX = NoiseOffset.x + x / (float)ChunkSize.x * NoiseScale.x;
                float PerlinCoordY = NoiseOffset.y + z / (float)ChunkSize.z * NoiseScale.y;
                int Heightgen = Mathf.RoundToInt(Mathf.PerlinNoise(PerlinCoordX, PerlinCoordY) * HeightIntensity + HeighOffset);
            }
        }
    }

}
