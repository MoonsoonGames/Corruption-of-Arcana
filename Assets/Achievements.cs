using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Necropanda;

public class Achievements : MonoBehaviour
{
    public Achievement_Object Achievement;

    public TMPro.TMP_Text AchName;
    public TMPro.TMP_Text AchDesc;
    public TMPro.TMP_Text AchCount;
    public TMPro.TMP_Text AchTotal;

    public GameObject CompleteOverlay;
    public bool isComplete;

    void Start()
    {
        AchName.text = Achievement.name;
        AchDesc.text = Achievement.description;

        AchCount.text = Achievement.count.ToString();
        AchTotal.text = Achievement.total.ToString();
    }

    void Update()
    {
        //on pickup/kill, add one to count
        /*
            if (Achievement.count >= Achievement.total)
            {
                isComplete = true;
            }
            else
            {
                isComplete = false;
            }
        }

        if (isComplete == true)
        {
            CompleteOverlay.SetActive(true);
        }
        
        else
        {
            CompleteOverlay.SetActive(false);
        }
        */

        AchCount.text = Achievement.count.ToString();
        AchTotal.text = Achievement.total.ToString();
    }
}
