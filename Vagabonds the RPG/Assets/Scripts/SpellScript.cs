using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [SerializeField] private float speed;
    
    public Transform MyTarget { get; private set; }

    private int damage;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>(); // Creates a reference to the spell's rigidbody
    }

    public void Initialize(Transform target, int damage)
    {
        this.MyTarget = target;
        this.damage = damage;
    }
    
    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position; // Calculate the spells direction

            myRigidbody.velocity = direction.normalized * speed; // Moves the spell by using rigidbody

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculates the rotation angle

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Rotates the spell towards the target
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidbody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}
