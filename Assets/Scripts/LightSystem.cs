using Gameplay;
using Managers;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public enum LightSystemMode
    {
        Camera,
        World
    }

    public float radius;
    public LightSystemMode mode;

    [Header("Settings")]
    public LayerMask layerMask;
    public int textureResolution;
    public int resolutionInEditor;
    public Gradient maskGradient;
    [Range(0.001f, 1f)] public float sharpness;

    [Header("References")]
    public SpriteRenderer blackMask;
    public SpriteMask visionMask;
    public SpriteRenderer visionMaskSprite;

    private Camera _mainCamera;
    private Astronaut _localAstronaut;

    private Texture2D _spriteTexture2D;
    private Texture2D _maskTexture2D;

    private int _width;
    private int _height;

    private float _cameraWorldWidth;
    private float _cameraWorldHeight;

    private void Start()
    {
#if UNITY_EDITOR
        textureResolution = resolutionInEditor;
#endif
        _localAstronaut = Astronaut.LocalAstronaut;
        _mainCamera = Camera.main;

        visionMask.sprite = CreateSprite(ref _maskTexture2D);
        visionMaskSprite.sprite = CreateSprite(ref _spriteTexture2D);

        float resolution = (float)Screen.width / Screen.height;
        float ortho = _mainCamera.orthographicSize;
        float ppp = 100f / _width;
        float orthoCameraWidth = resolution * ortho * 2f;
        float maskScale = orthoCameraWidth * ppp;
        visionMask.transform.localScale = maskScale * Vector3.one;

        Vector3 tmp1 = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector3 tmp2 = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        _cameraWorldWidth = Mathf.Abs(tmp1.x - tmp2.x);
        _cameraWorldHeight = Mathf.Abs(tmp1.y - tmp2.y);
    }

    private Sprite CreateSprite(ref Texture2D texture)
    {
        _width = Screen.width / textureResolution;
        _height = _width;

        texture = new Texture2D(_width, _height);
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                texture.SetPixel(i, j, Color.clear);
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
    }

    private float GetRange()
    {
        return radius / _cameraWorldWidth * Screen.width / 2f;
    }

    private void UpdateTexture()
    {
        Vector2 playerPos = _localAstronaut.transform.position;
        Vector2 center = new Vector2(_width, _height) / 2f;
        float maxPixelDist = GetRange() / textureResolution;

        Vector2 bottomLeftWorldPos = playerPos + new Vector2(-_cameraWorldWidth / 2f, -_cameraWorldWidth / 2f);
        Vector2 fullWorldCameraPos = new Vector2(_cameraWorldWidth, _cameraWorldWidth);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Vector2 pos = new Vector2(i, j);

                float dist = Vector2.Distance(pos, center);
                float percent = Mathf.Clamp01(dist / maxPixelDist);

                if (percent < 1f)
                {
                    Vector2 viewportCoord = new Vector2(pos.x / _width, pos.y / _height);
                    Vector2 worldPos = Vector2.zero;

                    if (mode == LightSystemMode.World)
                    {
                        Vector2 offset = fullWorldCameraPos;
                        offset.Scale(viewportCoord);
                        worldPos = bottomLeftWorldPos + offset;
                    }
                    else if (mode == LightSystemMode.Camera)
                    {
                        worldPos = _mainCamera.ViewportToWorldPoint(viewportCoord);
                    }

                    Vector2 dir = worldPos - playerPos;
                    Ray ray = new Ray(playerPos, dir);

                    RaycastHit2D raycast = Physics2D.Raycast(ray.origin, ray.direction, dir.magnitude, layerMask);
                    if (raycast) percent = 1f;
                }

                Color color = new Color(1f, 1f, 1f, percent >= 1f ? 0f : 1f);

                _maskTexture2D.SetPixel(i, j, color);
                _spriteTexture2D.SetPixel(i, j, maskGradient.Evaluate(percent));
            }
        }

        _maskTexture2D.Apply();
        _spriteTexture2D.Apply();
    }

    private void LateUpdate()
    {
        Vector3 position = _localAstronaut.transform.position;
        if (mode == LightSystemMode.Camera)
            position = _mainCamera.transform.position;

        blackMask.transform.position = position;
        visionMask.transform.position = position;

        visionMask.alphaCutoff = sharpness;
        blackMask.color = maskGradient.Evaluate(1f);

        UpdateTexture();
    }
}
