using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int iD;
    public string target;
    public int heal = 30;

    private string status_name;
    private float lifetime;
    
    void Start(){
        target = "Player";
    }

    public void AddStatusEffect(string status_name, float lifetime){
        this.status_name = status_name;
        this.lifetime = lifetime;
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == target)
        {
            col.GetComponent<Health_Player>().GetHeal(heal);
            Destroy(gameObject);
        }
    }
}
