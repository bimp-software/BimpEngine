using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Math;
using BimpEngine.Engine.Physics;
using BimpEngine.Engine.World;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class BoxColliderControl : UserControl, IInspectorComponent
    {
        private Objetos _objeto;
        private BoxCollider _collider;
        private bool _loading = false;

        private CheckBox chkEnabled = new CheckBox();
        private CheckBox chkTrigger = new CheckBox();

        private NumericUpDown numOffsetX = new NumericUpDown();
        private NumericUpDown numOffsetY = new NumericUpDown();
        private NumericUpDown numOffsetZ = new NumericUpDown();

        private NumericUpDown numSizeX = new NumericUpDown();
        private NumericUpDown numSizeY = new NumericUpDown();
        private NumericUpDown numSizeZ = new NumericUpDown();

        public event Action<Objetos> OnObjectModified;

        public event Action OnChanged;

        public BoxColliderControl()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            Height = 190;
            Dock = DockStyle.Top;
            BackColor = Color.FromArgb(45, 45, 48);

            var title = new Label
            {
                Text = "Box Collider",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(8, 8),
                AutoSize = true
            };

            chkEnabled.Text = "Enabled";
            chkEnabled.ForeColor = Color.White;
            chkEnabled.Location = new Point(8, 35);

            chkTrigger.Text = "Is Trigger";
            chkTrigger.ForeColor = Color.White;
            chkTrigger.Location = new Point(100, 35);

            AddVectorRow("Offset", 65, numOffsetX, numOffsetY, numOffsetZ);
            AddVectorRow("Size", 115, numSizeX, numSizeY, numSizeZ);

            Controls.Add(title);
            Controls.Add(chkEnabled);
            Controls.Add(chkTrigger);

            chkEnabled.CheckedChanged += (s, e) => SaveValues();
            chkTrigger.CheckedChanged += (s, e) => SaveValues();

            foreach (var n in new[] { numOffsetX, numOffsetY, numOffsetZ, numSizeX, numSizeY, numSizeZ })
            {
                n.DecimalPlaces = 2;
                n.Minimum = -1000;
                n.Maximum = 1000;
                n.Increment = 0.1M;
                n.ValueChanged += (s, e) => SaveValues();
            }

            numSizeX.Minimum = 0.1M;
            numSizeY.Minimum = 0.1M;
            numSizeZ.Minimum = 0.1M;
        }

        private void AddVectorRow(string label, int y, NumericUpDown x, NumericUpDown yBox, NumericUpDown z)
        {
            Controls.Add(new Label
            {
                Text = label,
                ForeColor = Color.White,
                Location = new Point(8, y),
                Size = new Size(55, 22)
            });

            x.Location = new Point(70, y);
            yBox.Location = new Point(145, y);
            z.Location = new Point(220, y);

            x.Size = yBox.Size = z.Size = new Size(65, 22);

            Controls.Add(x);
            Controls.Add(yBox);
            Controls.Add(z);
        }

        private void LoadValues()
        {
            if (_collider == null)
                return;

            _loading = true;

            chkEnabled.Checked = _collider.Enabled;
            chkTrigger.Checked = _collider.IsTrigger;

            numOffsetX.Value = (decimal)_collider.Offset.X;
            numOffsetY.Value = (decimal)_collider.Offset.Y;
            numOffsetZ.Value = (decimal)_collider.Offset.Z;

            numSizeX.Value = (decimal)_collider.Size.X;
            numSizeY.Value = (decimal)_collider.Size.Y;
            numSizeZ.Value = (decimal)_collider.Size.Z;

            _loading = false;
        }

        private void SaveValues()
        {
            if (_loading || _collider == null) return;

            _collider.Enabled = chkEnabled.Checked;
            _collider.IsTrigger = chkTrigger.Checked;

            _collider.Offset.X = (double)numOffsetX.Value;
            _collider.Offset.Y = (double)numOffsetY.Value;
            _collider.Offset.Z = (double)numOffsetZ.Value;

            _collider.Size = new Vector3((float)numSizeX.Value, (float)numSizeY.Value, (float)numSizeZ.Value);

            OnObjectModified?.Invoke(_objeto);
        }

        public void SetObject(Objetos obj)
        {
            _objeto = obj;

            if (_objeto.BoxCollider == null)
            {
                _objeto.BoxCollider = new BoxCollider();
                _collider = _objeto.BoxCollider;

                AjustarColliderAlMesh();
            }

            _collider = _objeto.BoxCollider;

            LoadValues();
        }

        public void Refresh(Objetos obj)
        {
            _objeto = obj;
        }

        private void AjustarColliderAlMesh()
        {
            if (_objeto?.MeshFilter?.Mesh == null)
                return;

            var vertices = _objeto.MeshFilter.Mesh.Vertices;

            if (vertices == null || vertices.Count == 0)
                return;

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double minZ = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double maxZ = double.MinValue;

            foreach (var v in vertices)
            {
                var p = v.vector;

                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                minZ = Math.Min(minZ, p.Z);

                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
                maxZ = Math.Max(maxZ, p.Z);
            }

            _collider.Size = new Vector3(
                (maxX - minX) * _objeto.Transform.Scale.X,
                (maxY - minY) * _objeto.Transform.Scale.Y,
                (maxZ - minZ) * _objeto.Transform.Scale.Z
            );

            _collider.Offset = new Vector3(
                ((minX + maxX) / 2.0) * _objeto.Transform.Scale.X,
                ((minY + maxY) / 2.0) * _objeto.Transform.Scale.Y,
                ((minZ + maxZ) / 2.0) * _objeto.Transform.Scale.Z
            );
        }
    }
}
