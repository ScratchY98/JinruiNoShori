using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [Header("Scripts's References")]
    [SerializeField] private PlayerController playerControllerRef;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraController;
    [SerializeField] private PlayerAttack playerAttackRef;
    [SerializeField] private GameOverManager gameOverManagerRef;
    [SerializeField] private PauseMenu pauseMenuRef;

    [Header("Components's References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [SerializeField] private float eatAnimationDelay = 1.25f;
    [SerializeField] private float deadDelay = 1f;

    [Header("Others")]
    [SerializeField] private string titanHandTag = "TitanHand";

    private void Start()
    {
        UseBloodParticle(false);
        playerControllerRef.enabled = true;
        ODMGearControllerRef.enabled = true;
        playerAttackRef.enabled = true;
        pauseMenuRef.enabled = true;
        Time.timeScale = 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == titanHandTag)
        {
            TitanController titan = playerAttackRef.GetTitanControllerFromParent(other.transform.gameObject, 10);
            if (titan.isDead)
                return;
            else
            {
                pauseMenuRef.enabled = false;
                titan.Eat();

                ODMGearControllerRef.StopODMGear();
                ODMGearControllerRef.canUseODMGear = false;
                playerAttackRef.canAttack = false;

                Transform playerDeadPos = other.transform.GetChild(0);
                transform.parent = other.transform;
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;

                Transform cameraDeadPos = titan.transform.GetChild(2);
                thirdPersonCameraController.transform.parent = cameraDeadPos.transform;

                thirdPersonCameraController.ActiveDeathCamera(cameraDeadPos.position, cameraDeadPos.rotation);

                playerControllerRef.canMove = false;
                rb.isKinematic = true;

                Invoke("EatAnimationDelay", eatAnimationDelay);
            }
        }
    }

    private void EatAnimationDelay()
    {
        UseBloodParticle(true);
        Invoke("Dead", deadDelay);
    }

    private void UseBloodParticle(bool active)
    {
        for (int i = 0; i < bloodParticleSystem.Length; i++)
        {
            if (active)
                bloodParticleSystem[i].Play();
            else
                bloodParticleSystem[i].Stop();
        }
    }


    private void Dead()
    {
        gameOverManagerRef.OnPlayerDeath();
    }
}
