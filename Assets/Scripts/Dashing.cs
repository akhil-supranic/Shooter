using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("Dash Variables")]
    private float dashStartTime;
    [SerializeField] private int dashAttepts = 0;
    [SerializeField] private int dashLimit = 10;
    [SerializeField] public bool isDashing;
    [SerializeField] private float dashForce = 2f;

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
        if (Input.GetKeyDown(KeyCode.E) && !isDashing && dashAttepts <= dashLimit)
        {
            OnDashStart();
        }
        if (isDashing && (Time.time - dashStartTime <= 0.01f))
        
        {       rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        }
        else
        {
            OnDashEnd();
        }
    }
}
