using UnityEngine;

public static class Helpers
{
    public static float PerlinNoise(float x, float y, float[,] frequencies, float offset)
    {
        float noiseSample = 0f;
        float totalWeight = 0f;
        for (int i = 0; i < frequencies.GetLength(1); i++)
        {
            noiseSample += frequencies[0, i] * Mathf.PerlinNoise(x * frequencies[1, i] + offset, y * frequencies[1, i] + offset);
            totalWeight += frequencies[0, i];
        }
        return noiseSample / totalWeight;
    }

    public static Texture2D ComposeTextures(Texture2D forground, Texture2D background)
    {
        Texture2D result = new Texture2D(background.width, background.height);
        for (int x = 0; x < background.width; x++)
        {
            for (int y = 0; y < background.height; y++)
            {
                result.SetPixel(x, y, ComposePixels(forground.GetPixel(x, y), background.GetPixel(x, y)));
            }
        }
        result.Apply();
        return result;
    }

    public static Color ComposePixels(Color forgroundPixel, Color backgroundPixel)
    {
        float r = ((forgroundPixel.r * forgroundPixel.a) + (backgroundPixel.r * backgroundPixel.a) * (1f - forgroundPixel.a)) / (forgroundPixel.a + backgroundPixel.a * (1f - forgroundPixel.a));
        float g = ((forgroundPixel.g * forgroundPixel.a) + (backgroundPixel.g * backgroundPixel.a) * (1f - forgroundPixel.a)) / (forgroundPixel.a + backgroundPixel.a * (1f - forgroundPixel.a));
        float b = ((forgroundPixel.b * forgroundPixel.a) + (backgroundPixel.b * backgroundPixel.a) * (1f - forgroundPixel.a)) / (forgroundPixel.a + backgroundPixel.a * (1f - forgroundPixel.a));
        float a = forgroundPixel.a + backgroundPixel.a * (1f - forgroundPixel.a);
        return new Color(r, g, b, a);
    }

    public static Texture2D ApplyMask(Texture2D texture, Texture2D mask)
    {
        Texture2D result = new Texture2D(texture.width, texture.height);
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color color = texture.GetPixel(x, y);
                color.a = mask.GetPixel(x, y).a;
                result.SetPixel(x, y, color);
            }
        }
        result.Apply();
        return result;
    }

    /// <summary>
    /// Return the rect formed by the intersection of two rectangles. Width and
    /// height will be negative if the rectangles do not overlap.
    /// </summary>
    /// <param name="rect1"></param>
    /// <param name="rect2"></param>
    /// <returns></returns>
    public static Rect RectOverlap(Rect rect1, Rect rect2)
    {
        float x = Mathf.Max(rect1.x, rect2.x);
        float y = Mathf.Max(rect1.y, rect2.y);
        float width = Mathf.Min(rect1.x + rect1.width, rect2.x + rect2.width) - x;
        float height = Mathf.Min(rect1.y + rect1.height, rect2.y + rect2.height) - y;
        return new Rect(x, y, width, height);
    }

    public static Vector2 InsideUnitCircle(System.Random rng)
    {
        float radius = Mathf.Sqrt((float)rng.NextDouble());
        float theta = (float)rng.NextDouble() * 2 * Mathf.PI;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sign(theta);
        return new Vector2(x, y);
    }
}