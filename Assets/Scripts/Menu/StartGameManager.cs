using System.Collections;
using Gameplay;
using Managers;
using UnityEngine;

namespace Menu
{
    public class StartGameManager : MonoBehaviour
    {
        public float preDelay;
        public float postDelay;
        public GameObject shPanel;
        public RoleMenu rolePanel;

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(preDelay);

            Astronaut localAstronaut = null;
            while (localAstronaut == null)
            {
                localAstronaut = Astronaut.LocalAstronaut;
                yield return null;
            }

            shPanel.SetActive(false);
            rolePanel.Show(localAstronaut);
            
            yield return new WaitForSeconds(postDelay);

            gameObject.SetActive(false);
        }
    }
}
