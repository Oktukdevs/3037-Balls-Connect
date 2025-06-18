using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundActivator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
    [SerializeField] private SpriteRenderer _fieldSpriteRenderer;
    [SerializeField] private SpriteRenderer _headerSpriteRenderer;
    
    public void SetBackgroundSprite(Sprite sprite) => _backgroundSpriteRenderer.sprite = sprite;

    public void SetFieldSprite((Sprite field, Sprite header) skin)
    {
        _fieldSpriteRenderer.sprite = skin.field;
        _headerSpriteRenderer.sprite = skin.header;
    }

    public void Enable(bool enable)
    {
        _backgroundSpriteRenderer.enabled = enable;
        _fieldSpriteRenderer.enabled = enable;
        _headerSpriteRenderer.enabled = enable;
    }
}
