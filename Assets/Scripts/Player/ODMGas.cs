using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ODMGas : MonoBehaviour
{
    [Header("Gas")]
    public int gas;
    public int maxGas;
    [SerializeField] private Slider gasBar;

    [Header("GasBoost")]
    [SerializeField] private float boostStrength;
    [SerializeField] private ParticleSystem[] boostParticle;
    [SerializeField] private int gasAmount;

    [Header ("Scripts's Reference")]
    [SerializeField] private ODMGearController ODMleft;
    [SerializeField] private ODMGearController ODMright;

    private Rigidbody playerRb;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gasBar.maxValue = maxGas;
        gasBar.value = gas;
    }
    void FixedUpdate()
    {
        if (LoadData.instance.playerInput.actions["UseGas"].IsPressed() && gas >= gasAmount)
            GasBoost();

        if (!IsUseODMGear())
            transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
    }

    private void GasBoost()
    {
        Vector3 pointA = ODMleft.joint ? ODMleft.ODMGearPoint.position : Vector3.zero;
        Vector3 pointB = ODMright.joint ? ODMright.ODMGearPoint.position : Vector3.zero;

        int activeCount = (ODMleft.joint ? 1 : 0) + (ODMright.joint ? 1 : 0);

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
                return;
        }

        if (boostPoint != Vector3.zero)
        {
            UseBoostParticle();
            gas = Mathf.Max(gas - gasAmount, 0);
            gasBar.value = gas;
        }

        Vector3 direction = (boostPoint - transform.position).normalized;
        playerRb.AddForce(direction * boostStrength, ForceMode.Impulse);
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
        for (int i = 0; i < boostParticle.Length; i++)
        {
            boostParticle[i].Play();
        }
    }

}
