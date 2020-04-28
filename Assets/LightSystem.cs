using Managers;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public enum LightSystemMode
    {
        Camera, World
    }
    
    public float radius;
    public LightSystemMode mode;

    [Header("Settings")] public LayerMask layerMask;
    public int textureResolution;
    public Gradient maskGradient;
    [Range(0.001f, 1f)] public float sharpness;

    [Header("References")] public SpriteRenderer blackMask;
    public SpriteMask visionMask;
    public SpriteRenderer visionMaskSprite;

    private Camera _mainCamera;
    private Transform _localAstronaut;

    private Texture2D _spriteTexture2D;
    private Texture2D _maskTexture2D;

    private int _width;
    private int _height;

    private float _cameraWorldWidth;
    private float _cameraWorldHeight;

    private void Start()
    {
        _localAstronaut = GameManager.Instance.LocalAstronaut.transform;
        _mainCamera = Camera.main;

        visionMask.sprite = CreateSprite(ref _maskTexture2D);
        visionMaskSprite.sprite = CreateSprite(ref _spriteTexture2D);
        visionMask.transform.localScale = textureResolution * Vector3.one;

        var tmp1 = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        var tmp2 = _mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f));
        _cameraWorldWidth = Mathf.Abs(tmp1.x - tmp2.x);

        tmp1 = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        tmp2 = _mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f));
        _cameraWorldHeight = Mathf.Abs(tmp1.y - tmp2.y);
    }

    private Sprite CreateSprite(ref Texture2D texture2D)
    {
        _width = Screen.width / textureResolution;
        _height = Screen.height / textureResolution;

        texture2D = new Texture2D(_width, _height);
        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                texture2D.SetPixel(i, j, Color.clear);
            }
        }

        texture2D.Apply();

        return Sprite.Create(texture2D, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
    }

    private float GetRange()
    {
        return radius / _cameraWorldWidth * Screen.width / 2f;
    }

    private void UpdateTexture()
    {
        Vector2 playerPos = _localAstronaut.transform.position;
        var center = new Vector2(_width, _height) / 2f;
        var maxPixelDist = GetRange() / textureResolution;

        var bottomLeftWorldPos = playerPos + new Vector2(-_cameraWorldWidth / 2f, -_cameraWorldHeight / 2f);
        var fullWorldCameraPos = new Vector2(_cameraWorldWidth, _cameraWorldHeight);

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var pos = new Vector2(i, j);
                
                var dist = Vector2.Distance(pos, center);
                var percent = Mathf.Clamp01(dist / maxPixelDist);
                
                if (percent < 1f)
                {
                    var viewportCoord = new Vector2(pos.x / _width, pos.y / _height);
                    var worldPos = Vector2.zero;
                    if (mode == LightSystemMode.World)
                    {
                        var offset = fullWorldCameraPos;
                        offset.Scale(viewportCoord);
                        worldPos = bottomLeftWorldPos + offset;
                    }
                    else if (mode == LightSystemMode.Camera)
                    {
                        worldPos = _mainCamera.ViewportToWorldPoint(viewportCoord);
                    }

                    var dir = worldPos - playerPos;
                    var ray = new Ray(playerPos, dir);

                    var raycast = Physics2D.Raycast(ray.origin, ray.direction, dir.magnitude, layerMask);
                    if (raycast) percent = 1f;
                }

                var color = new Color(1f, 1f, 1f, percent >= 1f ? 0f : 1f);
                
                _maskTexture2D.SetPixel(i, j, color);
                _spriteTexture2D.SetPixel(i, j, maskGradient.Evaluate(percent));
            }
        }

        _maskTexture2D.Apply();
        _spriteTexture2D.Apply();
    }

    private void LateUpdate()
    {
        var position = _localAstronaut.position;
        if (mode == LightSystemMode.Camera)
            position = _mainCamera.transform.position;
        
        blackMask.transform.position = position;
        visionMask.transform.position = position;

        visionMask.alphaCutoff = sharpness;
        blackMask.color = maskGradient.Evaluate(1f);

        UpdateTexture();
    }
}