using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class NoiseShader : MonoBehaviour
{
    [SerializeField] private int _width = 256;
    [SerializeField] private int _height = 256;
    [SerializeField] private float _offsetX = 100f;
    [SerializeField] private float _offsetY = 100f;

    private TilemapRenderer _tilemapRenderer; 
    
    private void Awake()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        _tilemapRenderer.material.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture2D = new Texture2D(_width, _height);

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Color color = CalculateColor(x,y);
                
                texture2D.SetPixel(x,y, color);
            }
        }
        
        texture2D.Apply();

        return texture2D;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoordinates = (float)x / _width + _offsetX;
        float yCoordinates = (float)y / _width + _offsetY;
        
        float sample = Mathf.PerlinNoise(xCoordinates, yCoordinates);

        var color = new Color(sample, sample, sample);

        return color;
    }
}
