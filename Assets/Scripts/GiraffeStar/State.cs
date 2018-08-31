using System.Collections.Generic;

namespace GiraffeStar
{
    public abstract class State
    {
        List<Module> cachedModules;

		public virtual void OnEnter(object msg) { }
		public virtual void OnState(object msg) { }
		public virtual void OnExit(object msg) { }

        protected void RegisterAndHold(Module module)
        {
            if(cachedModules == null)
            {
                cachedModules = new List<Module>();
            }

            GiraffeSystem.Register(module);
            cachedModules.Add(module);
        }

        protected void UnRegisterAll()
        {
            foreach (var module in cachedModules)
            {
                GiraffeSystem.UnRegister(module);
            }

            cachedModules.Clear();
        }
    }
}


