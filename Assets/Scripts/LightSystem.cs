using System;
using Gameplay;
using UnityEngine;

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
    private Vector2 _bottomLeftCameraOffset;

    private bool[,] _calculatePixels;

    private void Start()
    {
        var size = mapCollider.size;

        var width = Mathf.RoundToInt(size.x * resolution);
        var height = Mathf.RoundToInt(size.y * resolution);

        var mainCamera = Camera.main;
        var tmp1 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        var tmp2 = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
        _cameraWorldWidth = Mathf.Abs(tmp1.x - tmp2.x);
        _cameraWorldHeight = Mathf.Abs(tmp1.y - tmp2.y);
        _bottomLeftCameraOffset = new Vector2(-_cameraWorldWidth / 2f, -_cameraWorldHeight / 2f);

        _texture = new Texture2D(width, height);
        _maskTexture = new Texture2D(width, height);
        
        _calculatePixels = new bool[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var pos = TexturePointToWorldPosition(new Point(i, j));
                var ray = new Ray(pos, Vector3.forward);
                var value = !Physics2D.Raycast(ray.origin, ray.direction, 100f, layerMask);
                _calculatePixels[i, j] = value;
                if (!value)
                    _texture.SetPixel(i, j, gradient.Evaluate(1f));
            }
        }
        _texture.Apply();

        transform.position = mapCollider.offset;
        spriteRenderer.transform.localScale = new Vector3(size.x * 100 / width, size.y * 100 / height);

        spriteRenderer.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one / 2f);
        spriteMask.sprite = Sprite.Create(_maskTexture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one / 2f);
        blackMask.color = gradient.Evaluate(1f);
    }

    private void LateUpdate()
    {
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        var offset = 0.1f;
        var start = ViewportToTexturePoint(-offset, -offset);
        var end = ViewportToTexturePoint(1f + offset, 1f + offset);

        Vector2 playerPos = Astronaut.LocalAstronaut.transform.position;
            
        for (var i = start.I; i <= end.I; i++)
        {
            for (var j = start.J; j < end.J; j++)
            {
                if (_calculatePixels[i, j])
                {
                    var pos = TexturePointToWorldPosition(new Point(i, j));
                    var dir = pos - playerPos;
                    var dist = dir.magnitude;
                    var percent = dir.magnitude / range;
                
                    var color = gradient.Evaluate(1f);
                    var maskColor = Color.clear;
                
                    if (percent <= 1f)
                    {
                        var ray = new Ray(playerPos, dir);
                        if (!Physics2D.Raycast(ray.origin, ray.direction, dist, layerMask))
                        {
                            color = gradient.Evaluate(percent);
                            maskColor = Color.white;
                        }
                    }
                
                    _texture.SetPixel(i, j, color);
                    _maskTexture.SetPixel(i, j, maskColor);
                }
            }
        }
        
        _texture.Apply();
        _maskTexture.Apply();
    }

    private Point ViewportToTexturePoint(float i, float j)
    {
        return WorldPositionToTexturePoint(ViewportToWorldPosition(new Vector2(i, j)));
    }

    private Vector2 TexturePointToWorldPosition(Point point)
    {
        var x = (float) point.I / _texture.width;
        var y = (float) point.J / _texture.height;
        var pos = new Vector2(x, y);
        pos.Scale(mapCollider.size);
        
        return mapCollider.offset - mapCollider.size / 2f + pos;
    }

    private Vector2 ViewportToWorldPosition(Vector2 viewport)
    {
        Vector2 playerPos = Astronaut.LocalAstronaut.transform.position;
        var fullWorldCameraPos = new Vector2(_cameraWorldWidth, _cameraWorldHeight);
        var offset = fullWorldCameraPos;
        offset.Scale(viewport);
        return playerPos + offset + _bottomLeftCameraOffset;
    }

    private Point WorldPositionToTexturePoint(Vector2 pos)
    {
        var offset = mapCollider.offset;
        var size = mapCollider.size;
        pos += offset;
        pos += size / 2f;

        var point = new Point(pos.x * _texture.width / size.x, pos.y * _texture.height / size.y);
        point.I = Mathf.Clamp(point.I, 0, _texture.width - 1);
        point.J = Mathf.Clamp(point.J, 0, _texture.height - 1);
        return point;
    }
}