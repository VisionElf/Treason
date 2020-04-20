using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    public CameraFollow cameraFollow;
    public Player playerPrefab;

    private Player _localCharacter;

    public void Start()
    {
        if (PhotonNetwork.connected)
            _localCharacter = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0).GetComponent<Player>();
        else
        {
            _localCharacter = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            _localCharacter.isLocalCharacter = true;
        }
        cameraFollow.SetTarget(_localCharacter.transform);
    }

    public float GetDistanceToLocalCharacter(Vector3 position)
    {
        return Vector3.Distance(position, _localCharacter.transform.position);
    }
}
