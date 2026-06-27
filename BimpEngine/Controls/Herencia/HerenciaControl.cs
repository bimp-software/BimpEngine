using BimpEngine.Engine.World;

namespace BimpEngine.Controls.Herencia
{
    public partial class HerenciaControl : UserControl
    {
        public event Action<Objetos, int> OnObjectSelected;

        public HerenciaControl()
        {
            InitializeComponent();

            ListHerencia.AfterSelect += (s, e) =>
            {
                if(e.Node?.Tag is Objetos obj)
                    OnObjectSelected?.Invoke(obj, e.Node.Index);
            };
        }

        public void AddObject(Objetos obj)
        {
            var node = new TreeNode(obj.Name)
            {
                Tag = obj
            };

            ListHerencia.Nodes.Add(node);
        }

        public void SelectObject(Objetos obj)
        {
            foreach (TreeNode node in ListHerencia.Nodes)
            {
                if (node.Tag == obj)
                {
                    ListHerencia.SelectedNode = node;
                    break;
                }
            }
        }

        public void RemoveObject(Objetos obj)
        {
            foreach (TreeNode node in ListHerencia.Nodes)
            {
                if (node.Tag == obj)
                {
                    ListHerencia.Nodes.Remove(node);
                    break;
                }
            }
        }

        public void RefreshObject(Objetos obj)
        {
            foreach (TreeNode node in ListHerencia.Nodes)
            {
                if (node.Tag == obj)
                {
                    node.Text = obj.Name;
                    break;
                }
            }
        }
    }
}