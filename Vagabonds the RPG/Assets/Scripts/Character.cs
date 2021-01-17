using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour // Az abstract megakadályozza azt, hogy ne húzd rá semmire véletlenül ezt a scriptet
{
    [SerializeField] private float speed;

    [SerializeField] private float initHealth;

    public Animator MyAnimator { get; set; }

    private Vector2 direction;

    private Rigidbody2D myRigidbody;

    public bool IsAttacking { get; set; }

    protected Coroutine attackRoutine;

    [SerializeField] protected Transform hitBox;

    [SerializeField] protected Stat health;
	
	public Stat Health
	{
		get { return health; }
	}

    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    protected virtual void Awake()
    {
        health.Initialize(initHealth, initHealth);

        myRigidbody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        myRigidbody.velocity = Direction.normalized * Speed; // ez mozgatja a karaktereket
    }

    public void HandleLayers()
    {
        if (IsMoving) 
        {
            ActivateLayer("WalkLayer"); // ez a mozgás animáció

            MyAnimator.SetFloat("x", Direction.x);
            MyAnimator.SetFloat("y", Direction.y);
        }

        else if (IsAttacking) ActivateLayer("AttackLayer");
        else ActivateLayer("IdleLayer"); // ez vált vissza az idle-re
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0) MyAnimator.SetTrigger("die"); // DIE
    }
}
