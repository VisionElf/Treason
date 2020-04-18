using TMPro;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

public class Player : MonoBehaviour
{
    public TMP_Text playerNameText;
    public Animator animator;
    public float speed;
    public Transform flipTransform;

    private bool _running;

    private Vector3 _previousPosition;

    private void Start()
    {
        _previousPosition = transform.position;
        playerNameText.text = photonView.owner.NickName;
    }
    
    public void Update()
    {
        UpdateAnimations();
        
        if (!photonView.isMine) return;
        
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
        transform.position += speed * Time.deltaTime * new Vector3(x, y).normalized;
    }
}
