using System;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    public sealed partial class TestAddress : DataContext
    {
        [SerializeField]
        [OnValueChanged(nameof(OnCityChanged))]
        [Delayed]
        private string city;

        public TestAddress()
        {
            this.school = new TestSchool(nameof(School));
        }

        public TestAddress(string baseDataContextPath = null) : base(baseDataContextPath)
        {
            this.school = new TestSchool(nameof(School));
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

        [SerializeField]
        [OnValueChanged(nameof(OnTestSchoolChanged))]
        [Delayed]
        private TestSchool school;

        public TestSchool School
        {
            get => this.school;
            set { this.SetField(ref this.school, value); }
        }

        private void OnTestSchoolChanged()
        {
            this.OnDataContextChanged(nameof(this.School));
        }
    }

    public sealed partial class TestSchool : DataContext
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
        [OnValueChanged(nameof(OnStreetChanged))]
        [Delayed]
        private string street;

        public TestSchool(string baseDataContextPath = null) : base(baseDataContextPath)
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

    public sealed class ArenaDataContext : DataContext
    {
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
        [OnValueChanged(nameof(OnAddressChanged))]
        [Delayed]
        private TestAddress address = new TestAddress(nameof(Address));

        public TestAddress Address
        {
            get => this.address;
            set { this.SetField(ref this.address, value); }
        }

        private void OnAddressChanged()
        {
            this.OnDataContextChanged(nameof(this.Address));
        }
    }
}