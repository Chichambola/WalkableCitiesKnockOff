using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ball : BasicObject, IKickable, ISoundOwner, ISearchable
{
    [SerializeField] private ESoundType _type = ESoundType.Ball;
    [SerializeField] private SoundRequestChannel _soundRequestChannel;

    private bool _canBeKicked;
    
    public ESoundType Type => _type;
    public Vector2 Position => transform.position;

    private void OnEnable()
    {
        _canBeKicked = true;
    }

    public async UniTaskVoid ChangeDirection(Vector2 directionForce)
    {
        if (!_canBeKicked)
            return;
        
        Rigidbody.AddForce(directionForce.normalized, ForceMode2D.Impulse);
        
        _soundRequestChannel.Raise(_type, transform.position);

        _canBeKicked = false;
        
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));

        _canBeKicked = true;
    }
}
