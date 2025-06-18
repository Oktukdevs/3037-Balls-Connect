using UnityEngine.UIElements;

namespace Tools.AssetCreation.PopupCreator
{
    public class CustomPopupDataElement : VisualElement
    {
        public TextField TextField { get; private set; }
        public Toggle DataToggle { get; private set; }
        public Toggle StateToggle { get; private set; }

        public string PopupName => TextField.text;
        public bool CreateData => DataToggle.value;
        public bool CreateState => StateToggle.value;

        public CustomPopupDataElement()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.FlexStart;

            TextField = new TextField { style = 
                { 
                    width = 295 
                } 
            };
            Add(TextField);

            Label dataLabel= new Label("Create Data?");
            Add(dataLabel);

            DataToggle = new Toggle();
            Add(DataToggle);
            
            Label stateLabel = new Label("Create State?");
            Add(stateLabel);

            StateToggle = new Toggle();
            Add(StateToggle);
        }
    }
}