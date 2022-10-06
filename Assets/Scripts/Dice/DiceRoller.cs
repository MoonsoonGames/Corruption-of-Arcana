using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceRoller : MonoBehaviour
{
    Dice[] diceArray;

    private void Start()
    {
        diceArray = GameObject.FindObjectsOfType<Dice>();
    }

    public void RollDiceButton()
    {
        int totalValue = 0;

        Debug.Log("Roll all dice");
        foreach (Dice die in diceArray)
        {
            totalValue += die.RollDice();
        }

        Debug.Log(totalValue);
    }
}
