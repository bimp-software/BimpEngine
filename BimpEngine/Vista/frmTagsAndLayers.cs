using BimpEngine.Engine.Project;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BimpEngine.Vista
{
    public class frmTagsAndLayers : Form
    {
        // ── Controles Tags ────────────────────────────────────────────────────
        private ListBox lstTags;
        private TextBox tbNewTag;
        private Button btnAddTag;
        private Button btnRemoveTag;

        // ── Controles Layers ──────────────────────────────────────────────────
        private ListBox lstLayers;
        private TextBox tbNewLayer;
        private Button btnAddLayer;
        private Button btnRemoveLayer;

        // ── Acciones globales ─────────────────────────────────────────────────
        private Button btnSaveGlobal;
        private Button btnClose;

        public frmTagsAndLayers()
        {
            Text = "Tags and Layers";
            Size = new Size(520, 520);
            MinimumSize = new Size(420, 420);
            BackColor = Color.FromArgb(35, 35, 35);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;

            BuildUI();
            RefreshLists();
        }

        // ── Build UI ──────────────────────────────────────────────────────────

        private void BuildUI()
        {
            // Tabs
            var tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };

            tabs.TabPages.Add(BuildTagsPage());
            tabs.TabPages.Add(BuildLayersPage());

            // Footer
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 48 };

            btnSaveGlobal = MakeButton("Guardar como predeterminado global", Color.FromArgb(0, 100, 160));
            btnSaveGlobal.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            btnSaveGlobal.Location = new Point(8, 10);
            btnSaveGlobal.Width = 240;
            btnSaveGlobal.Click += (s, e) =>
            {
                TagLayerManager.SaveAsGlobal();
                TagLayerManager.Save();
                MessageBox.Show(
                    "Los tags y layers se guardaron como predeterminados globales.\nNuevos proyectos los heredarán automáticamente.",
                    "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnClose = MakeButton("Cerrar", Color.FromArgb(60, 60, 65));
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.Location = new Point(footer.Width - 108, 10);
            btnClose.Width = 100;
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Click += (s, e) =>
            {
                TagLayerManager.Save();
                Close();
            };

            footer.Controls.Add(btnSaveGlobal);
            footer.Controls.Add(btnClose);

            Controls.Add(tabs);
            Controls.Add(footer);
        }

        private TabPage BuildTagsPage()
        {
            var page = new TabPage("Tags") { BackColor = Color.FromArgb(40, 40, 40) };

            // Builtin (read-only)
            var lblBuiltin = MakeLabel("Builtin Tags (solo lectura)");
            lblBuiltin.Location = new Point(8, 8);

            var lstBuiltin = new ListBox
            {
                Location = new Point(8, 28),
                Size = new Size(480, 120),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(140, 140, 140),
                BorderStyle = BorderStyle.FixedSingle,
                Enabled = false
            };
            foreach (var t in TagLayerSettings.BuiltinTags)
                lstBuiltin.Items.Add(t);

            // Custom
            var lblCustom = MakeLabel("Tags del proyecto");
            lblCustom.Location = new Point(8, 162);

            lstTags = new ListBox
            {
                Location = new Point(8, 182),
                Size = new Size(480, 180),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Input row
            tbNewTag = new TextBox
            {
                Location = new Point(8, 372),
                Size = new Size(350, 24),
                BackColor = Color.FromArgb(55, 55, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Nombre del nuevo tag..."
            };
            tbNewTag.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) AddTag(); };

            btnAddTag = MakeButton("Agregar", Color.FromArgb(0, 130, 80));
            btnAddTag.Location = new Point(368, 370);
            btnAddTag.Width = 60;
            btnAddTag.Click += (s, e) => AddTag();

            btnRemoveTag = MakeButton("Quitar", Color.FromArgb(140, 40, 40));
            btnRemoveTag.Location = new Point(436, 370);
            btnRemoveTag.Width = 52;
            btnRemoveTag.Click += (s, e) => RemoveTag();

            page.Controls.AddRange(new Control[]
            {
                lblBuiltin, lstBuiltin, lblCustom,
                lstTags, tbNewTag, btnAddTag, btnRemoveTag
            });

            return page;
        }

        private TabPage BuildLayersPage()
        {
            var page = new TabPage("Layers") { BackColor = Color.FromArgb(40, 40, 40) };

            var lblBuiltin = MakeLabel("Builtin Layers (solo lectura)");
            lblBuiltin.Location = new Point(8, 8);

            var lstBuiltin = new ListBox
            {
                Location = new Point(8, 28),
                Size = new Size(480, 100),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(140, 140, 140),
                BorderStyle = BorderStyle.FixedSingle,
                Enabled = false
            };
            foreach (var l in TagLayerSettings.BuiltinLayers)
                lstBuiltin.Items.Add(l.ToString());

            var lblCustom = MakeLabel("Layers del proyecto (slots 5–31)");
            lblCustom.Location = new Point(8, 142);

            lstLayers = new ListBox
            {
                Location = new Point(8, 162),
                Size = new Size(480, 200),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            tbNewLayer = new TextBox
            {
                Location = new Point(8, 372),
                Size = new Size(350, 24),
                BackColor = Color.FromArgb(55, 55, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Nombre del nuevo layer..."
            };
            tbNewLayer.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) AddLayer(); };

            btnAddLayer = MakeButton("Agregar", Color.FromArgb(0, 130, 80));
            btnAddLayer.Location = new Point(368, 370);
            btnAddLayer.Width = 60;
            btnAddLayer.Click += (s, e) => AddLayer();

            btnRemoveLayer = MakeButton("Quitar", Color.FromArgb(140, 40, 40));
            btnRemoveLayer.Location = new Point(436, 370);
            btnRemoveLayer.Width = 52;
            btnRemoveLayer.Click += (s, e) => RemoveLayer();

            page.Controls.AddRange(new Control[]
            {
                lblBuiltin, lstBuiltin, lblCustom,
                lstLayers, tbNewLayer, btnAddLayer, btnRemoveLayer
            });

            return page;
        }

        // ── Actions ───────────────────────────────────────────────────────────

        private void AddTag()
        {
            string name = tbNewTag.Text.Trim();
            if (!TagLayerManager.Current.AddTag(name))
            {
                MessageBox.Show($"El tag \"{name}\" ya existe o no es válido.",
                    "Tag duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tbNewTag.Clear();
            RefreshLists();
        }

        private void RemoveTag()
        {
            if (lstTags.SelectedItem is not string tag) return;
            if (!TagLayerManager.Current.RemoveTag(tag))
            {
                MessageBox.Show("No se pueden quitar los tags predeterminados.",
                    "Protegido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            RefreshLists();
        }

        private void AddLayer()
        {
            string name = tbNewLayer.Text.Trim();
            if (!TagLayerManager.Current.AddLayer(name))
            {
                MessageBox.Show("Nombre inválido o se alcanzó el límite de 32 layers.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tbNewLayer.Clear();
            RefreshLists();
        }

        private void RemoveLayer()
        {
            if (lstLayers.SelectedItem is not string entry) return;

            // Extraer índice del string "5: Ground"
            int idx = int.Parse(entry.Split(':')[0].Trim());
            if (!TagLayerManager.Current.RemoveLayer(idx))
            {
                MessageBox.Show("No se pueden quitar los layers predeterminados.",
                    "Protegido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            RefreshLists();
        }

        private void RefreshLists()
        {
            lstTags.Items.Clear();
            foreach (var t in TagLayerManager.Current.CustomTags)
                lstTags.Items.Add(t);

            lstLayers.Items.Clear();
            foreach (var l in TagLayerManager.Current.CustomLayers)
                lstLayers.Items.Add(l.ToString());
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static Label MakeLabel(string text) => new Label
        {
            Text = text,
            AutoSize = true,
            ForeColor = Color.FromArgb(170, 170, 170),
            Font = new Font("Segoe UI", 8f, FontStyle.Bold)
        };

        private static Button MakeButton(string text, Color back) => new Button
        {
            Text = text,
            Height = 28,
            BackColor = back,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
    }
}