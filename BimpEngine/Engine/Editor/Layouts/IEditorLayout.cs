using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor.Layouts
{
    public interface IEditorLayout
    {
        string Name { get; }

        void Build(Panel container,Dictionary<string, EditorView> views);
    }
}
