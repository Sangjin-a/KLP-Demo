using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoticeManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject noticeItem;
    [SerializeField] Transform noticeParent;

    private RectTransform rectTransformComponent;
    public Vector2 rectTransform;
    public void Awake()
    {
        rectTransformComponent = GetComponent<RectTransform>();
        rectTransform = rectTransformComponent.anchoredPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position += new Vector3(0, eventData.delta.y, 0);
        Debug.Log(rectTransformComponent.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransformComponent.anchoredPosition.y > -350)
        {
            DOTween.To(() => rectTransformComponent.anchoredPosition, x => rectTransformComponent.anchoredPosition = x, new Vector2(0, 0), 0.5f).SetEase(Ease.OutExpo);
        }
        else
            DOTween.To(() => rectTransformComponent.anchoredPosition, x => rectTransformComponent.anchoredPosition = x, rectTransform, 0.5f).SetEase(Ease.OutExpo);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
