using Managers;
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

        private void Start()
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(menuSound);

            var role = GameManager.Instance.LocalAstronaut.Role;
            roleText.text = role.roleName;
            roleText.color = role.roleColor;
            backgroundImage.color = role.roleColor;

            descriptionText.text = "";
            if (role.roleName.Equals("Crewmate"))
                descriptionText.text = $"There is <color=red>{1} Impostor</color> among us";
        }
    }
}
