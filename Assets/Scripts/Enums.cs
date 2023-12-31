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
    Physical, Ember, Static, Bleak, Septic, Random, Perforation, Healing, Shield, Arcana, Summon
}

public enum E_SpellTargetType
{
    Caster, Target, Cleave, Chain, RandomEnemyTeam, RandomAll, All, AllEnemies
}

public enum E_StatusTargetType
{
    Self, Team, OpponentTeam, Reflect, SelfHit
}

public enum E_Statuses
{
    None, Banish, Charm, Silence, Stun, Curse, EmpowerDeck, WeakenDeck, Reflect, Redirect, Confuse, Enlightened, Blinded
}

public enum E_ProjectilePoints
{
    Caster, Target, Ground, OpponentGround, Backstab, BackstabOpponent, Above, AboveOpponents, Sky, TimeBlock, Wheel, SpawnEnemy
}

public enum E_Scenes
{
    Null,
    [InspectorName("Dev Room")]
    DevRoom,
    Combat,
    Tiertarock,
    PuzzleRoom,
    Thoth,
    Cave,
    ArenaMode,
    SplashScreen,
    Navigation,
    EastForest,
    Tutorial,
    [InspectorName("Intro Cutscene")]
    IntroCutscene,
    MobileSplashScreen
}

public enum E_UtilityScripts
{
    Position, Rotation, Scale
}

public enum E_QuestStates
{
    NotStarted, InProgress, Completed
}