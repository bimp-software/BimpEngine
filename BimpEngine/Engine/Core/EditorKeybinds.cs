using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Core
{
    public class EditorKeybinds
    {
        public Keys Guardar { get; set; } = Keys.Control | Keys.S;
        public Keys NuevaEscena { get; set; } = Keys.Control | Keys.N;
        public Keys AbrirEscena { get; set; } = Keys.Control | Keys.O;
        public Keys Deshacer { get; set; } = Keys.Control | Keys.Z;
        public Keys Rehacer { get; set; } = Keys.Control | Keys.Y;
        public Keys Duplicar { get; set; } = Keys.Control | Keys.D;
        public Keys Eliminar { get; set; } = Keys.Delete;
        public Keys ModoTraslacion { get; set; } = Keys.W;
        public Keys ModoRotacion { get; set; } = Keys.E;
        public Keys ModoEscala { get; set; } = Keys.R;
        public Keys FocoObjeto { get; set; } = Keys.F;
    }
}
