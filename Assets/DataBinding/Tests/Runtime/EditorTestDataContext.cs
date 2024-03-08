using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;

namespace UnityEngine
{
    public sealed partial class EditorTestAddress : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnCityChanged))]
        [Delayed]
        private string city;

        public EditorTestAddress()
        {
            this.school = new EditorTestSchool(nameof(School));
        }

        public EditorTestAddress(string baseDataContextPath = null) : base(baseDataContextPath)
        {
            this.school = new EditorTestSchool(nameof(School));
        }

        public string City
        {
            get => this.city;
            set { this.SetField(ref this.city, value); }
        }

        private void OnCityChanged()
        {
            this.OnDataContextChanged(nameof(this.City));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnLoopScrollRectItemsChanged))]
        [Delayed]
        private List<LoopScrollRectItem> loopScrollRectItems = new();

        public List<LoopScrollRectItem> LoopScrollRectItems
        {
            get => this.loopScrollRectItems;
            set { this.SetField(ref this.loopScrollRectItems, value); }
        }

        private void OnLoopScrollRectItemsChanged()
        {
            this.OnDataContextChanged(nameof(this.LoopScrollRectItems));
        }


        [SerializeField]
        [OnValueChanged(nameof(OnStreetChanged))]
        [Delayed]
        private string street;

        public string Street
        {
            get => this.street;
            set => this.SetField(ref this.street, value);
        }

        private void OnStreetChanged()
        {
            this.OnDataContextChanged(nameof(this.Street));
        }

        [HideReferenceObjectPicker]
        [SerializeField]
        [OnValueChanged(nameof(OnRivalNamesChanged))]
        [Delayed]
        private List<TMP_Dropdown.OptionData> rivalNames = new();

        public List<TMP_Dropdown.OptionData> RivalNames
        {
            get => this.rivalNames;
            set => this.SetField(ref this.rivalNames, value);
        }

        private void OnRivalNamesChanged()
        {
            this.OnDataContextChanged(nameof(this.RivalNames));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnToggleGroupOptionsChanged))]
        [Delayed]
        private List<string> toggleGroupOptions = new();

        public List<string> ToggleGroupOptions
        {
            get => this.toggleGroupOptions;
            set { this.SetField(ref this.toggleGroupOptions, value); }
        }

        private void OnToggleGroupOptionsChanged()
        {
            this.OnDataContextChanged(nameof(this.ToggleGroupOptions));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnToggleGroupValueChanged))]
        [Delayed]
        private int toggleGroupValue = -1;

        public int ToggleGroupValue
        {
            get => this.toggleGroupValue;
            set { this.SetField(ref this.toggleGroupValue, value); }
        }

        private void OnToggleGroupValueChanged()
        {
            this.OnDataContextChanged(nameof(this.ToggleGroupValue));
        }


        [SerializeField]
        [OnValueChanged(nameof(OnTestSchoolChanged))]
        [Delayed]
        private EditorTestSchool school;

        public EditorTestSchool School
        {
            get => this.school;
            set { this.SetField(ref this.school, value); }
        }

        private void OnTestSchoolChanged()
        {
            this.OnDataContextChanged(nameof(this.School));
        }
    }

    public sealed partial class EditorTestSchool : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnCityChanged))]
        [Delayed]
        private string city;

        public string City
        {
            get => this.city;
            set { this.SetField(ref this.city, value); }
        }

        private void OnCityChanged()
        {
            this.OnDataContextChanged(nameof(this.City));
        }


        [SerializeField]
        [OnValueChanged(nameof(OnEnableGraphicChanged))]
        [Delayed]
        private bool enableGraphic;

        public bool EnableGraphic
        {
            get => this.enableGraphic;
            set { this.SetField(ref this.enableGraphic, value); }
        }

        private void OnEnableGraphicChanged()
        {
            this.OnDataContextChanged(nameof(this.EnableGraphic));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnStreetChanged))]
        [Delayed]
        private string street;

        public EditorTestSchool(string baseDataContextPath = null) : base(baseDataContextPath)
        {
        }

        public string Street
        {
            get => this.street;
            set => this.SetField(ref this.street, value);
        }

        private void OnStreetChanged()
        {
            this.OnDataContextChanged(nameof(this.Street));
        }
    }

    public sealed class EditorTestDataContext : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnAnchoredPositionChanged))]
        [Delayed]
        private List<float> anchoredPosition = new();

        public List<float> AnchoredPosition
        {
            get => this.anchoredPosition;
            set { this.SetField(ref this.anchoredPosition, value); }
        }

        private void OnAnchoredPositionChanged()
        {
            this.OnDataContextChanged(nameof(this.AnchoredPosition));
        }


        [SerializeField]
        [OnValueChanged(nameof(OnNameChanged))]
        [Delayed]
        private string name;

        public string Name
        {
            get => this.name;
            set { this.SetField(ref this.name, value); }
        }

        private void OnNameChanged()
        {
            this.OnDataContextChanged(nameof(this.Name));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnAgeChanged))]
        [Delayed]
        private int age;

        public int Age
        {
            get => this.age;
            set { this.SetField(ref this.age, value); }
        }

        private void OnAgeChanged()
        {
            this.OnDataContextChanged(nameof(this.Age));
        }
        
        [SerializeField]
        [OnValueChanged(nameof(OnLongValueChanged))]
        [Delayed]
        private long longValue;

        public long LongValue
        {
            get => this.longValue;
            set { this.SetField(ref this.longValue, value); }
        }

        private void OnLongValueChanged()
        {
            this.OnDataContextChanged(nameof(this.LongValue));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnAddressChanged))]
        [Delayed]
        private EditorTestAddress address = new EditorTestAddress(nameof(Address));

        public EditorTestAddress Address
        {
            get => this.address;
            set { this.SetField(ref this.address, value); }
        }

        private void OnAddressChanged()
        {
            this.OnDataContextChanged(nameof(this.Address));
        }
    }

    public sealed class TestCardDataContext : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnBackgroundChanged))]
        [Delayed]
        private string background;

        public string Background
        {
            get => this.background;
            set { this.SetField(ref this.background, value); }
        }

        private void OnBackgroundChanged()
        {
            this.OnDataContextChanged(nameof(this.Background));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnAvatarChanged))]
        [Delayed]
        private string avatar;

        public string Avatar
        {
            get => this.avatar;
            set { this.SetField(ref this.avatar, value); }
        }

        private void OnAvatarChanged()
        {
            this.OnDataContextChanged(nameof(this.Avatar));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnRuneChanged))]
        [Delayed]
        private string rune;

        public string Rune
        {
            get => this.rune;
            set { this.SetField(ref this.rune, value); }
        }

        private void OnRuneChanged()
        {
            this.OnDataContextChanged(nameof(this.Rune));
        }

        [SerializeField]
        [OnValueChanged(nameof(OnCostChanged))]
        [Delayed]
        private int cost;

        public int Cost
        {
            get => this.cost;
            set { this.SetField(ref this.cost, value); }
        }

        private void OnCostChanged()
        {
            this.OnDataContextChanged(nameof(this.Cost));
        }
    }

    public sealed class TestLoopScrollRectDataSource : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnCardRepositoryChanged))]
        [Delayed]
        private List<LoopScrollRectItem> cardRepository = new();

        public List<LoopScrollRectItem> CardRepository
        {
            get => this.cardRepository;
            set { this.SetField(ref this.cardRepository, value); }
        }

        private void OnCardRepositoryChanged()
        {
            this.OnDataContextChanged(nameof(this.CardRepository));
        }
    }
}