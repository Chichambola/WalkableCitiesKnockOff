using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicker : MonoBehaviour
{
    [SerializeField] private float _force;

    public void Execute( IKickable kickable, Vector2 direction)
    {
        var directionForce = direction * _force;
        
        kickable.ChangeDirection(directionForce);
    }
}
