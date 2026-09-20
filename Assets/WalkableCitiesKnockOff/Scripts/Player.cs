using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPlayer, ICapturable, IStuckDetector
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private FeetHandler _feetHandler;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Kicker _kicker;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private SoundVerifier _soundVerifier;
    [SerializeField] private Transform _body;
    [SerializeField] private StuckPreventor _stuckPreventor;
    
    public event Action RequestedRestart;
    public event Action<ISoundOwner> HitSoundOwner;
    public event Action Stepped;
    
    private Rigidbody2D _rigidbody;
    private bool _isFrozen;

    public Vector2 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public bool IsAccepting => _inputReader.IsHoldingAccept;
    public bool IsStuck => _stuckPreventor.IsStuck;
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
        _feetHandler.Init(this);
        _rotator.Init(this);
        
        StateSaver.Register(this);
        
        SubscribeEvents();

        _inputReader.ResetPressed += OnResetPressed;
        _stuckPreventor.DetectedOverlap += OnDetectedOverlap;
        
        _isFrozen = false;
    }

    private void OnDisable()
    {
        if (!_isFrozen)
        {
            UnsubscribeEvents();
        }
        
        _inputReader.ResetPressed -= OnResetPressed;
        _stuckPreventor.DetectedOverlap -= OnDetectedOverlap;
    }

    private void SubscribeEvents()
    {
        _inputReader.ChangeKeyPressed += OnChangeKeyPressed;
        _collisionHandler.HitKickable += OnHitKickable;
        _collisionHandler.HitWall += OnHitWall;
        _collisionHandler.HitSoundOwner += OnHitSoundOwner;
    }
    
    private void UnsubscribeEvents()
    {
        _inputReader.ChangeKeyPressed -= OnChangeKeyPressed;
        _collisionHandler.HitKickable -= OnHitKickable;
        _collisionHandler.HitWall -= OnHitWall;
        _collisionHandler.HitSoundOwner -= OnHitSoundOwner;
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
        _feetHandler.Switch();
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
    
    private void OnHitSoundOwner(ISoundOwner soundOwner) => HitSoundOwner?.Invoke(soundOwner);
    
    private void OnDetectedOverlap(Vector2 separation)
    {
        ResetToLastState(separation);

        _rotator.ResetToLastState(separation);
        _feetHandler.ResetToLastState(separation);

        _feetHandler.Switch();
        var pos = _feetHandler.GetPosition();
        _rotator.SetPoint(pos);
        _rotator.SwitchPosition();
        
        return;
        
        Debug.Log("Stuck");
        
        Vector3 correctSeparation = new Vector3(separation.x, separation.y, 0);
        
        Vector3 currentPos = new Vector3(_feetHandler.Position.x, _feetHandler.Position.y, 0);
        
        transform.position += correctSeparation;
        
        currentPos += correctSeparation;
        _feetHandler.Set(currentPos);

        currentPos = new Vector3(_rotator.Position.x, _rotator.Position.y, 0);
        currentPos += correctSeparation;
        _rotator.SetPoint(currentPos);
        
        return;
    }

    public void ResetToLastState(Vector2 offset)
    {
        var value = StateSaver.GetState(this);
        
        transform.position = value.Position + offset;
        transform.rotation = value.Rotation;
    }
}