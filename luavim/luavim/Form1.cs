using luavim.Properties;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luavim
{
    public partial class Form1 : Form
    {
        public static void SetDoubleBuffered(Control c)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            PropertyInfo aProp =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        public static string currDir = "";
        public static string projLocation = "";
        public static bool is_ok = false;

        public static string lastPic_Sidebar = null;
        public static string lastPic_panel = null;
        public Form1()
        {
            InitializeComponent();
            projLocation = Settings.Default.projLoc;

            if (projLocation != "" && Directory.Exists(projLocation))
            {
                currDir = projLocation + "\\src\\";
                is_ok = true;
            }

            SetDoubleBuffered(listView1);
        }
        void handle_files(ListView treeView1, string dir){ treeView1.Items.Clear(); foreach (var dirs in Directory.GetDirectories(dir)) {DirectoryInfo DirInfo = new DirectoryInfo(dirs);var nodes = treeView1.Items.Add(DirInfo.Name, DirInfo.Name, 0);nodes.Tag = DirInfo.FullName;} foreach (var Files in Directory.GetFiles(dir)) {FileInfo fileInfo = new FileInfo(Files);var nodes = treeView1.Items.Add(fileInfo.Name, fileInfo.Name, 1);nodes.Tag = fileInfo.FullName; } }

        bool is_directory(string dir) {  return File.Exists(dir);    }
        bool is_directory_2(string dir) { return Directory.Exists(dir); }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var cmd = richTextBox1.Text;
                if (cmd.StartsWith("createf"))
                {
                    var dir = cmd.Replace("createf ", "");
                    if (Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir + "\\src");
                        projLocation = dir;
                        Settings.Default.projLoc = projLocation;
                        currDir = projLocation + "\\src\\";
                        Settings.Default.Save();
                        handle_files(listView1, currDir);
                    }
                }
                if (cmd.StartsWith("loadf"))
                {
                    var dir = cmd.Replace("loadf ", "");
                    if (Directory.Exists(dir) && Directory.Exists(dir + "\\src"))
                    {
                        projLocation = dir;
                        Settings.Default.projLoc = projLocation;
                        currDir = projLocation + "\\src\\";
                        Settings.Default.Save();
                        handle_files(listView1, currDir);
                    }
                }
                if (cmd.StartsWith("mkfile") && is_ok)
                {
                    var filedata = cmd.Replace("mkfile ", "");
                    if(!is_directory(currDir + "\\"+filedata))
                    {
                        File.WriteAllText(currDir + "\\" + filedata, "");
                        handle_files(listView1, currDir);
                    }
                }
                // #cf > ~/root/lv2.sidebar.select_on_click
                // #cf > ~/dev/themes/lv2.default
                // help
                // #cf > ~/dev/configuration/lv2.export
                if (cmd.StartsWith("#cf >"))
                {
                    var flag = cmd.Replace("#cf > ", "");
                    if (flag.ToLower().Contains("~/root/lv2.sidebar.select_on_click"))
                    {
                        var is_en = Settings.Default.select_on_click;
                        if (is_en) { Settings.Default.select_on_click = false; Settings.Default.Save(); } else { Settings.Default.select_on_click = true;Settings.Default.Save(); }
                        MessageBox.Show("Select_On_Click : SidebarAddon has been successfully enabled. : " + Settings.Default.select_on_click, "lv2");
                    }
                    if (flag.ToLower().Contains("~/dev/themes/lv2.default"))
                    {
                        new Config().Show();
                    }

                }
                if (cmd.StartsWith("mkfolder") && is_ok)
                {
                    var filedata = cmd.Replace("mkfolder ", "");
                    if (!is_directory_2(currDir + "\\" + filedata))
                    {
                        Directory.CreateDirectory(currDir + "\\" + filedata);
                        var current_dir = currDir;
                        currDir = current_dir + "\\" + filedata;
                        var ex = new DirectoryInfo(currDir + "\\" + filedata);
                        label1.Text = "~/src/" + ex.FullName.Replace(Settings.Default.projLoc + "\\src\\", "").Replace("\\", "/").Replace(filedata + "/" + filedata, filedata + "/");
                        handle_files(listView1, currDir);
                    }
                    
                }

                if (cmd.StartsWith("exit"))
                {
                    Application.Exit();

                }

                if (cmd.StartsWith("main") && is_ok)
                {
                    currDir = projLocation + "\\src\\";
                    label1.Text = "~/src/";
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                    {
                         ""
                    });
                    Settings.Default.currFile = "";
                    handle_files(listView1 , currDir);
                }
                if (cmd.StartsWith("openf") && is_ok)
                {
                    var filedata = cmd.Replace("openf ", "");
                    if (is_directory(currDir + "\\" + filedata))
                    {
                        /*HtmlDocument document = webBrowser1.Document;
                        string scriptName = "GetText";
                        object[] args = new string[0];
                        object obj = document.InvokeScript(scriptName, args);
                        string script = obj.ToString();
                        */
                        
                        var ex = new FileInfo(currDir + "\\" + filedata);
                        label1.Text = "~/src/" + ex.FullName.Replace(Settings.Default.projLoc + "\\src\\", "").Replace("\\", "/");

                        Settings.Default.currFile = ex.FullName;
                        webBrowser1.Document.InvokeScript("SetText", new object[]
                        {
                             File.ReadAllText(currDir + "\\" + filedata)
                        });
                    }
                }
                if (cmd.StartsWith("cdir") && is_ok)
                {
                    var directory_attempt = cmd.Replace("cdir ", "");
                    if (is_directory_2(currDir + "\\" + directory_attempt))
                    {
                        var current_dir = currDir;
                        currDir = current_dir + "\\"+directory_attempt;
                        var ex = new DirectoryInfo(currDir + "\\" + directory_attempt);
                        label1.Text = "~/src/" + ex.FullName.Replace(Settings.Default.projLoc + "\\src\\", "").Replace("\\", "/").Replace(directory_attempt + "/" + directory_attempt, directory_attempt + "/");
                        handle_files(listView1, currDir);
                    }
                }
                if (cmd.StartsWith("ls") && is_ok)
                {
                    var str = "";
                    foreach(var file_ex in Directory.GetFiles(currDir))
                    {
                        var lulz = new FileInfo(file_ex);
                        str += lulz.Name + "\n";
                    }
                    MessageBox.Show(str, "Listed files - luavim v2.1");
                }
                if (cmd.StartsWith("relf") && is_ok)
                {
                    if (is_directory(Settings.Default.currFile))
                    {
                        /*HtmlDocument document = webBrowser1.Document;
                        string scriptName = "GetText";
                        object[] args = new string[0];
                        object obj = document.InvokeScript(scriptName, args);
                        string script = obj.ToString();
                        */
                        HtmlDocument document = webBrowser1.Document;
                        string scriptName = "GetText";
                        object[] args = new string[0];
                        object obj = document.InvokeScript(scriptName, args);
                        string script = obj.ToString();
                        File.WriteAllText(Settings.Default.currFile, script);
                        Settings.Default.currFile = "";
                    }
                }

                richTextBox1.Clear();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.projLoc = projLocation;
            Settings.Default.Save();

        }

        WebClient wc = new WebClient();
        private string defPath = Application.StartupPath + "//Monaco//";

        private void addIntel(string label, string kind, string detail, string insertText)
        {
            string text = "\"" + label + "\"";
            string text2 = "\"" + kind + "\"";
            string text3 = "\"" + detail + "\"";
            string text4 = "\"" + insertText + "\"";
            webBrowser1.Document.InvokeScript("AddIntellisense", new object[]
            {
                label,
                kind,
                detail,
                insertText
            });
        }

        private void addGlobalF()
        {
            string[] array = File.ReadAllLines(this.defPath + "//globalf.txt");
            foreach (string text in array)
            {
                bool flag = text.Contains(':');
                if (flag)
                {
                    this.addIntel(text, "Function", text, text.Substring(1));
                }
                else
                {
                    this.addIntel(text, "Function", text, text);
                }
            }
        }

        private void addGlobalV()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalv.txt"))
            {
                this.addIntel(text, "Variable", text, text);
            }
        }

        private void addGlobalNS()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalns.txt"))
            {
                this.addIntel(text, "Class", text, text);
            }
        }

        private void addMath()
        {
            foreach (string text in File.ReadLines(this.defPath + "//classfunc.txt"))
            {
                this.addIntel(text, "Method", text, text);
            }
        }

        private void addBase()
        {
            foreach (string text in File.ReadLines(this.defPath + "//base.txt"))
            {
                this.addIntel(text, "Keyword", text, text);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            handle_files(listView1, currDir);
            WebClient wc = new WebClient();
            wc.Proxy = null;
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                bool flag2 = registryKey.GetValue(friendlyName) == null;
                if (flag2)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                registryKey = null;
                friendlyName = null;
            }
            catch (Exception)
            {
            }
            webBrowser1.Url = new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory()));
            await Task.Delay(500);
            webBrowser1.Document.InvokeScript("SetTheme", new string[]
            {
                   "Dark"
            });
            addBase();
            addMath();
            addGlobalNS();
            addGlobalV();
            addGlobalF();
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                 "--> luavim v2.1 @ ~/src/ <--"
            });
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (Settings.Default.select_on_click)
            {
                if (File.Exists(e.Item.Tag.ToString()))
                {
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                    {
                         File.ReadAllText(e.Item.Tag.ToString())
                    });
                }

                if (Directory.Exists(e.Item.Tag.ToString()))
                {
                    currDir = e.Item.Tag.ToString();
                    handle_files(listView1, currDir);
                }
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (Settings.Default.select_on_click)
            {
                if (File.Exists(e.Item.Tag.ToString()))
                {
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                    {
                         File.ReadAllText(e.Item.Tag.ToString())
                    });
                }

                if (Directory.Exists(e.Item.Tag.ToString()))
                {
                    currDir = e.Item.Tag.ToString();
                    handle_files(listView1, currDir);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Settings.Default.directoryImage != "nan")
            {
                if (File.Exists(Settings.Default.directoryImage) )
                {
                    panel1.BackgroundImage = Image.FromFile(Settings.Default.directoryImage);
                }
            }
            else
            {
                panel1.BackgroundImage = null;
            }

            if (Settings.Default.sidebarImage != "nan")
            {
                if (File.Exists(Settings.Default.sidebarImage) && listView1.BackgroundImage != Image.FromFile(Settings.Default.sidebarImage))
                {
                    listView1.BackgroundImage = Image.FromFile(Settings.Default.sidebarImage);
                    
                }
            }
            else
            {
                listView1.BackgroundImage = null;
            }
        }
    }
}
