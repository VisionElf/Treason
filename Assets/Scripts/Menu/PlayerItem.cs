using TMPro;
using UnityEngine;

namespace Menu
{
    public class PlayerItem : MonoBehaviour
    {
        public TMP_Text playerNameText;

        public void SetPlayer(PhotonPlayer player)
        {
            playerNameText.text = player.NickName;
        }
    }
}
