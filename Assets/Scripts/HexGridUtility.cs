using UnityEngine;

public static class HexGridUtility
{
    public static Vector3 HexToWorld(
        int column,
        int row,
        Vector2 hexSize,
        bool evenWidthLayout)
    {
        float horizontalSpacing =
            Mathf.Sqrt(3f) * hexSize.x;

        float verticalSpacing =
            1.5f * hexSize.y;

        float x = column * horizontalSpacing;

        if ((row & 1) == 1)
        {
            float offset =
                horizontalSpacing * 0.5f;

            x += evenWidthLayout
                ? -offset
                : offset;
        }

        float y = row * verticalSpacing;

        return new Vector3(x, y, 0f);
    }
}
