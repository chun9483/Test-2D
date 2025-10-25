using UnityEngine;

public class MarkerFloat : MonoBehaviour
{
    public float floatSpeed = 1f; // �B�ʳt��
    public float floatAmplitude = 0.2f; // �B�ʴT��
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // �ϥ� Time.time ��{�����i�B�ʡA������Y�b�W�U�B��
        Vector3 newPos = startPos;
        newPos.y += Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = newPos;
    }
}