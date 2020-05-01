using System.Collections.Generic;
using Gameplay;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Lights
{
    public class FieldOfView : MonoBehaviour
    {
        public const float Angle = 361f;

        [Header("Base")] public float radius = 3f;

        [Header("Settings")] public int textureResolution;
        public int cameraMult;
        public LayerMask layerMask;

        [Header("Edge Smoother")] public int edgeResolveIterations = 2;
        public float edgeDstThreshold = 0.5f;

        [Header("References")] public Camera fovCamera;
        public MeshFilter meshFilter;
        public SpriteMask spriteMask;
        public SpriteRenderer blackMask;

        [Header("Gradient")] public int gradientTextureSize;
        public Gradient gradient;
        public SpriteRenderer gradientSpriteRenderer;

        private RenderTexture _renderTexture;
        private Texture2D _maskTexture;

        private Mesh _mesh;
        public Vector3 SourcePosition => Astronaut.LocalAstronaut.GetCenter();

        private int _width;
        private int _height;

        private void Start()
        {
            fovCamera.orthographicSize = 3 * cameraMult;
            var tmp1 = fovCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
            var tmp2 = fovCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

            var cameraWidth = Mathf.Abs(tmp1.x - tmp2.x);
            var cameraHeight = Mathf.Abs(tmp1.y - tmp2.y);

            _mesh = new Mesh();
            meshFilter.mesh = _mesh;

            _width = cameraMult * Screen.width / textureResolution;
            _height = cameraMult * Screen.height / textureResolution;
            _renderTexture = new RenderTexture(_width, _height, 16);

            fovCamera.targetTexture = _renderTexture;

            _maskTexture = new Texture2D(_renderTexture.width, _renderTexture.height);
            spriteMask.sprite = Sprite.Create(_maskTexture, new Rect(0, 0, _width, _height), Vector3.one / 2f);

            spriteMask.transform.localScale =
                new Vector3(cameraWidth * 100f / _width, cameraHeight * 100f / _height, 1f);

            blackMask.color = gradient.Evaluate(1f);
            blackMask.enabled = true;
        }

        private void UpdateMaskTexture()
        {
            var currentRt = RenderTexture.active;
            RenderTexture.active = fovCamera.targetTexture;
            fovCamera.Render();
            _maskTexture.ReadPixels(new Rect(0, 0, _width, _height), 0, 0);
            _maskTexture.Apply();
            RenderTexture.active = currentRt;
        }

        private void LateUpdate()
        {
            var position = Astronaut.LocalAstronaut.transform.position;
            spriteMask.transform.position = position;
            position.z = 0;

            var scale = 100f * radius * 2.01f / gradientTextureSize;
            gradientSpriteRenderer.transform.position = position;
            gradientSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1f);

            var pos = position;
            pos.z = -10;
            fovCamera.transform.position = pos;

            DrawFieldOfView();
            UpdateMaskTexture();
        }

        private EdgeInfo FindEdge(PointInfo minViewCast, PointInfo maxViewCast)
        {
            var minAngle = minViewCast.Angle;
            var maxAngle = maxViewCast.Angle;
            var minPoint = Vector3.zero;
            var maxPoint = Vector3.zero;

            for (var i = 0; i < edgeResolveIterations; i++)
            {
                var angle = (minAngle + maxAngle) / 2;
                var newViewCast = GetPointInfo(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;
                if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.Position;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.Position;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        private PointInfo GetPointInfo(float angle)
        {
            var pos = SourcePosition;
            var dir = DirFromAngle(angle);

            var hit = Physics2D.Raycast(pos, dir, radius, layerMask);
            if (hit)
                return new PointInfo(true, hit.point, hit.distance, angle);
            return new PointInfo(false, pos + dir * radius, radius, angle);
        }

        private void DrawFieldOfView()
        {
            var count = 360;
            var stepAngle = Angle / count;

            var currentAngle = -Angle / 2f;

            var points = new List<Vector3>();
            var oldViewCast = new PointInfo();

            for (var i = 0; i < count; i++)
            {
                var newViewCast = GetPointInfo(currentAngle);

                if (i > 0 && edgeResolveIterations > 0)
                {
                    var edgeDstThresholdExceeded =
                        Mathf.Abs(oldViewCast.Distance - newViewCast.Distance) > edgeDstThreshold;
                    if (oldViewCast.Hit != newViewCast.Hit ||
                        oldViewCast.Hit && newViewCast.Hit && edgeDstThresholdExceeded)
                    {
                        var edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.PointA != Vector3.zero) points.Add(edge.PointA);
                        if (edge.PointB != Vector3.zero) points.Add(edge.PointB);
                    }
                }

                points.Add(newViewCast.Position);
                currentAngle += stepAngle;
                oldViewCast = newViewCast;
            }

            CreateMesh(points);
        }

        private void CreateMesh(IList<Vector3> points)
        {
            var vertexCount = points.Count + 1;
            var vertices = new Vector3[vertexCount];
            var colors = new Color[vertexCount];
            var triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = SourcePosition;
            colors[0] = Color.white;
            for (var i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = points[i];

                colors[i + 1] = Color.white;

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            _mesh.Clear();

            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.colors = colors;
            _mesh.RecalculateNormals();
        }

        private Vector3 DirFromAngle(float currentAngle)
        {
            return new Vector3(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0);
        }

        public Texture2D GenerateGradientTexture()
        {
            var texture = new Texture2D(gradientTextureSize, gradientTextureSize);
            var halfSize = gradientTextureSize / 2;
            var center = new Vector2(halfSize, halfSize);

            for (var i = 0; i < gradientTextureSize; i++)
            {
                for (var j = 0; j < gradientTextureSize; j++)
                {
                    var pos = new Vector2(i, j);
                    var dist = Vector2.Distance(pos, center);
                    var percent = dist / halfSize;

                    texture.SetPixel(i, j, gradient.Evaluate(percent));
                }
            }

            texture.Apply();
            return texture;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 PointA;
        public Vector3 PointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {
            PointA = pointA;
            PointB = pointB;
        }
    }

    public struct PointInfo
    {
        public Vector3 Position;
        public float Angle;
        public bool Hit;
        public float Distance;

        public PointInfo(bool hit, Vector3 position, float distance, float angle)
        {
            Position = position;
            Angle = angle;
            Hit = hit;
            Distance = distance;
        }
    }
}
