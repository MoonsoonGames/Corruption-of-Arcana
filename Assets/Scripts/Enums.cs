using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 When adding a new value for any enums,
always put it at the end of the list. 
Otherwise, you will have to reassign
all references that go after in the list!!!
DO NOT ADD ANY ENUMS AT THE START OF THE LIST!!!
*/

public enum E_DamageTypes
{
    Physical, Ember, Static, Bleak, Septic, Random, Perforation, Healing, Shield, Arcana
}

public enum E_SpellTargetType
{
    Caster, Target, Cleave, Chain, RandomTargetTeam, RandomAll, All
}

public enum E_StatusTargetType
{
    Self, Team, OpponentTeam, Reflect, SelfHit
}

public enum E_Statuses
{
    None, Banish, Charm, Silence, Stun, Curse
}

public enum E_Scenes
{
    [InspectorName("Dev Room")]
    DevRoom,
    Combat
}

public enum E_UtilityScripts
{
    Position, Rotation, Scale
}