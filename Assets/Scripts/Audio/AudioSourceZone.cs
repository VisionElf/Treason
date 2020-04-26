using Gameplay;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AudioSourceZone : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CheckIfLocalPlayer(collision.gameObject)) return;

        if (audioSource != null)
            audioSource.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CheckIfLocalPlayer(collision.gameObject)) return;

        if (audioSource != null)
            audioSource.enabled = false;
    }

    private bool CheckIfLocalPlayer(GameObject gameObject)
    {
        Astronaut player = gameObject.GetComponent<Astronaut>();

        if (player == null)
            return false;

        return player.isLocalCharacter;
    }
}
