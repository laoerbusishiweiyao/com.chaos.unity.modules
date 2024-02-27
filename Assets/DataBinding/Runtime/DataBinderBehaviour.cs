using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace UnityEngine
{
    /// <summary>
    /// 数据绑定器基类
    /// </summary>
    [ExecuteAlways]
    public abstract partial class DataBinderBehaviour : SerializedMonoBehaviour
    {
        [BoxGroup("数据源", centerLabel: true)]
        [ShowInInspector]
        [HideLabel]
        [Required("必须选择数据源")]
        [ReadOnly]
        [NonSerialized]
        [PropertyOrder(-99)]
        protected DataSource dataSource;

        [PropertySpace]
        [OdinSerialize]
        [LabelText("绑定数据")]
        [DictionaryDrawerSettings(KeyLabel = "数据路径", ValueLabel = "数据内容", IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        protected Dictionary<string, DataBinder> binders = new();

        public IReadOnlyDictionary<string, DataBinder> Binders => this.binders;

        protected virtual void Awake()
        {
            this.Initialize();
            this.Unbind();
            this.Bind();
            this.Refresh();
        }

        public virtual void Initialize()
        {
            this.QueryDataSourceBehaviour();
        }

        protected virtual void QueryDataSourceBehaviour()
        {
            this.dataSource ??= this.GetComponentInParent<DataSource>();
        }
        
        protected DataBinder FirstDataBinder()
        {
            foreach (DataBinder binder in this.binders.Values)
            {
                return binder;
            }

            return default;
        }

        protected virtual void OnDestroy()
        {
            this.Unbind();
        }

        public void Bind()
        {
            if (this.dataSource is null)
            {
                return;
            }

            foreach (DataBinder binder in this.binders.Values)
            {
                this.Bind(binder);
            }
        }

        public void Unbind()
        {
            if (this.dataSource is null)
            {
                return;
            }

            foreach (DataBinder binder in this.binders.Values)
            {
                this.Unbind(binder);
            }
        }

        public void Bind(DataBinder binder)
        {
            this.dataSource.AddDataBinderBehaviour(binder.Source, this);
        }

        public void Unbind(DataBinder binder)
        {
            this.dataSource.RemoveDataBinderBehaviour(binder.Source, this);
        }

        public abstract void Refresh();
    }
}