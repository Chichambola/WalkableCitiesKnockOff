using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IKickable
{
    UniTaskVoid ChangeDirection(Vector2 directionForce);
}
