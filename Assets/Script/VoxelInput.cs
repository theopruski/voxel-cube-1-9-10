using UnityEngine;
using UnityEngine.InputSystem;

public class VoxelInput : MonoBehaviour
{
    public VoxelManager voxelManager;
    public VoxelColorManager colorManager;
    public VoxelTextureManager textureManager;
    public float maxDistance = 50f;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.cKey.wasPressedThisFrame)
            HandleAction(true);

        if (Keyboard.current.rKey.wasPressedThisFrame)
            HandleAction(false);

        if (Keyboard.current.f5Key.wasPressedThisFrame)
            voxelManager.SaveModel();

        if (Keyboard.current.f9Key.wasPressedThisFrame)
            voxelManager.LoadModel();

        if (Keyboard.current.vKey.wasPressedThisFrame)
            HandleMaterialChange();

        if (Keyboard.current.tKey.wasPressedThisFrame)
            textureManager.ToggleMode();

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (textureManager.IsTextureMode())
                textureManager.NextTexture();
            else
                colorManager.NextColor();
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            if (textureManager.IsTextureMode())
                textureManager.PreviousTexture();
            else
                colorManager.PreviousColor();
        }

        for (int i = 0; i < 10; i++)
        {
            if (Keyboard.current[(Key)(Key.Digit0 + i)].wasPressedThisFrame)
            {
                if (textureManager.IsTextureMode())
                    textureManager.SetTextureIndex(i);
                else
                    colorManager.SetColorIndex(i);
            }
        }

        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Keyboard.current[(Key)(Key.Digit0 + i)].wasPressedThisFrame)
                {
                    textureManager.OpenTextureDialog(i);
                }
            }
        }
    }

    void HandleAction(bool add)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance)) return;

        Vector3Int pos;
        if (add)
        {
            if (hit.collider.gameObject.name == "Ground")
            {
                pos = voxelManager.WorldToGrid(hit.point);
                pos.y = 0;
                PlaceVoxel(pos);
            }
            else if (hit.collider.gameObject.name.StartsWith("Voxel"))
            {
                pos = voxelManager.WorldToGrid(hit.point + hit.normal * 0.5f);
                PlaceVoxel(pos);
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

    void PlaceVoxel(Vector3Int pos)
    {
        if (textureManager.IsTextureMode())
        {
            Texture2D texture = textureManager.GetCurrentTexture();
            voxelManager.AddVoxel(pos, texture != null ? Color.white : colorManager.GetCurrentColor(), texture);
        }
        else
        {
            voxelManager.AddVoxel(pos, colorManager.GetCurrentColor(), null);
        }
    }

    void HandleMaterialChange()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance)) return;
        if (!hit.collider.gameObject.name.StartsWith("Voxel")) return;

        Vector3Int pos = voxelManager.WorldToGrid(hit.transform.position);

        if (textureManager.IsTextureMode())
        {
            Texture2D texture = textureManager.GetCurrentTexture();
            if (texture != null)
                voxelManager.ChangeVoxelMaterial(pos, Color.white, texture);
        }
        else
        {
            voxelManager.ChangeVoxelMaterial(pos, colorManager.GetCurrentColor(), null);
        }
    }
}