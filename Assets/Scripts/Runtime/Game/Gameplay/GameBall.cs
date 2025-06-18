using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameBall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private int _value;
    public int Value => _value;

    public void SetValue(int value)
    {
        _value = value;
    }

    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;
}
