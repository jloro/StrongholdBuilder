using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsCamera : MonoBehaviour
{
    public float speed = 5.0f;
    public float zoomSpeed = 1.0f;
    public Vector2 xLimit;
    public Vector2 yLimit;
    public Vector2 zLimit;
    public GameObject currentPosition;

    private void HandleMouse()
    {
        
        Vector2 mousePos = Input.mousePosition;
        float currentSpeed = speed * 5.0f * (Time.timeScale == 0 ? 0: Time.unscaledDeltaTime);
        Vector3 nextPos = transform.position;
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        if (mousePos.x < 10)
        {
            nextPos.x -= currentSpeed;
        }
        else if (mousePos.x > Screen.width - 10)
        {
            nextPos.x += currentSpeed;
        }
        if (mousePos.y < 10)
        {
            nextPos.z -= currentSpeed;
        }
        else if (mousePos.y > Screen.height -10)
        {
            nextPos.z += currentSpeed;
        }
        if (zoom != 0.0f)
        {
            nextPos.y += -zoom * zoomSpeed * 10.0f;
        }
        
        nextPos.x = Mathf.Clamp(nextPos.x, xLimit.x, xLimit.y);
        nextPos.y = Mathf.Clamp(nextPos.y, yLimit.x, yLimit.y);
        nextPos.z = Mathf.Clamp(nextPos.z, zLimit.x, zLimit.y);
        transform.position = nextPos;

        currentPosition.transform.localScale = new Vector3(16.0f * 2.0f * (nextPos.y / yLimit.y), 1, 9.0f * 2.0f * (nextPos.y / yLimit.y));
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit info;

        if (Physics.Raycast(ray, out info))
        {
            currentPosition.transform.position = new Vector3(info.point.x, 750, info.point.z);
        }
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Escape))
            return;
#endif
        HandleMouse();
    }
}
