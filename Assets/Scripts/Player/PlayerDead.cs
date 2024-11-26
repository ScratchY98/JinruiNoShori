using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [Header("Scripts's References")]
    //[SerializeField] private PlayerController playerControllerRef;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraControllerRef;
    [SerializeField] private PlayerAttack playerAttackRef;
    [SerializeField] private GameOverManager gameOverManagerRef;
    [SerializeField] private PauseMenu pauseMenuRef;

    [Header("Components's References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem[] bloodParticleSystem;

    [SerializeField] [Min(0)] private float eatAnimationDelay = 1.25f;
    [SerializeField] [Min(0)] private float deadDelay = 1f;

    [Header("Others")]
    [SerializeField] private string titanHandTag = "TitanHand";

    private void Start()
    {
        //Disables blood particles on Start.
        UseBloodParticle(false);

        // Enable player scripts on Start.
        //playerControllerRef.enabled = true;
        ODMGearControllerRef.enabled = true;
        playerAttackRef.enabled = true;

        // Enables the pause menu on Start.
        pauseMenuRef.enabled = true;

        // Sets the time to 1 (normal value).
        Time.timeScale = 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the tag is correct.
        if (other.transform.tag == titanHandTag)
        {
            // Check if the titan is dead.      
            TitanController titan = playerAttackRef.GetTitanControllerFromParent(other.transform.gameObject, 10);
            if (titan.isDead)
                // If the titan is dead, the player won't get eaten, so let's return the fonction.
                return;
            else
            {
                // Disables the pause menu.
                pauseMenuRef.enabled = false;

                // Make the titan eat (see TitanController.cs).
                titan.Eat();

                // Disables ODM Gear.
                ODMGearControllerRef.StopODMGear();
                ODMGearController.canUseODMGear = false;
                ODMGearControllerRef.enabled = false;

                // Disables the player's attack.
                playerAttackRef.canAttack = false;
                playerAttackRef.enabled = false;

                // Puts the player in the correct position.
                Transform playerDeadPos = other.transform.GetChild(0);
                transform.parent = other.transform;
                transform.position = other.transform.position;
                transform.rotation = other.transform.rotation;

                // Puts the camera in the correct position
                Transform cameraDeadPos = titan.transform.GetChild(2);
                thirdPersonCameraControllerRef.transform.parent = cameraDeadPos.transform;
                thirdPersonCameraControllerRef.ActiveDeathCamera(cameraDeadPos.position, cameraDeadPos.rotation);

                // Disables the camera
                thirdPersonCameraControllerRef.enabled = false;

                // Prevents the player from moving
                //playerControllerRef.canMove = false;

                // Makes the rigidbody kinematic
                rb.isKinematic = true;

                // Disable the playerController script
                //playerControllerRef.enabled = false;

                // Wait for the Eat animation to finish
                Invoke("EatAnimationDelay", eatAnimationDelay);
            }
        }
    }

    // Launches once the Eat animation is complete
    private void EatAnimationDelay()
    {
        // Once the Eat animation is complete, activate the blood particles.
        UseBloodParticle(true);

        // Waits for a delay (deadDelay) then runs the dead function.
        Invoke("ActiveGameOverScreen", deadDelay);
    }


    // Enables or disables blood particles based on a bool : active.
    private void UseBloodParticle(bool active)
    {
        for (int i = 0; i < bloodParticleSystem.Length; i++)
        {
            if (active)
                // Activates particles if "active" is true.
                bloodParticleSystem[i].Play();
            else
                // Disables particles if "active" is false.
                bloodParticleSystem[i].Stop();
        }
    }


    private void ActiveGameOverScreen()
    {
        // Activates the GameOver screen.
        gameOverManagerRef.OnPlayerDeath();
    }
}
