using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam2D : MonoBehaviour
{
    public float speed = 5.0f;
    public bool isBoundaries;
    public Vector2 minBound;
    public Vector2 maxBound;
    private void CheckBoundaries(ref Vector3 nextPos)
    {
        if (nextPos.x < minBound.x) { nextPos.x = minBound.x; }
        if (nextPos.x > maxBound.x) { nextPos.x = maxBound.x; }
        if (nextPos.y < minBound.y) { nextPos.y = minBound.y; }
        if (nextPos.y > maxBound.y) { nextPos.y = maxBound.y; }
    }
    private void HandleMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        float currentSpeed = speed * 5.0f * Time.unscaledDeltaTime;
        Vector3 nextPos = transform.position;

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
            nextPos.y -= currentSpeed;
        }
        else if (mousePos.y > Screen.height - 10)
        {
            nextPos.y += currentSpeed;
        }
        if (isBoundaries) { CheckBoundaries(ref nextPos); }
        transform.position = nextPos;
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
