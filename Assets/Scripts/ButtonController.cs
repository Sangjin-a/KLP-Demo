using Coffee.UIExtensions;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ShinyEffectForUGUI))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ShinyEffectForUGUI shinyEffect;
    Button button;

    [SerializeField] private RandomMove[] randomMove;
    private void Awake()
    {
        shinyEffect = GetComponent<ShinyEffectForUGUI>();
    }
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ActiveDemo());
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
