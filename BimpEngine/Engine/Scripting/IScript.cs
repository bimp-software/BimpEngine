using BimpEngine.Engine.Core;
using BimpEngine.Engine.Input;
using BimpEngine.Engine.World;
using System.Collections.Generic;

namespace BimpEngine.Engine.Scripting
{
    public abstract class IScript
    {
        public Objetos? gameObject { get; internal set; }
        public Transform? transform => gameObject?.Transform;
        protected ScriptContext? Context { get; private set; }

        internal void SetContext(ScriptContext context) => Context = context;

        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
        public virtual void OnDestroy() { }

        protected void Log(string mensaje) => Debug.Console.Log(mensaje, gameObject?.Name ?? "Script");
        protected void LogWarning(string mensaje) => Debug.Console.LogWarning(mensaje, gameObject?.Name ?? "Script");
        protected void LogError(string mensaje) => Debug.Console.LogError(mensaje, gameObject?.Name ?? "Script");

        protected bool GetKey(string accion) => InputManager.GetAction(accion);
        protected float GetAxis(string nombre) => InputManager.GetAxis(nombre);
        protected float GetAxisRaw(string nombre) => InputManager.GetAxisRaw(nombre);
        protected Objetos? Find(string nombre) => Context?.Scene?.FindByName(nombre);
        protected List<Objetos> FindByTag(string tag) => Context?.Scene?.FindByTag(tag) ?? new List<Objetos>();
    }
}