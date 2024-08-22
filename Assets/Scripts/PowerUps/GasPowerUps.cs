using UnityEngine;

public class GasPowerUp : MonoBehaviour
{
    [SerializeField] private int GasAmounts;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {
        if (ODMGearControllerRef == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            ODMGearControllerRef = player.GetComponent<ODMGearController>();
        }


        if (collision.CompareTag("Player"))
        {
            if (ODMGearControllerRef.currentGas != ODMGearControllerRef.maxGas)
            {
                AudioManager.instance.PlayClipAt(pickupSound, transform.position);
                ODMGearControllerRef.FillGas(GasAmounts);
                Destroy(gameObject);
            }
        }
    }
}