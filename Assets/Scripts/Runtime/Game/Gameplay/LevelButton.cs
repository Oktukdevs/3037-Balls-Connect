using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _lock;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _activeStar;
    
    public event Action<int> OnLevelSelected;
    
    public void Initialize(Sprite sprite, int level, int stars, bool locked)
    {
        _button.interactable = !locked;
        _button.image.sprite = sprite;
        
        _levelText.text = (level + 1).ToString();

        SetStars(stars);
        
        _lock.gameObject.SetActive(locked);
        _levelText.gameObject.SetActive(!locked);
        
        _button.onClick.AddListener(() => OnLevelSelected?.Invoke(level));
    }

    private void SetStars(int stars)
    {
        for (int i = 0; i < stars; i++)
            _stars[i].sprite = _activeStar;
    }
}
