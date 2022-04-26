using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Player : MonoBehaviour
{
    public int maxHP = 100;
    public int HP;

    void Start(){
        HP = maxHP;
    }

    public void GetHurt(int dmg){
        if(dmg < 0) return;
        HP -= dmg;
        // Debug.Log("Enemy (HP "+ HP +"/"+maxHP+") take dmg " +dmg);
        if(HP <= 0){
            //Gameover
            Destroy(gameObject);
        }
    }

    public void GetHeal(int heal){
        HP += heal;
        // Debug.Log("Enemy (HP "+ HP +"/"+maxHP+") take dmg " +dmg);
        if(HP > maxHP){
            HP = maxHP;
        }
    }
}
