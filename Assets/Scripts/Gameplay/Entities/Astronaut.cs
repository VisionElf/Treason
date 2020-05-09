using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

using CustomExtensions;
using Data;
using Gameplay.Abilities;
using Gameplay.Abilities.Data;
using Gameplay.Data;
using Gameplay.Entities.Data;

namespace Gameplay.Entities
{
    public enum AstronautState
    {
        Normal,
        Ghost
    }

    public enum Direction
    {
        Right,
        Left
    }

    public class Astronaut : MonoBehaviourPun, IEntity
    {
        public static Astronaut LocalAstronaut;
        public static List<Astronaut> Astronauts = new List<Astronaut>();

        private static readonly int AnimatorHashSpawn = Animator.StringToHash("Spawn");
        private static readonly int AnimatorHashSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimatorHashRunning = Animator.StringToHash("Running");
        private static readonly int AnimatorHashGhost = Animator.StringToHash("Ghost");

        private const string GhostLayerName = "Ghost";

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
        public int debugColorIndex;
        public EntityTypeData entityType;

        [Header("Dead")]
        public EventData localAstronautToGhostEvent;
        public GameObject astronautBodyPrefab;

        [Header("Roles")]
        public RoleData[] roleList;
        public int debugRoleIndex;

        [Header("Sounds")]
        public AudioClip spawnSound;

        public bool IsRunning { get; private set; }
        public RoleData Role { get; private set; }
        public AstronautState State { get; set; }
        public List<Ability> Abilities { get; private set; }

        public ColorData ColorData { get; private set; }
        private Vector3 _previousPosition;
        private Rigidbody2D _body;
        private Collider2D _hitbox;
        private AudioSource _audioSource;
        private Direction _facingDirection;
        private bool _isFrozen;

        private void Awake()
        {
            Astronauts.Add(this);
            entityType.Add(this);

            _body = GetComponent<Rigidbody2D>();
            _hitbox = GetComponent<Collider2D>();
            _audioSource = GetComponent<AudioSource>();
            _isFrozen = false;
            State = AstronautState.Normal;

            if (photonView && photonView.Owner != null)
            {
                SetRole(photonView.Owner.GetRoleIndex());
                SetColor(photonView.Owner.GetColorIndex());
            }
            else
            {
                SetColor(debugColorIndex);
                SetRole(debugRoleIndex);
            }
        }

        private void Start()
        {
            _previousPosition = transform.localPosition;

            if (photonView && photonView.Owner != null)
            {
                playerNameText.text = photonView.Owner.NickName;

                if (!isLocalCharacter && SceneManager.GetActiveScene().buildIndex == 1)
                    Spawn();
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

            SetHighlight(false);
            
            if (isLocalCharacter || Role == LocalAstronaut.Role)
                playerNameText.color = Role.playerNameColor;
            else
                playerNameText.color = Color.white;
        }

        public void CreateAbilities()
        {
            Abilities = new List<Ability>();
            foreach (AbilityData abilityData in Role.abilities)
                Abilities.Add(new Ability(abilityData, this));
        }

        public void RemoveNonGhostAbilities()
        {
            List<Ability> currentAbilities = new List<Ability>(Abilities);
            foreach (Ability ability in currentAbilities)
                if (!ability.GhostKeepAbility) Abilities.Remove(ability);
        }

        private void OnDestroy()
        {
            entityType.Remove(this);
            Astronauts.Remove(this);
        }

        public void Update()
        {
            UpdateDepth();

            Abilities?.ForEach((a) => a.Update());

            if (isLocalCharacter && CanMove())
                Move(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")));
        }

        private void LateUpdate()
        {
            outline.sprite = spriteRenderer.sprite;
            outline.transform.localPosition = spriteRenderer.transform.localPosition;
            outline.transform.localScale = spriteRenderer.transform.localScale;

            if (!isLocalCharacter && State != AstronautState.Ghost)
            {
                Astronaut localCharacter = LocalAstronaut;
                if (localCharacter && localCharacter.State != AstronautState.Ghost)
                {
                    Vector2 pos = transform.position;
                    Vector2 localPos = localCharacter.transform.position;

                    Vector2 dir = pos - localPos;
                    float dist = dir.magnitude;

                    bool visible = false;
                    if (dist <= visionRange)
                        visible = !Physics2D.Raycast(localPos, dir.normalized, dist, visibleLayerMask);

                    SetVisible(visible);
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
            ColorData = colorList.list[colorIndex];
            spriteRenderer.material = ColorData.material;
        }

        private void SetVisible(bool visible)
        {
            playerNameText.enabled = visible;
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
                SetFacingDirection(dist.x < 0 ? Direction.Left : Direction.Right);

            _previousPosition = transform.localPosition;

            if (State == AstronautState.Normal)
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

        private bool CanMove() => !_isFrozen;

        public void Freeze()
        {
            ResetSpeed();
            _isFrozen = true;
        }
        public void Unfreeze()
        {
            ResetSpeed();
            _isFrozen = false;
        }

        private void OnSpawnBegin()
        {
            Freeze();
            _audioSource.PlayOneShot(spawnSound);
            animator.SetTrigger(AnimatorHashSpawn);
        }

        // Animation Event
        private void OnSpawnEnd()
        {
            Unfreeze();
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

        public void WalkIn(Vector3 direction) => _body.velocity = direction * speed;
        public void WalkTowards(Vector3 point) => WalkIn((point - transform.position).normalized);
        public void ResetSpeed() => _body.velocity = Vector3.zero;
        public void Spawn() => OnSpawnBegin();
        public void UpdateAstronaut()
        {
            if (!PhotonNetwork.IsConnected) return;
            SetColor(photonView.Owner.GetColorIndex());
        }

        public void Kill()
        {
            GameObject deadAstronaut = Instantiate(astronautBodyPrefab, transform.position, transform.rotation, transform.parent);
            deadAstronaut.transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y);
            deadAstronaut.GetComponent<AstronautBody>().SetColor(ColorData);
            ToGhost();
        }

        public void ToGhost()
        {
            State = AstronautState.Ghost;
            gameObject.SetLayerRecursively(GhostLayerName);
            spriteRenderer.sortingLayerName = GhostLayerName;
            _hitbox.isTrigger = true;
            animator.SetTrigger(AnimatorHashGhost);

            if (isLocalCharacter)
            {
                RemoveNonGhostAbilities();

                foreach (Camera camera in FindObjectsOfType<Camera>())
                    camera.cullingMask |= 1 << LayerMask.NameToLayer(GhostLayerName);

                foreach (Astronaut astronaut in FindObjectsOfType<Astronaut>())
                    astronaut.SetVisible(true);

                localAstronautToGhostEvent.TriggerEvent();
            }
        }

        public Vector3 GetCenter() => (Vector2)transform.position + _hitbox.offset;
        public Vector3 GetPosition() => transform.position;
        public void SetOutline(bool value) { }
        public void SetHighlight(bool value) => outline.enabled = value;
        
        public string GetColorName()
        {
            return ColorData.colorName;
        }
    }
}
