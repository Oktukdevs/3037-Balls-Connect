using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UserInputService : IInitializable
{
    private Camera _camera;
    
    public void Initialize()
    {
        _camera = Camera.main;
    }

    public bool IsTouching => Input.touchCount > 0;

    public Vector2 GetTouchWorldPos()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 worldPos = _camera.ScreenToWorldPoint(touch.position);
        return new Vector2(worldPos.x, worldPos.y);
    }
}
