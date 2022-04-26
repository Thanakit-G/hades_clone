using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Player_Skills skills;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    // private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    // MELEE
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem explosionPrefab;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    
    // RANGE
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private int maxMagazine = 3;
    [SerializeField] private float reloadCoolTime = 1f;
    private int magazine;
    private float reloadTime;

    // DASH
    [SerializeField] private int maxDash = 2;
    [SerializeField] private float dashCoolTime = 0.25f;
    private int dash;
    private float dashCooldown;
    private float dashSpeed = 20f;
    private float dashTime = 0.25f;

    // Status Effect
    [SerializeField] private float baseStatusEffectLifetime = 2f;
    [SerializeField] private ParticleSystem powderPrefab;
    [SerializeField] private float powderRange = 3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        skills =GetComponent<Player_Skills>();
        magazine = maxMagazine;
        reloadTime = reloadCoolTime;
        dash = maxDash;
        dashCooldown = dashCoolTime;
    }

    void Update()
    {
        PlayerMove();
        PlayerAttack();
        Reload();
        DashCooldown();
    }

    private void PlayerMove(){
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            animator.SetBool("isMove",true);
            gameObject.transform.forward = move;
        }else {
            animator.SetBool("isMove",false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(dash > 0){
                dash--;
                animator.SetTrigger("dash");
                StartCoroutine(Dash());
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void PlayerAttack(){
        if(Input.GetMouseButtonDown(0)){
            MeleeAttack();
        }else if(Input.GetMouseButtonDown(1)){
            RangeAttack();
        }
    }
    void MeleeAttack(){
        if(Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + 1f / attackRate;

        if(explosionPrefab != null){
            Instantiate(explosionPrefab, attackPoint.position, attackPoint.rotation);
        }
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider enemy in hitEnemies){
            // Check Hit
            // Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<Health_Enemy>().GetHurt(attackDamage);
            // Status Effect
            enemy.GetComponent<Enemy>().ApplyStatus(skills.getMeleeSkill(),baseStatusEffectLifetime);
        }
    }
    void RangeAttack(){
        if(magazine > 0){
            // Shoot
            magazine--;
            GameObject myBullet = Instantiate(projectile, transform.position + transform.forward + new Vector3(0.0f, (controller.height/2), 0.0f), transform.rotation);
            // Status Effect
            myBullet.GetComponent<Bullet>().AddStatusEffect(skills.getRangeSkill(),baseStatusEffectLifetime);
        }
    }
    void Reload(){
        if(magazine >= maxMagazine) return;
        if(reloadTime <= 0) {
            reloadTime = reloadCoolTime;
            magazine++;
        }else{
            reloadTime -= Time.deltaTime;
        }
    }
    public int getMagazine(){
        return magazine;
    }

    void DashCooldown(){
        if(dash >= maxDash) return;
        if(dashCooldown <= 0) {
            dashCooldown = dashCoolTime;
            dash = maxDash;
        }else{
            dashCooldown -= Time.deltaTime;
        }
    }
    IEnumerator Dash(){
        float startTime = Time.time;
        gameObject.layer = LayerMask.NameToLayer("Ghost");

        while(Time.time < startTime + dashTime){
            controller.Move(controller.transform.forward * Time.deltaTime * dashSpeed);
            
            yield return null;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");

        //Status Effect
        if(skills.getDashSkill() != "Normal"){
            if(powderPrefab != null){
                Instantiate(powderPrefab, transform.position, transform.rotation);
            }
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, powderRange, enemyLayer);

            foreach(Collider enemy in hitEnemies){
                // Debug.Log("We hit" + enemy.name);
                enemy.GetComponent<Enemy>().ApplyStatus(skills.getDashSkill(),baseStatusEffectLifetime);
            }
        }
    }

    void OnDrawGizmosSelected(){
        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
