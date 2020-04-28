using Microsoft.Win32;
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

using System.Runtime.CompilerServices;
using System.Net.Http.Headers;

namespace 鱼蛋修改系统工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string PICPath;
        string SubBufferPath = "C:\\SystemBuffer\\";
        string SubPath = "C:\\SystemIamge\\";
        string CopyBufferPath = "C:\\SystemBuffer\\oemlogo.bmp";
        string CopyPath = "C:\\SystemIamge\\oemlogo.bmp";
        private Point mPoint;

        /// <summary>
        /// 改注册表
        /// </summary>
        private void ModifyRegistry()
        {
            try
            {
                if (pictureBox1.Image != null)
                {
                    using (RegistryKey registryKey = Registry.LocalMachine)
                    {
                        RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                        RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                        RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                        RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                        RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                        OEMInformation.SetValue("logo", "C:\\SystemIamge\\oemlogo.bmp", RegistryValueKind.String);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入失败原因：" + ex);
            }
        }

        /// <summary>
        /// 改注册表
        /// </summary>
        /// <param name="Name">供应商名</param>
        private void ModifyRegistry(string Name)
        {
            try
            {
                if (pictureBox1.Image != null)
                {
                    using (RegistryKey registryKey = Registry.LocalMachine)
                    {
                        RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                        RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                        RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                        RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                        RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                        OEMInformation.SetValue("logo", "C:\\SystemIamge\\oemlogo.bmp", RegistryValueKind.String);
                    }
                }

                if (textBox1.Text != null)
                {
                    using (RegistryKey registryKey = Registry.LocalMachine)
                    {
                        RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                        RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                        RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                        RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                        RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                        OEMInformation.SetValue("Manufacturer", Name, RegistryValueKind.String);
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("写入失败原因：" + ex);
            }
        }
        int a = 1;
        private void pictureBox1_Click(object sender, EventArgs e)
        {       
            try
            {
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())  //显示选择文件对话框 
                {
                    openFileDialog1.InitialDirectory = "c:\\";
                    openFileDialog1.Filter = "*.jpg,*.png,*.jpeg,*.bmp|*.jgp;*.png;*.jpeg;*.bmp";//"bmp files (*.bmp)|*.bmp|All file(*.*)|*.*";
                    openFileDialog1.FilterIndex = 2;
                    openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        PICPath = openFileDialog1.FileName;


                        Transform.MakeSquareImage(PICPath, CopyBufferPath, 120);

                        string BufferPathForView = "C:\\SystemBuffer\\oemlogo" + a + ".bmp";
                        File.Copy(CopyBufferPath, BufferPathForView);
                        pictureBox1.Image = Image.FromFile(BufferPathForView);
                        a++;

                        //pictureBox1.Image = Image.FromFile(PICPath);
                        if (false == Directory.Exists(SubPath))
                        {
                            Directory.CreateDirectory(SubPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("插入图片失败，错误原因为：" + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = null;
            object ManufacturerValue = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", "Manufacturer", null);
            object LogoValue = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", "logo", null);
            
            if(ManufacturerValue != null && LogoValue == null)
            {
                using (RegistryKey registryKey = Registry.LocalMachine)
                {
                    RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                    RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                    RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                    RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                    RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                    OEMInformation.DeleteValue("Manufacturer", true);
                    MessageBox.Show("清除成功！", "提示");
                }
            }

            if(ManufacturerValue == null && LogoValue != null)
            {
                using (RegistryKey registryKey = Registry.LocalMachine)
                {
                    RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                    RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                    RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                    RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                    RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                    OEMInformation.DeleteValue("logo", true);
                    MessageBox.Show("清除成功！", "提示");
                }
            }

            if (ManufacturerValue != null && LogoValue != null)
            {
                using (RegistryKey registryKey = Registry.LocalMachine)
                {
                    RegistryKey software = registryKey.OpenSubKey("SOFTWARE", true);
                    RegistryKey Microsoft = software.OpenSubKey("Microsoft", true);
                    RegistryKey Windows = Microsoft.OpenSubKey("Windows", true);
                    RegistryKey CurrentVersion = Windows.OpenSubKey("CurrentVersion", true);
                    RegistryKey OEMInformation = CurrentVersion.OpenSubKey("OEMInformation", true);
                    OEMInformation.DeleteValue("Manufacturer", true);
                    OEMInformation.DeleteValue("logo", true);
                    MessageBox.Show("清除成功！", "提示");
                }
            }

            if (LogoValue == null && ManufacturerValue==null)
            {
                MessageBox.Show("没有内容可清空","提示");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text =="" && pictureBox1.Image != null)
            {
                File.Copy(CopyBufferPath, CopyPath,true);
                ModifyRegistry();
                MessageBox.Show("修改成功！", "提示");
            }

            if(textBox1.Text =="" && pictureBox1.Image == null)
            {
                MessageBox.Show("请输入要修改的项目","提示");
            }

            if(textBox1.Text != "" && pictureBox1.Image == null)
            {
                ModifyRegistry(textBox1.Text.ToString());
                MessageBox.Show("修改成功！", "提示");
            }       

            if(textBox1.Text != "" && pictureBox1.Image != null)
            {
                File.Copy(CopyBufferPath, CopyPath,true);
                ModifyRegistry(textBox1.Text.ToString());
                MessageBox.Show("修改成功！", "提示");
            }
        }

        /// <summary>
        /// 打开赞助页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://zhongrongzhao.github.io/support.html");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (false == Directory.Exists(SubBufferPath))
            {
                Directory.CreateDirectory(SubBufferPath);
                
            }    
            else
            {
                for (int a = 1; a < 1000; a++)
                {
                    if (File.Exists("C:\\SystemBuffer\\oemlogo" + a + ".bmp"))
                    {
                        File.Delete("C:\\SystemBuffer\\oemlogo" + a + ".bmp");
                    }
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }
    }
}
