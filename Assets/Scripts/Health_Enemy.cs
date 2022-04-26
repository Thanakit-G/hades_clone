using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Enemy : MonoBehaviour
{
    public Slider healthbar;

    [SerializeField] private int maxHP = 10;
    private int HP;

    [SerializeField] private float dropRate = 0.25f;
    public GameObject dropItem;

    void Start(){
        HP = maxHP;
    }

    void Update()
    {
        healthbar.value = (float)HP/(float)maxHP;
    }

    public void GetHurt(int dmg){
        HP -= dmg;
        // Debug.Log("Enemy (HP "+ HP +"/"+maxHP+") take dmg " +dmg);
        if(HP<=0){
            Die();
        }
    }

    void Die(){
        if(dropItem != null) DropItem();
        Destroy(gameObject);
    }

    void DropItem(){
        float dropChance = Random.Range(0, 1);;
        if(dropChance < dropRate)
            Instantiate(dropItem, transform.position, transform.rotation);
    }
}
