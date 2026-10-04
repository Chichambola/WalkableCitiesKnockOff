using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TMPro;
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
    public event Action Stepped;
    public event Action<ESoundType, Vector2> HitSoundOwner;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _currentFootPosition;
    private float _currentFootRotation;

    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public bool IsAccepting => _inputReader.IsHoldingAccept;
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
        SubscribeEvents();
    }

    private void Start()
    {
        _rotator.SetPoint(_feetHandler.GetPosition());
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _inputReader.ResetPressed += OnResetPressed;
        _collisionHandler.HitKickable += OnHitKickable;
        _collisionHandler.HitWall += OnHitWall;
        _collisionHandler.HitSoundOwner += OnHit;
    }
    
    private void UnsubscribeEvents()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _inputReader.ResetPressed -= OnResetPressed;
        _collisionHandler.HitKickable -= OnHitKickable;
        _collisionHandler.HitWall -= OnHitWall;
        _collisionHandler.HitSoundOwner -= OnHit;
    }

    public void Set(Vector3 spawnPointPosition) => transform.position = spawnPointPosition;

    public void Set(Quaternion rotation) => transform.rotation = rotation;
    
    public void ResetCharacteristics()
    {
        _rotator.ResetCharacteristics(Vector2.zero);

        _feetHandler.ResetFeet();
    }
    
    private void OnChangeKeyPressed()
    {
        if (Game.IsPaused)
            return;
        
        ChangePosition();
        
        Stepped?.Invoke();
        
        var soundOwner = _soundVerifier.DetermineSound(_currentFootPosition, _currentFootRotation);
        
        HitSoundOwner?.Invoke(soundOwner, Position);
    }

    private void OnResetPressed() => RequestedRestart?.Invoke();
    
    private void OnHitKickable(IKickable kickable, Vector2 direction)
    {
        _kicker.Execute(kickable, direction);
    }

    private void OnHitWall() => _rotator.SwitchDirection();
    
    private void OnHit(ISoundOwner soundOwner) => HitSoundOwner?.Invoke(soundOwner.Type, Position);
    
    private void OnStuck(Vector2 separation)
    {
        Vector3 offset = new Vector3(separation.x, separation.y, 0);
        
        transform.position += offset;
        
        ChangePosition();
    }
    
    private void ChangePosition()
    {
        _feetHandler.Switch();
        _currentFootPosition = _feetHandler.GetPosition();
        _currentFootRotation = _feetHandler.GetRotation();
        _rotator.SetPoint(_currentFootPosition);
        _rotator.SwitchPosition();
    }
}