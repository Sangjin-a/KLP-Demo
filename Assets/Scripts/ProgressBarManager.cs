using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    [SerializeField] private Image currentImage;
    [SerializeField] private TextMeshProUGUI value;
    void Start()
    {
        Sample();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Sample()
    {
        DOTween.To(() => currentImage.fillAmount, x => currentImage.fillAmount = x, 1f, 5f).SetEase(Ease.Linear).onUpdate = UpdateTextValue;
    }
    public void UpdateTextValue()
    {
        value.text = (currentImage.fillAmount * 100).ToString("0");
    }
}
