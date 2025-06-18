using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Controllers;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.Shop;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class InventoryController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserInventoryService _userInventoryService;
        private readonly InventoryItemsFactory _inventoryItemsFactory;
        private readonly IAudioService _audioService;

        private List<InventoryItemDisplay> _backgrounds;
        private List<InventoryItemDisplay> _fields;
        
        public InventoryController(IUiService uiService, UserInventoryService userInventoryService,
            InventoryItemsFactory inventoryItemsFactory, IAudioService audioService)
        {
            _uiService = uiService;
            _userInventoryService = userInventoryService;
            _inventoryItemsFactory = inventoryItemsFactory;
            _audioService = audioService;
        }

        public override UniTask Run(CancellationToken cancellationToken)
        {
            base.Run(cancellationToken);

            InventoryPopup popup = _uiService.GetPopup<InventoryPopup>(ConstPopups.InventoryPopup);
            popup.Show(null, cancellationToken).Forget();

            _backgrounds = _inventoryItemsFactory.GetInventoryBackgrounds();
            _fields = _inventoryItemsFactory.GetInventoryFields();
            
            popup.SetBackgrounds(_backgrounds);
            popup.SetFields(_fields);
            
            foreach (var bg in _backgrounds)
                bg.OnSelected += ProcessBackgroundSelection;
            
            foreach (var field in _fields)
                field.OnSelected += ProcessFieldSelect;
            
            CurrentState = ControllerState.Complete;
            return UniTask.CompletedTask;
        }

        private void ProcessBackgroundSelection(int id)
        {
            _userInventoryService.SetUsedBackground(id);

            _audioService.PlaySound(ConstAudio.SelectSound);
            
            foreach (var bg in _backgrounds)
            {
                if(id == bg.Id)
                    bg.SetStatus(InventoryItemsFactory.SelectedStatus, _inventoryItemsFactory.SelectedSprite);
                else
                    bg.SetStatus(InventoryItemsFactory.NotSelectedStatus, _inventoryItemsFactory.NotSelectedSprite);
            }
        }

        private void ProcessFieldSelect(int id)
        {
            _userInventoryService.SetUsedField(id);

            _audioService.PlaySound(ConstAudio.SelectSound);
            
            foreach (var item in _fields)
            {
                if(id == item.Id)
                    item.SetStatus(InventoryItemsFactory.SelectedStatus, _inventoryItemsFactory.SelectedSprite);
                else
                    item.SetStatus(InventoryItemsFactory.NotSelectedStatus, _inventoryItemsFactory.NotSelectedSprite);
            }
        }
    }
}