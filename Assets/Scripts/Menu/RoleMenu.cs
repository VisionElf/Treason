using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class RoleMenu : MonoBehaviour
    {
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
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(menuSound);
        }

        public void Show(Astronaut localAstronaut)
        {
            gameObject.SetActive(true);

            _animator.enabled = true;

            var role = localAstronaut.Role;
            var impostorsCount = 1;

            roleText.text = role.roleName;
            roleText.color = role.roleColor;
            backgroundImage.color = role.roleColor;

            descriptionText.text = "";
            if (role.roleName.Equals("Crewmate"))
                descriptionText.text = $"There is <color=red>{impostorsCount} Impostor</color> among us";
        }
    }
}
