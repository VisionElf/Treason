using System.Collections;
using UnityEngine;

using Gameplay;

[RequireComponent(typeof(Collider2D))]
public class AudioSourceZone : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxVolume;
    public float fadeInDuration;
    public float fadeOutDuration;

    private Coroutine _fadeIn;
    private Coroutine _fadeOut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CheckIfLocalPlayer(collision.gameObject)) return;

        if (audioSource != null)
        {
            if (_fadeOut != null) StopCoroutine(_fadeOut);
            if (_fadeIn != null) StopCoroutine(_fadeIn);
            _fadeIn = StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CheckIfLocalPlayer(collision.gameObject)) return;

        if (audioSource != null)
        {
            if (_fadeOut != null) StopCoroutine(_fadeOut);
            if (_fadeIn != null) StopCoroutine(_fadeIn);
            _fadeOut = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.enabled = true;

        float time = 0f;

        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            audioSource.volume = (time / fadeInDuration) * maxVolume;
            yield return null;
        }

        audioSource.volume = maxVolume;
        yield return null;
    }

    private IEnumerator FadeOut()
    {
        float time = fadeOutDuration;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            audioSource.volume = (time / fadeOutDuration) * maxVolume;
            yield return null;
        }
        audioSource.enabled = false;
        audioSource.volume = maxVolume;
        yield return null;
    }

    private bool CheckIfLocalPlayer(GameObject gameObject)
    {
        Astronaut player = gameObject.GetComponent<Astronaut>();

        if (player == null)
            return false;

        return player.isLocalCharacter;
    }
}
