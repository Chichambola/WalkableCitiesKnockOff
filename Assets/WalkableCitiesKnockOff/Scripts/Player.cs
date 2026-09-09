using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
    public event Action Stepped;
    
    private Rigidbody2D _rigidbody;
    private float _count;
    private bool _isFrozen;

    public Vector2 CurrentPosition => transform.position;
    public bool IsAccepting => _inputReader.IsHoldingAccept;
    public bool HasPressedKey { get; private set; }
    public string AcceptButton => _inputReader.AcceptButton;
    public string RestartButton => _inputReader.RestartButton;
    public string ChangeFootButton => _inputReader.ChangeFootButton;

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
        SubscribeEvents();

        _inputReader.ResetPressed += OnResetPressed;
        
        HasPressedKey = false;
        _isFrozen = false;
    }

    private void OnDisable()
    {
        if (!_isFrozen)
        {
            UnsubscribeEvents();
        }
        
        _inputReader.ResetPressed -= OnResetPressed;
    }

    private void UnsubscribeEvents()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _collisionHandler.HitKickable -= OnHitKickable;
        _collisionHandler.HitWall -= OnHitWall;
        _collisionHandler.HitManyCollisons -= OnHitManyCollisions;
        _collisionHandler.HitSoundOwner -= OnHitSoundOwner;
    }
    
    private void SubscribeEvents()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _collisionHandler.HitKickable += OnHitKickable;
        _collisionHandler.HitWall += OnHitWall;
        _collisionHandler.HitManyCollisons += OnHitManyCollisions;
        _collisionHandler.HitSoundOwner += OnHitSoundOwner;
    }

    public void Freeze()
    {
        _isFrozen = true;
        
        UnsubscribeEvents();
    }

    public void UnFreeze()
    {
        _isFrozen = false;
        
        SubscribeEvents();
    }
    
    private void OnChangeKeyPressed()
    {
        _rigidbody.Sleep();
        
        Vector3 value = _feetHandler.GetPosition();
        _rotator.SetPoint(value);
        _rotator.SwitchPosition();
        
        _rigidbody.WakeUp();
        
        Stepped?.Invoke();
        
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
