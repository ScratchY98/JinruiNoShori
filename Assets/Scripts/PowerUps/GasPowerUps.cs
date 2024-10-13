using UnityEngine;

public class GasPowerUp : MonoBehaviour
{
    [SerializeField] private int GasAmounts;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private static ODMGas ODMGasRef;
    private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (ODMGasRef == null)
                ODMGasRef = collision.GetComponent<ODMGas>();

            if (ODMGasRef.gas != ODMGasRef.maxGas)
            {

                SpawnObject.instance.SpawnObjectsAtRandomPosition(1);
                AudioManager.instance.PlayClipAt(pickupSound, transform.position);
                ODMGasRef.FillGas(GasAmounts);
                Destroy(gameObject);
            }
        }
    }
}