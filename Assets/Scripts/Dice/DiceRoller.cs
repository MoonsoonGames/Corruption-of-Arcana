using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceRoller : MonoBehaviour
{
    Dice[] diceArray;

    TextChanger textChanger;

    private void Start()
    {
        diceArray = GameObject.FindObjectsOfType<Dice>();
        textChanger = GameObject.FindObjectOfType<TextChanger>();
    }

    public void RollDiceButton()
    {
        int totalValue = 0;

        Debug.Log("Roll all dice");
        foreach (Dice die in diceArray)
        {
            totalValue += die.RollDice();
        }

        textChanger.AdjustTotal(totalValue);
    }

    public Vector2 GetDiceRange()
    {
        Vector2 totalRange = new Vector2(0, 0);

        foreach (Dice die in diceArray)
        {
            totalRange += die.diceValues;
        }

        return totalRange;
    }
}
