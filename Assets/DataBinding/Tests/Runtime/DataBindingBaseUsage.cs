using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine
{
    public sealed class DataBindingBaseUsage : MonoBehaviour
    {
        private void Awake()
        {
            Dictionary<string, Type> types = new Assembly[]
                {
                    typeof(DataBindingBaseUsage).Assembly
                }
                .SelectMany(assembly => assembly.GetTypes())
                .ToDictionary(type => type.FullName);
            DataContextOptions.Default.LoadAllDataContextType(types);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var dataSource = GameObject.FindObjectOfType<DataSource>();
                dataSource.Initialize(new ArenaDataContext());
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                var dataSource = GameObject.FindObjectOfType<DataSource>();
                string value = Time.realtimeSinceStartup.ToString("F2");
                (dataSource.DataContext as ArenaDataContext).Address.School.Street = value;
            }
        }
    }
}