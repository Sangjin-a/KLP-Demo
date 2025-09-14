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
    private void Awake()
    {
        shinyEffect = GetComponent<ShinyEffectForUGUI>();
        //image = GetComponent<Image>();
    }
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ActiveDemo();
            StartCoroutine(CoBlurDeaActive());
            gameObject.GetComponent<RectTransform>().DOAnchorPos(deActivePos, 1.5f).SetEase(Ease.InBack).OnComplete(() =>
            {

            });
        });
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
            item.ResetTransform();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        shinyEffect.Play(0.5f, AnimatorUpdateMode.UnscaledTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //shinyEffect.Play(0.5f, AnimatorUpdateMode.UnscaledTime);
    }
}
