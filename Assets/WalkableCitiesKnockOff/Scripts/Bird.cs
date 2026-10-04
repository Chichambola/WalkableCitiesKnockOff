using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bird : BasicObject, IKickable, ISoundOwner
{
    [SerializeField] private ESoundType _type = ESoundType.Bird;
    [SerializeField] private SoundRequestChannel _soundRequestChannel;
    
    private bool _canBeKicked;
    
    public ESoundType Type => _type;

    private void OnEnable()
    {
        _canBeKicked = true;
    }
    
    public async UniTaskVoid ChangeDirection(Vector2 directionForce)
    {
        if (!_canBeKicked)
            return;
        
        Rigidbody.AddRelativeForce(directionForce, ForceMode2D.Impulse);
        
        _soundRequestChannel.Raise(_type, transform.position);

        _canBeKicked = false;
        
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));

        _canBeKicked = true;
    }
}
