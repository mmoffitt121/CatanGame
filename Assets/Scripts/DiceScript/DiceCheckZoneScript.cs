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
   
    void OnTriggerStay(Collider col)
    {
       if(true)//diceVelocity1.x == 0f && diceVelocity1.y == 0f && diceVelocity1.z == 0f && diceVelocity2.x == 0f && diceVelocity2.y == 0f && diceVelocity2.z == 0f) 
       {
            Debug.Log("-" + col.gameObject.name + "-");
            switch (col.gameObject.name)
            {
                case "Side1":
                    Debug.Log("1!");
                    diceNumber1 = 6;
                    break;
                case "Side2":
                    diceNumber1 = 5;
                    break;
                case "Side3":
                    diceNumber1 = 4;
                    break;
                case "Side4":
                    diceNumber1 = 3;
                    break;
                case "Side5":
                    diceNumber1 = 2;
                    break;
                case "Side6":
                    diceNumber1 = 1;
                    break;
                case "Side1b":
                    diceNumber2 = 6;
                    break;
                case "Side2b":
                    diceNumber2 = 5;
                    break;
                case "Side3b":
                    diceNumber2 = 4;
                    break;
                case "Side4b":
                    diceNumber2 = 3;
                    break;
                case "Side5b":
                    diceNumber2 = 2;
                    break;
                case "Side6b":
                    diceNumber2 = 1;
                    break;
            }
       }
       
    }        
}
