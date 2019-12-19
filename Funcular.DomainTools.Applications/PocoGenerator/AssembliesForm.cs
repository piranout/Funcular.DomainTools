using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace Funcular.DomainTools.Applications
{
    public partial class AssembliesForm : Form
    {
        private static readonly ICollection<Assembly> _inheritableAssemblies = new List<Assembly>();

        public AssembliesForm()
        {
            InitializeComponent();
        }

        public static ICollection<Assembly> InheritableAssemblies
        {
            get { return _inheritableAssemblies; }
        }

        public void AddAssembly()
        {
            var success = false;
            var dialogResult = openFileDialog1.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                try
                {
                    var assemblyToLoad = Assembly.LoadFile(fileName);
                    InheritableAssemblies.Add(assemblyToLoad);
                    listBox1.Items.Add(openFileDialog1.SafeFileName);
                    success = true;
                    MessageBox.Show(success
                            ? $"Assembly load succeeded for {openFileDialog1.SafeFileName}"
                            : $"Assembly load failed for {openFileDialog1.SafeFileName}",
                        icon: MessageBoxIcon.Information,
                        caption: "Assembly Loader",
                        buttons: MessageBoxButtons.OK);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBox.Show($@"Oops! Couldn't load assembly from {openFileDialog1.SafeFileName}!",
                        icon: MessageBoxIcon.Error,
                        caption: @"Assembly Loader",
                        buttons: MessageBoxButtons.OK);
                }
            }
        }

        public bool RemoveAssemblies(string[] assemblyNames)
        {
            bool retValue = false;
            try
            {
                InheritableAssemblies.Where(x => assemblyNames.Contains(x.FullName)).ToList()
                    .ForEach(x => retValue = InheritableAssemblies.Remove(x));
                //foreach (var assembly in InheritableAssemblies.Where(x => x.FullName == assemblyName))
                //{
                //     retValue = InheritableAssemblies.Remove(assembly);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retValue;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddAssembly();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            object[] list = new object[listBox1.SelectedItems.Count];
            listBox1.SelectedItems.CopyTo(list, 0);
            foreach (var listBox1SelectedItem in list)
            {
                listBox1.Items.Remove(listBox1SelectedItem.ToString());
            }

            RemoveAssemblies(listBox1.SelectedItems
                .Cast<string>()
                .Select(x => x.ToString())
                .ToArray());
        }
    }
}
