using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public sealed class DataBindingSamples : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScreen()
        {
            Dictionary<string, Type> types = new[]
                {
                    typeof(DataBindingBaseUsage).Assembly
                }
                .SelectMany(assembly => assembly.GetTypes())
                .ToDictionary(type => type.FullName);
            DataContextOptions.Default.LoadAllDataContextType(types);
            DataContext.Build();
        }
    }
}