using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleController : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 15f;


    public Transform firePoint;
    public GameObject fireProjectile;
    public float fireForce = 250f;
    public float fireDelay = .3f;
    float lastFire = 0f;

    bool fire;

    public Transform visual;


    private Vector2 moveInput;
    private Rigidbody rb;

    public  float jumpForce = 5f;
    public LayerMask groundLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value) 
    {
        moveInput = value.Get<Vector2>();
    }


    void OnJump()
    {
        Jump();
    }

    void OnFire(InputValue value)
    {
        fire = value.isPressed;
    }


    private void Update()
    {
        if(fire && Time.time > lastFire)
        {
            lastFire = Time.time + fireDelay;
            GameObject projectile = Instantiate(fireProjectile, firePoint.position, Quaternion.LookRotation(firePoint.forward));
            projectile.GetComponent<Rigidbody>().AddForce(firePoint.forward * fireForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        if(movement.magnitude > 0f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement, Vector3.up);
            visual.rotation = Quaternion.RotateTowards(visual.rotation, newRotation, rotateSpeed);
        }
    }

    private void Jump() 
    {
        if(Physics.Raycast(transform.position, Vector3.down, 4.1f, groundLayer)) 
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
