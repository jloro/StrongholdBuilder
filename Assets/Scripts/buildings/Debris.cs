using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private BoxCollider _collider;
    public GameObject rock;
    public Vector3 offset;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }
    public void Setup(BoxCollider other, float time = 5f)
    {
        _collider.center = other.center;
        _collider.size = other.size;
        transform.position = other.transform.position;
        //transform.localScale = other.transform.localScale;
        transform.rotation = other.transform.rotation;
        Vector3 start = _collider.center + transform.position
            - transform.right.normalized * (_collider.size.x / 2.0f)
            - transform.forward.normalized * (_collider.size.y / 2.0f)
            - transform.up.normalized * (_collider.size.y / 2.0f);
        start += transform.right.normalized * offset.x / 2.0f + transform.forward * offset.z / 2.0f;
        Vector3 pos = start;
        float x = Mathf.Round(_collider.size.x / offset.x);
        float z = Mathf.Round(_collider.size.z / offset.z);
        for (int i = 1; i <= x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Instantiate(rock, pos, Quaternion.identity, transform);
                pos += transform.forward.normalized * offset.z;
            }
            pos = start + transform.right.normalized * offset.x * i;
        }

        StartCoroutine(Dispawn_coroutine(time));

    }

    private IEnumerator Dispawn_coroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
