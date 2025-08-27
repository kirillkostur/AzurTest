using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;    // персонаж
    public float distance = 5f; // расстояние до персонажа
    public float sensitivity = 3f; // чувствительность мыши
    public float yMin = -20f;
    public float yMax = 60f;

    private float rotX = 20f;
    private float rotY = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        rotY += Input.GetAxis("Mouse X") * sensitivity;
        rotX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotX = Mathf.Clamp(rotX, yMin, yMax);

        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}