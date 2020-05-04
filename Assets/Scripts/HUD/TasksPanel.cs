using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TasksPanel : MonoBehaviour
{
    public RectTransform taskList;
    public LayoutElement panelElt;
    public Button tabButton;
    public float slideDuration = 0.4f;

    private RectTransform _rectTransform;
    private bool _isShowing;

    private void Awake()
    {
        _isShowing = true;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        tabButton.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        tabButton.onClick.RemoveListener(Toggle);
    }

    private void Toggle()
    {
        _isShowing = !_isShowing;
        var mult = _isShowing ? 0f : -1f;

        var pos = _rectTransform.anchoredPosition;
        pos.x = mult * panelElt.preferredWidth;
        _rectTransform.DOComplete();
        _rectTransform.DOAnchorPos(pos, slideDuration);
    }

    private void Update()
    {
        if (taskList && panelElt)
        {
            panelElt.preferredWidth = taskList.sizeDelta.x;
        }
    }
}
