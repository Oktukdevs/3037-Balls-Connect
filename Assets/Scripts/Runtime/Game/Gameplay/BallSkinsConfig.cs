using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

[CreateAssetMenu(fileName = "BallSkinsConfig", menuName = "Config/BallSkinsConfig")]
public class BallSkinsConfig : BaseSettings
{
    public List<Sprite> Sprites = new List<Sprite>();
}
