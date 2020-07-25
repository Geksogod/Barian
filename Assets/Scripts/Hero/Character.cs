using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected float health;
    [SerializeField]
    protected Animator animator;

    public bool IsDead()
    {
        return health <= 0;
    }

    public void SetAnimatorTriger(string trigerName,float trigerValue)
    {
        animator.SetFloat(trigerName, trigerValue);
    }

}
