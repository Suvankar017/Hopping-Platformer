using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width = 1;
    public int height = 1;
    public float hexSize = 1.0f;
    public int radius = 1;

    [Header("Gizmos Settings")]
    public Color sphereColor = Color.magenta;
    public float sphereRadius = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;

        //for (int q = 0; q < width; q++)
        //{
        //    for (int r = 0; r < height; r++)
        //    {
        //        Vector3 pos = HexToWorld(q, r, hexSize);

        //        Gizmos.DrawSphere(pos, sphereRadius);
        //    }
        //}

        //for (int q = -radius; q <= radius; q++)
        //{
        //    int rMin = Mathf.Max(-radius, -q - radius);
        //    int rMax = Mathf.Min(radius, -q + radius);

        //    for (int r = rMin; r <= rMax; r++)
        //    {
        //        Gizmos.DrawSphere(HexToWorld(q, r), sphereRadius);
        //    }
        //}

        HexGridDrawer(width, height, size);
    }

    //private void HexGridDrawer(int width, int height, Vector2 size)
    //{
    //    int c = ((width & 1) == 0) ? 1 : 0;

    //    int h = Mathf.Max(height - 1, 0);
    //    int w = (c == 0) ? Mathf.Max(width - 1, 0) : Mathf.Max(width - 2, 0);

    //    Vector3 s = Vector3.zero;
    //    s.x = w * Mathf.Sqrt(3f) * size.x;

    //    for (int row = 0; row < height; row++)
    //    {
    //        int newWidth = ((row & 1) == c) ? width : width - 1;
    //        for (int col = 0; col < newWidth; col++)
    //        {
    //            float x = col * Mathf.Sqrt(3f) * size.x;

    //            if (row % 2 == 1)
    //            {
    //                if (c == 0)
    //                    x += Mathf.Sqrt(3f) * size.x * 0.5f;
    //                else
    //                    x -= Mathf.Sqrt(3f) * size.x * 0.5f;
    //            }

    //            float z = row * 1.5f * size.y;

    //            Gizmos.DrawSphere(new Vector3(x, z, 0f) - s * 0.5f, sphereRadius);
    //        }
    //    }
    //}

    private void HexGridDrawer(int width, int height, Vector2 size)
    {
        bool evenWidth = (width & 1) == 0;

        int maxColumns = evenWidth
            ? Mathf.Max(width - 2, 0)
            : Mathf.Max(width - 1, 0);

        Vector3 centerOffset = new(
            maxColumns * Mathf.Sqrt(3f) * size.x,
            Mathf.Max(height - 1, 0) * 1.5f * size.y,
            0f);

        centerOffset = Vector3.Scale(centerOffset, Vector3.right);

        float horizontalSpacing = Mathf.Sqrt(3f) * size.x;
        float verticalSpacing = 1.5f * size.y;

        for (int row = 0; row < height; row++)
        {
            int rowWidth = ((row & 1) == (evenWidth ? 1 : 0))
                ? width
                : width - 1;

            for (int col = 0; col < rowWidth; col++)
            {
                Vector3 position = HexToWorld(
                    col,
                    row,
                    horizontalSpacing,
                    verticalSpacing,
                    evenWidth);

                Gizmos.DrawSphere(
                    position - centerOffset * 0.5f,
                    sphereRadius);
            }
        }
    }

    private static Vector3 HexToWorld(
        int column,
        int row,
        Vector2 hexSize,
        bool evenWidthLayout)
    {
        float horizontalSpacing = Mathf.Sqrt(3f) * hexSize.x;
        float verticalSpacing = 1.5f * hexSize.y;

        float x = column * horizontalSpacing;

        if ((row & 1) == 1)
        {
            float offset = horizontalSpacing * 0.5f;
            x += evenWidthLayout ? -offset : offset;
        }

        float y = row * verticalSpacing;

        return new Vector3(x, y, 0f);
    }

    private static Vector3 HexToWorld(
        int column,
        int row,
        float horizontalSpacing,
        float verticalSpacing,
        bool evenWidthLayout)
    {
        float x = column * horizontalSpacing;

        if ((row & 1) == 1)
        {
            float offset = horizontalSpacing * 0.5f;
            x += evenWidthLayout ? -offset : offset;
        }

        float y = row * verticalSpacing;

        return new Vector3(x, y, 0f);
    }

    private static Vector3 HexToWorld(int col, int row, Vector2 size)
    {
        float x = (col + (row & 1) * 0.5f)
                  * Mathf.Sqrt(3f) * size.x;

        float y = row * 1.5f * size.y;

        return new Vector3(x, y, 0f);
    }

    public Vector2 size = Vector2.one;

    private Vector3 HexToWorld(int q, int r, float hexSize)
    {
        float x = hexSize * Mathf.Sqrt(3f) * (q + r * 0.5f);
        float z = hexSize * 1.5f * r;

        return new Vector3(x, z, 0f);
    }

    private Vector3 HexToWorld(int q, int r)
    {
        float x = hexSize * Mathf.Sqrt(3f) * (q + r * 0.5f);
        float z = hexSize * 1.5f * r;

        return new Vector3(x, z, 0.0f);
    }
}
