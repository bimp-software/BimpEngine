using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.World;
using System;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class TransformControl : UserControl, IInspectorComponent
    {
        private Objetos objeto;
        private bool _actualizando = false;

        public event Action<Objetos> OnObjectModified;

        public TransformControl()
        {
            InitializeComponent();

            SuscribirValidacion(tbPosition_X, () => objeto?.Transform.Position.X.ToString("F2"));
            SuscribirValidacion(tbPosition_Y, () => objeto?.Transform.Position.Y.ToString("F2"));
            SuscribirValidacion(tbPosition_Z, () => objeto?.Transform.Position.Z.ToString("F2"));

            SuscribirValidacion(tbRotation_X, () => objeto?.Transform.Rotation.X.ToString("F2"));
            SuscribirValidacion(tbRotation_Y, () => objeto?.Transform.Rotation.Y.ToString("F2"));
            SuscribirValidacion(tbRotation_Z, () => objeto?.Transform.Rotation.Z.ToString("F2"));

            SuscribirValidacion(tbScale_X, () => objeto?.Transform.Scale.X.ToString("F2"));
            SuscribirValidacion(tbScale_Y, () => objeto?.Transform.Scale.Y.ToString("F2"));
            SuscribirValidacion(tbScale_Z, () => objeto?.Transform.Scale.Z.ToString("F2"));

            // Aplicar al objeto al perder foco
            tbPosition_X.TextChanged += (s, e) => AplicarTransform();
            tbPosition_Y.TextChanged += (s, e) => AplicarTransform();
            tbPosition_Z.TextChanged += (s, e) => AplicarTransform();

            tbRotation_X.TextChanged += (s, e) => AplicarTransform();
            tbRotation_Y.TextChanged += (s, e) => AplicarTransform();
            tbRotation_Z.TextChanged += (s, e) => AplicarTransform();

            tbScale_X.TextChanged += (s, e) => AplicarTransform();
            tbScale_Y.TextChanged += (s, e) => AplicarTransform();
            tbScale_Z.TextChanged += (s, e) => AplicarTransform();
        }

        private void AplicarTransform()
        {
            if (objeto == null || _actualizando) return;

            if (double.TryParse(tbPosition_X.Text, out double px)) objeto.Transform.Position.X = px;
            if (double.TryParse(tbPosition_Y.Text, out double py)) objeto.Transform.Position.Y = py;
            if (double.TryParse(tbPosition_Z.Text, out double pz)) objeto.Transform.Position.Z = pz;

            if (double.TryParse(tbRotation_X.Text, out double rx)) objeto.Transform.Rotation.X = rx;
            if (double.TryParse(tbRotation_Y.Text, out double ry)) objeto.Transform.Rotation.Y = ry;
            if (double.TryParse(tbRotation_Z.Text, out double rz)) objeto.Transform.Rotation.Z = rz;

            if (double.TryParse(tbScale_X.Text, out double sx)) objeto.Transform.Scale.X = sx;
            if (double.TryParse(tbScale_Y.Text, out double sy)) objeto.Transform.Scale.Y = sy;
            if (double.TryParse(tbScale_Z.Text, out double sz)) objeto.Transform.Scale.Z = sz;

            OnObjectModified?.Invoke(objeto);
        }

        private void SuscribirValidacion(TextBox tb, Func<string> valorAnterior)
        {
            tb.KeyPress += (s, e) =>
            {
                bool esNumero = char.IsDigit(e.KeyChar);
                bool esPunto = e.KeyChar == '.' && !tb.Text.Contains('.');
                bool esNegativo = e.KeyChar == '-' && tb.SelectionStart == 0 && !tb.Text.Contains('-');
                bool esControl = char.IsControl(e.KeyChar);

                if (!esNumero && !esPunto && !esNegativo && !esControl)
                    e.Handled = true;
            };

            tb.Leave += (s, e) =>
            {
                if (objeto == null) return;
                if (!double.TryParse(tb.Text, out _))
                    tb.Text = valorAnterior();
            };
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;
            Refresh(obj);
        }

        public void Refresh(Objetos obj)
        {
            if (obj == null) return;
            objeto = obj;

            _actualizando = true;

            tbPosition_X.Text = obj.Transform.Position.X.ToString("F2");
            tbPosition_Y.Text = obj.Transform.Position.Y.ToString("F2");
            tbPosition_Z.Text = obj.Transform.Position.Z.ToString("F2");

            tbRotation_X.Text = obj.Transform.Rotation.X.ToString("F2");
            tbRotation_Y.Text = obj.Transform.Rotation.Y.ToString("F2");
            tbRotation_Z.Text = obj.Transform.Rotation.Z.ToString("F2");

            tbScale_X.Text = obj.Transform.Scale.X.ToString("F2");
            tbScale_Y.Text = obj.Transform.Scale.Y.ToString("F2");
            tbScale_Z.Text = obj.Transform.Scale.Z.ToString("F2");

            _actualizando = false;
        }

    }
}