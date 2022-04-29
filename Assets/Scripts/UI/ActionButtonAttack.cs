using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonAttack : MonoBehaviour
{
    public AttackEvent TheAttackEvent;
    public Attack TheAttack;

    public void OnClick()
    {
        TheAttackEvent.Raise(TheAttack);
    }
}