using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float SpeedIncreaseCoefficient = 20;
    [SerializeField]
    private Character character;
    [SerializeField]
    private string animatorMoveTriger;
    [SerializeField]
    private string animatorJumpTriger;
    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float jumpForce = 1;
    [SerializeField]
    private Transform groundCheckLeftLeg;
    [SerializeField]
    private Transform groundCheckRightLeg;
    [SerializeField]
    private Rigidbody2D rb2D;

    private bool isRotated;
    private bool prevRotateState;
    [SerializeField]
    private bool grounded;

    private void Awake()
    {
        if (rb2D == null)
            rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = IsToucGround();
        Movement();
        Jumping();
        Rotate();
    }

    private bool IsToucGround()
    {
        bool leftLeghInGround = Physics2D.Linecast(transform.position, groundCheckLeftLeg.position, 1 << LayerMask.NameToLayer("Ground"));
        bool rightLeghInGround = Physics2D.Linecast(transform.position, groundCheckRightLeg.position, 1 << LayerMask.NameToLayer("Ground"));
        //Debug.Log("leftLeghInGround " + leftLeghInGround);
        //Debug.Log("rightLeghInGround " + rightLeghInGround);
        return leftLeghInGround || rightLeghInGround;
    }

    private void Movement()
    {
        Vector2 direction = Vector2.zero;
        if (!grounded)
        {
            rb2D.AddForce(direction);
            Animate(animatorMoveTriger, direction);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += movementSpeed * SpeedIncreaseCoefficient * Time.fixedDeltaTime;
            if (!isRotated)
                isRotated = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x += -1 * movementSpeed * SpeedIncreaseCoefficient * Time.fixedDeltaTime;
            if (isRotated)
                isRotated = false;
        }
        rb2D.AddForce(direction);
        Animate(animatorMoveTriger, direction);
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
        if (Input.GetKey(KeyCode.Space) && grounded)
            direction.y += jumpForce * SpeedIncreaseCoefficient * Time.fixedDeltaTime;
        rb2D.AddForce(direction, ForceMode2D.Impulse);
        Animate(animatorJumpTriger, direction);
    }

    private void Animate(string trigerName, Vector2 direction)
    {
        character.SetAnimatorTriger(trigerName, direction.normalized.magnitude);
    }
}
