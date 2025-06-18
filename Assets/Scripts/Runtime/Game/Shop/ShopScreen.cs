using DG.Tweening;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using Runtime.Game.Shop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopScreen : MonoBehaviour
{
    private const float AnimTime = 0.33f;
    
    [SerializeField] private Button _themesButton;
    [SerializeField] private Button _itemsButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;

    [SerializeField] private UseItemDisplay _randomizeItem;
    [SerializeField] private UseItemDisplay _swapItem;
    
    [SerializeField] private Sprite _activeSectionSprite;
    [SerializeField] private Sprite _inactiveSectionSprite;
    
    [SerializeField] private RectTransform _bgItemsParent;
    [SerializeField] private RectTransform _fieldItemsParent;
    [SerializeField] private RectTransform _useItemsParent;

    [SerializeField] private RectTransform _errorTransform;
    
    private RectTransform _lastThemesSection;
    
    private ShopItemsFactory _shopItemsFactory;
    private UserInventoryService _userInventoryService;
    private IAudioService _audioService;
    
    private Tween _errorTween;

    [Inject]
    private void Construct(ShopItemsFactory shopItemsFactory, UserInventoryService userInventoryService, IAudioService audioService)
    {
        _shopItemsFactory = shopItemsFactory;
        _userInventoryService = userInventoryService;
        _audioService = audioService;
    }
    
    private void Awake()
    {
        _lastThemesSection = _bgItemsParent;
        
        _themesButton.onClick.AddListener(ShowThemes);
        _itemsButton.onClick.AddListener(ShowUseItems);
        _rightButton.onClick.AddListener(ProcessSwitchThemeSection);
        _leftButton.onClick.AddListener(ProcessSwitchThemeSection);

        foreach (var item in _shopItemsFactory.CreateBackgroundShopItems())
        {
            item.transform.SetParent(_bgItemsParent, false);
            item.OnPurchasePressed += ProcessBackgroundPurchase;
        }
        
        foreach (var item in _shopItemsFactory.CreateFieldShopItems())
        {
            item.transform.SetParent(_fieldItemsParent, false);
            item.OnPurchasePressed += ProcessFieldPurchase;
        }
        
        _randomizeItem.OnPurchasePressed += ProcessRandomizePurchase;
        _swapItem.OnPurchasePressed += ProcessSwapPurchase;
    }

    private void ProcessSwitchThemeSection()
    {
        if (_lastThemesSection == _bgItemsParent)
        {
            _bgItemsParent.gameObject.SetActive(false);
            _fieldItemsParent.gameObject.SetActive(true);
            _lastThemesSection = _fieldItemsParent;
        }
        else
        {
            _bgItemsParent.gameObject.SetActive(true);
            _fieldItemsParent.gameObject.SetActive(false);
            _lastThemesSection = _bgItemsParent;
        }
    }

    private void ShowThemes()
    {
        if(_lastThemesSection == _bgItemsParent)
            _bgItemsParent.gameObject.SetActive(true);
        if(_lastThemesSection == _fieldItemsParent)
            _fieldItemsParent.gameObject.SetActive(true);
        
        _useItemsParent.gameObject.SetActive(false);
        
        _rightButton.gameObject.SetActive(true);
        _leftButton.gameObject.SetActive(true);
        
        _themesButton.image.sprite = _activeSectionSprite;
        _itemsButton.image.sprite = _inactiveSectionSprite;

        _themesButton.transform.DOKill();
        _itemsButton.transform.DOKill();
        _themesButton.transform.DOScale(1, AnimTime).SetLink(gameObject);
        _itemsButton.transform.DOScale(0.75f, AnimTime).SetLink(gameObject);
    }

    private void ShowUseItems()
    {
        _useItemsParent.gameObject.SetActive(true);
        _bgItemsParent.gameObject.SetActive(false);
        _fieldItemsParent.gameObject.SetActive(false);
        
        _rightButton.gameObject.SetActive(false);
        _leftButton.gameObject.SetActive(false);
        
        _themesButton.image.sprite = _inactiveSectionSprite;
        _itemsButton.image.sprite = _activeSectionSprite;
        
        _themesButton.transform.DOKill();
        _itemsButton.transform.DOKill();
        _themesButton.transform.DOScale(0.75f, AnimTime).SetLink(gameObject);
        _itemsButton.transform.DOScale(1, AnimTime).SetLink(gameObject);
    }

    private void ProcessBackgroundPurchase(GameObject go, int id, int price)
    {
        if(!TryPurchase(price))
            return;
        
        _userInventoryService.AddBackground(id);
        _audioService.PlaySound(ConstAudio.SelectSound);
        Destroy(go);
    }

    private void ProcessFieldPurchase(GameObject go, int id, int price)
    {
        if(!TryPurchase(price))
            return;
        
        _userInventoryService.AddField(id);
        _audioService.PlaySound(ConstAudio.SelectSound);
        Destroy(go);
    }

    private void ProcessRandomizePurchase(int price)
    {
        if(!TryPurchase(price))
            return;
        
        _userInventoryService.AddRandomize(1);
        _audioService.PlaySound(ConstAudio.SelectSound);
    }

    private void ProcessSwapPurchase(int price)
    {
        if(!TryPurchase(price))
            return;
        
        _userInventoryService.AddSwaps(1);
        _audioService.PlaySound(ConstAudio.SelectSound);
    }

    private bool TryPurchase(int price)
    {
        if (_userInventoryService.GetBalance() < price)
        {
            PlayErrorAnimation();
            _audioService.PlaySound(ConstAudio.ErrorSound);
            return false;
        }
        
        _userInventoryService.AddBalance(-price);
        return true;
    }
    
    public void PlayErrorAnimation()
    {
        _errorTween?.Kill();

        float slideDuration = 0.25f;
        float visibleDuration = 0.25f;
        float width = _errorTransform.rect.width;

        _errorTween = DOTween.Sequence()
            .Append(_errorTransform.DOAnchorPosX(-width, slideDuration).SetEase(Ease.OutCubic))
            .AppendInterval(visibleDuration)
            .Append(_errorTransform.DOAnchorPosX(0, slideDuration).SetEase(Ease.InCubic))
            .SetUpdate(true);
    }
}
