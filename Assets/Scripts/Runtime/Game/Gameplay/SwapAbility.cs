using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.Audio;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SwapAbility : MonoBehaviour
{
    [SerializeField] private Button _abilityButton;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private RectTransform _randomEffectImage;

    [SerializeField] private float _rotateSpeed;
    
    private BallBoard _ballBoard;
    private PathDrawController _pathDrawController;
    private SwapDrawController _swapDrawController;
    private BallsAnimationController _animationController;
    private UserInventoryService _userInventoryService;
    private IAudioService _audioService;

    [Inject]
    private void Construct(BallBoard ballBoard, PathDrawController pathDrawController, SwapDrawController swapDrawController,
        BallsAnimationController ballsAnimationController, UserInventoryService userInventoryService, IAudioService audioService)
    {
        _ballBoard = ballBoard;
        _pathDrawController = pathDrawController;
        _swapDrawController = swapDrawController;
        _animationController = ballsAnimationController;
        _userInventoryService = userInventoryService;
        _audioService = audioService;
    }
    
    private void Awake()
    {
        UpdateCount(_userInventoryService.GetSwaps());
        
        _abilityButton.onClick.AddListener(ProcessAbilityClick);
        _userInventoryService.OnSwapChanged += UpdateCount;
        _swapDrawController.OnSwapped += ProcessSwap;
    }

    private void Update()
    {
        _randomEffectImage.RotateAround(Vector3.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        _userInventoryService.OnSwapChanged -= UpdateCount;
        _swapDrawController.OnSwapped -= ProcessSwap;
    }

    private void UpdateCount(int count) => _countText.text = count.ToString();

    private void ProcessAbilityClick()
    {
        if(_userInventoryService.GetSwaps() <= 0)
            return;
        
        _userInventoryService.AddSwaps(-1);
        EnableSwap();
    }

    private void EnableSwap()
    {
        _audioService.PlaySound(ConstAudio.SelectSound);

        _abilityButton.interactable = false;
        _pathDrawController.SetEnabled(false);
        _swapDrawController.SetEnabled(true);
        _randomEffectImage.gameObject.SetActive(true);
    }

    private async void ProcessSwap(GameBall ball1, GameBall ball2)
    {
        _randomEffectImage.gameObject.SetActive(false);
        
        _ballBoard.SwapBalls(ball1, ball2);
        _swapDrawController.SetEnabled(false);

        _audioService.PlaySound(ConstAudio.SwapSound);
        
        await _animationController.AnimateSwap(ball1, ball2);
        
        _pathDrawController.SetEnabled(true);
        _abilityButton.interactable = true;
    }
}
