using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameSystem.AI
{
    public class ContextClickOverrideAttribute : Attribute
    {
        public string path;

        public ContextClickOverrideAttribute(string path)
        {
            this.path = path;
        }
    }
}