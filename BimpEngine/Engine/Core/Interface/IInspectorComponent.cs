using BimpEngine.Engine.World;

namespace BimpEngine.Engine.Core.Interface
{
    public interface IInspectorComponent
    {
        void SetObject(Objetos obj);
        void Refresh(Objetos obj);
    }
}
