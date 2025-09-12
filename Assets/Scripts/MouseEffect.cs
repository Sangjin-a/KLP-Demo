using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MouseEffect : MonoBehaviour
{
    public Canvas canvas; // Assign the Canvas in the Inspector
    public GameObject mouseEffectPrefab; // Assign the prefab in the Inspector
    [Range(0, 50)] public int randomPosRange = 30; // Range for random position offset 
    ObjectPool<GameObject> effectPool;

    void Start()
    {
        effectPool = new ObjectPool<GameObject>(() =>
        {
            var obj = Instantiate(mouseEffectPrefab, canvas.transform);
            obj.SetActive(false);
            return obj;
        },
        obj =>
        {
            obj.SetActive(true);
        },
        obj =>
        {
            obj.SetActive(false);
        },
        obj =>
        {
            Destroy(obj);
        },
        false, 10, 50);
    }
    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer > 0.01f)
        {
            timer = 0f;
            var obj = effectPool.Get();
            obj.GetComponent<Image>().color = GetRandomColor();
            obj.transform.SetAsLastSibling();
            obj.GetComponent<Animator>().GetComponent<AnimationEventHandler>().onAnimationComplete = () =>
            {
                effectPool.Release(obj);
            };
            obj.transform.position = Input.mousePosition + GetRandomPos();
        }
    }

    public Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(-randomPosRange, randomPosRange), Random.Range(-randomPosRange, randomPosRange), 0);
    }

    public Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}
