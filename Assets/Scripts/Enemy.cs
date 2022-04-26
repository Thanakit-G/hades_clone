using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Health_Enemy health;

    [SerializeField] private float movementSpeed = 1.0f;
    private float step;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float attackRate = 1f;
    private float nextAttackTime = 0f;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private ParticleSystem joltPrefab;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 1;

    private Transform target;
    private Dictionary <string, StatusEffect_Enemy> statusEffects;
    public Text statusEffects_display;

    void Start(){
        target = GameObject.Find("Player").transform;
        health = GetComponent<Health_Enemy>();
        statusEffects = new Dictionary <string, StatusEffect_Enemy>();
        statusEffects_display = transform.Find("Enemy UI Canvas").Find("statusEffects").GetComponent<Text>();
    }

    void Update()
    {
        // Calculate distance to move
        step =  movementSpeed * Time.deltaTime; 
        // Status effect
        if(statusEffects.Count != 0) {
            HandleStatusEffect();
            statusEffects_display.text = "";
            foreach(string status_name in statusEffects.Keys){
                statusEffects_display.text += status_name[0];
            }
        }

        //Movement and melee
        if(target != null){
            float distance = Vector3.Distance (transform.position, target.transform.position);
            
            if(distance < attackRange){
                if(Time.time >= nextAttackTime){
                    transform.Find("Mesh").LookAt(target.transform);
                    MeleeAttack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }else {
                // Move our position a step closer to the target.
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                transform.Find("Mesh").LookAt(target.transform);
            }
        }
    }

    void MeleeAttack(){
        if(explosionPrefab != null){
            Instantiate(explosionPrefab, attackPoint.position, attackPoint.rotation);
        }
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider player in hitEnemies){
            // Debug.Log("We hit" + enemy.name);
            player.GetComponent<Health_Player>().GetHurt(attackDamage);
        }
    }

    void HandleStatusEffect(){
        for(int i = 0; i < statusEffects.Count; i++){
            //Tick
            string current_key = statusEffects.ElementAt(i).Key;
            StatusEffect_Enemy status = statusEffects[current_key];
            status.duration += Time.deltaTime;
            if(status.duration > status.lifetime){
                RemoveStatusEffect(current_key);
            }

            //Effect
            if(status.burnDmg > 0){
                if(status.duration > status.nextTick){
                    status.nextTick = status.duration + status.tick;
                    health.GetHurt(status.burnDmg);
                }
            }
            if(status.slow > 0){
                step = step * (status.slow/100);
            }
            if(status.joltDmg > 0){
                if(status.duration > status.nextTick){
                    status.nextTick = status.duration + status.tick;
                    if(joltPrefab != null){
                        Instantiate(joltPrefab, transform.position, transform.rotation);
                    }
                    Collider[] hitEnemies = Physics.OverlapSphere(transform.position, status.joltRange, layerMask);

                    foreach(Collider enemy in hitEnemies){
                        // Debug.Log("We hit" + enemy.name);
                        enemy.GetComponent<Health_Enemy>().GetHurt(status.joltDmg);
                    }
                }
            }
        }
    }

    public void RemoveStatusEffect(string status_name){
        Destroy(statusEffects[status_name]);
        statusEffects.Remove(status_name);
    }

    public void ApplyStatus(string status_name, float lifetime){
        if(statusEffects.ContainsKey(status_name)){
            statusEffects[status_name].lifetime += lifetime;
        }else{
            StatusEffect_Enemy newStatusEffect = gameObject.AddComponent<StatusEffect_Enemy>();
            newStatusEffect.Init(status_name, lifetime);
            statusEffects.Add(status_name, newStatusEffect);
        }
    }
}
