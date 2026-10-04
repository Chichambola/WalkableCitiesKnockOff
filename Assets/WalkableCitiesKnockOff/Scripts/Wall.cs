using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BasicObject, ISoundOwner
{
    [SerializeField] private ESoundType _type = ESoundType.Wall;

    public ESoundType Type => _type;

    protected override void OnValidate()
    {
        RigidbodyType = RigidbodyType2D.Kinematic;
        
        base.OnValidate();
    }
}
