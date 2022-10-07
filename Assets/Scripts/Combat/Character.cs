using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    CharacterHealth health; public CharacterHealth GetHealth() { return health; }


    private void Start()
    {
        health = GetComponent<CharacterHealth>();
    }
}
