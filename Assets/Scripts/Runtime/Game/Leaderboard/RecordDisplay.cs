using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _placeText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Initialize(Sprite sprite, int place, string name, int score)
    {
        _image.sprite = sprite;
        _placeText.text = place.ToString();
        _nameText.text = name;
        _scoreText.text = score.ToString();
    }
}
