using System.Collections;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private const float SpeedIncreaseCoefficient = 20;
    [SerializeField]
    private Character character;
    #region AnimationTrigers;
    [SerializeField]
    private string animatorMoveTriger;
    [SerializeField]
    private string animatorMoveSpeed;
    [SerializeField]
    private string animatorJumpTriger;
    [SerializeField]
    private string animatorScrambleTriger;
    [SerializeField]
    private AnimationClip animationScramble;
    [SerializeField]
    private AnimationCurve scrambleCurve;
    private float scrambleTime;
    #endregion
    #region MovmentSetup
    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float jumpForce = 1;
    [SerializeField]
    private float jumpKD = 0.1f;
    [SerializeField]
    private float currentJumpKD = 0;
    #endregion
    #region References
    [SerializeField]
    private Transform groundCheckLeftLeg;
    [SerializeField]
    private Transform groundCheckRightLeg;
    [SerializeField]
    private Collider2D head;
    [SerializeField]
    private Rigidbody2D rb2D;
    #endregion
    private bool isRotated;
    private bool isScramble;
    private bool prevRotateState;
    private bool isStoped;
    [SerializeField]
    [ReadOnly]
    private bool grounded;
    public delegate void OnMoving(Vector3 newPosition);
    public OnMoving onMoving;

    private void Awake()
    {
        currentJumpKD = jumpKD;
        if (rb2D == null)
            rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = IsToucGround();
        if (!isStoped)
            Movement();
        Jumping();
        Rotate();
    }

    private bool IsToucGround()
    {
        bool leftLeghInGround = Physics2D.Linecast(transform.position, groundCheckLeftLeg.position, 1 << LayerMask.NameToLayer("Tangible"));
        bool rightLeghInGround = Physics2D.Linecast(transform.position, groundCheckRightLeg.position, 1 << LayerMask.NameToLayer("Tangible"));
        return leftLeghInGround || rightLeghInGround;
    }

    private void Movement()
    {
        Vector2 direction = Vector2.zero;
        if (!grounded)
        {
            character.SetAnimatorTriger(animatorMoveTriger, 0);
            character.SetAnimatorTriger(animatorMoveSpeed, 0);
            return;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            direction.x += Input.GetKey(KeyCode.D) ? movementSpeed * SpeedIncreaseCoefficient * Time.fixedDeltaTime : -1 * movementSpeed * SpeedIncreaseCoefficient * Time.fixedDeltaTime;
            isRotated = Input.GetKey(KeyCode.D) ? true : false;
        }
        //if (!Vector2.Equals(Vector2.zero, direction))
           // onMoving(transform.position);
        rb2D.AddForce(direction);
        if (Vector2.Equals(direction, Vector2.zero) && rb2D.velocity.magnitude > 0.4f)
            StopMoving();
        character.SetAnimatorTriger(animatorMoveTriger, direction);
        character.SetAnimatorTriger(animatorMoveSpeed, rb2D.velocity.magnitude / 10f);
    }

    public void StopMoving()
    {
        rb2D.AddForce(-1 * rb2D.velocity * 6);
        character.SetAnimatorTriger(animatorMoveTriger, 0);
        character.SetAnimatorTriger(animatorMoveSpeed, 0);
        rb2D.angularVelocity = 0.0f;
    }

    public void Stop(bool _isStoped)
    {
        isStoped = _isStoped;
        if (isStoped)
            StopMoving();
    }


    private void Rotate()
    {
        if (isRotated != prevRotateState)
        {
            transform.Rotate(new Vector3(0, 180, 0));
            prevRotateState = isRotated;
        }
    }

    private void Jumping()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.Space) && grounded && currentJumpKD <= 0)
        {
            direction.y += jumpForce * SpeedIncreaseCoefficient * Time.fixedDeltaTime;
            currentJumpKD = jumpKD;
        }
        else if (grounded && currentJumpKD > 0)
            currentJumpKD -= Time.deltaTime;
        rb2D.AddForce(direction, ForceMode2D.Impulse);
        character.SetAnimatorTriger(animatorJumpTriger, direction);
    }

    private void Scramble()
    {
        character.SetAnimatorTriger(animatorScrambleTriger, 1);
        rb2D.velocity = Vector2.zero;
        Vector2 moveTo = isRotated ? new Vector2(transform.position.x + 0.6f, transform.position.y + 1f) : new Vector2(transform.position.x - 0.6f, transform.position.y + 1f);
        StartCoroutine(ScrambleIEnumerator(moveTo));
        isScramble = true;
    }

    private IEnumerator ScrambleIEnumerator(Vector2 moveTo)
    {
        while (scrambleTime < animationScramble.length)
        {
            rb2D.gravityScale = 0;
            scrambleTime += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, moveTo, scrambleCurve.Evaluate(MathUtils.Normalize(scrambleTime, 0, 1, 0, animationScramble.length)));
            yield return new WaitForFixedUpdate();
        }
        scrambleTime = 0;
        character.SetAnimatorTriger(animatorScrambleTriger, 0);
        isScramble = false;
        rb2D.gravityScale = 1;
        yield break;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isScramble && collision.gameObject.layer == 10 && !grounded)
            Scramble();
    }
}
