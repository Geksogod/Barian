using System.Collections;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField]
    private string fastAttackAnimTriger;
    [SerializeField]
    private AnimationClip fastAttackAnimClip;
    [SerializeField]
    private Character character;
    [SerializeField]
    private CharacterMovement characterMovement;

    private bool isAttaced;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !isAttaced)
            FastAttack();
    }

    private void FastAttack()
    {
        characterMovement.StopMoving();
        characterMovement.Stop(true);
        character.SetAnimatorTriger(fastAttackAnimTriger, 1f);
        isAttaced = true;
        StartCoroutine(Attacking(fastAttackAnimClip.length));
    }

    private IEnumerator Attacking(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        character.SetAnimatorTriger(fastAttackAnimTriger, 0f);
        isAttaced = false;
        characterMovement.Stop(false);
        yield break;
    }


}
