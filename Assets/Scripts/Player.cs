using TMPro;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class Player : MonoBehaviour
{
    public TMP_Text playerNameText;
    public Animator animator;
    public float speed;
    public Transform flipTransform;
    
    public float visionRange;
    public SpriteMask visionMask;
    
    public bool isLocalCharacter;

    private bool _running;
    private Vector3 _previousPosition;

    private void Start()
    {
        _previousPosition = transform.position;
        if (photonView && photonView.owner != null)
        {
            isLocalCharacter = photonView.isMine;
            playerNameText.text = photonView.owner.NickName;
        }

        if (!isLocalCharacter)
            visionMask.gameObject.SetActive(false);
        else
            visionMask.transform.localScale = visionRange / 2f * Vector3.one;
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
        _running = dist.magnitude > 0.01f;

        if (_running && dist.x != 0f)
        {
            var flip = dist.x < 0 ? 1f : -1f;
            flipTransform.localScale = new Vector3(flip, 1f, 1f);
        }
        
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
