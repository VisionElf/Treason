using System.Collections;
using UnityEngine;

namespace Menu
{
    public class StartGameManager : MonoBehaviour
    {
        public float preDelay;
        public float postDelay;
        public GameObject shPanel;
        public GameObject rolePanel;

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(preDelay);

            shPanel.SetActive(false);
            rolePanel.SetActive(true);
            
            yield return new WaitForSeconds(postDelay);

            gameObject.SetActive(false);
        }
    }
}
