using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestOnceAttribute : Attribute
    {
        
    }

}