using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 When adding a new value for any enums,
always put it at the end of the list. 
Otherwise, you will have to reassign
all references that go after the list!!!
DO NOT ADD ANY ENUMS AT THE START OF THE LIST!!!
*/

public enum E_DamageTypes
{
    Physical, Ember, Static, Bleak, Septic, Random, Perforation, Healing, Arcana, Shield
}

public enum E_Scenes
{
    [InspectorName("Dev Room")]
    DevRoom,
    Combat
}