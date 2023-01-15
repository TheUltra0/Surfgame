using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")] public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Keybinds")] 
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")] 
    public float playerHeight;

    public LayerMask whatisGround;

    public Transform orientation;
    public float sensY;
    public Transform camera;
    bool grounded;


    float horizontalInput;


    Vector3 moveDirection;

    Rigidbody rb;
    float verticalInput;

    float y;

  
    private Vector2 startPos;
    public int pixelDistToDetect = 20;
    private bool fingerDown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            // If so, we're going to set the startPos to the first touch's position, 
            startPos = Input.touches[0].position;
            // ... and set fingerDown to true to start checking the direction of the swipe.
            fingerDown = true;
        }
        if (fingerDown && Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            fingerDown = true;

        }
        if (fingerDown)
        {
            //Did we swipe up?
            if (Input.touches[0].position.y >= startPos.y + pixelDistToDetect)
            {
                fingerDown = false;
                if (readyToJump && grounded)
                {
                    readyToJump = false;
                    Jump();
                    Invoke(nameof(ResetJump), jumpCooldown);
                }
            }
            //Did we swipe left?
            else if (Input.touches[0].position.x <= startPos.x - pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe left");
            }
            //Did we swipe right?
            else if (Input.touches[0].position.x >= startPos.x + pixelDistToDetect)
            {
                fingerDown = false;
                Debug.Log("Swipe right");
            }
            else if (Input.touches[0].position.y <= startPos.y - pixelDistToDetect)
            {
                fingerDown = false;
                
                Crouch();
                
            }
        }
        //transform.rotation=Quaternion.Euler(xRotation, yRotation, 0);
        //rb.MovePosition(transform.position + new Vector3(0, 0, 2f) * Time.deltaTime * 10f);

        //rb.AddForce(moveDirection.normalized * 10f * 10f, ForceMode.Force);

        
    }

    // Update is called once per frame
    void MovePlayer()
    {
        y = camera.rotation.eulerAngles.y;

        //transform.rotation=Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, y, 0);
        //transform.rotation=Quaternion.Euler(xRotation, yRotation, 0);


        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //rb.forward = Vector3.Slerp(rb.forward, moveDirection.normalized, Time.deltaTime * moveSpeed);
        if (grounded)
        {
            ResetJump();
            //rb.forward = Vector3.Slerp(rb.forward, moveDirection.normalized, Time.deltaTime * moveSpeed);

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void Crouch()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(-transform.up * jumpForce*2f, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }
}