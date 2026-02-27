using UnityEngine;
using UnityEngine.InputSystem;

public class VoxelInput : MonoBehaviour
{
    public VoxelManager voxelManager;
    public float maxDistance = 50f;
    void Update()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.cKey.wasPressedThisFrame)
            HandleAction(true);
        if (Keyboard.current.rKey.wasPressedThisFrame)
            HandleAction(false);
    }
    void HandleAction(bool add)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Vector3Int pos;
            if (add)
            {
                if (hit.collider.gameObject.name == "Ground")
                {
                    pos = voxelManager.WorldToGrid(hit.point);
                    pos.y = 0;
                    voxelManager.AddVoxel(pos);
                }
                else if (hit.collider.gameObject.name.StartsWith("Voxel"))
                {
                    pos = voxelManager.WorldToGrid(hit.point + hit.normal * 0.5f);
                    voxelManager.AddVoxel(pos);
                }
            }
            else
            {
                if (hit.collider.gameObject.name.StartsWith("Voxel"))
                {
                    pos = voxelManager.WorldToGrid(hit.transform.position);
                    voxelManager.RemoveVoxel(pos);
                }
            }
        }
    }
}