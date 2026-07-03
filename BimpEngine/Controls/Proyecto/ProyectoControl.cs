using BimpEngine.Engine.Project;
using BimpEngine.Engine.Project.Theme;


namespace BimpEngine.Controls.Proyecto
{
    public partial class ProyectoControl : UserControl
    {
        public event Action<string>? OnOpenScene;
        public event Action<string>? OnOpenBlueprint;
        public event Action<string>? OnInstanciarMolde;
        public event Action<string>? OnImportModelRequested;
        public event Action<string>? OnOpenScript;


        private string? _rootPath;

        private const int ICON_FOLDER_CLOSED = 0;
        private const int ICON_FOLDER_OPEN = 1;
        private const int ICON_SCENE = 2;
        private const int ICON_FILE = 3;
        private const int ICON_PROJECT = 4;
        private const int ICON_BLUEPRINT = 5;
        private const int ICON_MOLDE = 6;
        private const int ICON_SCRIPT_CS = 7;
        private const int ICON_SCRIPT_LUA = 8;
        private const int ICON_SCRIPT_PY = 9;

        public ProyectoControl()
        {
            InitializeComponent();
            BuildIcons();
            _tree.ImageList = _icons;
            _tree.DrawMode = TreeViewDrawMode.OwnerDrawText;

            _tree.AllowDrop = false;
        }

        private void BuildIcons()
        {
            _icons.ImageSize = new Size(16, 16);
            _icons.ColorDepth = ColorDepth.Depth32Bit;

            // 0 — folder closed
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(220, 175, 70)), 1, 5, 14, 9);
                g.FillRectangle(new SolidBrush(Color.FromArgb(240, 200, 80)), 1, 3, 7, 4);
            }));
            // 1 — folder open
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(220, 175, 70)), 0, 5, 15, 9);
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 220, 100)), 0, 3, 7, 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 235, 130)), 2, 7, 12, 5);
            }));
            // 2 — .bscene
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(80, 140, 220)), 2, 1, 10, 13);
                g.FillRectangle(new SolidBrush(Color.FromArgb(130, 190, 255)), 4, 4, 6, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(130, 190, 255)), 4, 7, 6, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(130, 190, 255)), 4, 10, 4, 1);
            }));
            // 3 — generic file
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(170, 170, 175)), 2, 1, 10, 13);
                g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 105)), 4, 4, 6, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 105)), 4, 7, 6, 1);
            }));
            // 4 — project root
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.RoyalBlue), 1, 1, 14, 14);
                g.DrawString("B", new Font("Segoe UI", 7f, FontStyle.Bold),
                    Brushes.White, 3f, 2f);
            }));
            // En BuildIcons() agrega:
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(140, 60, 180)), 2, 1, 10, 13);
                g.DrawString("B", new Font("Segoe UI", 7f, FontStyle.Bold), Brushes.White, 3f, 2f);
            }));
            _icons.Images.Add(DrawIcon((g, r) =>
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(110, 170, 255)), 2, 1, 10, 13);
                g.FillRectangle(new SolidBrush(Color.FromArgb(160, 200, 255)), 4, 4, 6, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(160, 200, 255)), 4, 7, 6, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb(160, 200, 255)), 4, 10, 4, 1);
            }));
            _icons.Images.Add(DrawIcon((g, r) => // 7 - C#
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(80, 140, 80)), 2, 1, 10, 13);
                g.DrawString("C#", new Font("Segoe UI", 5.5f, FontStyle.Bold), Brushes.White, 1f, 4f);
            }));
            _icons.Images.Add(DrawIcon((g, r) => // 8 - Lua
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(60, 90, 180)), 2, 1, 10, 13);
                g.DrawString("Lu", new Font("Segoe UI", 6f, FontStyle.Bold), Brushes.White, 1f, 4f);
            }));
            _icons.Images.Add(DrawIcon((g, r) => // 9 - Python
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(210, 170, 60)), 2, 1, 10, 13);
                g.DrawString("Py", new Font("Segoe UI", 6f, FontStyle.Bold), Brushes.White, 1f, 4f);
            }));
        }

        private static Bitmap DrawIcon(Action<Graphics, Rectangle> draw)
        {
            var bmp = new Bitmap(16, 16);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            draw(g, new Rectangle(0, 0, 16, 16));
            return bmp;
        }

        public void CargarProyecto(string projectFolder)
        {
            _rootPath = projectFolder;
            RefrescarArbol();
        }

        public void RefrescarArbol()
        {
            if (_rootPath == null || !Directory.Exists(_rootPath))
            {
                _tree.Nodes.Clear();
                var empty = new TreeNode("Ningún proyecto abierto")
                { ForeColor = Color.Gray, ImageIndex = ICON_FILE, SelectedImageIndex = ICON_FILE };
                _tree.Nodes.Add(empty);
                return;
            }

            _tree.BeginUpdate();
            _tree.Nodes.Clear();

            string name = ProjectManager.CurrentProject?.Name ?? Path.GetFileName(_rootPath);
            var root = new TreeNode(name)
            {
                Tag = _rootPath,
                ImageIndex = ICON_PROJECT,
                SelectedImageIndex = ICON_PROJECT
            };

            PopulateNode(root, _rootPath);
            _tree.Nodes.Add(root);
            root.Expand();

            _tree.EndUpdate();
        }

        private static bool HasChildren(string folder)
        {
            try
            {
                return Directory.GetDirectories(folder).Length > 0
                    || Directory.GetFiles(folder).Length > 0;
            }
            catch { return false; }
        }

        private void PopulateNode(TreeNode parent, string folder)
        {
            // Folders first
            foreach (var dir in Directory.GetDirectories(folder))
            {
                string dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) continue; // skip hidden

                var node = new TreeNode(dirName)
                {
                    Tag = dir,
                    ImageIndex = ICON_FOLDER_CLOSED,
                    SelectedImageIndex = ICON_FOLDER_OPEN
                };
                // Lazy-load: add a dummy child so the expander arrow shows
                if (HasChildren(dir))
                    node.Nodes.Add(new TreeNode("...") { Tag = "__lazy__" });

                parent.Nodes.Add(node);
            }

            // Files
            foreach (var file in Directory.GetFiles(folder))
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ProjectManager.ProjectExtension) continue;
                if (Path.GetFileName(file).StartsWith(".")) continue;

                int icon = ext switch
                {
                    ".bscene" => ICON_SCENE,
                    ".bscript" => ICON_BLUEPRINT,
                    ".bmold" => ICON_MOLDE,
                    ".cs" => ICON_SCRIPT_CS,
                    ".lua" => ICON_SCRIPT_LUA,
                    ".py" => ICON_SCRIPT_PY,
                    _ => ICON_FILE
                };

                var node = new TreeNode(Path.GetFileName(file))
                {
                    Tag = file,
                    ImageIndex = icon,
                    SelectedImageIndex = icon
                };
                parent.Nodes.Add(node);
            }
        }

        private void _tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node!;
            if (node.Nodes.Count == 1 && node.Nodes[0].Tag?.ToString() == "__lazy__")
            {
                _tree.BeginUpdate();
                node.Nodes.Clear();
                if (node.Tag is string folder && Directory.Exists(folder))
                    PopulateNode(node, folder);
                _tree.EndUpdate();
            }
        }

        private void _tree_DoubleClick(object sender, EventArgs e)
        {
            if (_tree.SelectedNode?.Tag is string path && File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToLower();
                if (ext == ProjectManager.SceneExtension)
                    OnOpenScene?.Invoke(path);
                else if (ext == ".bscript")
                    OnOpenBlueprint?.Invoke(path);
                else if (ext == ".bmold")
                    OnInstanciarMolde?.Invoke(path);
                else if (ext is ".cs" or ".lua" or ".py")
                    OnOpenScript?.Invoke(path);
            }
        }

        private void _tree_KeyDown(object sender, KeyEventArgs e)
        {
            if (_tree.SelectedNode == null) return;
            if (e.KeyCode == Keys.F2) { IniciarRenombrar(_tree.SelectedNode); e.Handled = true; }
            if (e.KeyCode == Keys.Delete) { EliminarNodo(_tree.SelectedNode); e.Handled = true; }
            if (e.KeyCode == Keys.F5) { RefrescarArbol(); e.Handled = true; }
        }

        private void _tree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var node = _tree.GetNodeAt(e.X, e.Y);
            if (node != null) _tree.SelectedNode = node;

            bool isFolder = node?.Tag is string p && Directory.Exists(p);
            bool isFile = node?.Tag is string f && File.Exists(f);

            var menu = new ContextMenuStrip();
            menu.BackColor = Color.FromArgb(40, 40, 42);
            menu.ForeColor = Color.White;
            menu.Renderer = new DarkMenuRenderer();

            if (isFolder)
            {
                string folder = (string)node!.Tag!;

                Add(menu, "Nueva Escena aquí", () => NuevaEscena(folder, node));
                Add(menu, "Nueva Carpeta aquí", () => NuevaCarpeta(folder, node));
                Add(menu, "Importar Modelo (FBX,OBJ)...", () => ImportarModeloDialog(folder));
                menu.Items.Add(new ToolStripSeparator());
                Add(menu, "Nuevo Blueprint aquí", () => NuevoBlueprint(folder, node));
                menu.Items.Add(new ToolStripSeparator());
                Add(menu, "Mostrar en Explorador…", () => AbrirEnExplorador(folder));
                menu.Items.Add(new ToolStripSeparator());
                Add(menu, "Actualizar", () => RefrescarArbol());
                var subScript = new ToolStripMenuItem("Nuevo Script");
                subScript.ForeColor = Color.White;
                Add2(subScript, "C#", () => NuevoScript(folder, "cs"));
                Add2(subScript, "Lua", () => NuevoScript(folder, "lua"));
                Add2(subScript, "Python", () => NuevoScript(folder, "py"));
                menu.Items.Add(subScript);
            }
            else if (isFile)
            {
                string file = (string)node!.Tag!;
                string ext = Path.GetExtension(file).ToLower();

                if (ext == ProjectManager.SceneExtension)
                    Add(menu, "Abrir Escena", () => OnOpenScene?.Invoke(file));

                if (ext == ".bscript")
                    Add(menu, "Abrir Blueprint", () => OnOpenBlueprint?.Invoke(file));

                if (ext == ".bmold")
                    Add(menu, "Instanciar en la Escena", () => OnInstanciarMolde?.Invoke(file));

                if (ext is ".cs" or ".lua" or ".py")
                    Add(menu, "Editar Script", () => OnOpenScript?.Invoke(file));

                menu.Items.Add(new ToolStripSeparator());
                Add(menu, "Renombrar (F2)", () => IniciarRenombrar(node));
                Add(menu, "Eliminar (Del)", () => EliminarNodo(node));
                menu.Items.Add(new ToolStripSeparator());
                Add(menu, "Mostrar en Explorador…", () => AbrirEnExplorador(Path.GetDirectoryName(file)!));
            }
            else
            {
                // Clicked on empty space → root actions
                if (_rootPath != null)
                {
                    Add(menu, "Nueva Escena", () => NuevaEscena(_rootPath + "/Scenes", null));
                    Add(menu, "Importar Modelo (FBX/OBJ)…", () => ImportarModeloDialog(_rootPath + "/Models"));
                    Add(menu, "Actualizar (F5)", () => RefrescarArbol());
                    menu.Items.Add(new ToolStripSeparator());
                    Add(menu, "Mostrar en Explorador…", () => AbrirEnExplorador(_rootPath));
                }
            }

            if (menu.Items.Count > 0)
                menu.Show(_tree, e.Location);
        }

        private static bool EsModelo3D(string ext) => ext is ".fbx" or ".obj" or ".dae" or ".gltf" or ".glb" or ".3ds" or ".stl";

        private void ImportarModeloDialog(string carpetaDestino)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Importar Modelo 3D",
                Filter = "Modelos 3D (*.fbx;*.obj;*.dae;*.gltf;*.glb;*.3ds;*.stl)|*.fbx;*.obj;*.dae;*.gltf;*.glb;*.3ds;*.stl|Todos los archivos|*.*"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                Directory.CreateDirectory(carpetaDestino);

                string nombreArchivo = Path.GetFileName(dialog.FileName);
                string destino = Path.Combine(carpetaDestino, nombreArchivo);

                if (!File.Exists(destino))
                    File.Copy(dialog.FileName, destino);

                // Copiar también texturas que estén en la misma carpeta de origen
                string carpetaOrigen = Path.GetDirectoryName(dialog.FileName)!;
                foreach (var ext in new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.tga" })
                {
                    foreach (var tex in Directory.GetFiles(carpetaOrigen, ext))
                    {
                        string texDestino = Path.Combine(carpetaDestino, Path.GetFileName(tex));
                        if (!File.Exists(texDestino))
                            File.Copy(tex, texDestino);
                    }
                }

                RefrescarArbol();
                OnImportModelRequested?.Invoke(destino);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo importar el modelo:\n{ex.Message}",
                    "Error de importación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevaEscena(string folder, TreeNode? parentNode)
        {
            string name = Prompt("Nombre de la nueva escena:", "NuevaEscena");
            if (string.IsNullOrWhiteSpace(name)) return;

            string path = Path.Combine(folder, name + ProjectManager.SceneExtension);
            if (File.Exists(path))
            { MessageBox.Show("Ya existe una escena con ese nombre."); return; }

            Directory.CreateDirectory(folder);
            File.WriteAllText(path, SceneSerializer.EmptySceneJson(name));
            RefrescarArbol();
        }

        private void NuevaCarpeta(string parentFolder, TreeNode parentNode)
        {
            string name = Prompt("Nombre de la nueva carpeta:", "NuevaCarpeta");
            if (string.IsNullOrWhiteSpace(name)) return;

            string path = Path.Combine(parentFolder, name);
            try { Directory.CreateDirectory(path); }
            catch (Exception ex) { MessageBox.Show(ex.Message); return; }
            RefrescarArbol();
        }

        private void IniciarRenombrar(TreeNode node)
        {
            if (node.Tag is not string path) return;

            string oldName = Path.GetFileNameWithoutExtension(path);
            string ext = File.Exists(path) ? Path.GetExtension(path) : "";
            string newName = Prompt("Nuevo nombre:", oldName);
            if (string.IsNullOrWhiteSpace(newName) || newName == oldName) return;

            try
            {
                string dir = Path.GetDirectoryName(path)!;
                string newPath = Path.Combine(dir, newName + ext);
                if (File.Exists(path)) File.Move(path, newPath);
                else if (Directory.Exists(path)) Directory.Move(path, newPath);
                RefrescarArbol();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void EliminarNodo(TreeNode node)
        {
            if (node.Tag is not string path) return;
            bool isDir = Directory.Exists(path);

            string tipo = isDir ? "carpeta" : "archivo";
            string aviso = isDir
                ? $"¿Eliminar la carpeta '{Path.GetFileName(path)}' y todo su contenido?"
                : $"¿Eliminar '{Path.GetFileName(path)}'?";

            if (MessageBox.Show(aviso, "Confirmar eliminación",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                if (isDir) Directory.Delete(path, recursive: true);
                else File.Delete(path);
                node.Remove();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void NuevoBlueprint(string folder, TreeNode? parentNode)
        {
            string name = Prompt("Nombre del Blueprint:", "MiBlueprint");
            if (string.IsNullOrWhiteSpace(name)) return;

            string path = Path.Combine(folder, name + ".bscript");
            if (File.Exists(path)) { MessageBox.Show("Ya existe un blueprint con ese nombre."); return; }

            Directory.CreateDirectory(folder);
            File.WriteAllText(path, "{}"); // JSON vacío por ahora
            RefrescarArbol();
        }


        private static void AbrirEnExplorador(string path)
        {
            if (Directory.Exists(path))
                System.Diagnostics.Process.Start("explorer.exe", path);
            else if (File.Exists(path))
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{path}\"");
        }

        private void _tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool selected = (e.State & TreeNodeStates.Selected) != 0;
            var bounds = e.Bounds;

            if (selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(60, 100, 180)), bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text,
                    _tree.Font, bounds, Color.White,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private static void Add(ContextMenuStrip menu, string text, Action action)
        {
            var item = new ToolStripMenuItem(text);
            item.ForeColor = Color.White;
            item.Click += (s, e) => action();
            menu.Items.Add(item);
        }

        private static string Prompt(string message, string defaultValue)
        {
            using var frm = new Form
            {
                Width = 380,
                Height = 140,
                Text = message,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(32, 32, 34),
                ForeColor = Color.White
            };
            var tb = new TextBox
            {
                Text = defaultValue,
                Location = new Point(12, 20),
                Width = 340,
                BackColor = Color.FromArgb(50, 50, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            var btn = new Button
            {
                Text = "Aceptar",
                Location = new Point(240, 56),
                Width = 110,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btn.FlatAppearance.BorderSize = 0;
            frm.Controls.AddRange(new Control[] { tb, btn });
            frm.AcceptButton = btn;
            tb.SelectAll();
            return frm.ShowDialog() == DialogResult.OK ? tb.Text.Trim() : "";
        }

        private void _tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node && node.Tag is string path && File.Exists(path))
            {
                string ext = Path.GetExtension(path).ToLower();
                if (EsModelo3D(ext) || ext is ".cs" or ".lua" or ".py")
                    DoDragDrop(path, DragDropEffects.Copy);
            }
        }

        private void NuevoScript(string folder, string ext)
        {
            string name = Prompt("Nombre del Script:", "MiScript");
            if (string.IsNullOrWhiteSpace(name)) return;

            string path = Path.Combine(folder, name + "." + ext);
            if (File.Exists(path)) { MessageBox.Show("Ya existe un script con ese nombre."); return; }

            Directory.CreateDirectory(folder);
            File.WriteAllText(path, PlantillaScript(name, ext));
            RefrescarArbol();
            OnOpenScript?.Invoke(path);
        }

        private static string PlantillaScript(string nombre, string ext) => ext switch
        {
            "cs" =>
        $@"using BimpEngine.Engine.Scripting;

public class {nombre} : IScript
{{
    public override void Start()
    {{
        Log(""{nombre} iniciado en "" + gameObject.Name);
    }}

    public override void Update(float deltaTime)
    {{
    }}
}}",
            "lua" =>
        @"function start()
    log(""Script Lua iniciado en "" .. gameObject.Name)
end

function update(deltaTime)
end",
            "py" =>
        @"def start():
    log(""Script Python iniciado en "" + gameObject.Name)

def update(deltaTime):
    pass",
            _ => ""
        };

        private static void Add2(ToolStripMenuItem parent, string text, Action action)
        {
            var item = new ToolStripMenuItem(text);
            item.ForeColor = Color.White;
            item.Click += (s, e) => action();
            parent.DropDownItems.Add(item);
        }
    }
}
