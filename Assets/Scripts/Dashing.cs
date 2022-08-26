using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("Dash Variables")]
    private float dashStartTime;
    [SerializeField] private int dashAttepts = 0;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashSpeed = 2f;
    PlayerController playerController;
    Rigidbody rb;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Dash();
    }

    void OnDashStart()
    {
        dashStartTime = Time.time;
        isDashing = true;
        dashAttepts += 1;
    }

    void OnDashEnd()
    {
        isDashing = false;
        dashStartTime = 0;
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isDashing && dashAttepts <= 3)
        {
            OnDashStart();
        }
        if (isDashing && (Time.time - dashStartTime <= 0.5f))
        {
            if (playerController.input.Equals(Vector3.zero))
            {
                rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(transform.forward * playerController.input.normalized.magnitude * dashSpeed, ForceMode.Impulse);


            }
        }
        else
        {
            OnDashEnd();
        }
    }
}
