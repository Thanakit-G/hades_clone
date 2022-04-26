using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Enemy : MonoBehaviour
{
    public string status_name;
    public float duration = 0f;
    public float lifetime;
    public float tick = 0f;
    public float nextTick;

    public int burnDmg = 0;
    public float slow = 0f;
    public int joltDmg = 0;
    public float joltRange = 0;

    public void Init(string status_name, float lifetime){
        switch(status_name){
            case "Burn": burnDmg = 5; tick = 1f; nextTick = tick; lifetime += 8; break;
            case "Slow": slow = 50; break;
            case "Jolt": joltDmg = 5; joltRange = 3f; tick = 1f; nextTick = tick; lifetime += 3; break;
            default: lifetime = 0; break;
        }
        this.status_name = status_name;
        this.lifetime = lifetime;
    }

}
