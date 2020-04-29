using System;
using System.Collections;
using CustomExtensions;
using Data;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Photon.Realtime;

namespace Gameplay
{
    public enum PlayerState
    {
        NORMAL,
        IN_VENT,
        DEAD
    }

    public class Astronaut : MonoBehaviourPun
    {
        public static Astronaut LocalAstronaut;

        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        private static readonly int AnimatorHashSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimatorHashRunning = Animator.StringToHash("Running");
        private static readonly int AnimatorHashKilled = Animator.StringToHash("Killed");

        [Header("Visual")]
        public TMP_Text playerNameText;

        [Header("Animation")]
        public float minAnimationSpeed;
        public float maxAnimationSpeed;

        [Header("Movement")]
        public float speed;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public float visionRange;
        public LayerMask visibleLayerMask;
        public SpriteRenderer outline;

        [Header("Misc")]
        public bool isLocalCharacter;
        public ColorData[] colorList;

        [Header("Roles")]
        public RoleData[] roleList;
        public int debugRoleIndex;
        public float interactRange;

        [Header("Sounds")]
        public AudioClip spawnSound;

        private Vector3 _previousPosition;
        private Rigidbody2D _body;
        private Collider2D _hitbox;

        private Astronaut _currentKillTarget;
        private Astronaut _currentReportTarget;

        public bool IsRunning { get; private set; }
        public RoleData Role { get; private set; }
        public PlayerState State { get; set; }

        public static Action OnKillInteractEnable;
        public static Action OnKillInteractDisable;

        public static Action OnReportInteractEnable;
        public static Action OnReportInteractDisable;

        private AudioSource _audioSource;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _hitbox = GetComponent<Collider2D>();
            _audioSource = GetComponent<AudioSource>();
            State = PlayerState.NORMAL;

            if (photonView && photonView.Owner != null)
            {
                string roleStr = photonView.Owner.GetCustomProperty("RoleIndex", "0");
                SetRole(roleStr);

                string colorStr = photonView.Owner.GetCustomProperty("ColorIndex", "0");
                SetColor(int.Parse(colorStr));
            }
        }

        private IEnumerator Start()
        {
            _previousPosition = transform.localPosition;

            if (photonView && photonView.Owner != null)
            {
                isLocalCharacter = photonView.IsMine;
                playerNameText.text = photonView.Owner.NickName;
            }
            else
            {
                SetRole(debugRoleIndex.ToString());
            }

            if (!isLocalCharacter)
            {
                _body.bodyType = RigidbodyType2D.Static;
                _hitbox.enabled = false;
            }
            else
            {
                gameObject.AddComponent<AudioListener>();
            }

            if (isLocalCharacter) LocalAstronaut = this;

            outline.enabled = false;

            yield return null;

            DelayedSetup();
        }

        private void DelayedSetup()
        {
            if (isLocalCharacter || Role == LocalAstronaut.Role)
                playerNameText.color = Role.playerNameColor;
            else
                playerNameText.color = Color.white;
        }

        public void Update()
        {
            UpdateDepth();

            if (State == PlayerState.NORMAL && isLocalCharacter)
                Move(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));
        }

        private void LateUpdate()
        {
            outline.sprite = spriteRenderer.sprite;
            outline.transform.localPosition = spriteRenderer.transform.localPosition;
            outline.transform.localScale = spriteRenderer.transform.localScale;

            if (!isLocalCharacter)
            {
                var localCharacter = LocalAstronaut;
                if (localCharacter)
                {
                    Vector2 pos = transform.position;
                    Vector2 localPos = localCharacter.transform.position;

                    var dir = pos - localPos;
                    var dist = dir.magnitude;

                    var visible = false;
                    if (dist <= visionRange)
                    {
                        visible = !Physics2D.Raycast(localPos, dir.normalized, dist, visibleLayerMask);
                    }

                    SetVisible(visible);

                    if (State != PlayerState.DEAD)
                    {
                        if (visible && dist <= interactRange && localCharacter.IsImpostor() && !IsImpostor())
                            localCharacter.SetKillInteract(this, dist);
                        else
                            localCharacter.RemoveKillInteract(this);
                    }
                    else
                    {
                        if (visible && dist <= interactRange)
                            localCharacter.SetReportInteract(this, dist);
                        else
                            localCharacter.RemoveReportInteract(this);
                    }
                }
            }

            if (State != PlayerState.DEAD)
                UpdateAnimations();
        }

        private void SetRole(string roleStr)
        {
            var index = int.Parse(roleStr);
            Role = roleList[index];
        }

        private void SetColor(int colorIndex)
        {
            SetColor(colorList[colorIndex]);
        }

        private void SetColor(ColorData data)
        {
            if (!data || !spriteRenderer) return;

            var material = spriteRenderer.material;
            material.SetColor(ShaderColor1, data.color1);
            material.SetColor(ShaderColor2, data.color2);
            material.SetColor(ShaderColor3, data.color3);
        }

        private void RemoveKillInteract(Astronaut target)
        {
            if (_currentKillTarget == target)
            {
                _currentKillTarget.HideOutline();
                _currentKillTarget = null;

                OnKillInteractDisable?.Invoke();
            }
        }

        private void SetKillInteract(Astronaut target, float dist)
        {
            if (_currentKillTarget == target) return;

            if (_currentKillTarget)
            {
                var currDist = Vector2.Distance(_currentKillTarget.transform.position, transform.position);
                if (dist >= currDist) return;
                RemoveKillInteract(_currentKillTarget);
            }

            _currentKillTarget = target;
            target.ShowOutline();

            OnKillInteractEnable?.Invoke();
        }

        private void RemoveReportInteract(Astronaut target)
        {
            if (_currentReportTarget == target)
            {
                _currentReportTarget = null;

                OnReportInteractDisable?.Invoke();
            }
        }

        private void SetReportInteract(Astronaut target, float dist)
        {
            if (_currentReportTarget == target) return;

            if (_currentReportTarget)
            {
                var currDist = Vector2.Distance(_currentReportTarget.transform.position, transform.position);
                if (dist >= currDist) return;
                RemoveReportInteract(_currentReportTarget);
            }

            _currentReportTarget = target;
            OnReportInteractEnable?.Invoke();
        }

        private bool IsImpostor()
        {
            return Role.roleName.Equals("Impostor");
        }

        public void Kill()
        {
            animator.SetTrigger(AnimatorHashKilled);
            State = PlayerState.DEAD;
            playerNameText.color = Color.clear;
        }

        public void KillTarget()
        {
            _currentKillTarget.Kill();
            RemoveKillInteract(_currentKillTarget);
        }

        public void ReportTarget()
        {
            if (_currentReportTarget)
            {
                _currentReportTarget.transform.position = new Vector2(-5000, -5000);
                RemoveReportInteract(_currentReportTarget);
            }
        }

        private void SetVisible(bool value)
        {
            playerNameText.enabled = value;
        }

        private void HideOutline()
        {
            outline.enabled = false;
        }

        private void ShowOutline()
        {
            outline.enabled = true;
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
            var position = transform.localPosition;
            position.z = position.y / 1000f;
            transform.localPosition = position;
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

            WalkIn(new Vector3(x, y).normalized);
        }

        public Vector2 GetPosition2D() => transform.position;
        public void WalkIn(Vector3 direction) => _body.velocity = direction * speed;
        public void WalkTowards(Vector3 point) => WalkIn((point - transform.position).normalized);
        public void ResetSpeed() => _body.velocity = Vector3.zero;

        public void OnSpawn()
        {
            _audioSource.PlayOneShot(spawnSound);
            animator.SetTrigger("Spawn");
        }
    }
}
