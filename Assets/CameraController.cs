using UnityEngine;

public class CameraController : MonoBehaviour
{
    public readonly float MaximumFrequency = 0.02f;
    public readonly float MinimumFrequency = 0.001f;

    public readonly float PanSpeed = 0.1f;
    public readonly float ZoomSpeed = 1.2f;

    public GameObject Planet;
    public GameObject Star;
    public Gradient StarGradient;

    private void Awake()
    {
        Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Refresh();
        }

        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += PanSpeed * movement;

        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d < 0f)
        {
            GetComponent<Camera>().orthographicSize *= ZoomSpeed;
        }
        else if (d > 0f)
        {
            GetComponent<Camera>().orthographicSize /= ZoomSpeed;
        }
    }

    private void Refresh()
    {
        Texture2D backgroundTexture = CreateBackgroundTexture();
        Rect backgroundRect = new Rect(0, 0, 512, 512);
        Planet.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backgroundTexture, backgroundRect, new Vector2(0.5f, 0.5f), 512);

        Star.GetComponent<SpriteRenderer>().color = StarGradient.Evaluate(Random.Range(0.0f, 1.0f));
    }

    private Texture2D CreateBackgroundTexture()
    {
        Gradient colorGradient = CreateColorGradient();
        float noiseMultiplier = Random.Range(3.0f, 3.0f);
        float frequency1 = Random.Range(MinimumFrequency, MaximumFrequency);
        float frequency2 = frequency1 * 3;
        float[,] frequencies = new float[,] { { 0.5f, 0.5f }, { frequency1, frequency2 } };

        Texture2D backgroundTexture = new Texture2D(512, 512);
        float offset = Random.Range(0f, 100000f);
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                float noiseSample = PerlinNoise(x, y, frequencies, offset);
                noiseSample = (noiseSample - 0.5f) * noiseMultiplier + 0.5f;
                noiseSample = Mathf.Clamp01(noiseSample);
                Color colorSample = colorGradient.Evaluate(noiseSample);
                backgroundTexture.SetPixel(x, y, colorSample);
            }
        }
        backgroundTexture.Apply();
        return backgroundTexture;
    }
    
    private Gradient CreateColorGradient()
    {
        Gradient colorGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        colorKeys[1].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;

        colorGradient.SetKeys(colorKeys, alphaKeys);
        return colorGradient;
    }

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
}
