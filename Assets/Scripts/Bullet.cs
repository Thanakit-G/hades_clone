using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int iD;
    public float speed = 20f;
    public int dmg = 10;
    // public Transform explosionPrefab;
 
    private string status_name;
    private float lifetime;
    
    // Update is called once per frame
    void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void AddStatusEffect(string status_name, float lifetime){
        this.status_name = status_name;
        this.lifetime = lifetime;
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }else if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<Health_Enemy>().GetHurt(dmg);
            if(status_name != null && lifetime > 0){
                col.GetComponent<Enemy>().ApplyStatus(status_name, lifetime);
            }
            Destroy(gameObject);
        }
        // ContactPoint contact = collision.contacts[0];
        // Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        // Vector3 position = contact.point;
        // Instantiate(explosionPrefab, position, rotation);
    }
}