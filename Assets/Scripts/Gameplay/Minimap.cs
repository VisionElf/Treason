using Gameplay;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Minimap : MonoBehaviour
{
    public Transform astronautIcon;
    public Vector3 offset;
    public float scale;
    public GameObject graphics;

    private Astronaut _player;

    private void Start()
    {
        _player = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            graphics.SetActive(!graphics.activeSelf);

        if (!graphics.activeSelf) return;

        if (_player == null)
            _player = FindObjectsOfType<Astronaut>().First((p) => p.isLocalCharacter);

        astronautIcon.localPosition = (_player.transform.position / scale) - offset;
    }
}
