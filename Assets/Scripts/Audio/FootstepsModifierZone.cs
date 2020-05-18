using Audio;
using Gameplay.Entities;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FootstepsModifierZone : MonoBehaviour
{
    [Header("FootstepsModifier")]
    public FootstepType type;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Astronaut astronaut = collision.gameObject.GetComponent<Astronaut>();
        if (astronaut != null) astronaut.footsteps.SetCurrentType(type);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Astronaut astronaut = collision.gameObject.GetComponent<Astronaut>();
        if (astronaut != null) astronaut.footsteps.SetCurrentType();
    }
}
