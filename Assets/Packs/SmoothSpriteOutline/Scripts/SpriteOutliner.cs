using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (SpriteRenderer))]
public class SpriteOutliner : MonoBehaviour
{
    [Tooltip("Only recalculates the sprite data when the sprite image or its pivot changes. Useful for saving performance.")]
    public bool UpdateOnlyOnSpriteChange = true;
    
    private SpriteRenderer spriteRenderer;
    private Material mat;

    private int property_Size;
    private int property_SpriteUVs;
    private int property_PivotCorrection;

    private Sprite previousSprite;
    private Vector2 previoutPivot;

    private void Awake ()
    {
        Init ();
    }

    private void OnEnable ()
    {
        Init ();
    }

    private void Init ()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();

        if (Application.isPlaying)
        {
            mat = spriteRenderer.material;
        }
        else
        {
            mat = spriteRenderer.sharedMaterial;
        }

        property_Size = Shader.PropertyToID ("_Size");
        property_SpriteUVs = Shader.PropertyToID ("_SpriteUVs");
        property_PivotCorrection = Shader.PropertyToID ("_PivotCorrection");
    }

    private void LateUpdate ()
    {
        if (spriteRenderer.sprite == null) return;

        Sprite sprite = spriteRenderer.sprite;
        Vector2 pivot = sprite.pivot;
        Rect rect = sprite.rect;

        if (sprite.packed)
        {
            if (sprite.packingMode == SpritePackingMode.Rectangle)
                rect = sprite.textureRect;
            else
            {
                Debug.LogError ("Sprite Outline is not supported when Sprite Packer's packing policy is set to 'Tight'! Please change the packing policy to default ('Rectangle')");
                return;
            }
        }

        // If the sprite has changed, reupload the data to the shader.

        if (!UpdateOnlyOnSpriteChange || sprite != previousSprite || previoutPivot != pivot)
        {
            Vector2 uvMin = new Vector2 (rect.x / sprite.texture.width, rect.y / sprite.texture.height);
            Vector2 uvMax = new Vector2 ((rect.x + rect.width) / sprite.texture.width, (rect.y + rect.height) / sprite.texture.height);
            Vector4 spriteUVs = new Vector4 (uvMin.x, uvMin.y, uvMax.x, uvMax.y);
            mat.SetVector (property_SpriteUVs, spriteUVs);

            float size = mat.GetFloat (property_Size);

            float pivotCorrectionX = 0;
            float pivotCorrectionY = 0;

            if (spriteRenderer.flipX)
                pivotCorrectionX = rect.width / 2f - sprite.pivot.x;
            else
                pivotCorrectionX = -rect.width / 2f + sprite.pivot.x;

            if (spriteRenderer.flipY)
                pivotCorrectionY = rect.height / 2f - sprite.pivot.y;
            else
                pivotCorrectionY = -rect.height / 2f + sprite.pivot.y;

            Vector2 pivotCorrection = (size - 1) * new Vector2 (pivotCorrectionX, pivotCorrectionY) / sprite.pixelsPerUnit;
            Vector4 pivotVector = new Vector4 (pivotCorrection.x, pivotCorrection.y);
            mat.SetVector (property_PivotCorrection, pivotVector);
        }

        previoutPivot = pivot;
        previousSprite = sprite;
    }
}
