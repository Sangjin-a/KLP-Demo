using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour
{
    public int index;
    private Image image;
    private SwipeQueueManager manager;
    public Position position = Position.Deactivated;
    [SerializeField] private bool shake = false;
    private void Awake()
    {
        image = GetComponent<Image>();
        manager = FindAnyObjectByType<SwipeQueueManager>();
    }

    private void Start()
    {
        if (shake)
        {
            // Shake Animation
            transform.DOShakePosition(1f, new Vector3(5f, 5f, 0), 10, 90, false, true).SetLoops(-1, LoopType.Yoyo);
            transform.DOShakeRotation(1f, new Vector3(0, 0, 5f), 10, 90, false).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void SetPosition(Position newPosition)
    {
        position = newPosition;

        switch (newPosition)
        {
            case Position.Left:
                MoveLeft();
                break;

            case Position.Center:
                MoveCenter();
                break;

            case Position.Right:
                MoveRight();
                break;

            case Position.Deactivated:
                Deactivate();
                break;
        }
    }

    // 애니메이션 있는 이동
    public void MoveCenter()
    {
        gameObject.SetActive(true);
        transform.DOMove(manager.center.transform.position, manager.animationDuration);
        transform.DOScale(Vector3.one * 1.3f, manager.animationDuration);
        image.DOFade(1f, manager.animationDuration);
    }

    public void MoveLeft()
    {
        gameObject.SetActive(true);
        transform.DOMove(manager.left.transform.position, manager.animationDuration);
        transform.DOScale(Vector3.one * 1.0f, manager.animationDuration);
        image.DOFade(0.7f, manager.animationDuration);
    }

    public void MoveRight()
    {
        gameObject.SetActive(true);
        transform.DOMove(manager.right.transform.position, manager.animationDuration);
        transform.DOScale(Vector3.one * 1.0f, manager.animationDuration);
        image.DOFade(0.7f, manager.animationDuration);
    }
    public void MoveLeftInstant()
    {
        gameObject.SetActive(true);
        transform.position = manager.left.transform.position;
        transform.localScale = Vector3.one * 1.0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
    }

    public void MoveRightInstant()
    {
        gameObject.SetActive(true);
        transform.position = manager.right.transform.position;
        transform.localScale = Vector3.one * 1.0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
    }
    public void Deactivate()
    {
        image.DOFade(0f, manager.animationDuration)
            .OnComplete(() => gameObject.SetActive(false));
    }



    public enum Position
    {
        Left,
        Center,
        Right,
        Deactivated
    }
}