using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BallsTransformController
{
    public const float OffsetDistance = 6.5f;
    
    public Vector3 GetWorldPosition(int x, int y, bool invert = true)
    {
        int fieldSize = BallBoard.FieldSize;
        
        int yCell = y - fieldSize / 2;
        
        if(!invert)
            yCell *= -1;
        
        float yPos = yCell * OffsetDistance;

        return new Vector3(GetWorldPositionX(x), yPos, 0);
    }

    public float GetWorldPositionX(int x)
    {
        int fieldSize = BallBoard.FieldSize;
        int xCell = x - fieldSize / 2;

        float xPos = xCell * OffsetDistance;

        return xPos;
    }
}
