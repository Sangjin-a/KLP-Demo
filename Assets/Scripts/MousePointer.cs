using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Camera mainCamera;
    public float emissionRate = 50f;
    public bool followMouse = true;

    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        emission = particleSystem.emission;
    }

    void Update()
    {
        if (followMouse)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            transform.position = worldPos;

            // 마우스 움직임에 따라 방출량 조절
            float mouseSpeed = Input.mousePosition.magnitude;
            emission.rateOverTime = emissionRate * (mouseSpeed * 0.01f + 0.5f);
        }
    }
}