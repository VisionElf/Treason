using CustomExtensions;
using TMPro;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class Player : MonoBehaviour
{
    public TMP_Text playerNameText;
    public Animator animator;
    public float speed;
    public float diagonalMultiplier;
    public Transform flipTransform;
    public SpriteRenderer bodyRenderer;

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

        if (photonView && photonView.owner != null)
        {
            isLocalCharacter = photonView.isMine;
            playerNameText.text = photonView.owner.NickName;

            string colorStr = photonView.owner.GetCustomProperty("Color", "#FFFFFF");
            Color color = Color.white;
            if (ColorUtility.TryParseHtmlString(colorStr, out Color tmp))
                color = tmp;
            bodyRenderer.color = color;
        }

        if (!isLocalCharacter)
            visionMask.gameObject.SetActive(false);
        else
            visionMask.transform.localScale = visionRange / 2f * Vector3.one;
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
