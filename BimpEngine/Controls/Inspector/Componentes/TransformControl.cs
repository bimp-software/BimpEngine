using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class TransformControl : UserControl
    {
        private Objetos objeto;

        public TransformControl()
        {
            InitializeComponent();
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;

            tbPosition_X.Text = obj.Transform.Position.X.ToString();
            tbPosition_Y.Text = obj.Transform.Position.Y.ToString();
            tbPosition_Z.Text = obj.Transform.Position.Z.ToString();

            tbRotation_X.Text = obj.Transform.Rotation.X.ToString();
            tbRotation_Y.Text = obj.Transform.Rotation.Y.ToString();
            tbRotation_Z.Text = obj.Transform.Rotation.Z.ToString();

            tbScale_X.Text = obj.Transform.Scale.X.ToString();
            tbScale_Y.Text = obj.Transform.Scale.Y.ToString();
            tbScale_Z.Text = obj.Transform.Scale.Z.ToString();
        }
    }
}
