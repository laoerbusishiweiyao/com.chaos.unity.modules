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
        [LabelText("Toggle模板")]
        public GameObject Template;

        [LabelText("目标")]
        [ReadOnly]
        public ToggleGroup Target;

        private bool isChangingIndex;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                this.Clear();
                return;
            }

            if (this.binders.Count != 2)
            {
                this.Clear();
                return;
            }

            if (this.isChangingIndex)
            {
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
            foreach (DataBinder binder in this.binders.Values)
            {
                if (binder.DataType == typeof(int))
                {
                    this.isChangingIndex = true;
                    this.dataSource.DataContext.SetValue<int>(binder.Source, index);
                    this.isChangingIndex = false;
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
            var toggles = this.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                this.Target.UnregisterToggle(toggle);
                DestroyImmediate(toggle.gameObject);
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