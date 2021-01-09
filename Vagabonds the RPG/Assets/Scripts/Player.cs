using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat health;
    [SerializeField] private Stat mana;
    [SerializeField] private Stat exp;

    private float initHealth = 100;
    private float initMana = 50;
    private float initExp = 0;

    [SerializeField] private GameObject[] spellPrefab;

    [SerializeField] private Block[] blocks;

    [SerializeField] private Transform[] exitPoints;

    private int exitIndex;

    private Transform target;

    protected override void Awake()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        exp.Initialize(initExp, initExp);

        target = GameObject.Find("Target").transform;

        base.Awake();
    }

    protected override void Update()
    {
        GetInput();

        base.Update();
    }

    private void GetInput()
    {
        direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.I)) health.MyCurrentValue -= 10;
        if (Input.GetKeyDown(KeyCode.O)) health.MyCurrentValue += 10;

        if (Input.GetKey(KeyCode.W)) direction += Vector2.up;
        if (Input.GetKey(KeyCode.A)) direction += Vector2.left;
        if (Input.GetKey(KeyCode.S)) direction += Vector2.down;
        if (Input.GetKey(KeyCode.D)) direction += Vector2.right;

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_back_left")) exitIndex = 0;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_back_right")) exitIndex = 1;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_front_left")) exitIndex = 2;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_front_right")) exitIndex = 3;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Block();

            if (!isAttacking && !IsMoving && InLineOfSight()) attackRoutine = StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        myAnimator.SetBool("attack", isAttacking);

        yield return new WaitForSeconds(3);

        CastSpell();

        StopAttack();
    }

    public void CastSpell()
    {
        Instantiate(spellPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);
    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), 256);

        if (hit.collider == null) return true;

        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }
}
