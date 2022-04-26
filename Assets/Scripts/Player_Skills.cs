using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Skills : MonoBehaviour
{
    private enum Skills {Normal, Burn, Slow, Jolt};
    private int Skill_count = System.Enum.GetNames(typeof(Skills)).Length;

    private Skills meleeSkill;
    private Skills rangeSkill;
    private Skills dashSkill;

    // Update is called once per frame
    void Update()
    {
        SkillSelect();
    }

    void SkillSelect(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            meleeSkill += 1;
            if ((int)meleeSkill == Skill_count) meleeSkill = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            rangeSkill += 1;
            if ((int)rangeSkill == Skill_count) rangeSkill = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            dashSkill += 1;
            if ((int)dashSkill == Skill_count) dashSkill = 0;
        }
    }

    public string getMeleeSkill(){
        return meleeSkill.ToString();
    }
    public string getRangeSkill(){
        return rangeSkill.ToString();
    }
    public string getDashSkill(){
        return dashSkill.ToString();
    }
}
