using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Floor : BasicObject, ISoundOwner
{
    [SerializeField] private ESoundType _type = ESoundType.Grass;

    private Renderer _renderer;
    
    public ESoundType Type => _type;
    public int SortingLayerIndex => _renderer.sortingOrder;

    protected override void Awake()
    {
        base.Awake();

        _renderer = GetComponent<Renderer>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        
        RigidbodyType = RigidbodyType2D.Kinematic;
    }
}
