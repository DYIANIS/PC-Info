using System;
using System.Windows.Forms;

namespace PC_Manager
{
    static partial class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception)
            { }
        }
    }
}
