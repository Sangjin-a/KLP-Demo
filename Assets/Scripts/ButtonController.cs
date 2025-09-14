using Coffee.UIEffects;
using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ShinyEffectForUGUI))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ShinyEffectForUGUI shinyEffect;
    Button button;
    [SerializeField] private Image image;

    [SerializeField] private RandomMove[] randomMove;
    [SerializeField] private Vector3 deActivePos;

    [SerializeField] private GameObject demoPanel;
    [SerializeField] private float activeAnimDuration = 5f;

    [SerializeField] private GameObject popUpNotice;
    private UIEffect uIEffect;
    private bool isClicked = false;
    private void Awake()
    {
        shinyEffect = GetComponent<ShinyEffectForUGUI>();
        popUpNotice.SetActive(false);
        uIEffect = GetComponent<UIEffect>();
        uIEffect.edgeShinyAutoPlaySpeed = 0f;
        uIEffect.edgeShinyWidth = 0;

        //image = GetComponent<Image>();
    }
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            isClicked = true;
            ActiveDemo();
            StartCoroutine(CoBlurDeaActive());
            Sequence seq = DOTween.Sequence();
            seq.Append(gameObject.GetComponent<RectTransform>().DOAnchorPos(deActivePos, 1.5f).SetEase(Ease.InBack));
            //seq.Join(gameObject.transform.DOScale(1.5f, 1.5f));
            Invoke("DemoUIActive", activeAnimDuration);
        });
    }

    public void PopUpNoticeActive()
    {
        popUpNotice.SetActive(true);
    }
    public void DemoUIActive()
    {
        demoPanel.SetActive(true);
        Invoke("PopUpNoticeActive", 8);
    }
    private IEnumerator CoBlurDeaActive()
    {
        while (image.color.a >= 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.1f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ActiveDemo()
    {
        foreach (var item in randomMove)
        {
            item.ResetTransform(activeAnimDuration);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        shinyEffect.Play(0.5f, AnimatorUpdateMode.UnscaledTime);
        uIEffect.edgeShinyWidth = 0.2f;
        uIEffect.edgeShinyAutoPlaySpeed = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClicked == false)
        {
            uIEffect.edgeShinyAutoPlaySpeed = 0f;
            uIEffect.edgeShinyWidth = 0f;
        }
    }
}
