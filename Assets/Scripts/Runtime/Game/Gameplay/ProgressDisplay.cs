using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _progressText;

    public void Initialize(Sprite sprite, int target)
    {
        _image.sprite = sprite;
        UpdateProgress(0, target);
    }

    public void UpdateProgress(int progress, int target)
    {
        _progressText.text = $"{progress}/{target}";
    }
}
