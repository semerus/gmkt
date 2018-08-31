using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GiraffeStar
{
    public class GiraffeSystem
    {
        static Dictionary<Type, Module> RegisteredModules;
        static Dictionary<string, List<MethodInfo>> Subscriptions;

        static bool isInitialized;

        public static void Init()
        {
            if(isInitialized) { return; }

            RegisteredModules = new Dictionary<Type, Module>();
            Subscriptions = new Dictionary<string, List<MethodInfo>>();

            isInitialized = true;
        }

        public static void Register(Module module)
        {
            var type = module.GetType();
            if(RegisteredModules.ContainsKey(type))
            {
                Debug.LogWarning(string.Format("{0} is already registered", module.GetType().ToString()));
                return;
            }

            RegisteredModules.Add(type, module);
            var Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var method in Methods)
            {
                var attributes = method.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    var subscriber = attribute as SubscriberAttribute;
                    if(subscriber != null)
                    {
                        var parameters = method.GetParameters();
                        if(parameters.Length > 1)
                        {
                            Debug.LogWarning(string.Format("{0} of {1} has more than one parameters. Subscription system currently supporting only one parameter",
                                method.Name, type.Name));
                            break;
                        }

                        if(Subscriptions.ContainsKey(subscriber.service))
                        {
                            // add additional subscriber to existing service
                            var existing = Subscriptions[subscriber.service];
                            if(!existing.Contains(method))
                            {
                                existing.Add(method);
                            }
                        }
                        else
                        {
                            // No service, create new service
                            var addition = new List<MethodInfo>();
                            addition.Add(method);
                            Subscriptions.Add(subscriber.service, addition);
                        }
                        break;
                    }
                }
            }

            module.OnRegister();
        }

        public static void UnRegister(Module module)
        {
            var type = module.GetType();
            if (!RegisteredModules.ContainsKey(type))
            {
                Debug.LogWarning(string.Format("{0} is already unregistered", module.GetType().ToString()));
                return;
            }

            module.OnUnRegister();

            // unregister subscriptions
            var Methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var method in Methods)
            {
                var attributes = method.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    var subscriber = attribute as SubscriberAttribute;
                    if (subscriber != null)
                    {
                        if (Subscriptions.ContainsKey(subscriber.service))
                        {
                            var existing = Subscriptions[subscriber.service];
                            existing.Remove(method);
                        }
                        break;
                    }
                }
            }

            // unregister module
            RegisteredModules.Remove(type);
        }

        public static void ProcessMessage(MessageCore msg)
        {
            var service = msg.service;
            var message = msg.GetType();

            var methods = new List<MethodInfo>();
            var reserved = new List<MethodInfo>();
            if (Subscriptions.TryGetValue(service, out methods))
            {
                // if invoked during the enumeration operation, it might corrupt the collection, so invoke after complete search
                foreach (var method in methods)
                {
                    var parameters = method.GetParameters();
                    // currently allowing only one parameter
					// currently checks one more base type
					if(parameters[0].ParameterType == message || parameters[0].ParameterType == message.BaseType)
                    {
                        reserved.Add(method);
                    }
                }

				if (reserved.Count < 0) {
					Debug.LogWarning(string.Format("Subscriber for {0} not found.", message.ToString()));
				}

                foreach (var method in reserved)
                {
                    var module = RegisteredModules[method.DeclaringType];
                    method.Invoke(module, new object[] { msg });
                }
            }
            else
            {
                Debug.LogWarning(string.Format("Subscriber for the service {0} not found.", service));
            }
        }

        public static T FindModule<T>()
            where T:  Module
        {
            var type = typeof(T);
            return RegisteredModules.ContainsKey(type) ? (T)RegisteredModules[type] : null;
        }
    }
}


