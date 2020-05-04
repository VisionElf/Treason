using UnityEngine;

namespace Gameplay.Tasks
{
    public class SetCourseCheckpoint : MonoBehaviour
    {
        public GameObject defaultParent;
        public GameObject reachedParent;
        public GameObject destinationParent;

        private RectTransform _rectTransform;

        public Vector2 AnchoredPosition
        {
            get => _rectTransform.anchoredPosition;
            set => _rectTransform.anchoredPosition = value;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            defaultParent.SetActive(true);
            reachedParent.SetActive(false);
            destinationParent.SetActive(false);
        }

        public void SetReached()
        {
            defaultParent.SetActive(false);
            reachedParent.SetActive(true);
        }

        public void SetDestination()
        {
            destinationParent.SetActive(true);
        }
    }
}
