/// AUTHOR: Wuraola Alli
/// FILENAME: DiceCheckZoneScript.cs
/// SPECIFICATION: File that operates dice
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameManagement;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour
{
    public Vector3 diceVelocity1;
    public Vector3 diceVelocity2;
    public int diceNumber1;
    public int diceNumber2;
    public GameObject dice1;
    public GameObject dice2;

    public int diceValue
    {
        get
        {
            return diceNumber1 + diceNumber2;
        }
    }

    void FixedUpdate()
    {
        diceVelocity1 = dice1.GetComponent<DiceScript>().diceVelocity;
        diceVelocity2 = dice2.GetComponent<DiceScript>().diceVelocity;
    }

    public void Roll()
    {
        dice1.GetComponent<DiceScript>().Roll();
        dice2.GetComponent<DiceScript>().Roll();
    }
   
    void OnTriggerStay(Collider col)
    {
        if (diceVelocity1.x == 0f && diceVelocity1.y == 0f && diceVelocity1.z == 0f && diceVelocity2.x == 0f && diceVelocity2.y == 0f && diceVelocity2.z == 0f) 
       {
            
            switch (col.gameObject.name)
            {
                case "side1":
                    diceNumber1 = 6;
                    break;
                case "side2":
                    diceNumber1 = 5;
                    break;
                case "side3":
                    diceNumber1 = 4;
                    break;
                case "side4":
                    diceNumber1 = 3;
                    break;
                case "side5":
                    diceNumber1 = 2;
                    break;
                case "side6":
                    diceNumber1 = 1;
                    break;
                case "side1b":
                    diceNumber2 = 6;
                    break;
                case "side2b":
                    diceNumber2 = 5;
                    break;
                case "side3b":
                    diceNumber2 = 4;
                    break;
                case "side4b":
                    diceNumber2 = 3;
                    break;
                case "side5b":
                    diceNumber2 = 2;
                    break;
                case "side6b":
                    diceNumber2 = 1;
                    break;
            }
       }

       if (diceNumber1 != 0 && diceNumber2 != 0)
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().Rolled(diceValue);
            diceNumber1 = 0;
            diceNumber2 = 0;
        }
       
    }        
}
