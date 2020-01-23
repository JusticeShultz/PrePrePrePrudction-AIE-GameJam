﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rollSpeed = 500;
    public float rollCooldown = 1.2f;
    public float rollCooldownDelay = 0.5f;
    
    [ReadOnly] public Rigidbody rb;

    float rollcharge = 0f;
    float rollchargeCD = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        rollcharge -= Time.deltaTime;

        if (rollcharge <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            float distance;

            if (plane.Raycast(ray, out distance))
            {
                Vector3 target = ray.GetPoint(distance);
                Vector3 direction = target - transform.position;
                float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0, rotation, 0);
            }

            rb.AddForce(((new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime) * movementSpeed) * 100);

            rollchargeCD -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && rollcharge <= 0 && rollchargeCD <= 0)
        {
            rollcharge = rollCooldown;
            rollchargeCD = rollCooldownDelay;

            rb.AddForce((transform.forward * Time.deltaTime * rollSpeed) * 100, ForceMode.Acceleration);
        }

        if (rollcharge > 0)
        {
            rb.drag = 3;
        }
        else rb.drag = 25;
    }
}