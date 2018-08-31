using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GiraffeStar
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SubscriberAttribute : Attribute
    {
        public string service;

        public SubscriberAttribute(string service)
        {
            this.service = service;
        }

        public SubscriberAttribute() : this("Default")
        {
            
        }
    }
}


