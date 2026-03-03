using UnityEngine;

public class VoxelColorManager : MonoBehaviour
{
    public Color[] availableColors = new Color[]
    {
        Color.white,
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        new Color(1f, 0.5f, 0f),
        new Color(0.5f, 0.25f, 0f),
        Color.gray
    };

    private int currentColorIndex = 0;

    public Color GetCurrentColor() => availableColors[currentColorIndex];

    public void SetColorIndex(int index)
    {
        currentColorIndex = Mathf.Clamp(index, 0, availableColors.Length - 1);
    }

    public void NextColor()
    {
        currentColorIndex = (currentColorIndex + 1) % availableColors.Length;
    }

    public void PreviousColor()
    {
        currentColorIndex--;
        if (currentColorIndex < 0) currentColorIndex = availableColors.Length - 1;
    }
}