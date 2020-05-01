﻿using System;
using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using Data;
using Gameplay.Actions.Data;
using Gameplay.Data;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public enum PlayerState
    {
        NORMAL,
        IN_VENT,
        DEAD,
        SPAWNING
    }

    public enum Direction
    {
        RIGHT,
        LEFT
    }

    public class Astronaut : MonoBehaviourPun, ITarget
    {
        public static Astronaut LocalAstronaut;
        public static List<Astronaut> Astronauts = new List<Astronaut>();

        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        private static readonly int AnimatorHashSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimatorHashRunning = Animator.StringToHash("Running");
        private static readonly int AnimatorHashKilled = Animator.StringToHash("Killed");

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

        [Header("Misc")]
        public bool isLocalCharacter;
        public ColorListData colorList;
        public TargetTypeData targetTypeData;

        [Header("Roles")]
        public RoleData[] roleList;
        public int debugRoleIndex;
        public float interactRange;

        [Header("Sounds")]
        public AudioClip spawnSound;

        private Vector3 _previousPosition;
        private Rigidbody2D _body;
        private Collider2D _hitbox;

        public bool IsRunning { get; private set; }
        public RoleData Role { get; private set; }
        public PlayerState State { get; set; }

        private AudioSource _audioSource;
        private Direction _facingDirection;

        private void Awake()
        {
            Astronauts.Add(this);
            targetTypeData.Add(this);

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
            targetTypeData.Remove(this);
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

            foreach (var ability in Role.abilities)
            {
                var target = ability.FindTarget(this);
                
                var context = new ActionContext(
                    Context.SourceAstronaut, this,
                    Context.TargetAstronaut, target
                );
                
                if (ability.CanInteract(context))
                {
                    if (Input.GetKeyDown(ability.shortcutKey))
                    {
                        ability.Execute(context);
                    }
                }
            }

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
                }
            }

            if (State != PlayerState.DEAD)
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
                SetFacingDirection(dist.x < 0 ? Direction.LEFT : Direction.RIGHT);

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
            animator.SetTrigger("Spawn");
        }

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

        public void Kill()
        {
            animator.SetTrigger(AnimatorHashKilled);
            State = PlayerState.DEAD;
            playerNameText.color = Color.clear;
        }

        public Vector3 GetCenter()
        {
            Vector2 pos = transform.position;
            return pos + _hitbox.offset;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
