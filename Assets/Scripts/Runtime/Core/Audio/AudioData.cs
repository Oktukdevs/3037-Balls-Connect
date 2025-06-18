using System;
using UnityEngine;

namespace Runtime.Core.Audio
{
    [Serializable]
    public class AudioData
    {
        public string Id;
        public AudioType AudioType;
        public AudioClip Clip;
    }
}