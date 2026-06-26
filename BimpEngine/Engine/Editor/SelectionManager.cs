using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace BimpEngine.Engine.Editor
{
    public class SelectionManager
    {
        public Objetos SelectedObject { get; private set; }

        public int SelectedIndex { get; private set; } = -1;

        public event Action<Objetos> OnSelectionChanged;

        public void Select(Objetos obj, int index)
        {
            if (SelectedObject == obj)
                return;

            var previous = SelectedObject;


            ClearSelectionVisual();

            SelectedObject = obj;
            SelectedIndex = index;

            ApplySelectionVisual();

            OnSelectionChanged?.Invoke(SelectedObject);
        }

        public void Clear()
        {
            ClearSelectionVisual();

            SelectedObject = null;
            SelectedIndex = -1;

            OnSelectionChanged?.Invoke(null);
        }

        private void ApplySelectionVisual()
        {
            if (SelectedObject == null)
                return;

            SelectedObject.IsSelected = true;
        }

        private void ClearSelectionVisual()
        {
            if (SelectedObject == null)
                return;

            SelectedObject.IsSelected = false;
        }
    }
}
