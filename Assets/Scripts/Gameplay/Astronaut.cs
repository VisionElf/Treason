using System;
using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using Data;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Interactables;

namespace Gameplay
{
    public enum PlayerState
    {
        NORMAL,
        IN_VENT,
        GHOST,
        SPAWNING
    }

    public enum Direction
    {
        RIGHT,
        LEFT
    }

    public class Astronaut : MonoBehaviourPun
    {
        public static Astronaut LocalAstronaut;
        public static List<Astronaut> Astronauts = new List<Astronaut>();

        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        private static readonly int AnimatorHashSpawn = Animator.StringToHash("Spawn");
        private static readonly int AnimatorHashSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimatorHashRunning = Animator.StringToHash("Running");
        private static readonly int AnimatorHashGhost = Animator.StringToHash("Ghost");

        private const string GhostSortingLayerName = "Ghost";
        private const string GhostNameSortingLayerName = "GhostName";

        [Header("Visual")]
        public TMP_Text playerNameText;
        public SpriteRenderer outline;

        [Header("Animation")]
        public float minAnimationSpeed;
        public float maxAnimationSpeed;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Transform graphics;

        [Header("Movement")]
        public float speed;

        [Header("Vision")]
        public float visionRange;
        public LayerMask visibleLayerMask;
        public GameObject fieldOfViewPrefab;

        [Header("Misc")]
        public bool isLocalCharacter;
        public ColorListData colorList;

        [Header("Dead")]
        public GameObject deadAstronautPrefab;

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
        private DeadAstronaut _currentReportTarget;

        public bool IsRunning { get; private set; }
        public RoleData Role { get; private set; }
        public PlayerState State { get; set; }

        public static Action OnKillInteractEnable;
        public static Action OnKillInteractDisable;

        public static Action OnReportInteractEnable;
        public static Action OnReportInteractDisable;

        private AudioSource _audioSource;
        private FieldOfView _fieldOfView;
        private Direction _facingDirection;

        private void Awake()
        {
            Astronauts.Add(this);

            _body = GetComponent<Rigidbody2D>();
            _hitbox = GetComponent<Collider2D>();
            _audioSource = GetComponent<AudioSource>();
            State = PlayerState.NORMAL;

            if (photonView && photonView.Owner != null)
            {
                var roleIndex = photonView.Owner.GetColorIndex();
                SetRole(roleIndex);

                var colorIndex = photonView.Owner.GetRoleIndex();
                SetColor(colorIndex);
            }
        }

        private void OnDestroy()
        {
            Astronauts.Remove(this);
        }

        private IEnumerator Start()
        {
            _previousPosition = transform.localPosition;

            if (photonView && photonView.Owner != null)
            {
                isLocalCharacter = photonView.IsMine;
                playerNameText.text = photonView.Owner.NickName;

                if (!isLocalCharacter && SceneManager.GetActiveScene().buildIndex == 1)
                    Spawn();
            }
            else
            {
                SetRole(debugRoleIndex);
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

            if (isLocalCharacter && Input.GetKeyDown(KeyCode.Space))
                Kill();

            if ((State == PlayerState.NORMAL || State == PlayerState.GHOST) && isLocalCharacter)
                Move(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));
        }

        private void LateUpdate()
        {
            outline.sprite = spriteRenderer.sprite;
            outline.transform.localPosition = spriteRenderer.transform.localPosition;
            outline.transform.localScale = spriteRenderer.transform.localScale;

            if (!isLocalCharacter && State != PlayerState.GHOST)
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
                        visible = !Physics2D.Raycast(localPos, dir.normalized, dist, visibleLayerMask);

                    SetVisible(visible);

                    if (visible && dist <= interactRange && localCharacter.IsImpostor() && !IsImpostor())
                        localCharacter.SetKillInteract(this, dist);
                    else
                        localCharacter.RemoveKillInteract(this);
                }
            }

            UpdateAnimations();
        }

        private void SetRole(int roleIndex)
        {
            Role = roleList[roleIndex];
        }

        private void SetColor(int colorIndex)
        {
            SetColor(colorList.list[colorIndex]);
        }

        private void SetColor(ColorData data)
        {
            if (!data || !spriteRenderer) return;

            var material = spriteRenderer.material;
            material.SetColor(ShaderColor1, data.color1);
            material.SetColor(ShaderColor2, data.color2);
            material.SetColor(ShaderColor3, data.color3);
        }

        private bool IsImpostor()
        {
            return Role.roleName.Equals("Impostor");
        }

        private void SetVisible(bool value)
        {
            if (State != PlayerState.GHOST)
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
                SetFacingDirection(dist.x < 0 ? Direction.LEFT : Direction.RIGHT);

            _previousPosition = transform.localPosition;

            if (State != PlayerState.GHOST)
            {
                animator.SetFloat(AnimatorHashSpeed, Mathf.Clamp(speed, minAnimationSpeed, maxAnimationSpeed));
                animator.SetBool(AnimatorHashRunning, IsRunning);
            }
        }

        private void UpdateDepth()
        {
            var position = transform.localPosition;
            position.z = position.y / 1000f;
            transform.localPosition = position;
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

        private void OnSpawnBegin()
        {
            State = PlayerState.SPAWNING;
            _audioSource.PlayOneShot(spawnSound);
            animator.SetTrigger(AnimatorHashSpawn);
        }

        // Animation Event
        private void OnSpawnEnd()
        {
            State = PlayerState.NORMAL;
        }

        public void SetFacingDirection(Direction dir)
        {
            if (_facingDirection == dir) return;
            _facingDirection = dir;
            Vector3 newDirection = new Vector3(-1f, 1f, 1f);

            graphics.localScale = Vector3.Scale(graphics.localScale, newDirection);
            playerNameText.rectTransform.localScale = Vector3.Scale(playerNameText.rectTransform.localScale, newDirection);
        }

        public Vector2 GetPosition2D() => transform.position;

        #region Controls
        public void WalkIn(Vector3 direction) => _body.velocity = direction * speed;
        public void WalkTowards(Vector3 point) => WalkIn((point - transform.position).normalized);
        public void ResetSpeed() => _body.velocity = Vector3.zero;
        public void Spawn() => OnSpawnBegin();
        public void UpdateAstronaut()
        {
            if (!PhotonNetwork.IsConnected) return;
            SetColor(photonView.Owner.GetColorIndex());
        }
        #endregion Controls

        #region Kill
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

        public void Kill()
        {
            GameObject deadAstronaut = Instantiate(deadAstronautPrefab, transform.position, transform.rotation, transform.parent);
            deadAstronaut.transform.localScale = transform.localScale;
            _hitbox.enabled = false;
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.25f);
            spriteRenderer.sortingLayerID = SortingLayer.NameToID(GhostSortingLayerName);
            State = PlayerState.GHOST;
            animator.SetTrigger(AnimatorHashGhost);
        }

        public void KillTarget()
        {
            _currentKillTarget.Kill();
            RemoveKillInteract(_currentKillTarget);
        }
        #endregion Kill

        #region Report
        public void RemoveReportInteract(DeadAstronaut target)
        {
            if (_currentReportTarget == target)
            {
                _currentReportTarget = null;
                OnReportInteractDisable?.Invoke();
            }
        }

        public void SetReportInteract(DeadAstronaut target, float dist)
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

        public void ReportTarget()
        {
            if (_currentReportTarget)
            {
                _currentReportTarget.transform.position = new Vector2(-5000, -5000);
                RemoveReportInteract(_currentReportTarget);
            }
        }
        #endregion Report
    }
}
