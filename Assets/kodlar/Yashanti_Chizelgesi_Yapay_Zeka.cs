using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class Yashanti_Chizelgesi_Yapay_Zeka : MonoBehaviour
{
    public string targetTag;
    public string Running_Anim;
    public string dead_Anim;

    public float distance;
    public float age, strength, social_skills, smithing_skills, carpenter_skills, lockpicing_skills, speed, punch_figth_knowlech, sword_figth_knowlech, bow_figth_knowlech, magic_figth_knowlech;
    public float health, maxhealth;
    public float searchRadius = 10000f;
    public float attackDistance = 30f;
    public float rotationSpeed;

    public bool yasam = true;
    public int asker_tipi; 
    public bool saldırıyor = false;

    public Animator animator;
    public GameObject closestObject;
    public GameObject gun, bullet;
    public can_gostergesi can;
    public Transform gun_position_rotation;
    public NavMeshAgent navMeshAgent;
    public Color originalColor;
    public Renderer childRenderer;
    public Collider enemyCollider;

    void Start()
    {   
        yasam = true;
        childRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
        can = GetComponent<can_gostergesi>();

        renk();

        punch_figth_knowlech = Random.RandomRange(2, 9);
        sword_figth_knowlech = Random.RandomRange(5, 16);
        bow_figth_knowlech = Random.RandomRange(4, 13);
        magic_figth_knowlech = Random.RandomRange(10, 21);

        health = Random.RandomRange(30, 99);
        speed = Random.RandomRange(11, 34);

        maxhealth = health;

        rotationSpeed = Random.RandomRange(5, 11);
        gameObject.name = $"{gameObject.tag}_noc_{Random.Range(00000000, 99999999)}";
        navMeshAgent.speed = speed;

        string[] runAnimations = { "Running", "Running_1", "Running_2"};
        Running_Anim = runAnimations[Random.Range(0, runAnimations.Length)];
        string[] run_dead_Animations = { "dead", "dead_1", "dead_2", "dead_3" };
        dead_Anim = run_dead_Animations[Random.Range(0, run_dead_Animations.Length)];
    }

    public void Update()
    {   
        if (yasam == false || health <= 0)
            {
                StartCoroutine(Dead());
            }
    }

    void FixedUpdate()
    {
        switch (asker_tipi)
        {
            case 0: // dovuscu
                attackDistance = 25;
                break;

            case 1: // kılıçlı
                attackDistance = 35;
                break;

            case 2: // okçu
                attackDistance = 250;
                break;
                
            case 3: // büyücü
                attackDistance = 250;
                break;
        }
        if (yasam)
        {   
            FindClosestObject();
            if (closestObject != null)
            {
                look();
                PerformActions();
            }
            else
            {
                Idle();
            }
        }
    }

    public void FindClosestObject()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistanceSquared = Mathf.Infinity;
        GameObject closestObj = null;

        foreach (GameObject taggedObject in taggedObjects)
        {
            if (taggedObject == gameObject || taggedObject.GetComponent<HW_Yapay_Zeka>().yasam == false)
            {
                continue;
            }

            float distanceSquared = (transform.position - taggedObject.transform.position).sqrMagnitude;

            if (distanceSquared < closestDistanceSquared && distanceSquared <= searchRadius * searchRadius)
            {
                closestDistanceSquared = distanceSquared;
                closestObj = taggedObject;
            }
        }
        closestObject = closestObj;
    }

    public void PerformActions()
    {
        distance = (transform.position - closestObject.transform.position).sqrMagnitude;
        if (distance <= attackDistance && !saldırıyor && yasam == true)
        {
            navMeshAgent.SetDestination(this.gameObject.transform.position);
            StartCoroutine(StartAttack());
        }
        if(distance > attackDistance)
        {
            MoveToTarget();
        }
    }
        
    private IEnumerator StartAttack()
    {
        if(closestObject != null)
        {
            saldırıyor = true;
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch (1)")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Uppercut")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("UpperCut (1)")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Punching")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Punching (1)")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Hook Punch")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Hook Punch (1)")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash 0")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash 0 0")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash 0 0 0")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Standing 2H Magic Attack 01")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Standing 2H Magic Attack 02")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Standing 2H Magic Attack 02 (1)")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Standing 2H Magic Attack 05")
            || !animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting Arrow")           
            )
               {
                    string randomAttackAnim;
                    float animationLength;
                    GameObject ok;
                    Rigidbody rigid;
                    Collider targetCollider = closestObject.GetComponent<Collider>();
                    if (targetCollider != null && enemyCollider.bounds.Intersects(targetCollider.bounds))
                    {
                        switch (asker_tipi)
                        {
                            case 0: // dovuscu
                                        string[] attackAnimations = { "punch_left_1", "punch_left_2", "punch_left_3", "punch_right_1", "punch_right_2", "punch_right_3"};
                                        randomAttackAnim = attackAnimations[Random.Range(0, attackAnimations.Length)];
                                        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                                        animator.SetTrigger(randomAttackAnim);
                                        yield return new WaitForSeconds(animationLength);
                            break;
                            
                            case 1: // kılıçlı
                                        string[] attackAnimations_1 = { "sword_attack_0", "sword_attack_1", "sword_attack_2", "sword_attack_3" };
                                        randomAttackAnim = attackAnimations_1[Random.Range(0, attackAnimations_1.Length)];
                                        animator.SetTrigger(randomAttackAnim);
                                        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                                        yield return new WaitForSeconds(animationLength);
                            break;

                            case 2: // okçu
                                        string[] attackAnimations_2 = { "bow_arrow" };
                                        randomAttackAnim = attackAnimations_2[Random.Range(0, attackAnimations_2.Length)];
                                        animator.SetTrigger(randomAttackAnim);
                                        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                                        ok = Instantiate(bullet, gun_position_rotation.position,gun_position_rotation.rotation);
                                        rigid = ok.GetComponent<Rigidbody>();
                                        ok.SendMessage("veri_al", this.gameObject.name, SendMessageOptions.DontRequireReceiver);
                                        rigid.AddForce(gun_position_rotation.forward * 58f, ForceMode.Impulse);
                                        yield return new WaitForSeconds(animationLength);
                            break;

                            case 3: // büyücü
                                        string[] attackAnimations_3 = { "magic_attack_0", "magic_attack_1", "magic_attack_2", "magic_attack_3"  };
                                        randomAttackAnim = attackAnimations_3[Random.Range(0, attackAnimations_3.Length)];
                                        animator.SetTrigger(randomAttackAnim);
                                        animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                                        ok = Instantiate(bullet, gun_position_rotation.position,gun_position_rotation.rotation);
                                        ok.SendMessage("veri_al", this.gameObject.name, SendMessageOptions.DontRequireReceiver);
                                        rigid = ok.GetComponent<Rigidbody>();
                                        rigid.AddForce(gun_position_rotation.forward * 58f, ForceMode.Impulse);
                                        yield return new WaitForSeconds(animationLength);
                            break;
                                                        
                        }
                    }
                }
            saldırıyor = false;
        }
        else
        {
            yield break;
        }
    }

    public void hasar_ver()
    {       
        float attack_option_0 = punch_figth_knowlech + Random.Range(punch_figth_knowlech-10,punch_figth_knowlech+10);
        float attack_option_1 = sword_figth_knowlech + Random.Range(sword_figth_knowlech-10,sword_figth_knowlech+10);
        
        switch(asker_tipi){
            case 0:
                    closestObject.GetComponent<Yashanti_Chizelgesi_Yapay_Zeka>().hasar_al(attack_option_0);
                break;
            case 1:
                    closestObject.GetComponent<Yashanti_Chizelgesi_Yapay_Zeka>().hasar_al(attack_option_1);
                break;}
    }

    public void hasar_al(float hasar)
    {
        health -= hasar;
        can.can_bar_metodu(health,maxhealth);
        StartCoroutine(FlashCharacter());
    }

    public void train()
    {
        targetTag="trainplace";
        
    }
        
    public IEnumerator FlashCharacter()
    {
        if (childRenderer != null)
        {
            childRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.20f);
            renk();
        }
    }

    public void renk()
    {
        switch(gameObject.tag)
        {
            case "Player":
                childRenderer.material.color = Color.blue;
                break;
            case "Enemy":
                childRenderer.material.color = Color.red;
                break;
        }
    }

    private void look()
    {
        if  (closestObject != null)
        {
            Vector3 direction = closestObject.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void Idle()
    {
        navMeshAgent.SetDestination(transform.position);
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fighting Idle"))
        {
            animator.SetTrigger("Fighting Idle");
        }
    }

    private IEnumerator Dead()
    {   
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") 
        || !animator.GetCurrentAnimatorStateInfo(0).IsName("Knocked Out") 
        || !animator.GetCurrentAnimatorStateInfo(0).IsName("Dying Backwards") 
        || !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Death"))
        {
            animator.SetTrigger(dead_Anim);
            yasam = false;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 1.20f);
            Destroy(this.gameObject);
        }
    }

    public void MoveToTarget()
    {
            if (navMeshAgent.remainingDistance >= attackDistance * 1.25f)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Slow Run") || 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    navMeshAgent.speed = speed;
                    animator.SetTrigger(Running_Anim);
                }
            }
            else
            {    
                 if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Slow Run") || 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || 
                !animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    navMeshAgent.speed = Mathf.Floor(speed * 0.5f);
                    animator.SetTrigger("Running_2");
                }
            }
        
        navMeshAgent.SetDestination(closestObject.transform.position);
    }
}
