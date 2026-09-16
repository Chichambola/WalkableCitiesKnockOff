using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPlayer, ICapturable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private FeetHandler _feetHandler;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Kicker _kicker;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private SoundVerifier _soundVerifier;
    [SerializeField] private Transform _body;
    [SerializeField] private StateSaver _stateSaver;
    
    public event Action RequestedRestart;
    public event Action<ISoundOwner> HitSoundOwner;
    public event Action Stepped;
    
    private Rigidbody2D _rigidbody;
    private bool _isFrozen;

    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
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
    }
    
    private void OnEnable()
    {
        _stateSaver.Execute(this);
        
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
    
    private void SubscribeEvents()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _collisionHandler.HitKickable += OnHitKickable;
        _collisionHandler.HitWall += OnHitWall;
        _collisionHandler.HitSoundOwner += OnHitSoundOwner;
        _collisionHandler.Stuck += OnStuck;
        _feetHandler.Stuck += OnStuck;
    }
    
    private void UnsubscribeEvents()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _collisionHandler.HitKickable -= OnHitKickable;
        _collisionHandler.HitWall -= OnHitWall;
        _collisionHandler.HitSoundOwner -= OnHitSoundOwner;
        _collisionHandler.Stuck -= OnStuck;
        _feetHandler.Stuck -= OnStuck;
    }

    public void Set(Vector3 spawnPointPosition) => transform.position = spawnPointPosition;

    public void Set(Quaternion rotation) => transform.rotation = rotation;

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
        Vector3 value = _feetHandler.GetPosition();
        _rotator.SetPoint(value);
        _rotator.SwitchPosition();
        
        Stepped?.Invoke();
        
        var soundOwner = _soundVerifier.DetermineSound(value);

        if (soundOwner != null)
            HitSoundOwner?.Invoke(soundOwner);
    }

    private void OnResetPressed() => RequestedRestart?.Invoke();
    
    private void OnHitKickable(IKickable kickable, Vector2 direction) => _kicker.Execute(direction, kickable);
    
    private void OnHitWall() => _rotator.SwitchDirection();

    private void OnHitManyCollisions()
    {
        _rotator.SwitchDirection();
    }

    private void OnStuck()
    {
        transform.position = _stateSaver.GetPosition();
        transform.rotation = _stateSaver.GetRotation();
        
        _rotator.SwitchDirection();
    }
    
    private void OnHitSoundOwner(ISoundOwner soundOwner) => HitSoundOwner?.Invoke(soundOwner);
}
