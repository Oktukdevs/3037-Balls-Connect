using System;
using Runtime.Core.Audio;
using Runtime.Game.Gameplay;
using Runtime.Game.Services.Audio;
using Runtime.Game.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RandomizeAbility : MonoBehaviour
{
    [SerializeField] private Button _abilityButton;
    [SerializeField] private TextMeshProUGUI _countText;

    private BallBoard _ballBoard;
    private PathDrawController _pathDrawController;
    private BallsAnimationController _animationController;
    private UserInventoryService _userInventoryService;
    private IAudioService _audioService;

    [Inject]
    private void Construct(BallBoard ballBoard, PathDrawController pathDrawController, 
        BallsAnimationController ballsAnimationController, UserInventoryService userInventoryService,
        IAudioService audioService)
    {
        _ballBoard = ballBoard;
        _pathDrawController = pathDrawController;
        _animationController = ballsAnimationController;
        _userInventoryService = userInventoryService;
        _audioService = audioService;
    }
    
    private void Awake()
    {
        UpdateCount(_userInventoryService.GetRandomizes());
        
        _abilityButton.onClick.AddListener(ProcessAbilityClick);
        _userInventoryService.OnRandomizeChanged += UpdateCount;
    }

    private void OnDestroy()
    {
        _userInventoryService.OnRandomizeChanged -= UpdateCount;
    }

    private void UpdateCount(int count) => _countText.text = count.ToString();

    private void ProcessAbilityClick()
    {
        if(_userInventoryService.GetRandomizes() <= 0)
            return;
        
        _userInventoryService.AddRandomize(-1);
        RandomizeBoard();
    }

    private async void RandomizeBoard()
    {
        _ballBoard.RandomizeBoard();
        _pathDrawController.SetEnabled(false);
        _abilityButton.interactable = false;
        
        _audioService.PlaySound(ConstAudio.RandomizeSound);
        
        await _animationController.AnimateBoardToCurrentPositions(_ballBoard);
        
        _pathDrawController.SetEnabled(true);
        _abilityButton.interactable = true;
    }
}
