using UnityEngine;
using UnityEngine.UI;

using Gameplay.Entities;

namespace Managers
{
    public class GameEventManager : SingletonMB<GameEventManager>
    {
        [Header("Sounds")]
        public AudioClip deadBodyReportedSound;
        public AudioClip emergencyMeetingSound;

        [Header("References")]
        public Animator animator;
        public AudioSource audioSource;
        public Image[] astronautImages;

        private static readonly int AnimatorHashAnim = Animator.StringToHash("Anim");
        private static readonly int AnimatorHashDeadBodyReported = Animator.StringToHash("DeadBodyReported");
        private static readonly int AnimatorHashEmergencyMeeting = Animator.StringToHash("EmergencyMeeting");

        public void ReportDeadBody(Astronaut reporter, AstronautBody body)
        {
            foreach (Image image in astronautImages)
                body.ApplyColor(image.material);

            audioSource.PlayOneShot(deadBodyReportedSound);
            animator.SetBool(AnimatorHashDeadBodyReported, true);
            animator.SetBool(AnimatorHashEmergencyMeeting, false);
            animator.SetTrigger(AnimatorHashAnim);
            Destroy(body.gameObject);
        }

        public void EmergencyMeeting(Astronaut source)
        {
            foreach (Image image in astronautImages)
                source.ApplyColor(image.material);

            audioSource.PlayOneShot(emergencyMeetingSound);
            animator.SetBool(AnimatorHashDeadBodyReported, false);
            animator.SetBool(AnimatorHashEmergencyMeeting, true);
            animator.SetTrigger(AnimatorHashAnim);
        }
    }
}
