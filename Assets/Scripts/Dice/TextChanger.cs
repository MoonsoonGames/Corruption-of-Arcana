using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    [TextArea(3, 10)]
    public string rawDescription;
    string shownDescription;
    public TextMeshProUGUI totalText;

    DiceRoller diceRoller;

    private void Start()
    {
        diceRoller = GameObject.FindObjectOfType<DiceRoller>();
        CalculateDice();
    }

    public void CalculateDice()
    {
        Vector2 diceRange = diceRoller.GetDiceRange();
        shownDescription = rawDescription;
        shownDescription = shownDescription.Replace("$diceValue$", diceRange.x + "-" + diceRange.y);
        descriptionText.text = shownDescription;
    }

    public void AdjustTotal(int newTotal)
    {
        totalText.text = newTotal.ToString();
    }
}
