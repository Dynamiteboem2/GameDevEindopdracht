using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform groundCheck;

    public LayerMask groundMask;
    public LayerMask wallMask;
          
    Vector3 move;
    Vector3 input;
    Vector3 yVelocity;

    Vector3 forwardDirection;

    int jumpCharges;

    bool isGrounded;
    bool isSprinting;
    bool isCrouching;
    bool isSliding;
    bool isWallrunning;

    public float slideSpeedIncrease;
    public float slideSpeedDecrease;
    public float wallRunSpeedIncrease;
    public float wallRunSpeedDecrease;

    float speed;
    public float runSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float airSpeed;

    float gravity;
    public float normalGravity;
    public float wallRunGravity;
    public float jumpHeight;

    float startHeight;
    public float crouchHeight = 0.5f;

    Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    Vector3 standingCenter = new Vector3(0, 0, 0);

    float slideTimer;
    public float maxSlideTimer;

    bool hasWallRun = false;
    bool onLeftWall;
    bool onRightWall;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    Vector3 wallNormal;
    Vector3 lastWallNormal;

    public Camera playerCamera;

    float normalFov;

    public float specialFov;   

    public float cameraChangeTime;
    public float wallRunTilt; 
    public float tilt;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startHeight = controller.height;
        normalFov = playerCamera.fieldOfView;
    }

    void IncreaseSpeed(float speedIncrease)
    {
        speed += speedIncrease;
    }

    void DecreaseSpeed(float speedDecrease)
    {
        speed -= speedDecrease * Time.deltaTime;
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        input = new Vector3(horizontal, 0f, vertical);
        input = transform.TransformDirection(input);
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetKeyDown(KeyCode.Space) && jumpCharges > 0)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            ExitCrouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
    }

    void CameraEffects()
    {
        float fov = isWallrunning ? specialFov : isSliding ? specialFov : normalFov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, cameraChangeTime * Time.deltaTime);

        if (isWallrunning)
        {
            if (onRightWall)
            {
                tilt = Mathf.Lerp(tilt, wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
            if (onLeftWall)
            {
                tilt = Mathf.Lerp(tilt, -wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
        }

        if (!isWallrunning)
        {
            tilt = Mathf.Lerp(tilt, 0f, cameraChangeTime * Time.deltaTime);
        }
    }
     
    void Update()
    {
        HandleInput();
        CheckWallRun();

        if (isGrounded && !isSliding)
        {
            GroundedMovement();
        }
        else if (!isGrounded && !isWallrunning)
        {
            AirMovement();
        }
        else if (isSliding)
        {
            SlideMovement();
            DecreaseSpeed(slideSpeedDecrease);
            slideTimer -= Time.deltaTime;
            if (slideTimer < 0)
            {
                isSliding = false;
            }
        }
        else if (isWallrunning)
        {
            WallRunMovement();
            DecreaseSpeed(wallRunSpeedDecrease);
        }

        CheckGround();
        controller.Move(move * Time.deltaTime);
        ApplyGravity();
        CameraEffects();
    }

    void GroundedMovement()
    {
        speed = isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : runSpeed;
        move.x = input.x * speed;
        move.z = input.z * speed;

        move = Vector3.ClampMagnitude(move, speed);
    }

    void AirMovement()
    {
        move.x += input.x * airSpeed;
        move.z += input.z * airSpeed;

        move = Vector3.ClampMagnitude(move, speed);
    }

    void SlideMovement()
    {
        move += forwardDirection;
        move = Vector3.ClampMagnitude(move, speed);
    }

    void WallRunMovement()
    {
        if (input.z > (forwardDirection.z - 10f) && input.z < (forwardDirection.z + 10f))
        {
            move += forwardDirection;
        }
        else if (input.z < (forwardDirection.z - 10f) || input.z > (forwardDirection.z + 10f))
        {
            move.x = 0f;
            move.z = 0f;
            ExitWallRun();
        }
        move.x += input.x * airSpeed;

        move = Vector3.ClampMagnitude(move, speed);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (isGrounded)
        {
            jumpCharges = 1;
            hasWallRun = false;
        }
    }

    void CheckWallRun()
    {
        onLeftWall = Physics.Raycast(transform.position, -transform.right, out leftWallHit, 0.7f, wallMask);
        onRightWall = Physics.Raycast(transform.position, transform.right, out rightWallHit, 0.7f, wallMask);

        if ((onRightWall || onLeftWall) && !isWallrunning)
        {
            TestWallRun();
        }
        if ((!onRightWall && !onLeftWall) && isWallrunning)
        {
            ExitWallRun();
        }
    }

    void TestWallRun()
    {
        wallNormal = onLeftWall ? leftWallHit.normal : rightWallHit.normal;
        if (hasWallRun)
        {
            float wallAngle = Vector3.Angle(wallNormal, lastWallNormal);
            if (wallAngle > 15)
            {
                WallRun();
            }
        }
        else 
        {
            WallRun();
            hasWallRun = true;
        }
    }

    void ApplyGravity()
    {
        gravity = isWallrunning ? wallRunGravity : normalGravity;
        yVelocity.y += gravity * Time.deltaTime;
        controller.Move(yVelocity * Time.deltaTime);
    }

    void Jump()
    {
        if (!isGrounded && !isWallrunning)
        {
            jumpCharges = 1;
        }
        else if (isWallrunning)
        {
            ExitWallRun();
            IncreaseSpeed(wallRunSpeedIncrease);
        }
        
        jumpCharges--;

        yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
    }

    void Crouch()
    {
        controller.height = crouchHeight;
        controller.center = crouchingCenter;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        isCrouching = true;
        if (speed > runSpeed)
        {
            isSliding = true;
            forwardDirection = transform.forward;

            if (isGrounded)
            {
                IncreaseSpeed(slideSpeedIncrease);
            }
            slideTimer = maxSlideTimer;
        }
    }

    void WallRun()
    {
        isWallrunning = true;
        jumpCharges = 1;
        IncreaseSpeed(wallRunSpeedIncrease);
        yVelocity = Vector3.zero;

        forwardDirection = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(forwardDirection, transform.forward) < 0)
        {
            forwardDirection = -forwardDirection;
        }
    }

    void ExitWallRun()
    {
        isWallrunning = false; 
        lastWallNormal = wallNormal;
    }

    void ExitCrouch()
    {
        controller.height = startHeight;
        controller.center = standingCenter;
        transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);
        isCrouching = false;
        isSliding = false;
    }  
}
