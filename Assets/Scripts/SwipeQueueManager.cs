using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeQueueManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;  // 최소 스와이프 거리
    public float swipeTimeLimit = 1f;   // 최대 스와이프 시간

    private Vector2 startPos;
    private float startTime;
    private bool isDragging = false;

    // 스와이프 이벤트
    public System.Action OnSwipeLeft;
    public System.Action OnSwipeRight;

    public Image center;
    public Image left;
    public Image right;

    public List<ImageItem> images; // 이미지 배열

    private ImageItem centerItem;
    internal float animationDuration = 0.5f;

    private void Start()
    {
        if (images.Count < 3)
        {
            Debug.LogError("이미지가 3개 이상이어야 합니다!");
            return;
        }
        // 초기 중앙 이미지 설정
        centerItem = images[1];
        centerItem.position = ImageItem.Position.Center;
        InitializePositions();
        //SwipeLeft();
    }
    void InitializePositions()
    {
        if (images.Count >= 3)
        {
            // 3개 이상일 때 초기 배치
            images[0].SetPosition(ImageItem.Position.Left);
            images[1].SetPosition(ImageItem.Position.Center);
            images[2].SetPosition(ImageItem.Position.Right);

            // 나머지는 비활성화
            for (int i = 3; i < images.Count; i++)
            {
                images[i].SetPosition(ImageItem.Position.Deactivated);
            }
        }
        else if (images.Count == 3)
        {
            // 정확히 3개일 때
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

        Debug.Log("드래그 시작!");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중 (필요시 실시간 처리)
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

        // 시간 제한 체크
        if (deltaTime > swipeTimeLimit)
        {
            isDragging = false;
            return;
        }

        // 스와이프 거리 계산
        Vector2 swipeVector = endPos - startPos;
        float swipeDistance = Mathf.Abs(swipeVector.x);

        // 수직 움직임이 너무 크면 무시 (순수한 좌우 스와이프만 감지)
        if (Mathf.Abs(swipeVector.y) > swipeDistance * 0.5f)
        {
            Debug.Log("세로 움직임이 너무 큽니다!");
            isDragging = false;
            return;
        }

        // 스와이프 판단
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
        Debug.Log("왼쪽 스와이프!");

        ImageItem currentCenter = GetImageAtPosition(ImageItem.Position.Center);
        ImageItem currentRight = GetImageAtPosition(ImageItem.Position.Right);
        ImageItem currentLeft = GetImageAtPosition(ImageItem.Position.Left);

        if (currentCenter == null || currentRight == null || currentLeft == null) return;

        // 위치 변경
        currentCenter.SetPosition(ImageItem.Position.Left);
        currentRight.SetPosition(ImageItem.Position.Center);

        // 왼쪽에 있던 이미지를 오른쪽으로 (순환)
        currentLeft.SetPosition(ImageItem.Position.Right);
        //currentLeft.MoveRightInstant();

    }

    public void SwipeRight()
    {
        Debug.Log("오른쪽 스와이프!");

        ImageItem currentCenter = GetImageAtPosition(ImageItem.Position.Center);
        ImageItem currentLeft = GetImageAtPosition(ImageItem.Position.Left);
        ImageItem currentRight = GetImageAtPosition(ImageItem.Position.Right);

        if (currentCenter == null || currentLeft == null || currentRight == null) return;

        // 위치 변경
        currentCenter.SetPosition(ImageItem.Position.Right);
        currentLeft.SetPosition(ImageItem.Position.Center);

        // 오른쪽에 있던 이미지를 왼쪽으로 (순환)
        currentRight.SetPosition(ImageItem.Position.Left);
        //currentRight.MoveLeftInstant();
    }
    ImageItem GetImageAtPosition(ImageItem.Position pos)
    {
        return images.Find(item => item.position == pos);
    }
}



