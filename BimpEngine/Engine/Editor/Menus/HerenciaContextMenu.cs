using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Menus
{
    public class HerenciaContextMenu : ContextMenuStrip
    {
        public event Action Duplicar;
        public event Action Renombrar;
        public event Action Eliminar;
        public event Action Subir;
        public event Action Bajar;
        public event Action CrearMolde;
        public event Action AplicarMolde;

        private ToolStripMenuItem itemAplicarMolde;

        public HerenciaContextMenu()
        {
            BackColor = Color.FromArgb(45, 45, 45);
            ForeColor = Color.White;
            RenderMode = ToolStripRenderMode.System;

            CrearItems();
        }

        private void CrearItems()
        {
            var itemDuplicar = CrearItem("Duplicar", () => Duplicar?.Invoke());
            var itemRenombrar = CrearItem("Renombrar", () => Renombrar?.Invoke());
            var itemEliminar = CrearItem("Eliminar", () => Eliminar?.Invoke());

            var itemSubir = CrearItem("Subir", () => Subir?.Invoke());
            var itemBajar = CrearItem("Bajar", () => Bajar?.Invoke());

            var itemCrearMolde = CrearItem("Crear Molde", () => CrearMolde?.Invoke());

            itemAplicarMolde = CrearItem("Aplicar cambios al Molde",
                () => AplicarMolde?.Invoke());

            Items.Add(itemDuplicar);
            Items.Add(itemRenombrar);
            Items.Add(new ToolStripSeparator());

            Items.Add(itemSubir);
            Items.Add(itemBajar);

            Items.Add(new ToolStripSeparator());

            Items.Add(itemCrearMolde);
            Items.Add(itemAplicarMolde);

            Items.Add(new ToolStripSeparator());

            Items.Add(itemEliminar);
        }

        private ToolStripMenuItem CrearItem(string texto, Action accion)
        {
            var item = new ToolStripMenuItem(texto);
            item.Click += (s, e) => accion();
            return item;
        }

        public void MostrarAplicarMolde(bool visible)
        {
            itemAplicarMolde.Visible = visible;
        }
    }
}
