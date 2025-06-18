using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplay : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private Image _image;
    [SerializeField] private Image _headerImage;
    
    public event Action<int> OnSelected;

    public int Id;

    public void Initialize(int id, Sprite statusSprite, string statusText, Sprite itemSprite)
    {
        Id = id;
        _selectButton.onClick.AddListener(() => OnSelected?.Invoke(id));
        _statusText.text = statusText;
        _image.sprite = itemSprite;
        _selectButton.image.sprite = statusSprite;
    }
    
    public void Initialize(int id, Sprite statusSprite, string statusText, Sprite itemSprite, Sprite headerSprite)
    {
        Initialize(id, statusSprite, statusText, itemSprite);
        _headerImage.sprite = headerSprite;
    }

    public void SetStatus(string text, Sprite statusSprite)
    {
        _statusText.text = text;
        _selectButton.image.sprite = statusSprite;
    }
}
