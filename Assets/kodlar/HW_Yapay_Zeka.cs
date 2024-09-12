using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class HW_Yapay_Zeka : MonoBehaviour
{
    public string targetTag;
    public string Running_Anim;
    public string dead_Anim;
    public string randomAttackAnim;
    public float distance;
    public float dovuscu_attack, kilicli_attack, okcu_attack, buyucu_attack;
    public float speed;
    public float health, maxhealth;
    public float searchRadius = 10000f;
    public float attackDistance;
    public float rotationSpeed;

    public bool yasam = true;
    public int asker_tipi;
    public bool saldiriyor = false;

    public Animator animator;
    public GameObject closestObject;
    public GameObject gun, bullet;
    GameObject ok;
    Rigidbody rigid;

    public can_gostergesi can;
    public Transform gun_position_rotation;
    public NavMeshAgent navMeshAgent;
    public Color originalColor;
    public Renderer childRenderer;
    public Collider enemyCollider;

    void Awake()
    {
        yasam = true;
        childRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCollider = GetComponent<Collider>();
    }

    void Start()
    {
        renk();

        dovuscu_attack = Random.Range(2, 9);
        asker_tipi = Random.Range(0, 4);
        kilicli_attack = Random.Range(5, 16);
        okcu_attack = Random.Range(4, 13);
        buyucu_attack = Random.Range(10, 21);

        health = Random.Range(30, 99);
        speed = Random.Range(11, 34);
        rotationSpeed = Random.Range(10, 21);
        maxhealth = health;

        gameObject.name = $"{gameObject.tag}_noc_{Random.Range(000000000, 999999999)}";
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = rotationSpeed;

        string[] runAnimations = { "Running", "Running_1", "Running_2" };
        //string[] runAnimations = { "Running", "Slow Run", "Run" };
        Running_Anim = runAnimations[Random.Range(0, runAnimations.Length)];
        string[] run_dead_Animations = { "dead", "dead_1", "dead_2", "dead_3" };
        //string[] run_dead_Animations = { "Knocked Out", "Sword And Shield Death", "Falling Back Death", "Dying Backwards" };        
        dead_Anim = run_dead_Animations[Random.Range(0, run_dead_Animations.Length)];
    }

    void Update()
    {
        if (yasam == false || health <= 0)
        {
            StartCoroutine(Dead());
        }

        switch (asker_tipi)
        {
            case 0: // dovuscu
                attackDistance = 25;
                break;

            case 1: // kılıçlı
                attackDistance = 35;
                break;

            case 2: // okçu
                attackDistance = 1750;
                break;

            case 3: // büyücü
                attackDistance = 1500;
                break;
        }
        if (yasam)
        {
            FindClosestObject();
            if (closestObject != null)
            {
                PerformActions();
            }
            else
            {
                StartCoroutine(Idle());
                //StartCoroutine(devriye());
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

    private IEnumerator StartAttack()
    {
        navMeshAgent.SetDestination(this.gameObject.transform.position);
        if (closestObject != null)
        {
            saldiriyor = true;

            if (yasam)
            {
                string[] attackAnimations_0 = { "punch_left_1", "punch_left_2", "punch_left_3", "punch_right_1", "punch_right_2", "punch_right_3" };
                string[] attackAnimations_1 = { "sword_attack_0", "sword_attack_1", "sword_attack_2", "sword_attack_3" };
                string[] attackAnimations_2 = { "bow_arrow" };
                string[] attackAnimations_3 = { "magic_attack_0", "magic_attack_1", "magic_attack_2", "magic_attack_3" };
                /*string[] attackAnimations_0 = { "Hook Punch", "Uppercut", "Cross Punch", "Hook Punch (1)", "Uppercut (1)", "Cross Punch (1)" };
                string[] attackAnimations_1 = { "Sword And Shield Slash", "Sword And Shield Slash (1)", "Sword And Shield Slash (2)", "Sword And Shield Slash (3)" };
                string[] attackAnimations_2 = { "Shooting Arrow" };
                string[] attackAnimations_3 = { "Standing 2H Magic Attack 01", "Standing 2H Magic Attack 02", "Standing 2H Magic Attack 02 (1)", "Standing 2H Magic Attack 05" };*/


                switch (asker_tipi)
                {
                    case 0: // dovuscu
                        randomAttackAnim = attackAnimations_0[Random.Range(0, attackAnimations_0.Length)];
                        yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        break;

                    case 1: // kılıçlı
                        randomAttackAnim = attackAnimations_1[Random.Range(0, attackAnimations_1.Length)];
                        yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        break;

                    case 2: // okçu
                        if (distance > 55)
                        {
                            randomAttackAnim = attackAnimations_2[Random.Range(0, attackAnimations_2.Length)];
                            yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        }
                        else
                        {
                            //animator.ResetTrigger(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                            randomAttackAnim = attackAnimations_1[Random.Range(0, attackAnimations_1.Length)];
                            yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        }
                        break;

                    case 3: // büyücü
                        if (distance > 55)
                        {
                            randomAttackAnim = attackAnimations_3[Random.Range(0, attackAnimations_3.Length)];
                            yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        }
                        else
                        {
                            //animator.ResetTrigger(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                            randomAttackAnim = attackAnimations_1[Random.Range(0, attackAnimations_1.Length)];
                            yield return StartCoroutine(PlayAnimation(randomAttackAnim));
                        }
                        break;
                }
                Look();
            }
            else
            {
                //animator.ResetTrigger(animator.GetCurrentAnimatorStateInfo(0).IsName());
                StartCoroutine(Dead());
            }
            saldiriyor = false;
        }
        else
        {
            yield break;
        }
    }

    private IEnumerator PlayAnimation(string animasyonun_Adi)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animasyonun_Adi))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
            {
                yield break;
            }
        }
        animator.SetTrigger(animasyonun_Adi);
        float animasyonun_uzunlugu = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //animator.Play(animasyonun_Adi);
        yield return new WaitForSeconds(animasyonun_uzunlugu);
        animator.ResetTrigger(randomAttackAnim);
        animator.ResetTrigger(Running_Anim);
        animator.ResetTrigger(dead_Anim);
    }

    public void PerformActions()
    {
        Look();
        distance = (transform.position - closestObject.transform.position).sqrMagnitude;

        if (distance <= attackDistance && !saldiriyor && yasam == true)
        {
            if (IsObjectInFront())
            {
                // En uygun yönü bul
                Vector3 bestDirection = FindBestDirection();
                Vector3 newTarget = transform.position + bestDirection * 100.0f; // 100 birim kadar uzaklık
                StartCoroutine(MoveToTarget(newTarget));
            }
            else
            {
                StartCoroutine(StartAttack());
            }
        }
        else if (distance > attackDistance)
        {
            if (IsObjectInFront())
            {
                // En uygun yönü bul
                Vector3 bestDirection = FindBestDirection();
                Vector3 newTarget = transform.position + bestDirection * 100.0f; // 100 birim kadar uzaklık
                StartCoroutine(MoveToTarget(newTarget));
            }
            else
            {
                StartCoroutine(MoveToTarget(closestObject.transform.position));
            }
        }
    }

    private Vector3 FindBestDirection()
    {
        Vector3 bestDirection = Vector3.zero;
        float maxDistance = 0f;

        // Hedef objenin yönünü hesapla
        Vector3 targetDirection = (closestObject.transform.position - transform.position).normalized;

        // Belirli açılarla yönleri kontrol et
        for (int angle = -90; angle <= 90; angle += 20)
        {
            Vector3 direction = Quaternion.Euler(0, angle, 0) * targetDirection;

            // Raycast yaparak bu yönde engel olup olmadığını kontrol et
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, attackDistance))
            {
                // Eğer bu yön, en uzak mesafedeki engelle çarpışıyorsa ve hedefle aynı tag'a sahip değilse en iyi yön olarak belirle
                if (hit.distance > maxDistance && !hit.collider.gameObject.CompareTag(gameObject.tag))
                {                 
                        maxDistance = hit.distance;
                        bestDirection = direction;                    
                }
            }
            else // Eğer raycast hiçbir şeye çarpmadıysa, bu yön boş ve en iyi yön olabilir
            {               
                    return direction;               
            }
        }

        // Eğer en iyi yön bulunamadıysa, hedefe doğru yönel
        return bestDirection == Vector3.zero ? targetDirection : bestDirection;
    }

    private bool IsObjectInFront()
    {
        RaycastHit hit;
        Vector3 direction = closestObject.transform.position - transform.position;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance))
        {
            if (hit.collider.gameObject.CompareTag(gameObject.tag))
            {
                return true;
            }
        }
        return false;
    }

    public void hasar_ver(float attack)
    {
        switch (asker_tipi)
        {
            case 0:
                closestObject.GetComponent<HW_Yapay_Zeka>().hasar_al(dovuscu_attack);
                break;
            case 1:
                closestObject.GetComponent<HW_Yapay_Zeka>().hasar_al(kilicli_attack);
                break;
            case 2:
                if (distance > 55)
                {
                    bullet.GetComponent<mermi>().ad = this.gameObject.name;
                    ok = Instantiate(bullet, gun_position_rotation.position, gun_position_rotation.rotation);
                    rigid = ok.GetComponent<Rigidbody>();
                    rigid.AddForce(this.gameObject.transform.forward * 60f, ForceMode.Impulse);
                }
                else
                {
                    closestObject.GetComponent<HW_Yapay_Zeka>().hasar_al(kilicli_attack);
                }
                break;
            case 3:
                if (distance > 55)
                {
                    bullet.GetComponent<mermi>().ad = this.gameObject.name;
                    ok = Instantiate(bullet, gun_position_rotation.position, gun_position_rotation.rotation);
                    rigid = ok.GetComponent<Rigidbody>();
                    rigid.AddForce(this.gameObject.transform.forward * 60f, ForceMode.Impulse);
                }
                else
                {
                    closestObject.GetComponent<HW_Yapay_Zeka>().hasar_al(kilicli_attack);
                }
                break;
        }
    }

    public void hasar_al(float hasar)
    {
        health -= hasar;
        can.can_bar_metodu(health, maxhealth);
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
        switch (gameObject.tag)
        {
            case "Player":
                childRenderer.material.color = Color.blue;
                break;
            case "Enemy":
                childRenderer.material.color = Color.red;
                break;
        }
    }

    private void Look()
    {
        if (closestObject != null)
        {
            Vector3 direction = closestObject.transform.position - transform.position;
            direction.y = 0; // Y eksenindeki farkı sıfırlayarak sadece yatay düzlemde dönmesini sağlıyoruz.      
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);          
        }
    }

    private IEnumerator Idle()
    {
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
        yield return StartCoroutine(PlayAnimation("Fighting Idle"));
    }

    private IEnumerator Dead()
    {   
        yasam = false;
        yield return StartCoroutine(PlayAnimation(dead_Anim));
        Destroy(this.gameObject);
    }

    public IEnumerator MoveToTarget(Vector3 target)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target);
        Look();
        if (distance >= attackDistance * 2f)
        {
            navMeshAgent.speed = speed;
            yield return StartCoroutine(PlayAnimation(Running_Anim));
        }
        else
        {
            navMeshAgent.speed = Mathf.Floor(speed * 0.5f);
            navMeshAgent.acceleration = Mathf.Floor(rotationSpeed * 10f);
            yield return StartCoroutine(PlayAnimation("Running_2"));
            //yield return StartCoroutine(PlayAnimation("Slow Run"));
        }
    }

   /* public IEnumerator devriye()
    {
        Vector3 yollar = FindBestDirection();
        Vector3 rastgele_devriye_noktasi = transform.position + yollar * 500.0f; // 500 birim kadar uzaklık
        StartCoroutine(MoveToTarget(rastgele_devriye_noktasi));
        if(distance>=5f)
        {
            yield break;
        }
    }*/
}