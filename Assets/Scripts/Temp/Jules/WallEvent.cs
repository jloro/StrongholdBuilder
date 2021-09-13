using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEvent : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Destroy_coroutine());
    }

    private IEnumerator Destroy_coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
