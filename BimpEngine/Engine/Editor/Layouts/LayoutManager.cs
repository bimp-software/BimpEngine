using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Layouts
{
    public class LayoutManager
    {
        private readonly Panel _container;

        public LayoutManager(Panel container)
        {
            _container = container;
        }

        public void SetLayout(
            IEditorLayout layout,
            Dictionary<string, EditorView> views)
        {
            _container.Controls.Clear();

            layout.Build(_container,views);
        }
    }
}
