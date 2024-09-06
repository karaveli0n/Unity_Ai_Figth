using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class yapay_zeka : MonoBehaviour
{
    public string targetTag;
    public string Running_Anim;
    public string dead_Anim;

    public float distance;
    public float attack;
    public float speed;
    public float health, maxhealth;
    public float searchRadius = 100f;
    public float attackDistance = 30f;
    public float rotationSpeed = 5f;

    public bool yasam = true;
    public bool saldırıyor = false;

    public Animator animator;
    public GameObject closestObject;
    public GameObject gun, gun_position_rotation;
    public NavMeshAgent navMeshAgent;
    public Color originalColor;
    public Renderer childRenderer;
    public Collider enemyCollider;
    public AnimatorStateInfo currentState;
    public can_gostergesi can;

    void Start()
    {   
        yasam=true;
        childRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
        can = GetComponent<can_gostergesi>();

        renk();
        maxhealth = health;

        attack = Random.RandomRange(2, 9);
        health = Random.RandomRange(30, 90);
        speed = Random.RandomRange(11, 22);

        rotationSpeed = Random.RandomRange(5, 11);
        gameObject.name = $"{gameObject.tag}_noc_{Random.Range(0, 279)}";
        navMeshAgent.speed = speed;

        string[] runAnimations = { "Running", "Running_1", "Running_2"/*, "Running_3"*/ };
        Running_Anim = runAnimations[Random.Range(0, runAnimations.Length)];
        string[] run_dead_Animations = { "dead", "dead_1", "dead_2", "dead_3" };
        dead_Anim = run_dead_Animations[Random.Range(0, run_dead_Animations.Length)];
    }

    public void Update()
    {   
        currentState = animator.GetCurrentAnimatorStateInfo(0);
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
        if (yasam==false||health <= 0)
            {
                StartCoroutine(Dead());
            }
    }

    public void FindClosestObject()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        float closestDistanceSquared = Mathf.Infinity;
        GameObject closestObj = null;

        foreach (GameObject taggedObject in taggedObjects)
        {
            if (taggedObject == gameObject || taggedObject.GetComponent<yapay_zeka>().yasam==false)
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
        if ((distance <= attackDistance) && !saldırıyor && yasam==true)
        {   
           StartCoroutine(StartAttack());
        }
        if(distance > attackDistance)
        {   
            MoveToTarget();
        }            
    }

    private IEnumerator StartAttack()
    {    
        if(closestObject!=null)
        {
            saldırıyor=true;
            if(navMeshAgent && navMeshAgent.isActiveAndEnabled)
            {     
                navMeshAgent.SetDestination(this.gameObject.transform.position);
            }
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch (1)"))
            {
                string[] attackAnimations = { "punch_left_1", "punch_right_1" };
                string randomAttackAnim = attackAnimations[Random.Range(0, attackAnimations.Length)];
                animator.SetTrigger(randomAttackAnim);
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                float animationLength = currentState.length;
                float hasarTiming = animationLength * 0.55f;
                Collider targetCollider = closestObject.GetComponent<Collider>();
                yield return new WaitForSeconds(hasarTiming);
                
                if (currentState.IsName("Cross Punch") || currentState.IsName("Cross Punch (1)"))
                {
                    if (targetCollider != null && enemyCollider.bounds.Intersects(targetCollider.bounds))
                        {    
                            yapay_zeka targetEnemy = closestObject.GetComponent<yapay_zeka>();
                            targetEnemy.hasar_al(attack);
                        }
                }
            }
            saldırıyor=false;
        }
        else
        {
            yield break; // Coroutine'i durdur
        }
    }

    public void hasar_al(float hasar)
    {
        health -= hasar;
        can.can_bar_metodu(health,maxhealth);
        StartCoroutine(FlashCharacter());
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
        navMeshAgent.SetDestination(this.gameObject.transform.position);
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fighting Idle"))
        {
            animator.SetTrigger("Fighting Idle");
        }
    }

    private IEnumerator Dead()
    {   
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Back Death") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Knocked Out") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dying Backwards") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Death"))
        {
            animator.SetTrigger(dead_Anim);
            yasam = false;
            currentState = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(currentState.length * 1.25f);
            /*if (currentState.normalizedTime == currentState.length)
            {*/
                Destroy(this.gameObject);
            //}
        }
    }

    public void MoveToTarget()
    {   
        if(navMeshAgent.remainingDistance >= attackDistance * 1.25f)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Slow Run") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Running")/* && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fast Run")*/)
            { 
                navMeshAgent.speed = speed;
                animator.SetTrigger(Running_Anim);
            }
        }
        else
        {            
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
            navMeshAgent.speed = Mathf.Floor(speed * 0.5f);
            animator.SetTrigger("Running_2");
            }
        }
        navMeshAgent.SetDestination(closestObject.transform.position);
    }
}