using luavim.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luavim
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("#"))
            {
                Shared.dirColor = ColorTranslator.FromHtml(textBox1.Text);
            }
            if (textBox2.Text.Contains("#"))
            {
                Shared.sideColor = ColorTranslator.FromHtml(textBox2.Text);
            }
            if (textBox3.Text.Contains("#"))
            {
                Shared.termColor = ColorTranslator.FromHtml(textBox3.Text);
            }
            Settings.Default.Save();
            timer1.Stop();
            timer1.Dispose();
            Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            foreach(var file in Directory.GetFiles("Config\\images\\"))
            {
                var ex = new FileInfo(file);
                if (ex.FullName.Contains(".png"))
                {
                    comboBox1.Items.Add(ex.Name);
                    comboBox3.Items.Add(ex.Name);
                }else if (ex.FullName.Contains(".jpg"))
                {
                    comboBox1.Items.Add(ex.Name);
                    comboBox3.Items.Add(ex.Name);
                }else if (ex.FullName.Contains(".gif"))
                {
                    comboBox1.Items.Add(ex.Name);
                    comboBox3.Items.Add(ex.Name);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = comboBox1.Text;
            Settings.Default.sidebarImage = Application.StartupPath + "\\Config\\images\\" + name;
            Shared.img_side_t = name;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = comboBox3.Text;
            Settings.Default.directoryImage = Application.StartupPath + "\\Config\\images\\"+name;
            Shared.img_dir_t = name;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.Default.terminalImage = "nan";
            Settings.Default.sidebarImage = "nan";
            Settings.Default.directoryImage = "nan";
            Shared.img_dir_t = null;
            Shared.img_side_t = null;
            Settings.Default.Save();
        }
    }
}
