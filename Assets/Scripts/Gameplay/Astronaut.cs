using CustomExtensions;
using Data;
using Managers;
using Photon.Pun;
using TMPro;
using UnityEngine;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Gameplay
{
    public class Astronaut : MonoBehaviourPun
    {
        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        private static readonly int AnimatorHashSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimatorHashRunning = Animator.StringToHash("Running");

        [Header("Visual")]
        public TMP_Text playerNameText;

        [Header("Animation")]
        public float minAnimationSpeed;
        public float maxAnimationSpeed;

        [Header("Movement")]
        public float speed;
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        [Header("Vision")]
        public float visionRange;
        public SpriteMask visionMask;

        [Header("Misc")]
        public bool isLocalCharacter;
        public RoleData[] roleList;
        public ColorData[] colorList;

        private Vector3 _previousPosition;
        private Rigidbody2D _body;
        private Collider2D _hitbox;

        public bool IsRunning { get; private set; }
        public RoleData Role { get; private set; }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _hitbox = GetComponent<Collider2D>();

            if (photonView && photonView.Owner != null)
            {
                string roleStr = photonView.Owner.GetCustomProperty("RoleIndex", "0");
                SetRole(roleStr);
                
                string colorStr = photonView.Owner.GetCustomProperty("ColorIndex", "0");
                SetColor(int.Parse(colorStr));
            }
        }

        private void Start()
        {
            _previousPosition = transform.localPosition;

            if (photonView && photonView.Owner != null)
            {
                isLocalCharacter = photonView.IsMine;
                playerNameText.text = photonView.Owner.NickName;
            }
            else
            {
                //Debug
                SetRole(Random.Range(0, roleList.Length).ToString());
            }

            if (!isLocalCharacter)
            {
                if (visionMask) visionMask.gameObject.SetActive(false);
                _body.bodyType = RigidbodyType2D.Static;
                _hitbox.enabled = false;
            }
            else
            {
                if (visionMask) visionMask.transform.localScale = visionRange / 2f * Vector3.one;
                gameObject.AddComponent<AudioListener>();
            }
        }

        private void SetRole(string roleStr)
        {
            var index = int.Parse(roleStr);
            Role = roleList[index];
        }

        private void SetColor(int colorIndex)
        {
            SetColor(colorList[colorIndex], false);
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
            UpdateDepth();

            if (!isLocalCharacter) return;

            Move(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));
        }

        private void LateUpdate()
        {
            float dist = GameManager.Instance.GetDistanceToLocalCharacter(transform.position);
            SetVisible(dist <= visionRange / 4f);

            UpdateAnimations();
        }

        private void SetVisible(bool value)
        {
            playerNameText.enabled = value;
        }

        private void UpdateAnimations()
        {
            Vector2 dist = transform.localPosition - _previousPosition;
            if (dist.x.AlmostEquals(0f, 0.001f))
                dist.x = 0f;
            if (dist.y.AlmostEquals(0f, 0.001f))
                dist.y = 0f;
            IsRunning = dist.magnitude > 0f;
            if (IsRunning && dist.x != 0)
                SetFacingDirection(dist.x < 0 ? Vector3.left : Vector3.right);

            _previousPosition = transform.localPosition;
            animator.SetFloat(AnimatorHashSpeed, Mathf.Clamp(speed, minAnimationSpeed, maxAnimationSpeed));
            animator.SetBool(AnimatorHashRunning, IsRunning);
        }

        private void UpdateDepth()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y / 1000f);
        }

        private void SetFacingDirection(Vector3 direction)
        {
            spriteRenderer.flipX = direction == Vector3.left;
        }

        private void Move(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                _body.velocity = Vector3.zero;
                return;
            }

            var direction = new Vector3(x, y).normalized;
            _body.velocity = direction * speed;
        }

        public Vector3 GetPosition2D()
        {
            return new Vector3(transform.position.x, transform.position.y, 0);
        }

        public Vector2 GetCenter()
        {
            Vector2 pos = transform.position;
            return pos + _hitbox.offset;
        }
    }
}
