using CustomExtensions;
using Data;
using Managers;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviourPun
{
    private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
    private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
    private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

    public ColorData colorData;

    public TMP_Text playerNameText;
    public Animator animator;
    public float speed;
    public float diagonalMultiplier;
    public Transform flipTransform;
    public Renderer spriteRenderer;

    public float visionRange;
    public SpriteMask visionMask;

    public bool isLocalCharacter;

    private bool _running;
    private Vector3 _previousPosition;
    private Vector3 _facingDirection;

    private void Start()
    {
        _previousPosition = transform.localPosition;
        _facingDirection = Vector3.right;

        if (photonView && photonView.Owner != null)
        {
            isLocalCharacter = photonView.IsMine;
            playerNameText.text = photonView.Owner.NickName;

            string colorStr = photonView.Owner.GetCustomProperty("Color", "#FFFFFF");
            Color color = Color.white;
            if (ColorUtility.TryParseHtmlString(colorStr, out Color tmp))
                color = tmp;
            SetColor(color);
        }

        if (!isLocalCharacter)
            visionMask.gameObject.SetActive(false);
        else
            visionMask.transform.localScale = visionRange / 2f * Vector3.one;
    }

    private void OnDrawGizmosSelected()
    {
        SetColor(colorData, true);
    }

    private void SetColor(Color color)
    {
        if (!spriteRenderer) return;

        var material = spriteRenderer.material;

        material.SetColor(ShaderColor1, color);
        material.SetColor(ShaderColor2, Color.white);
        material.SetColor(ShaderColor3, color);
    }

    private void SetColor(ColorData data, bool useShared)
    {
        if (!data || !spriteRenderer) return;

        var material = useShared ? spriteRenderer.sharedMaterial : spriteRenderer.material;

        material.SetColor(ShaderColor1, data.color1);
        material.SetColor(ShaderColor2, data.color2);
        material.SetColor(ShaderColor3, data.color3);
    }

    public void Update()
    {
        UpdateAnimations();
        UpdateDepth();

        if (!isLocalCharacter) return;

        Move(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));
    }

    private void LateUpdate()
    {
        float dist = GameManager.Instance.GetDistanceToLocalCharacter(transform.position);
        SetVisible(dist <= visionRange / 4f);
    }

    private void SetVisible(bool value)
    {
        playerNameText.enabled = value;
    }

    private void UpdateAnimations()
    {
        Vector3 dist = transform.localPosition - _previousPosition;
        _running = dist.magnitude > 0f;

        if (_running && dist.x != 0f)
            SetFacingDirection(dist.x < 0 ? Vector3.left : Vector3.right);

        animator.SetFloat("Speed", speed);
        animator.SetBool("Running", _running);
        _previousPosition = transform.localPosition;
    }

    private void UpdateDepth()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y);
    }

    private void SetFacingDirection(Vector3 direction)
    {
        if (_facingDirection == direction) return;

        _facingDirection = direction;
        flipTransform.localScale = Vector3.Scale(flipTransform.localScale, new Vector3(-1f, 1f, 1f));
    }

    private void Move(int x, int y)
    {
        if (x == 0 && y == 0) return;

        float distance = speed * Time.deltaTime;

        foreach (Vector3 dir in new[] { new Vector3(x, y), new Vector3(x, 0), new Vector3(0, y) })
        {
            Vector3 direction = dir.normalized;

            if (!Physics2D.Raycast(transform.localPosition, direction, distance))
            {
                transform.position += distance * direction;
                return;
            }
        }
    }
}
