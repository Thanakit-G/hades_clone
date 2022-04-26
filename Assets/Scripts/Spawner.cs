using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Enemy;
    private GameObject ActiveEnemy;

    // Start is called before the first frame update
    void Start()
    {
        ActiveEnemy = Instantiate(Enemy, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(ActiveEnemy == null){
            ActiveEnemy = Instantiate(Enemy, transform.position, transform.rotation);
        }
    }
}
