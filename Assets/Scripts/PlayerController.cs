using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{

    [Header("Movement")]
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float groundDrag = 10.0f;
    [SerializeField] private float turnSpeed = 1080;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float dragDashing = 5.0f;
    bool readyToJump;
    public Vector3 input;
    public Vector3 mouseInput;
     public Camera Camera;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private float rayCastExtension = 0.01f;
    [SerializeField] public bool isGrounded;
    [SerializeField] private LayerMask Ground;
    public Dashing dashScript;
    public Shoot shoot;
    public Vector3 dirVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashScript = GetComponent<Dashing>();
        readyToJump = true;
    }

    void Update()
    {
        GetInput();
        //Look();
        DragHandler();
        RotateUsingMouse();
        dirVector=MovementwithStrafing(input);

    }

    void FixedUpdate()
    {
        Movement();
        GroundCheck();
        
    }

    void GetInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        mouseInput= Input.mousePosition;

        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
        {
            Jumping();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            shoot.Shooting();
            //shoot.bulletTrail.enabled=true;
        }
        else if(Input.GetButton("Fire1"))
        {
            shoot.Autoshoot();
            //shoot.bulletTrail.enabled=true;
        }
        else
        {
            //shoot.bulletTrail.enabled=false;
        }
    }

    void Movement()
    {
        if (isGrounded)
            rb.MovePosition(transform.position + transform.forward * input.normalized.magnitude * moveSpeed * Time.deltaTime);
        else if (!isGrounded)
            rb.MovePosition(transform.position + transform.forward * input.normalized.magnitude * moveSpeed * airMultiplier * Time.deltaTime);
    }

    // private void Look()
    // {
    //     if (input == Vector3.zero) return;

    //     var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
    //     transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    // }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + rayCastExtension, Ground);
    }
    private void Jumping()
    {
        readyToJump = false;

        Jump();

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    private void DragHandler()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else if (!isGrounded && dashScript.isDashing)
            rb.drag = dragDashing;
        else if (!isGrounded && !dashScript.isDashing)
            rb.drag=1.0f;
    }

      private void RotateUsingMouse()
    {
        Ray ray = Camera.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 100f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }

   
    }
        private Vector3 MovementwithStrafing(Vector3 dirVector)
    {
        var speed = moveSpeed * Time.deltaTime;
       

        dirVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y,0 ) * dirVector;
        var targetPosition = transform.position + dirVector * speed;
        transform.position = targetPosition;
        return dirVector;
}
}