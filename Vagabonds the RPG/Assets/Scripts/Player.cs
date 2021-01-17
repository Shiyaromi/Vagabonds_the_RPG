using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Stat mana;
    [SerializeField] private Stat exp;

    private float initMana = 50;
    private float initExp = 0;
    
    [SerializeField] private Block[] blocks;

    [SerializeField] private Transform[] exitPoints;
        
    private int exitIndex = 3;

    private SpellBook spellBook;

    private Vector3 min, max;

    public Transform MyTarget { get; set; }

    protected override void Awake()
    {
        spellBook = GetComponent<SpellBook>();
        mana.Initialize(initMana, initMana);
        exp.Initialize(initExp, initExp);

        base.Awake();
    }

    protected override void Update()
    {
        GetInput();

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
        //    Mathf.Clamp(transform.position.y, min.y, max.y), 
        //    transform.position.z);

        base.Update();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.I)) health.MyCurrentValue -= 10;
        if (Input.GetKeyDown(KeyCode.O)) health.MyCurrentValue += 10;

        if (Input.GetKey(KeyCode.W)) Direction += Vector2.up;
        if (Input.GetKey(KeyCode.A)) Direction += Vector2.left;
        if (Input.GetKey(KeyCode.S)) Direction += Vector2.down;
        if (Input.GetKey(KeyCode.D)) Direction += Vector2.right;

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_back_left")) exitIndex = 0;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_back_right")) exitIndex = 1;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_front_left")) exitIndex = 2;
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("warven_idle_front_right")) exitIndex = 3;

        if (IsMoving) StopAttack();
    }

    //public void SetLimits(Vector3 min, Vector3 max)
    //{
    //    this.min = min;
    //    this.max = max;
    //}

    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = spellBook.CastSpell(spellIndex);

        IsAttacking = true;

        MyAnimator.SetBool("attack", IsAttacking);

        yield return new WaitForSeconds(newSpell.CastTime);

        if (MyTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget, newSpell.Damage);
        }

        StopAttack();
    }

    public void CastSpell(int spellIndex)
    {
        Block();

        if (MyTarget != null && !IsAttacking && !IsMoving && InLineOfSight()) attackRoutine = StartCoroutine(Attack(spellIndex));
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null) return true;                       
        }

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

    public void StopAttack()
    {
        spellBook.StopCasting();

        IsAttacking = false;

        MyAnimator.SetBool("attack", IsAttacking);

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
}
