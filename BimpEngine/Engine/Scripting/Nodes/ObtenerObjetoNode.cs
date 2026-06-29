using BimpEngine.Engine.Scripting;
using BimpEngine.Engine.World;

namespace BimpEngine.Engine.Scripting.Nodes
{
    public class ObtenerObjetoNode : ScriptNode
    {
        private readonly NodePort _nombre;
        private readonly NodePort _objeto;

        public ObtenerObjetoNode()
        {
            Title = "Obtener Objeto";
            Category = NodeCategory.Variable;
            _nombre = AddInput("Nombre", PortType.String, "Cube");
            _objeto = AddOutput("Objeto", PortType.Object);
        }

        public override NodePort? Execute(ScriptContext ctx)
        {
            string nombre = _nombre.Value?.ToString() ?? "";
            // Buscar en la escena actual
            var obj = ctx.Scene?.Objetos.FirstOrDefault(o => o.Name == nombre);
            _objeto.RuntimeValue = obj;
            return null;
        }
    }
}
