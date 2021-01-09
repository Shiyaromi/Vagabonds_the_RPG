using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour // Az abstract megakadályozza azt, hogy ne húzd rá semmire véletlenül ezt a scriptet
{
    [SerializeField] private float speed;

    protected Animator myAnimator;

    protected Vector2 direction;

    private Rigidbody2D myRigidbody;

    protected bool isAttacking;

    protected Coroutine attackRoutine;

    public bool IsMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        myRigidbody.velocity = direction.normalized * speed; // ez mozgatja a karaktereket
    }

    public void HandleLayers()
    {
        if (IsMoving) 
        {
            ActivateLayer("WalkLayer"); // ez a mozgás animáció

            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack();
        }

        else if (isAttacking) ActivateLayer("AttackLayer");
        else ActivateLayer("IdleLayer"); // ez vált vissza az idle-re
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }

        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            myAnimator.SetBool("attack", isAttacking);
        }
    }
}
