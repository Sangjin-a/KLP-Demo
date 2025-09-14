using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StandardNotice : MonoBehaviour
{
    LayoutElement layoutElement;
    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
    }


    public void DeActive(Action action = null)
    {
        //layoutElement.ignoreLayout = true;
        gameObject.transform.DOMoveX(1000, 2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            action?.Invoke();
        });
    }
}
