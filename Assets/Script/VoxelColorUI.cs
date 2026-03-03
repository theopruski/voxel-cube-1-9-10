using UnityEngine;
using UnityEngine.UI;

public class VoxelColorUI : MonoBehaviour
{
    public VoxelColorManager colorManager;
    public VoxelTextureManager textureManager;
    public Image currentDisplay;

    private Sprite _cachedSprite;
    private Texture2D _cachedTex;

    void Update()
    {
        if (textureManager.IsTextureMode())
        {
            Texture2D tex = textureManager.GetCurrentTexture();
            if (tex != null)
            {
                if (_cachedTex != tex)
                {
                    _cachedTex = tex;
                    _cachedSprite = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f),
                        100f
                    );
                }
                currentDisplay.sprite = _cachedSprite;
                currentDisplay.color = Color.white;
            }
            else
            {
                currentDisplay.sprite = null;
                currentDisplay.color = Color.black;
            }
        }
        else
        {
            currentDisplay.sprite = null;
            currentDisplay.color = colorManager.GetCurrentColor();
        }
    }
}