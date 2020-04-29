using Gameplay;
using UnityEngine;
using Unity.Mathematics;

public class LightSystem : MonoBehaviour
{
    public float range;
    public BoxCollider2D mapCollider;
    public int resolution;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer blackMask;
    public SpriteMask spriteMask;
    public LayerMask layerMask;
    public Gradient gradient;

    private Texture2D _texture;
    private Texture2D _maskTexture;

    private float _cameraWorldWidth;
    private float _cameraWorldHeight;

    private bool[,] _calculatePixels;

    private float2 _bottomLeftCameraOffset;
    private int _textureWidth;
    private int _textureHeight;
    private float2 _size;
    private float2 _halfSize;
    private float2 _offset;

    private float2 _cameraWorldSize;

    private void Start()
    {
        var mainCamera = Camera.main;
        var tmp1 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        var tmp2 = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        _cameraWorldWidth = Mathf.Abs(tmp1.x - tmp2.x);
        _cameraWorldHeight = Mathf.Abs(tmp1.y - tmp2.y);
        _cameraWorldSize = new Vector2(_cameraWorldWidth, _cameraWorldHeight);

        _size = mapCollider.size;
        _halfSize = _size / 2f;
        _offset = mapCollider.offset;

        _textureWidth = Mathf.RoundToInt(_size.x * resolution);
        _textureHeight = Mathf.RoundToInt(_size.y * resolution);

        _bottomLeftCameraOffset = new Vector2(-_cameraWorldWidth / 2f, -_cameraWorldHeight / 2f);

        _texture = new Texture2D(_textureWidth, _textureHeight);
        _maskTexture = new Texture2D(_textureWidth, _textureHeight);

        _calculatePixels = new bool[_textureWidth, _textureHeight];
        for (var i = 0; i < _textureWidth; i++)
        {
            for (var j = 0; j < _textureHeight; j++)
            {
                var pos = TexturePointToWorldPosition(new int2(i, j));
                var value = !Physics2D.Raycast(pos, Vector3.forward, 100f, layerMask);
                _calculatePixels[i, j] = value;
                if (!value)
                    _texture.SetPixel(i, j, gradient.Evaluate(1f));
            }
        }

        _texture.Apply();

        transform.position = mapCollider.offset;
        spriteRenderer.transform.localScale =
            new Vector3(_size.x * 100 / _textureWidth, _size.y * 100 / _textureHeight);

        spriteRenderer.sprite =
            Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one / 2f);
        spriteMask.sprite =
            Sprite.Create(_maskTexture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one / 2f);
        blackMask.color = gradient.Evaluate(1f);
    }

    private void LateUpdate()
    {
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        const float offset = 0.1f;
        var start = ViewportToTexturePoint(-offset, -offset);
        var end = ViewportToTexturePoint(1f + offset, 1f + offset);

        var playerPos = Astronaut.LocalAstronaut.transform.position;

        for (var i = start.x; i <= end.x; i++)
        {
            for (var j = start.y; j < end.y; j++)
            {
                if (!_calculatePixels[i, j]) continue;
                
                var pos = TexturePointToWorldPosition(new int2(i, j));
                var dir = new float2(pos.x - playerPos.x, pos.y - playerPos.y);

                var dist = math.length(dir);
                var percent = dist / range;

                var color = gradient.Evaluate(1f);
                var maskColor = Color.clear;

                if (percent <= 1f)
                {
                    if (!Physics2D.Raycast(playerPos, dir, dist, layerMask))
                    {
                        color = gradient.Evaluate(percent);
                        maskColor = Color.white;
                    }
                }
                if (percent <= 1.05f)
                    _texture.SetPixel(i, j, color);
                _maskTexture.SetPixel(i, j, maskColor);
            }
        }

        _texture.Apply();
        _maskTexture.Apply();
    }

    private int2 ViewportToTexturePoint(float i, float j)
    {
        return WorldPositionToTexturePoint(ViewportToWorldPosition(new Vector2(i, j)));
    }

    private float2 TexturePointToWorldPosition(int2 point)
    {
        var x = (float) point.x / _textureWidth;
        var y = (float) point.y / _textureHeight;
        var pos = new float2(x * _size.x, y * _size.y);
        return _offset - _halfSize + pos;
    }

    private float2 ViewportToWorldPosition(float2 viewport)
    {
        var pos = Astronaut.LocalAstronaut.transform.position;
        var worldPos = new float2
        {
            x = _cameraWorldSize.x * viewport.x + pos.x + _bottomLeftCameraOffset.x,
            y = _cameraWorldSize.y * viewport.y + pos.y + _bottomLeftCameraOffset.y
        };
        return worldPos;
    }

    private int2 WorldPositionToTexturePoint(float2 pos)
    {
        pos += _offset + _halfSize;

        var x = (int) math.round(pos.x * _textureWidth / _size.x);
        var y = (int) math.round(pos.y * _textureHeight / _size.y);
        x = math.clamp(x, 0, _textureWidth - 1);
        y = math.clamp(y, 0, _textureHeight - 1);

        return new int2(x, y);
    }
}