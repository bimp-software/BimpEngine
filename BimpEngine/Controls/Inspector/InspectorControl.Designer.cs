namespace BimpEngine.Controls.Inspector
{
    partial class InspectorControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectorControl));
            panel1 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            flpContenedor = new FlowLayoutPanel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.BackColor = Color.FromArgb(40, 40, 40);
            label1.ForeColor = Color.White;
            label1.Name = "label1";
            // 
            // flpContenedor
            // 
            resources.ApplyResources(flpContenedor, "flpContenedor");
            flpContenedor.BackColor = Color.FromArgb(40, 40, 40);
            flpContenedor.Name = "flpContenedor";
            // 
            // InspectorControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(48, 48, 48);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(flpContenedor);
            Controls.Add(panel1);
            ForeColor = Color.FromArgb(250, 250, 250);
            Name = "InspectorControl";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label1;
        private FlowLayoutPanel flpContenedor;
    }
}
