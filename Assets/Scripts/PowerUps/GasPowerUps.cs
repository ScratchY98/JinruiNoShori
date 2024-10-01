using UnityEngine;

public class GasPowerUp : MonoBehaviour
{
    [SerializeField] private int GasAmounts;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (ODMGearControllerRef.currentGas != ODMGearControllerRef.maxGas)
            {
                if (ODMGearControllerRef == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                    ODMGearControllerRef = player.GetComponent<ODMGearController>();
                }

                SpawnObject.instance.SpawnObjectsAtRandomPosition(1);
                AudioManager.instance.PlayClipAt(pickupSound, transform.position);
                ODMGearControllerRef.FillGas(GasAmounts);
                Destroy(gameObject);
            }
        }
    }
}