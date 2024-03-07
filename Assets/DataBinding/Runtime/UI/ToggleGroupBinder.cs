using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ToggleGroup))]
    [AddComponentMenu("DataBinding/ToggleGroupBinder")]
    public sealed class ToggleGroupBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
            typeof(List<string>),
        };

        [LabelText("Toggle模板")]
        public GameObject Template;

        [LabelText("目标")]
        [ReadOnly]
        public ToggleGroup Target;

        private int value;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Clear();
                return;
            }

            if (this.binders.Count != 2)
            {
                this.Clear();
                return;
            }

            this.Clear();
            this.AddToggles(this.GetOptions());

            var toggles = this.GetComponentsInChildren<Toggle>();
            if (toggles.Length < 1)
            {
                return;
            }

            int index = Math.Clamp(GetIndex(), 0, toggles.Length - 1);
            if (this.value == index)
            {
                return;
            }

            toggles[index].isOn = true;
        }

        private int GetIndex()
        {
            foreach (DataBinder binder in this.binders.Values)
            {
                if (binder.DataType == typeof(int))
                {
                    return this.dataSource.DataContext.GetValue<int>(binder.Source);
                }
            }

            return -1;
        }

        private void SetIndex()
        {
            var toggles = new List<Toggle>(this.GetComponentsInChildren<Toggle>());
            var index = toggles.IndexOf(this.Target.GetFirstActiveToggle());
            if (this.value == index)
            {
                return;
            }

            foreach (DataBinder binder in this.binders.Values)
            {
                if (binder.DataType == typeof(int))
                {
                    this.value = index;
                    this.dataSource.DataContext.SetValue(binder.Source, index);
                }
            }
        }

        private List<string> GetOptions()
        {
            foreach (DataBinder binder in this.binders.Values)
            {
                if (binder.DataType == typeof(List<string>))
                {
                    return this.dataSource.DataContext.GetValue<List<string>>(binder.Source);
                }
            }

            return new List<string>();
        }

        private void Clear()
        {
            if (!this)
            {
                return;
            }

            var toggles = this.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                this.Target.UnregisterToggle(toggle);
            }

            if (Application.isPlaying)
            {
                var length = this.transform.childCount;
                for (int i = 1; i < length; i++)
                {
                    DestroyImmediate(this.transform.GetChild(1).gameObject);
                }
            }
            else
            {
                foreach (Toggle toggle in toggles)
                {
                    DestroyImmediate(toggle.gameObject);
                }
            }
        }

        private void AddToggles(List<string> options)
        {
            if (this.Template is null)
            {
                return;
            }

            foreach (string option in options)
            {
                var toggleGameObject = Instantiate(this.Template, this.transform);
                toggleGameObject.SetActive(true);
                toggleGameObject.name = option;
                Toggle toggle = toggleGameObject.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(this.OnValueChanged);
                this.Target.RegisterToggle(toggle);
                toggleGameObject.GetComponentInChildren<TextMeshProUGUI>().text = option;
            }

            var toggles = new List<Toggle>(this.GetComponentsInChildren<Toggle>());
            if (toggles.Count < 1)
            {
                return;
            }

            this.value = Math.Clamp(this.value, 0, toggles.Count - 1);
            toggles[this.value].isOn = true;
        }

        private void OnValueChanged(bool value)
        {
            if (value)
            {
                this.SetIndex();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<ToggleGroup>();
        }
    }
}