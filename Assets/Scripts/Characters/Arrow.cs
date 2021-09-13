using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Orc _enemy;
    private GameObject _target;
    private Targetable _archer; 
    public float speed;
    private int _damage;

    // Start is called before the first frame update
    private void Start()
    {
        transform.parent = null;
    }

    public void SetUp(Orc enemy, Targetable archer, int damage)
    {
        _enemy = enemy;
        _target = enemy.arrowTarget;
        _damage = damage;
        _archer = archer;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_enemy == null || (_enemy != null && _enemy.isDead))
            Destroy(gameObject);
        else
        {
            transform.LookAt(_target.transform);
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Enemy))
        {
            other.transform.parent.gameObject.GetComponent<Targetable>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
