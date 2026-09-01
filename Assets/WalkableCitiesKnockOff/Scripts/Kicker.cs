using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicker : MonoBehaviour
{
    [SerializeField] private float _force;

    public void Execute(Vector2 direction, IKickable kickable)
    {
        var directionForce = direction * _force;
        
        kickable.ChangeDirection(directionForce);
    }
}
