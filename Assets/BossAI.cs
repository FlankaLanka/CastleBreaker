using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private enum BossState
    {
        Standby,//default
        Attack,//attack if players are in range
        AngryAttack,//angry if hp lower than <x>
    };

    public GameObject projectile;

    private BossState state = BossState.Standby;
    private BossState prevState = BossState.Standby;
    private Vector2 standByPosition;
    private EnemyHealth BossHp;
    private TriggerBossRadius triggerRadius;
    private SpriteRenderer signal;

    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        standByPosition = transform.position;
        BossHp = GetComponent<EnemyHealth>();
        triggerRadius = transform.GetChild(0).GetComponent<TriggerBossRadius>();
        signal = transform.Find("Canvas").Find("SignalLight").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BossState.Standby:
                StandBy();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.AngryAttack:
                AngryAttack();
                break;
        }
    }


    private void StandBy()
    {
        if(triggerRadius.players_in_range != 0)
        {
            if(BossHp.currentHealth / BossHp.maxHealth > 0.25)
            {
                state = BossState.Attack;
            }
            else
            {
                state = BossState.AngryAttack;
            }
        }
        else
        {
            signal.color = Color.green;
            //heal boss
            BossHp.currentHealth = BossHp.maxHealth;
        }
    }

    private void Attack()
    {
        if(triggerRadius.players_in_range == 0)
        {
            state = BossState.Standby;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth <= 0.25)
        {
            state = BossState.AngryAttack;
        }
        else
        {
            signal.color = Color.magenta;

            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(RadialShot(2f, 6));
            }

        }
    }

    private void AngryAttack()
    {
        if(triggerRadius.players_in_range == 0)
        {
            state = BossState.Standby;
        }
        else if (BossHp.currentHealth / BossHp.maxHealth > 0.25)
        {
            state = BossState.Attack;
        }
        else
        {
            signal.color = Color.red;

            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(RadialShot(1f, 12));
            }
        }
    }

    private IEnumerator RadialShot(float cd, int numProjectiles)
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
