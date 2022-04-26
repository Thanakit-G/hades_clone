using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    enum MeleeSkill {Normal, Burn, Lighting};
    enum RangeSkill {Normal, Burn, Lighting};
    enum DashSkill {Normal, Burn, Lighting};

    public Slider healthbar;
    public GameObject gameover;
    public Button retry_btn;
    public Text skill1_display;
    public Text skill2_display;
    public Text skill3_display;
    public Text magazine_display;
    
    Health_Player playerHealth;
    Player_Skills playerSkills;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<Health_Player>();
        playerSkills = GameObject.Find("Player").GetComponent<Player_Skills>();
        player = GameObject.Find("Player").GetComponent<Player>();
		retry_btn.onClick.AddListener(Retry);
    }

    // Update is called once per frame
    void Update()
    {
        skill1_display.text = playerSkills.getMeleeSkill();
        skill2_display.text = playerSkills.getRangeSkill();
        skill3_display.text = playerSkills.getDashSkill();
        magazine_display.text = new string('O', player.getMagazine());
        healthbar.value = (float)playerHealth.HP/(float)playerHealth.maxHP;
        if(playerHealth.HP <= 0){
            gameover.SetActive(true);
        }
    }

	void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
