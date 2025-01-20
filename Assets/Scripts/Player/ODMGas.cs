using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ODMGas : MonoBehaviour
{
    [Header("Gas")]
    [Min(0)] public int gas;
    [Min(0)] public int maxGas;
    [SerializeField] private Image cooldownUI;    
    [SerializeField] private Slider gasBar;

    [Header("GasBoost")]
    [SerializeField] private float boostStrength;
    [SerializeField] private ParticleSystem[] boostParticle;
    [SerializeField] [Min(0)] private int gasAmount;

    [Header("Scripts's Reference")]
    [SerializeField] private ODMGearController ODMleft;
    [SerializeField] private ODMGearController ODMright;

    [Header("Cooldown")]
    [SerializeField] [Min(0)] private float cooldown;
    [SerializeField] private bool IsCooldown = false;
    [SerializeField] private bool canUseGasBoost = true;
    [SerializeField] private float currentCooldown;

    // Input
    private PlayerInput playerInput;

    private Rigidbody playerRb;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerInput = LoadData.instance.playerInput;
        gasBar.maxValue = maxGas;
        gasBar.value = gas;
    }

    private void Update()
    {
        if (IsCooldown)
            DoCooldown();
    }
    void FixedUpdate()
    {
        if (playerInput.actions["UseGas"].IsPressed() && gas >= gasAmount && canUseGasBoost)
            GasBoost();

        if (!IsUseODMGear())
            transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }

    private void GasBoost()
    {
        Vector3 pointA = ODMleft.joint ? ODMleft.ODMGearPoint.position : Vector3.zero;
        Vector3 pointB = ODMright.joint ? ODMright.ODMGearPoint.position : Vector3.zero;

        int activeCount = (ODMleft.joint ? 1 : 0) + (ODMright.joint ? 1 : 0);

        if (activeCount == 0)
            return;

        StartCooldown();
        canUseGasBoost = false;

        Vector3 boostPoint = Vector3.zero;

        switch (activeCount)
        {
            case 1:
                boostPoint = ODMleft.joint ? pointA : pointB;
                break;
            case 2:
                boostPoint = Vector3.Lerp(pointA, pointB, 0.5f);
                break;
            default:
                break;
        }

        if (boostPoint != Vector3.zero)
        {
            UseBoostParticle();
            gas = Mathf.Max(gas - gasAmount, 0);
            gasBar.value = gas;
        }

        Vector3 direction = (boostPoint - transform.position).normalized;
        playerRb.AddForce(direction * boostStrength, ForceMode.VelocityChange);
    }


    public void FillGas(int gasAmounts)
    {
        gas = Mathf.Min(gas + gasAmounts, maxGas);
        gasBar.value = gas;
    }

    public bool IsUseODMGear()
    {
      // Return true if we use a joint, else return false.
       return ODMleft.joint || ODMright.joint;
    }

    private void UseBoostParticle()
    {
        for (int i = 0; i < boostParticle.Length; i++){
            boostParticle[i].Play();
        }
    }

    private void DoCooldown()
    {
        currentCooldown += Time.deltaTime;

        cooldownUI.fillAmount = currentCooldown / cooldown;

        if (currentCooldown >= cooldown)
        {
            currentCooldown = cooldown;
            IsCooldown = false;
            canUseGasBoost = true;
        }
    }

    private void StartCooldown()
    {
        IsCooldown = true;
        currentCooldown = 0;
    }
}