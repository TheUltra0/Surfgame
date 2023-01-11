using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

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
    bool grounded;

    public Transform orientation;


    float horizontalInput;
    float verticalInput;
    float y;
    public float sensY;


    Vector3 moveDirection;
    public Transform camera;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
       
        rb=GetComponent<Rigidbody>();
        rb.freezeRotation = true;

       
    
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        MyInput();
        SpeedControl();

        if(grounded) 
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

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
      
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

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
        Vector3 flatVel=new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude>moveSpeed)
        {
            Vector3 limitedVel= flatVel.normalized*moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    void ResetJump()
    {
        readyToJump = true;
    }
}