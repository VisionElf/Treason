using CustomExtensions;
using Data;
using Managers;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviourPun
{
    public ColorData colorData;
    
    public TMP_Text playerNameText;
    public Animator animator;
    public float speed;
    public Transform flipTransform;
    public Renderer spriteRenderer;

    public float visionRange;
    public SpriteMask visionMask;

    public bool isLocalCharacter;

    private bool _running;
    private Vector3 _previousPosition;
    private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
    private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
    private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

    private void Start()
    {
        _previousPosition = transform.position;
        if (photonView && photonView.Owner != null)
        {
            isLocalCharacter = photonView.IsMine;
            playerNameText.text = photonView.Owner.NickName;

            var colorStr = photonView.Owner.GetCustomProperty("Color", "#FFFFFF");
            var color = Color.white;
            if (ColorUtility.TryParseHtmlString(colorStr, out var tmp))
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

        if (!isLocalCharacter) return;

        var x = 0;
        var y = 0;

        if (Input.GetKey(KeyCode.Z))
            y = 1;
        if (Input.GetKey(KeyCode.S))
            y = -1;
        if (Input.GetKey(KeyCode.Q))
            x = -1;
        if (Input.GetKey(KeyCode.D))
            x = 1;

        Move(x, y);
    }

    private void LateUpdate()
    {
        var dist = GameManager.Instance.GetDistanceToLocalCharacter(transform.position);
        SetVisible(dist <= visionRange / 4f);
    }

    private void SetVisible(bool value)
    {
        playerNameText.enabled = value;
    }

    private void UpdateAnimations()
    {
        var dist = transform.position - _previousPosition;
        _running = dist.magnitude > 0f;

        if (_running && dist.x != 0f)
        {
            var flip = dist.x < 0 ? 1f : -1f;
            flipTransform.localScale = new Vector3(flip, 1f, 1f);
        }

        animator.SetFloat("Speed", speed);
        animator.SetBool("Running", _running);
        _previousPosition = transform.position;
    }

    private void Move(int x, int y)
    {
        foreach (var dir in new[] { new Vector3(x, y), new Vector3(x, 0), new Vector3(0, y) })
        {
            var direction = dir.normalized;
            var distance = speed * Time.deltaTime;
            if (!Physics2D.Raycast(transform.position, direction, distance))
            {
                transform.position += distance * direction;
                return;
            }
        }
    }
}
