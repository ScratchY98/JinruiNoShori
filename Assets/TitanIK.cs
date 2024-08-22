using UnityEngine;

public class TitanIK : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private float speed;

    private void OnAnimatorIK(int layerIndex)
    {
        float dotProduct = Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(player.position));

        if (dotProduct > 0)
        {
            SetHandsIK(AvatarIKGoal.RightHand, player);
            SetHandsIK(AvatarIKGoal.LeftHand, player);

            SetHeadIK();
        }
        else
        {
            transform.LookAt(new Vector3 (player.position.x, transform.position.y, player.position.z));
        }
    }

    private void SetHandsIK(AvatarIKGoal IkGoal, Transform target)
    {
        animator.SetIKPositionWeight(IkGoal, 1);

        animator.SetIKPosition(IkGoal, target.position);
    }

    private void SetHeadIK()
    {
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(player.position);
    }
}
