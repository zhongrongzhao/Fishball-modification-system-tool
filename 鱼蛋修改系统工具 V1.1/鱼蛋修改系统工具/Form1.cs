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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();   //显示选择文件对话框 
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "bmp files (*.bmp)|*.bmp";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    PICPath = openFileDialog1.FileName;
                    pictureBox1.Image = Image.FromFile(PICPath);
                    string SubPath = "C:\\SystemIamge\\";
                    string CopyPath = "C:\\SystemIamge\\oemlogo.bmp";
                    if (false == Directory.Exists(SubPath))
                    {
                        Directory.CreateDirectory(SubPath);
                        File.Copy(PICPath, CopyPath);
                    }
                    else
                    {
                        File.Copy(PICPath, CopyPath,true);
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
    }
}
