using UnityEngine;

public class MarkerFloat : MonoBehaviour
{
    public float floatSpeed = 1f; // 浮動速度
    public float floatAmplitude = 0.2f; // 浮動幅度
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 使用 Time.time 實現正弦波運動，讓物件Y軸上下浮動
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = newPos;
    }
}