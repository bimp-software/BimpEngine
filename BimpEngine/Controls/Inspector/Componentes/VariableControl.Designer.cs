namespace BimpEngine.Controls.Inspector.Componentes
{
    partial class VariableControl
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
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            cbLayer = new ComboBox();
            cbTag = new ComboBox();
            cbEnabled = new CheckBox();
            tbName = new TextBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(227, 26);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(40, 40, 40);
            label1.Dock = DockStyle.Left;
            label1.ForeColor = Color.White;
            label1.ImeMode = ImeMode.NoControl;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(5, 0, 0, 0);
            label1.Size = new Size(103, 26);
            label1.TabIndex = 0;
            label1.Text = "Variable";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(40, 40, 40);
            panel2.Controls.Add(cbLayer);
            panel2.Controls.Add(cbTag);
            panel2.Controls.Add(cbEnabled);
            panel2.Controls.Add(tbName);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 26);
            panel2.Name = "panel2";
            panel2.Size = new Size(227, 73);
            panel2.TabIndex = 2;
            // 
            // cbLayer
            // 
            cbLayer.BackColor = Color.FromArgb(48, 48, 48);
            cbLayer.DropDownHeight = 50;
            cbLayer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLayer.DropDownWidth = 90;
            cbLayer.FlatStyle = FlatStyle.Flat;
            cbLayer.ForeColor = Color.White;
            cbLayer.FormattingEnabled = true;
            cbLayer.IntegralHeight = false;
            cbLayer.Location = new Point(124, 40);
            cbLayer.Name = "cbLayer";
            cbLayer.Size = new Size(93, 23);
            cbLayer.TabIndex = 3;
            cbLayer.TabStop = false;
            cbLayer.Tag = "Layouts";
            // 
            // cbTag
            // 
            cbTag.BackColor = Color.FromArgb(48, 48, 48);
            cbTag.DropDownHeight = 100;
            cbTag.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTag.DropDownWidth = 100;
            cbTag.FlatStyle = FlatStyle.Flat;
            cbTag.ForeColor = Color.White;
            cbTag.FormattingEnabled = true;
            cbTag.IntegralHeight = false;
            cbTag.Location = new Point(10, 40);
            cbTag.Name = "cbTag";
            cbTag.Size = new Size(108, 23);
            cbTag.TabIndex = 4;
            cbTag.TabStop = false;
            cbTag.Tag = "Layers";
            // 
            // cbEnabled
            // 
            cbEnabled.AutoSize = true;
            cbEnabled.ForeColor = Color.White;
            cbEnabled.Location = new Point(10, 13);
            cbEnabled.Name = "cbEnabled";
            cbEnabled.Size = new Size(15, 14);
            cbEnabled.TabIndex = 2;
            cbEnabled.UseVisualStyleBackColor = true;
            // 
            // tbName
            // 
            tbName.BackColor = Color.FromArgb(48, 48, 48);
            tbName.BorderStyle = BorderStyle.None;
            tbName.Font = new Font("Segoe UI", 12F);
            tbName.ForeColor = Color.White;
            tbName.Location = new Point(35, 8);
            tbName.Multiline = true;
            tbName.Name = "tbName";
            tbName.Size = new Size(182, 25);
            tbName.TabIndex = 1;
            // 
            // VariableControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(35, 35, 35);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "VariableControl";
            Size = new Size(227, 99);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private TextBox tbName;
        private CheckBox cbEnabled;
        private ComboBox cbLayer;
        private ComboBox cbTag;
    }
}
