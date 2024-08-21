using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    public Transform target;
    public bool isBottom = false;
    public int attackDamage = 1;
    public float maxY = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if(target != null)
        {
            Turn();
        }
        if(transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }
    void Turn()
    {
        var pos = target.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(pos);
        transform.rotation = rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<Monster>().isBottom == isBottom)
        {

            other.GetComponent<Monster>().Damage(attackDamage);
            Destroy(gameObject);
        }
    }
}
