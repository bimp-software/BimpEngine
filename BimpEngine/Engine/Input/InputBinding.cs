using System.Windows.Forms;

namespace BimpEngine.Engine.Input
{
    public class InputBinding
    {
        public string Id { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public string Tecla { get; set; }

        public InputBinding()
        {
            Id = "";
            Categoria = "";
            Descripcion = "";
            Tecla = "";
        }

        public InputBinding(string id, string categoria, string descripcion, string tecla)
        {
            Id = id;
            Categoria = categoria;
            Descripcion = descripcion;
            Tecla = tecla;
        }

        public Keys GetKey()
        {
            if (System.Enum.TryParse(Tecla, out Keys key))
                return key;

            return Keys.None;
        }
    }
}