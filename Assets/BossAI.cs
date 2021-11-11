using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private enum BossState
    {
        Standby,//default
        BowAttack,//level 0 attack, when players are in range
        SwordAttack,//level 1 attack, targets 1 player
        ScytheAttack//level 2 attack, targets 1 player
    };

    public GameObject projectile;
    public Sprite arrow;
    public Sprite sword;
    public Sprite scythe;

    [Header("Enabled Abilities")]
    //canStandby always true
    //canBowAttack always true
    public bool canSwordAttack;
    public bool canScytheAttack;

    private BossState state = BossState.Standby;
    private BossState prevState = BossState.Standby;
    private Vector2 standByPosition;
    private EnemyHealth BossHp;
    private TriggerBossRadius triggerRadius;
    private SpriteRenderer signal;
    private Animator anim;
    private Animator weaponAnim;
    private Animator coverAnim;

    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        standByPosition = transform.position;
        BossHp = GetComponent<EnemyHealth>();
        triggerRadius = transform.GetChild(0).GetComponent<TriggerBossRadius>();
        signal = transform.Find("Canvas").Find("SignalLight").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        weaponAnim = transform.Find("WeaponAnim").GetComponent<Animator>();
        coverAnim = transform.Find("CoverAnim").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.Standby:
                StandBy();
                break;
            case BossState.BowAttack:
                BowAttack();
                break;
            case BossState.SwordAttack:
                SwordAttack();
                break;
            case BossState.ScytheAttack:
                ScytheAttack();
                break;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<EnemyHealth>().loseHp(101);
        }

        if (GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            anim.SetTrigger("Death");
        }
    }


    private void StandBy()
    {
        if(triggerRadius.players_in_range != 0)
        {
            if(BossHp.currentHealth / BossHp.maxHealth <= 0.25 && canScytheAttack)
            {
                state = BossState.ScytheAttack;
            }
            else if (BossHp.currentHealth / BossHp.maxHealth <= 0.5 && canSwordAttack)
            {
                state = BossState.SwordAttack;
            }
            else
            {
                state = BossState.BowAttack;
            }
        }
        else
        {
            signal.color = Color.green;
        }
    }

    private void BowAttack()
    {
        if(triggerRadius.players_in_range == 0)
        {
            state = BossState.Standby;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth <= 0.25 && canScytheAttack)
        {
            state = BossState.ScytheAttack;
        }
        else if(BossHp.currentHealth / BossHp.maxHealth <= 0.5 && canSwordAttack)
        {
            state = BossState.SwordAttack;
        }
        else
        {
            signal.color = Color.magenta;

            if (canAttack)
            {
                anim.SetTrigger("Bow");
                weaponAnim.SetTrigger("Bow");
                canAttack = false;
                StartCoroutine(RadialShot(3f, 6, arrow));
            }

        }
    }

    private void SwordAttack()
    {
        if(triggerRadius.players_in_range == 0)
        {
            state = BossState.Standby;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth > 0.5)
        {
            state = BossState.BowAttack;
        }
        else if(BossHp.currentHealth / BossHp.maxHealth <= 0.25 && canScytheAttack)
        {
            state = BossState.ScytheAttack;
        }
        else
        {
            signal.color = Color.yellow;

            if (canAttack)
            {
                anim.SetTrigger("Sword");
                weaponAnim.SetTrigger("Sword");
                coverAnim.SetTrigger("Sword");
                canAttack = false;
                StartCoroutine(RadialShot(3f, 12, sword));
            }
        }
    }

    private void ScytheAttack()
    {
        if (triggerRadius.players_in_range == 0)
        {
            state = BossState.Standby;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth > 0.5)
        {
            state = BossState.BowAttack;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth > 0.25 && canSwordAttack)
        {
            state = BossState.SwordAttack;
        }
        else
        {
            signal.color = Color.red;

            if (canAttack)
            {
                anim.SetTrigger("Scythe");
                weaponAnim.SetTrigger("Scythe");
                coverAnim.SetTrigger("Scythe");
                canAttack = false;
                StartCoroutine(RadialShot(3f, 24, scythe));
            }
        }
    }

    private IEnumerator RadialShot(float cd, int numProjectiles, Sprite wep) //bow attack
    {
        float angleStep = 360 / numProjectiles;
        float angleStart = 0;
        for(int i = 0; i <= numProjectiles - 1; i++)
        {
            float x_dir = transform.position.x + Mathf.Sin(angleStart * Mathf.PI / 180) * 5;
            float y_dir = transform.position.y + Mathf.Cos(angleStart * Mathf.PI / 180) * 5;

            Vector2 projectileVector = new Vector2(x_dir, y_dir);
            Vector2 startPos = transform.position;
            Vector2 projectileMoveDir = (projectileVector - startPos).normalized * 3;

            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = projectileMoveDir;
            proj.transform.eulerAngles = new Vector3(0, 0, 270 - angleStart);
            proj.GetComponent<SpriteRenderer>().sprite = wep;
            StartCoroutine(ProjectileLifeLine(proj));

            angleStart += angleStep;
        }

        yield return new WaitForSeconds(cd);
        canAttack = true;
    }

    private IEnumerator ProjectileLifeLine(GameObject p)
    {
        yield return new WaitForSeconds(3f);
        Destroy(p);
    }

}
