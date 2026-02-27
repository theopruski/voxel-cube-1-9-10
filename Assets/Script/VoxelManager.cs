using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VoxelManager : MonoBehaviour
{
    public Material voxelMaterial;
    private HashSet<Vector3Int> voxelData = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> voxelObjects = new Dictionary<Vector3Int, GameObject>();
    private Mesh cubeMesh;
    void Start()
    {
        cubeMesh = CreateCubeMesh();
    }
    public void AddVoxel(Vector3Int gridPos)
    {
        if (voxelData.Contains(gridPos)) return;
        if (gridPos.y != 0 && !HasNeighbor(gridPos)) return;
        voxelData.Add(gridPos);
        Vector3 worldPos = GridToWorld(gridPos);
        GameObject voxel = new GameObject("Voxel");
        voxel.transform.position = worldPos;
        voxel.transform.parent = transform;
        MeshFilter meshFilter = voxel.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = voxel.AddComponent<MeshRenderer>();
        BoxCollider boxCollider = voxel.AddComponent<BoxCollider>();
        boxCollider.size = Vector3.one;
        boxCollider.center = Vector3.zero;
        meshFilter.sharedMesh = cubeMesh;
        meshRenderer.material = voxelMaterial;
        voxelObjects[gridPos] = voxel;
    }
    public void RemoveVoxel(Vector3Int gridPos)
    {
        if (!voxelData.Contains(gridPos)) return;
        voxelData.Remove(gridPos);
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
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down,
            new Vector3Int(0,0,1),
            new Vector3Int(0,0,-1)
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
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-0.5f,-0.5f,-0.5f),
            new Vector3( 0.5f,-0.5f,-0.5f),
            new Vector3( 0.5f, 0.5f,-0.5f),
            new Vector3(-0.5f, 0.5f,-0.5f),
            new Vector3(-0.5f,-0.5f, 0.5f),
            new Vector3( 0.5f,-0.5f, 0.5f),
            new Vector3( 0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };
        int[] triangles = new int[]
        {
            0,2,1, 0,3,2,
            1,2,6, 1,6,5,
            5,6,7, 5,7,4,
            3,7,6, 3,6,2,
            4,7,3, 4,3,0,
            0,1,5, 0,5,4
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}