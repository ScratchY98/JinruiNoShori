using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColossalBehaviour : TitanBehaviour
{
    [Header("Health Settings:")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int playerAttackDamage;
    [SerializeField] private float damageCooldown;
    private bool canTakeDamage;

    [Header("Force Field Settings:")]
    [SerializeField] private float forceFieldCooldown;
    [SerializeField] private float forceFieldDistance;
    [SerializeField] private float forceFieldPower;
    [SerializeField] private float forceFieldRadius;
    [SerializeField] private Transform forceFieldOrigin;
    [SerializeField] private Animation forceFieldAnimation;
    private bool canForceField;

    private GameObject player;
    private Rigidbody playerRb;

    public override void Start()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
        canForceField = true;

        shouldPatrol = false;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;


        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerRb = player.GetComponent<Rigidbody>();

        base.Start();
    }

    public override void Update()
    {

        base.Update();

        if (isDead) return;

        float distanceSqr = (forceFieldOrigin.position - player.transform.position).sqrMagnitude;

        if (!canForceField || distanceSqr > forceFieldDistance) return;

        ForceField();
    }

    private void ForceField()
    {
        canForceField = false;
        Invoke(nameof(ResetForceFieldCooldown), forceFieldCooldown);

        forceFieldAnimation?.Play();
        playerRb.AddExplosionForce(forceFieldPower, forceFieldOrigin.position, forceFieldRadius);
    }

    private void ResetForceFieldCooldown()
    {
        canForceField = true;
    }

    public override void Dead()
    {
        if (!canTakeDamage || currentHealth <= 0) return;

        canTakeDamage = false;
        Invoke(nameof(ResetDamageCooldown), damageCooldown);

        currentHealth = Mathf.Max(currentHealth - playerAttackDamage, 0);
        healthBar.value = currentHealth;

        if (currentHealth == 0)
        {
            Invoke("ResetScene", 4f);
            base.Dead();
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetDamageCooldown()
    {
        canTakeDamage = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (forceFieldOrigin == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(forceFieldOrigin.position, forceFieldRadius);
    }
}
