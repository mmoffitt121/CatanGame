/// AUTHOR: Wuraola Alli
/// FILENAME: DiceScript.cs
/// SPECIFICATION: File that operates dice
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 diceVelocity;
    public Vector3 initialPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        diceVelocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }
    }

    public void Roll()
    {
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = GameObject.Find("DiceCheckZone").transform.position + initialPos;
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
