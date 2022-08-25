using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10.0f; 
    [SerializeField] private float turnSpeed = 360; 
    private Vector3 input;
    void Start()
    {

    }

    void Update()
    {
        GetInput();
        Look();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void GetInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical"));
    }

    void Movement()
    {
        rb.MovePosition(transform.position + transform.forward * input.normalized.magnitude * moveSpeed *Time.deltaTime);
    }

        private void Look() {
        if (input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
}
}