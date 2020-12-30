using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour // Az abstract megakadályozza azt, hogy ne húzd rá semmire véletlenül ezt a scriptet
{
    [SerializeField] private float speed;

    protected Vector2 direction;
    void Start()
    {

    }

    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
