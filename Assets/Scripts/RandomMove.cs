using DG.Tweening;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RandomMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] public RectTransform rectTransform;

    private float punchScale = 1.5f;
    private float resetSpeed = 2f;
    private Vector3 originPos;


    private Camera cam;
    private Vector3 screenBounds;
    private Vector3 dir;
    private Vector3[] corners = new Vector3[4];
    private bool canMove = true;

    private void Awake()
    {
        originPos = transform.position;
    }
    void Start()
    {
        cam = Camera.main;
        dir = GetRandomDirection() * Time.deltaTime * 5f;
        corners[0] = cam.ViewportToWorldPoint(new Vector3(0, 0, 10f));//���� �ϴ�
        corners[1] = cam.ViewportToWorldPoint(new Vector3(0, 1, 10f));//���� ���
        corners[2] = cam.ViewportToWorldPoint(new Vector3(1, 0, 10f));//���� �ϴ�
        corners[3] = cam.ViewportToWorldPoint(new Vector3(1, 1, 10f));//���� ���

        // z�� 0���� ����
        for (int i = 0; i < 4; i++) corners[i].z = 0;
        foreach (var item in corners)
        {
            GameObject b = new GameObject("BoundsMarker");
            b.transform.position = item;
        }
    }

    void Update()
    {
        if (canMove)
        {
            gameObject.transform.position += dir * moveSpeed;
            CheckBounds();
        }
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0).normalized;
    }

    /// <summary>
    /// ��� üũ
    /// </summary>
    void CheckBounds()
    {
        Vector3 pos = transform.position;
        if (transform.position.x <= corners[0].x || transform.position.x >= corners[3].x)
        {
            dir.x = -dir.x; // X�� ����
            pos.x = Mathf.Clamp(pos.x, corners[0].x, corners[3].x);
            Effect();
        }

        if (transform.position.y >= corners[1].y || transform.position.y <= corners[0].y)
        {
            dir.y = -dir.y; // Y�� ����
            pos.y = Mathf.Clamp(pos.y, corners[0].y, corners[1].y);
            Effect();
        }

    }

    /// <summary>
    /// ��Կ� ����� �� ȿ��
    /// </summary>
    public void Effect()
    {
        gameObject.transform.DOPunchScale(new Vector3(punchScale, punchScale, 0), 0.2f, 10, 1).SetUpdate(true);
    }

    /// <summary>
    /// ��ư Ŭ�� �̺�Ʈ�� ���
    /// </summary>
    public void ResetTransform()
    {
        canMove = false;
        gameObject.transform.DOKill();
        gameObject.transform.DOMove(originPos, 5).SetUpdate(true);
    }

    private void OnDrawGizmos()
    {
        if (cam == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(screenBounds.x * 2, screenBounds.y * 2, 0));
    }
}
