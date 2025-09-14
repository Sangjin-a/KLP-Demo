using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoticeManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject noticeItem;
    [SerializeField] Transform noticeParent;
    [SerializeField] Transform popUpParent;
    private RectTransform rectTransformComponent;
    public Vector2 rectTransform;
    [SerializeField] private Button clearButton;
    private List<StandardNotice> noticeList = new List<StandardNotice>();
    public void Awake()
    {
        rectTransformComponent = GetComponent<RectTransform>();
        rectTransform = rectTransformComponent.anchoredPosition;
        noticeList = noticeParent.GetComponentsInChildren<StandardNotice>().ToList();
        clearButton.onClick.AddListener(() =>
        {
            StartCoroutine(ClearNotice());
        });
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public IEnumerator ClearNotice()
    {

        for (int i = 0; i < noticeList.Count; i++)
        {
            noticeList[i].DeActive();
            if (i < noticeList.Count - 1)
                noticeList[i].DeActive(() =>
            {
                DOTween.To(() => rectTransformComponent.anchoredPosition, x => rectTransformComponent.anchoredPosition = x, rectTransform, 1f).SetEase(Ease.OutExpo);
            });
            yield return new WaitForSeconds(0.2f);
        }
        noticeList.Clear();

    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransformComponent.anchoredPosition += new Vector2(0, eventData.delta.y);
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
