using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPlayer
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private FeetHandler _feetHandler;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Kicker _kicker;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private SoundVerifier _soundVerifier;
    [SerializeField] private Transform _body;

    public event Action RequestedRestart;
    public event Action<ISoundOwner> HitSoundOwner; 
    
    private Rigidbody2D _rigidbody;
    private float _count;

    public Vector2 CurrentPosition => transform.position;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<BoxCollider2D>().size = _body.transform.localScale;
    }

    private void OnEnable()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _inputReader.ResetPressed += OnResetPressed;
        _collisionHandler.HitKickable += OnHitKickable;
        _collisionHandler.HitWall += OnHitWall;
        _collisionHandler.HitManyCollisons += OnHitManyCollisions;
        _collisionHandler.HitSoundOwner += OnHitSoundOwner;
    }

    private void OnDisable()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _inputReader.ResetPressed -= OnResetPressed;
        _collisionHandler.HitKickable -= OnHitKickable;
        _collisionHandler.HitWall -= OnHitWall;
        _collisionHandler.HitManyCollisons -= OnHitManyCollisions;
        _collisionHandler.HitSoundOwner -= OnHitSoundOwner;
    }

    private void OnChangeKeyPressed()
    {
        _rigidbody.Sleep();
        
        Vector3 value = _feetHandler.GetPosition();
        _rotator.SetPoint(value);
        _rotator.SwitchPosition();
        
        _rigidbody.WakeUp();
        
        var soundOwner = _soundVerifier.DetermineSound(value);

        if (soundOwner != null)
            HitSoundOwner?.Invoke(soundOwner);
    }

    private void OnResetPressed() => RequestedRestart?.Invoke();
    
    private void OnHitKickable(IKickable kickable, Vector2 direction) => _kicker.Execute(direction, kickable);
    
    private void OnHitWall() => _rotator.SwitchDirection();
    
    private void OnHitManyCollisions() => _rotator.SwitchDirection();

    private void OnHitSoundOwner(ISoundOwner soundOwner) => HitSoundOwner?.Invoke(soundOwner);
}
