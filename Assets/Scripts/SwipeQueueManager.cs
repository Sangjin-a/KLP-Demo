using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeQueueManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;  // �ּ� �������� �Ÿ�
    public float swipeTimeLimit = 1f;   // �ִ� �������� �ð�

    private Vector2 startPos;
    private float startTime;
    private bool isDragging = false;

    // �������� �̺�Ʈ
    public System.Action OnSwipeLeft;
    public System.Action OnSwipeRight;

    public Image center;
    public Image left;
    public Image right;

    public List<ImageItem> images; // �̹��� �迭

    private ImageItem centerItem;
    internal float animationDuration = 0.5f;

    private void Start()
    {
        if (images.Count < 3)
        {
            Debug.LogError("�̹����� 3�� �̻��̾�� �մϴ�!");
            return;
        }
        // �ʱ� �߾� �̹��� ����
        centerItem = images[1];
        centerItem.position = ImageItem.Position.Center;
        InitializePositions();
        //SwipeLeft();
    }
    void InitializePositions()
    {
        if (images.Count >= 3)
        {
            // 3�� �̻��� �� �ʱ� ��ġ
            images[0].SetPosition(ImageItem.Position.Left);
            images[1].SetPosition(ImageItem.Position.Center);
            images[2].SetPosition(ImageItem.Position.Right);

            // �������� ��Ȱ��ȭ
            for (int i = 3; i < images.Count; i++)
            {
                images[i].SetPosition(ImageItem.Position.Deactivated);
            }
        }
        else if (images.Count == 3)
        {
            // ��Ȯ�� 3���� ��
            images[0].SetPosition(ImageItem.Position.Left);
            images[1].SetPosition(ImageItem.Position.Center);
            images[2].SetPosition(ImageItem.Position.Right);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = eventData.position;
        startTime = Time.time;
        isDragging = true;

        Debug.Log("�巡�� ����!");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� �� (�ʿ�� �ǽð� ó��)
        if (isDragging)
        {
            Vector2 currentPos = eventData.position;
            float deltaX = currentPos.x - startPos.x;


        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 endPos = eventData.position;
        float deltaTime = Time.time - startTime;

        // �ð� ���� üũ
        if (deltaTime > swipeTimeLimit)
        {
            isDragging = false;
            return;
        }

        // �������� �Ÿ� ���
        Vector2 swipeVector = endPos - startPos;
        float swipeDistance = Mathf.Abs(swipeVector.x);

        // ���� �������� �ʹ� ũ�� ���� (������ �¿� ���������� ����)
        if (Mathf.Abs(swipeVector.y) > swipeDistance * 0.5f)
        {
            Debug.Log("���� �������� �ʹ� Ů�ϴ�!");
            isDragging = false;
            return;
        }

        // �������� �Ǵ�
        if (swipeDistance >= swipeThreshold)
        {
            if (swipeVector.x > 0)
            {
                OnSwipeRight?.Invoke();
                SwipeRight();

            }
            else
            {
                OnSwipeLeft?.Invoke();
                SwipeLeft();
            }
        }
        else
        {

        }

        isDragging = false;
    }
    public void SwipeLeft()
    {
        Debug.Log("���� ��������!");

        ImageItem currentCenter = GetImageAtPosition(ImageItem.Position.Center);
        ImageItem currentRight = GetImageAtPosition(ImageItem.Position.Right);
        ImageItem currentLeft = GetImageAtPosition(ImageItem.Position.Left);

        if (currentCenter == null || currentRight == null || currentLeft == null) return;

        // ��ġ ����
        currentCenter.SetPosition(ImageItem.Position.Left);
        currentRight.SetPosition(ImageItem.Position.Center);

        // ���ʿ� �ִ� �̹����� ���������� (��ȯ)
        currentLeft.SetPosition(ImageItem.Position.Right);
        //currentLeft.MoveRightInstant();

    }

    public void SwipeRight()
    {
        Debug.Log("������ ��������!");

        ImageItem currentCenter = GetImageAtPosition(ImageItem.Position.Center);
        ImageItem currentLeft = GetImageAtPosition(ImageItem.Position.Left);
        ImageItem currentRight = GetImageAtPosition(ImageItem.Position.Right);

        if (currentCenter == null || currentLeft == null || currentRight == null) return;

        // ��ġ ����
        currentCenter.SetPosition(ImageItem.Position.Right);
        currentLeft.SetPosition(ImageItem.Position.Center);

        // �����ʿ� �ִ� �̹����� �������� (��ȯ)
        currentRight.SetPosition(ImageItem.Position.Left);
        //currentRight.MoveLeftInstant();
    }
    ImageItem GetImageAtPosition(ImageItem.Position pos)
    {
        return images.Find(item => item.position == pos);
    }
}



