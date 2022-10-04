/// AUTHOR: Wuraola Alli
/// FILENAME: DiceNumberTextScript.cs
/// SPECIFICATION: File that operates dice
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceNumberTextScript : MonoBehaviour 
{
    Text text;
    public static int diceNumber;
    void Start()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        text.text = diceNumber.ToString();
    }
}
