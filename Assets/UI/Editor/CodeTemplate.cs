using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityEditor
{
    [Serializable]
    public sealed class TemplateKeyword
    {
        [LabelText("名称")]
        [ReadOnly]
        public string Name;

        [LabelText("值")]
        public string Value;

        public string Keyword => $"${this.Name}$";
    }

    [Serializable]
    public sealed class CodeTemplate
    {
        [LabelText("模板路径")]
        [ReadOnly]
        public string Path;

        [PropertySpace]
        [LabelText("关键词")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false)]
        public List<TemplateKeyword> Keywords = new();

        [PropertySpace]
        [FoldoutGroup("模板内容", false)]
        [HideLabel]
        [TextArea(32, 32)]
        [OnValueChanged(nameof(UpdateKeywords))]
        [PropertyOrder(99)]
        public string Template;

        public void UpdateKeywords()
        {
            this.Keywords.Clear();

            MatchCollection matches = Regex.Matches(this.Template, @"(?<=\$).*?(?=\$)");
            foreach (Match match in matches)
            {
                if (this.Keywords.FindIndex(keyword => keyword.Name == match.Value) > -1)
                {
                    continue;
                }

                this.Keywords.Add(new TemplateKeyword()
                {
                    Name = match.Value,
                });
            }
        }

        [Button("生成至剪切板", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        [PropertyOrder(-1)]
        private void Build()
        {
            StringBuilder builder = new();
            this.Build(this.Keywords.Select(keyword => keyword.Name).ToDictionary(item => item), builder);
            EditorGUIUtility.systemCopyBuffer = builder.ToString();

            Debug.Log("模板已生成至剪切板");
        }

        public void Build(IReadOnlyDictionary<string, string> data, StringBuilder builder)
        {
            List<string> keys = this.Keywords.Select(keyword => keyword.Name).ToList();
            foreach (string key in keys)
            {
                if (!data.ContainsKey(key))
                {
                    Debug.LogError($"变量 {key} 不存在对应值");
                    return;
                }
            }

            builder.Clear();

            string content = this.Template;
            foreach (string key in keys)
            {
                content = content.Replace($"${key}$", data[key]);
            }

            builder.Append(content);
        }
    }
}