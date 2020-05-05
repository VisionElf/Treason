using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Gameplay.Data;
using Gameplay.Entities;

namespace Menu
{
    public class RoleMenu : MonoBehaviour
    {
        [Header("Role Menu")]
        public TMP_Text roleText;
        public TMP_Text descriptionText;
        public Image backgroundImage;
        public Image[] astronauts;
        public AudioClip menuSound;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
        }

        private void Start()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(menuSound);
        }

        public void Show(Astronaut localAstronaut)
        {
            gameObject.SetActive(true);

            _animator.enabled = true;

            RoleData role = localAstronaut.Role;
            int impostorsCount = 1;

            roleText.text = role.roleName;
            roleText.color = role.roleColor;
            backgroundImage.color = role.roleColor;

            descriptionText.text = "";
            if (role.roleName.Equals("Crewmate"))
                descriptionText.text = $"There is <color=red>{impostorsCount} Impostor</color> among us";
        }
    }
}
