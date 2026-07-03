using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Scripting
{
    public abstract class IScript
    {
        public virtual void Start() { }

        public virtual void Update(float deltaTime) { }

        public virtual void OnDestroy() { }
    }
}
