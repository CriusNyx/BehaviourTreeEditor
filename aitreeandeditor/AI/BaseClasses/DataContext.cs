using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// A wrapper class for the AI's variables.
    /// </summary>
    public class DataContext
    {
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
                if(key.StartsWith("\"") && key.EndsWith("\"") && key.Length >= 2)
                    return key.Substring(1, key.Length - 2);
                else
                    return data[key];
            }
            set
            {
                data[key] = value;
            }
        }
    }
}