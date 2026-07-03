using BimpEngine.Engine.Editor;
using BimpEngine.Engine.Editor.Menus;
using BimpEngine.Engine.Entities;
using BimpEngine.Engine.World;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BimpEngine.Controls.Herencia
{
    public partial class HerenciaControl : UserControl
    {
        public event Action<Objetos, int> OnObjectSelected;
        public event Action<Objetos, Objetos> OnObjectReparented;

        public event Action<Objetos> OnObjectDeleted;
        public event Action<Objetos> OnObjectDuplicated;

        public event Action<Objetos> OnCrearMolde;
        public event Action<Objetos> OnAplicarCambiosAlMolde;

        private ContextMenuStrip contextMenu;
        private Objetos _objetoContextMenu;

        private TreeNode _editorCameraNode;
        private EditorCamera _editorCamera;

        private readonly HerenciaContextMenu _menu = new();

        public HerenciaControl()
        {
            InitializeComponent();

            ConfigurarImageList();

            ListHerencia.AfterSelect += (s, e) =>
            {
                if (e.Node?.Tag is Objetos obj)
                    OnObjectSelected?.Invoke(obj, e.Node.Index);
            };

            ListHerencia.AfterLabelEdit += (s, e) =>
            {
                if (e.Label == null || string.IsNullOrWhiteSpace(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }

                if (e.Node?.Tag is Objetos obj)
                {
                    obj.Name = e.Label;
                    OnObjectSelected?.Invoke(obj, e.Node.Index);
                }
            };

            _menu.Duplicar += DuplicarObjeto;
            _menu.Renombrar += RenombrarObjeto;
            _menu.Eliminar += EliminarObjeto;
            _menu.Subir += MoverArriba;
            _menu.Bajar += MoverAbajo;

            _menu.CrearMolde += () =>
            {
                if (_objetoContextMenu != null)
                    OnCrearMolde?.Invoke(_objetoContextMenu);
            };

            _menu.AplicarMolde += () =>
            {
                if (_objetoContextMenu != null)
                    OnAplicarCambiosAlMolde?.Invoke(_objetoContextMenu);
            };

            ListHerencia.NodeMouseClick += (s, e) =>
            {
                if (e.Button != MouseButtons.Right)
                    return;

                ListHerencia.SelectedNode = e.Node;
                _objetoContextMenu = e.Node?.Tag as Objetos;

                _menu.MostrarAplicarMolde(
                    _objetoContextMenu?.Molde?.EsInstanciaDeMolde == true);

                _menu.Show(ListHerencia, e.Location);
            };
        }

        public void Clear()
        {
            ListHerencia.Nodes.Clear();
        }

        public void AddObject(Objetos obj, Objetos parent = null, bool includeChildren = false)
        {
            int iconIndex = obj switch
            {
                CameraObject => 1,
                _ => 0
            };

            var node = new TreeNode(obj.Name)
            {
                Tag = obj,
                ImageIndex = iconIndex,
                SelectedImageIndex = iconIndex
            };

            if (parent != null)
            {
                var parentNode = FindNode(ListHerencia.Nodes, parent);
                if (parentNode != null)
                {
                    parentNode.Nodes.Add(node);
                    parentNode.Expand();
                    if (includeChildren)
                        foreach (var child in obj.Children)
                            AddObject(child, obj, includeChildren: true);
                    return;
                }
            }

            ListHerencia.Nodes.Add(node);
            if (includeChildren)
                foreach (var child in obj.Children)
                    AddObject(child, obj, includeChildren: true);
        }

        public void SelectObject(Objetos obj)
        {
            var node = FindNode(ListHerencia.Nodes, obj);
            if (node != null) ListHerencia.SelectedNode = node;
        }

        public void RemoveObject(Objetos obj)
        {
            var node = FindNode(ListHerencia.Nodes, obj);
            node?.Remove();
        }

        public void RefreshObject(Objetos obj)
        {
            foreach (TreeNode node in ListHerencia.Nodes)
            {
                if (node.Tag == obj)
                {
                    node.Text = obj.Name;
                    node.ForeColor = obj.Molde?.EsInstanciaDeMolde == true
                        ? Color.FromArgb(110, 170, 255)
                        : ListHerencia.ForeColor;
                    break;
                }
            }
        }

        private TreeNode FindNode(TreeNodeCollection nodes, Objetos obj)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == obj) return node;
                var found = FindNode(node.Nodes, obj);
                if (found != null) return found;
            }
            return null;
        }

        private void ListHerencia_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node)
                ListHerencia.DoDragDrop(node, DragDropEffects.Move);
        }

        private void ListHerencia_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ListHerencia_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var pos = ListHerencia.PointToClient(new Point(e.X, e.Y));
            var targetNode = ListHerencia.GetNodeAt(pos);

            if (targetNode != null)
                ListHerencia.SelectedNode = targetNode;

            e.Effect = DragDropEffects.Move;
        }

        private void ListHerencia_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode))) return;

            var dragNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            var pos = ListHerencia.PointToClient(new Point(e.X, e.Y));
            var targetNode = ListHerencia.GetNodeAt(pos);

            // No soltar sobre sí mismo ni sobre un hijo propio
            if (targetNode == null || targetNode == dragNode || IsDescendant(dragNode, targetNode))
            {
                // Soltar en raíz si se suelta fuera de cualquier nodo
                if (targetNode == null)
                    MoverARaiz(dragNode);
                return;
            }

            MoverNodo(dragNode, targetNode);
        }

        private void MoverNodo(TreeNode dragNode, TreeNode targetNode)
        {
            var dragObj = dragNode.Tag as Objetos;
            var targetObj = targetNode.Tag as Objetos;

            if (dragObj == null || targetObj == null) return;

            // Quitar del padre actual en el TreeView
            dragNode.Parent?.Nodes.Remove(dragNode);
            ListHerencia.Nodes.Remove(dragNode);

            // Agregar como hijo del nodo destino
            targetNode.Nodes.Add(dragNode);
            targetNode.Expand();

            ListHerencia.SelectedNode = dragNode;

            // Notificar al engine
            OnObjectReparented?.Invoke(dragObj, targetObj);
        }

        private void MoverARaiz(TreeNode dragNode)
        {
            var dragObj = dragNode.Tag as Objetos;
            if (dragObj == null) return;

            dragNode.Parent?.Nodes.Remove(dragNode);
            ListHerencia.Nodes.Remove(dragNode);
            ListHerencia.Nodes.Add(dragNode);

            ListHerencia.SelectedNode = dragNode;

            // Notificar que ahora es raíz (padre null)
            OnObjectReparented?.Invoke(dragObj, null);
        }

        private bool IsDescendant(TreeNode parent, TreeNode child)
        {
            var node = child.Parent;
            while (node != null)
            {
                if (node == parent) return true;
                node = node.Parent;
            }
            return false;
        }


        ///////////////////////////////////////////////////////////////////////
        // Context menu actions
        private void DuplicarObjeto()
        {
            if (_objetoContextMenu == null) return;
            OnObjectDuplicated?.Invoke(_objetoContextMenu);
        }

        private void EliminarObjeto()
        {
            if (_objetoContextMenu == null) return;

            var confirm = MessageBox.Show(
                $"¿Eliminar '{_objetoContextMenu.Name}'?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            RemoveObject(_objetoContextMenu);
            OnObjectDeleted?.Invoke(_objetoContextMenu);
            _objetoContextMenu = null;
        }

        private void RenombrarObjeto()
        {
            if (ListHerencia.SelectedNode == null) return;
            ListHerencia.SelectedNode.BeginEdit();
        }

        private void MoverArriba()
        {
            var node = ListHerencia.SelectedNode;
            if (node == null) return;

            var col = node.Parent?.Nodes ?? ListHerencia.Nodes;
            int idx = col.IndexOf(node);
            if (idx <= 0) return;

            col.RemoveAt(idx);
            col.Insert(idx - 1, node);
            ListHerencia.SelectedNode = node;
        }

        private void MoverAbajo()
        {
            var node = ListHerencia.SelectedNode;
            if (node == null) return;

            var col = node.Parent?.Nodes ?? ListHerencia.Nodes;
            int idx = col.IndexOf(node);
            if (idx >= col.Count - 1) return;

            col.RemoveAt(idx);
            col.Insert(idx + 1, node);
            ListHerencia.SelectedNode = node;
        }

        private Bitmap DibujarIcono(Color color, string texto)
        {
            var bmp = new Bitmap(16, 16);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.FillEllipse(new SolidBrush(color), 2, 2, 12, 12);
            return bmp;
        }

        private void ConfigurarImageList()
        {
            var il = new ImageList { ImageSize = new Size(16, 16) };

            // 0 = objeto genérico (cubo gris)
            il.Images.Add(DibujarIcono(Color.FromArgb(160, 160, 160), "□"));
            // 1 = cámara (amarillo)
            il.Images.Add(DibujarIcono(Color.FromArgb(255, 220, 60), "📷"));
            // 2 = luz (naranja)
            il.Images.Add(DibujarIcono(Color.FromArgb(255, 160, 40), "☀"));
            // 3 = editor camera (azul claro)
            il.Images.Add(DibujarIcono(Color.FromArgb(100, 200, 255), "🎥"));

            ListHerencia.ImageList = il;
        }

        public void SetEditorCamera(EditorCamera camera)
        {
            _editorCamera = camera;

            _editorCameraNode = new TreeNode(FormatEditorCameraLabel())
            {
                ImageIndex = 3,
                SelectedImageIndex = 3,
                ForeColor = Color.FromArgb(100, 200, 255),
                Tag = null // no es un Objetos
            };

            ListHerencia.Nodes.Insert(0, _editorCameraNode);

        }

        public void RefreshEditorCamera()
        {
            if (_editorCameraNode == null || _editorCamera == null) return;
            _editorCameraNode.Text = FormatEditorCameraLabel();
        }

        private string FormatEditorCameraLabel() =>
           _editorCamera == null
               ? "EditorCamera"
               : $"EditorCamera   " +
                 $"X:{_editorCamera.EyeX:F1}  " +
                 $"Y:{_editorCamera.EyeY:F1}  " +
                 $"Z:{_editorCamera.EyeZ:F1}  " +
                 $"Yaw:{_editorCamera.Yaw:F0}°  " +
                 $"Pitch:{_editorCamera.Pitch:F0}°";
    }
}
