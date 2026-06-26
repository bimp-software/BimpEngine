namespace BimpEngine
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Vista.frmBimpEngine frm = new Vista.frmBimpEngine();
            frm.FormClosing += Frm_FormClosing;
            frm.FormClosed += Frm_FormClosed;

            frm.ShowDialog();

            Application.Run();
        }

        private static void Frm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
        }

        private static void Frm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
        }
    }
}