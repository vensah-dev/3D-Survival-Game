using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 15f;
    public float crouchSpeedDiff = 2f;
    public float JumpHeight = 5;
    public float Gravity = 20f;
    public float GNDDistance = 0.4f;

    public Transform GNDCheck;
    public CharacterController Controller;
    public Transform head;

    public LayerMask GNDMask;
    Vector3 Vel;

    bool isGrounded;
    bool isCrouching;

    void Update()
    {



        if (Input.GetButton("Jump") && isGrounded && isCrouching != true)
        {
            Vel.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        if (Input.GetKey(KeyCode.C))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        Vel.y += Gravity * Time.deltaTime;
        Controller.Move(Vel * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(GNDCheck.position, GNDDistance, GNDMask);
        if (isGrounded && Vel.y < 0)
        {
            Vel.y = -2f;
        }

        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * X + transform.forward * Z;

        if (isCrouching == false)
        {
            Controller.Move(move * Speed * Time.deltaTime);

        }
        else
        {
            Controller.Move(move * (Speed + crouchSpeedDiff) * Time.deltaTime);

        }
    }

}