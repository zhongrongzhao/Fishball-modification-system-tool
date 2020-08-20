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
using System.Threading;
using System.Management;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

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
        /// 修改处理器注册表
        /// </summary>
        /// <param name="Name"></param>
        private void ModifyCPU(string Name)
        {
            try
            {
                //计算机\HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0
                using (RegistryKey registryKey = Registry.LocalMachine)
                {
                    RegistryKey software = registryKey.OpenSubKey("HARDWARE", true);
                    RegistryKey Microsoft = software.OpenSubKey("DESCRIPTION", true);
                    RegistryKey Windows = Microsoft.OpenSubKey("System", true);
                    RegistryKey CurrentVersion = Windows.OpenSubKey("CentralProcessor", true);
                    RegistryKey OEMInformation = CurrentVersion.OpenSubKey("0", true);

                    OEMInformation.SetValue("ProcessorNameString", Name, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入失败原因：" + ex);
            }
        }

        /// <summary>
        /// 改制造商注册表
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
            catch (Exception ex)
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

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Text = null;
            txt_cpu.Text = null;

            object ManufacturerValue = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", "Manufacturer", null);
            object LogoValue = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\OEMInformation", "logo", null);

            if (ManufacturerValue != null && LogoValue == null)
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

            if (ManufacturerValue == null && LogoValue != null)
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

            if (LogoValue == null && ManufacturerValue == null && txt_cpu.Text == "")
            {
                MessageBox.Show("没有内容可清空", "提示");
            }

            if (LogoValue == null && ManufacturerValue == null && txt_cpu.Text != "")
            {

                MessageBox.Show("清除成功！", "提示");
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && pictureBox1.Image != null && txt_cpu.Text == "")
            {
                File.Copy(CopyBufferPath, CopyPath, true);
                ModifyRegistry();
                MessageBox.Show("修改成功！", "提示");
            }

            if (textBox1.Text == "" && pictureBox1.Image == null && txt_cpu.Text == "")
            {
                MessageBox.Show("请输入要修改的项目", "提示");
            }

            if (textBox1.Text != "" && pictureBox1.Image == null && txt_cpu.Text == "")
            {
                ModifyRegistry(textBox1.Text.ToString());
                MessageBox.Show("修改成功！", "提示");
            }

            if (textBox1.Text != "" && pictureBox1.Image != null && txt_cpu.Text == "")
            {
                File.Copy(CopyBufferPath, CopyPath, true);
                ModifyRegistry(textBox1.Text.ToString());
                MessageBox.Show("修改成功！", "提示");
            }

            //
            if (textBox1.Text == "" && pictureBox1.Image != null && txt_cpu.Text != "")
            {
                if (MessageBox.Show("修改“处理器”型号后将不会再恢复，确定修改“处理器”型号吗？", "鱼蛋提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    File.Copy(CopyBufferPath, CopyPath, true);
                    ModifyRegistry();
                    ModifyCPU(txt_cpu.Text);
                    MessageBox.Show("修改成功！", "提示");
                }
                else
                {
                    return;
                }
            }

            if (textBox1.Text != "" && pictureBox1.Image == null && txt_cpu.Text != "")
            {
                if (MessageBox.Show("修改“处理器”型号后将不会再恢复，确定修改“处理器”型号吗？", "鱼蛋提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ModifyRegistry(textBox1.Text.ToString());
                    ModifyCPU(txt_cpu.Text);
                    MessageBox.Show("修改成功！", "提示");
                }
                else
                {
                    return;
                }
            }

            if (textBox1.Text != "" && pictureBox1.Image != null && txt_cpu.Text != "")
            {
                if (MessageBox.Show("修改“处理器”型号后将不会再恢复，确定修改“处理器”型号吗？", "鱼蛋提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    File.Copy(CopyBufferPath, CopyPath, true);
                    ModifyRegistry(textBox1.Text.ToString());
                    ModifyCPU(txt_cpu.Text);
                    MessageBox.Show("修改成功！", "提示");
                }
                else
                {
                    return;
                }
            }

            if (textBox1.Text == "" && pictureBox1.Image == null && txt_cpu.Text != "")
            {
                if (MessageBox.Show("修改“处理器”型号后将不会再恢复，确定修改“处理器”型号吗？", "鱼蛋提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    ModifyCPU(txt_cpu.Text);
                    MessageBox.Show("修改成功！", "提示");
                }
                else
                {
                    return;
                }
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
            string name = "你好" + System.Environment.UserName + "，鱼蛋提示";
            //Application.Exit();
            Application.OpenForms["Form1"].Hide();//隐藏指定窗体
            notifyIcon1.ShowBalloonTip(500, name, "鱼蛋修改系统工具缩小成开始栏的图标", ToolTipIcon.None);
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            this.cpuChart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            this.cpuChart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            this.cpuChart.Legends.Clear();
            cpuThread = new Thread(new ThreadStart(this.getPerformanceCounters));
            cpuThread.IsBackground = true;
            cpuThread.Start();
            this.chart1.Legends.Clear();
            Thread thread = new Thread(() =>
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    sys_info();
                    ClearMemory();
                    get_men();

                    Get_Disk();
                }));
            }
            );
            thread.Start();

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

        /// <summary>
        /// 获取电脑所有盘符
        /// </summary>
        private void Get_Disk()
        {
            comboBox1.Items.Clear();
            System.IO.DriveInfo[] disk = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo di in disk)
            {
                comboBox1.Items.Add(di.Name);
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

        private void 作者介绍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Auhor auhor = new Auhor();
            auhor.Show();
        }

        private void 使用教程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tutorial tutorial = new Tutorial();
            tutorial.Show();
        }

        private void 二维码支付ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://zhongrongzhao.github.io/support.html");
        }

        private void one_Click(object sender, EventArgs e)
        {
            one_P.Visible = true;
            panel_one.Visible = true;

            two_P.Visible = false;
            panel_two.Visible = false;

            three_P.Visible = false;
            panel_three.Visible = false;

            four_P.Visible = false;
            panel_four.Visible = false;

            five_p.Visible = false;
            panel_five.Visible = false;

            six_p.Visible = false;
            panel_six.Visible = false;
        }

        private void two_Click(object sender, EventArgs e)
        {
            one_P.Visible = false;
            panel_one.Visible = false;

            two_P.Visible = true;
            panel_two.Visible = true;

            three_P.Visible = false;
            panel_three.Visible = false;

            four_P.Visible = false;
            panel_four.Visible = false;

            five_p.Visible = false;
            panel_five.Visible = false;

            six_p.Visible = false;
            panel_six.Visible = false;
        }

        private void three_Click(object sender, EventArgs e)
        {
            one_P.Visible = false;
            panel_one.Visible = false;

            two_P.Visible = false;
            panel_two.Visible = false;

            three_P.Visible = true;
            panel_three.Visible = true;

            four_P.Visible = false;
            panel_four.Visible = false;

            five_p.Visible = false;
            panel_five.Visible = false;

            six_p.Visible = false;
            panel_six.Visible = false;
        }
        private void four_Click(object sender, EventArgs e)
        {
            one_P.Visible = false;
            panel_one.Visible = false;

            two_P.Visible = false;
            panel_two.Visible = false;

            three_P.Visible = false;
            panel_three.Visible = false;

            four_P.Visible = true;
            panel_four.Visible = true;

            five_p.Visible = false;
            panel_five.Visible = false;

            six_p.Visible = false;
            panel_six.Visible = false;

            Thread thread = new Thread(() =>
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    get_men();
                }));
            });
            thread.Start();
        }

        private void five_Click(object sender, EventArgs e)
        {
            lab_check.Visible = false;

            one_P.Visible = false;
            panel_one.Visible = false;

            two_P.Visible = false;
            panel_two.Visible = false;

            three_P.Visible = false;
            panel_three.Visible = false;

            four_P.Visible = false;
            panel_four.Visible = false;

            five_p.Visible = true;
            panel_five.Visible = true;

            six_p.Visible = false;
            panel_six.Visible = false;
        }

        private void six_Click(object sender, EventArgs e)
        {
            one_P.Visible = false;
            panel_one.Visible = false;

            two_P.Visible = false;
            panel_two.Visible = false;

            three_P.Visible = false;
            panel_three.Visible = false;

            four_P.Visible = false;
            panel_four.Visible = false;

            five_p.Visible = false;
            panel_five.Visible = false;

            six_p.Visible = true;
            panel_six.Visible = true;


        }

        public enum WMIPath
        {
            // 硬件
            Win32_Processor,     // CPU 处理器
            Win32_PhysicalMemory,  // 物理内存条
            Win32_Keyboard,     // 键盘
            Win32_PointingDevice,  // 点输入设备，包括鼠标。
            Win32_FloppyDrive,    // 软盘驱动器
            Win32_DiskDrive,     // 硬盘驱动器
            Win32_CDROMDrive,    // 光盘驱动器
            Win32_BaseBoard,     // 主板
            Win32_BIOS,       // BIOS 芯片
            Win32_ParallelPort,   // 并口
            Win32_SerialPort,    // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice,    // 多媒体设置，一般指声卡。
            Win32_SystemSlot,    // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController,   // USB 控制器
            Win32_NetworkAdapter,  // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer,      // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob,     // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem,     // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor,  // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings,  // 显卡支持的显示模式。
                                  // 操作系统
            Win32_TimeZone,     // 时区
            Win32_SystemDriver,   // 驱动程序
            Win32_DiskPartition,  // 磁盘分区
            Win32_LogicalDisk,   // 逻辑磁盘
            Win32_LogicalDiskToPartition,   // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile,     // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem,  // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand,  // 系统自动启动程序
            Win32_Service,     // 系统安装的服务
            Win32_Group,      // 系统管理组
            Win32_GroupUser,    // 系统组帐号
            Win32_UserAccount,   // 用户帐号
            Win32_Process,     // 系统进程
            Win32_Thread,      // 系统线程
            Win32_Share,      // 共享
            Win32_NetworkClient,  // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
        }

        public void sys_info()
        {
            try
            {
                using (ManagementClass mc = new ManagementClass("win32_processor"))
                {
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        txt_number.Text = mo["processorid"].ToString();   // CPU编号
                    }
                    using (ManagementObjectSearcher driverID = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                    {
                        foreach (ManagementObject cpu in driverID.Get())
                        {
                            txt_Manu.Text = cpu["Manufacturer"].ToString();
                            txt_Ver.Text = cpu["Name"].ToString();
                        }
                    }
                }

                SelectQuery query = new SelectQuery("SELECT * FROM Win32_BaseBoard");
                ManagementObjectSearcher driveid = new ManagementObjectSearcher(query);
                ManagementObjectCollection.ManagementObjectEnumerator data = driveid.Get().GetEnumerator();
                data.MoveNext();
                ManagementBaseObject board = data.Current;
                txt_Num.Text = board.GetPropertyValue("SerialNumber").ToString();
                txt_manuf.Text = board.GetPropertyValue("ManuFacturer").ToString();
                txt_produ.Text = board.GetPropertyValue("Product").ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "提示");
                return;
            }
        }

        private Thread cpuThread;
        private double[] cpuArray = new double[80];

        private void getPerformanceCounters()
        {
            var cpuPerfCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            while (true)
            {
                cpuArray[cpuArray.Length - 1] = Math.Round(cpuPerfCounter.NextValue(), 0);
                Array.Copy(cpuArray, 1, cpuArray, 0, cpuArray.Length - 1);

                if (cpuChart.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { UpdateCpuChart(); });
                }
                else
                {
                    //......
                }

                Thread.Sleep(100);

            }

        }

        private void UpdateCpuChart()
        {
            cpuChart.Series["CPU使用率"].Points.Clear();

            for (int i = 0; i < cpuArray.Length - 1; ++i)
            {
                cpuChart.Series["CPU使用率"].Points.AddY(cpuArray[i]);
            }
        }


        private void Btn_Command_one_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "slmgr.vbs -dlv";
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                    process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                    process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                    process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
                    //process.StartInfo.UserName = "Administrator";
                    //System.Security.SecureString password = new System.Security.SecureString();
                    //password.AppendChar('1');
                    //password.AppendChar('2');
                    //password.AppendChar('3');
                    //process.StartInfo.Password = password;
                    process.Start();
                    process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.AutoFlush = true;

                    string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                    process.WaitForExit();         //等待程序执行完退出进程
                    process.Close();
                    process.Dispose();

                    //MessageBox.Show("command命令：" + output);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Btn_Command_two_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "slmgr.vbs -dli";
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                    process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                    process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                    process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出

                    process.Start();
                    process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.AutoFlush = true;

                    string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                    process.WaitForExit();         //等待程序执行完退出进程
                    process.Close();
                    process.Dispose();

                    //MessageBox.Show("command命令：" + output);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Btn_Command_three_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "slmgr.vbs -xpr";
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                    process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                    process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                    process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出

                    process.Start();
                    process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.AutoFlush = true;

                    string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                    process.WaitForExit();         //等待程序执行完退出进程
                    process.Close();
                    process.Dispose();

                    //MessageBox.Show("command命令：" + output);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Btn_Command_four_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    string command = "slmgr /skms kms.03k.org";
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                        process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                        process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                        process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                        process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
                        process.Start();
                        process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                        process.StandardInput.AutoFlush = true;

                        string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                        process.WaitForExit();         //等待程序执行完退出进程
                        process.Close();
                        process.Dispose();

                        // MessageBox.Show("command命令：" + output);
                    }
                }

                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }

                //陈氏计时法
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 3000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait);
                timer.AutoReset = false;
                timer.Enabled = true;
                timer.Start();
            });
            //thread.Start();

            if (MessageBox.Show("已激活的用户请注意，激活后会覆盖当前激活，确定激活win10吗？", "鱼蛋提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                thread.Start();
            }
            else
            {
                return;
            }
        }

        private void command_wait(object sender, EventArgs e)
        {
            try
            {
                string command = "slmgr /ato";
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                    process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                    process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                    process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出
                    process.Start();
                    process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                    process.StandardInput.AutoFlush = true;

                    string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                    process.WaitForExit();         //等待程序执行完退出进程
                    process.Close();
                    process.Dispose();

                    // MessageBox.Show("command命令：" + output);
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void 更新内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Updata updata = new Updata();
            updata.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 打开主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 赞助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://zhongrongzhao.github.io/support.html");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.chart1.Legends.Clear();
            Thread thread1 = new Thread(() =>
            {
                ClearMemory();
            });
            thread1.Start();

            Thread thread = new Thread(() =>
            {
                for (int a = 1; a <= 10; a++)
                {
                    this.Invoke((MethodInvoker)delegate { get_men(); });
                    Thread.Sleep(10);
                }
            });
            thread.Start();
        }

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                //对于系统进程会拒绝访问，导致出错，此处对异常不进行处理。
                try
                {
                    EmptyWorkingSet(process.Handle);
                }
                catch
                {
                }
            }
        }

        private void get_men()
        {
            //需要添加 System.Management 的引用
            //获取总物理内存大小
            ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            double available = 0, capacity = 0;
            foreach (ManagementObject mo1 in moc1)
            {
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
            }
            moc1.Dispose();
            cimobject1.Dispose();

            //获取内存可用大小
            ManagementClass cimobject2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            foreach (ManagementObject mo2 in moc2)
            {
                available += ((Math.Round(Int64.Parse(mo2.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1)));

            }
            moc2.Dispose();
            cimobject2.Dispose();

            //Console.WriteLine("总内存=" + capacity.ToString() + "G");
            //Console.WriteLine("可使用=" + available.ToString() + "G");
            //Console.WriteLine("已使用=" + ((capacity - available)).ToString() + "G," + (Math.Round((capacity - available) / capacity * 100, 0)).ToString() + "%");
            //Console.ReadKey();

            double useG = capacity - available;
            double useP = (Math.Round((capacity - available) / capacity * 100, 0));
            txt_AllMenory.Text = "总内存容量：" + capacity.ToString() + "G";
            //textBox2.Text = useG.ToString() + "G," + useP.ToString() + "%";

            string A = "已用" + useG.ToString() + "G";
            string B = "剩余" + available.ToString() + "G";

            List<string> xData = new List<string>() { A, B };
            List<double> yData = new List<double>() { useG, available };
            chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
            chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
            chart1.Series[0].Points.DataBindXY(xData, yData);
            return;
        }

        [DllImportAttribute("shell32.dll")]
        private static extern int SHEmptyRecycleBin(IntPtr handle, string root, int falgs);
        const int SHERB_NOCONFIRMATION = 0x000001;
        const int SHERB_NOPROGRESSUI = 0x000002;
        const int SHERB_NOSOUND = 0x000004;

        private void btn_clear_Click(object sender, EventArgs e)
        {
            ClearMemory();
            SHEmptyRecycleBin(this.Handle, "", SHERB_NOCONFIRMATION + SHERB_NOPROGRESSUI + SHERB_NOSOUND);
            lab_check.Visible = false;
            pic_clean.Visible = false;
            lab_clean_gif.Visible = true;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10600;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait2);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        private void command_wait2(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lab_clean_gif.Visible = false;
                pic_clean.Visible = true;
                lab_check.Visible = true;
            }));
        }

        public bool DeleteDir(string strPath)
        {
            try
            {
                // 清除空格
                strPath = @strPath.Trim().ToString();
                // 判断文件夹是否存在
                if (Directory.Exists(strPath))
                {
                    // 获得文件夹数组
                    string[] strDirs = Directory.GetDirectories(strPath);
                    // 获得文件数组
                    string[] strFiles = Directory.GetFiles(strPath);
                    // 遍历所有子文件夹
                    foreach (string strFile in strFiles)
                    {
                        // 删除文件夹
                        File.Delete(strFile);
                    }
                    // 遍历所有文件
                    foreach (string strdir in strDirs)
                    {
                        // 删除文件
                        Directory.Delete(strdir, true);
                    }
                }
                // 成功
                return true;
            }
            catch (Exception Exp) // 异常处理
            {
                // 异常信息
                Debug.Write(Exp.Message.ToString());
                // 失败
                return false;
            }
        }

        private void btn_refl_Click(object sender, EventArgs e)
        {
            Get_Disk();
        }

        private void btn_virus_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string u_path = comboBox1.SelectedItem.ToString() + "autorun.inf";
                FileInfo fileInfo = new FileInfo(u_path);
                if (!fileInfo.Exists)
                {
                    btn_fix_u.Enabled = false;
                    btn_u_check.Enabled = false;
                    btn_virus.Enabled = true;
                    btn_virus_Clean.Enabled = false;
                    Thread thread1 = new Thread(() =>
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            pic_U_Dick.Visible = false;
                            pic_load_U_Disk.Visible = true;
                            label15.Visible = false;
                        }));
                        //陈氏计时法
                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Interval = 10000;
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait3);
                        timer.AutoReset = false;
                        timer.Enabled = true;
                        timer.Start();
                    });

                    Thread thread2 = new Thread(() =>
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            File.Create(u_path).Dispose();
                            File.SetAttributes(u_path, FileAttributes.ReadOnly | FileAttributes.Hidden);//设置成只读且隐藏文件夹 
                        }));
                    });
                    thread1.Start();
                    thread2.Start();
                }
                else
                {
                    MessageBox.Show("已经有防病毒功能，不需要再设置", "提示");
                }
            }

            else
            {
                MessageBox.Show("请选择U盘位置", "提示");
            }
        }

        private void command_wait3(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                pic_U_Dick.Visible = true;
                pic_load_U_Disk.Visible = false;
                label15.Visible = true;

                btn_fix_u.Enabled = true;
                btn_u_check.Enabled = true;
                btn_virus.Enabled = true;
                btn_virus_Clean.Enabled = true;
            }));
        }

        private void btn_fix_u_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                btn_fix_u.Enabled = true;
                btn_u_check.Enabled = false;
                btn_virus.Enabled = false;
                btn_virus_Clean.Enabled = false;
                Thread thread1 = new Thread(() =>
                {
                    this.Invoke(new MethodInvoker(delegate
                     {
                         pic_U_Dick.Visible = false;
                         pic_load_U_Disk.Visible = true;
                         label15.Visible = false;
                     }));
                    //陈氏计时法
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 10000;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait3);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Start();
                });

                Thread thread2 = new Thread(() =>
                {
                    try
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            string u_path = comboBox1.SelectedItem.ToString();
                            string command = "chkdsk " + u_path.Substring(0, u_path.Length - 1) + "/" + u_path.Substring(0, u_path.Length - 2);
                            //textBox2.Text = command;

                         using (Process process = new Process())
                         {
                             process.StartInfo.FileName = "cmd.exe";
                             process.StartInfo.UseShellExecute = false;     //是否使用操作系统shell启动
                             process.StartInfo.CreateNoWindow = true;        //不显示程序窗口
                             process.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                             process.StartInfo.RedirectStandardInput = true;  //接受来自调用程序的输入信息
                             process.StartInfo.RedirectStandardError = true;  //重定向标准错误输出

                             process.Start();
                             process.StandardInput.WriteLine(command + "&exit");   //向cmd窗口发送输入信息，&exit意思为不论command命令执行成功与否，接下来都执行exit这句
                             process.StandardInput.AutoFlush = true;

                             string output = process.StandardOutput.ReadToEnd();  //获取cmd的输出信息
                             process.WaitForExit();         //等待程序执行完退出进程
                             process.Close();
                             process.Dispose();

                         }
                        }));
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.Message);
                    }
                });
                thread1.Start();
                thread2.Start();
            }
            else
            {
                MessageBox.Show("请选择U盘位置", "提示");
            }
        }

        private void btn_u_check_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                pic_ok1.Visible = false;
                pic_no1.Visible = false;
                pic_ok2.Visible = false;
                pic_no2.Visible = false;
                pic_ok3.Visible = false;
                pic_no3.Visible = false;

                label16.Visible = true;
                label17.Visible = true;
                label18.Visible = true;

                pic_load2.Visible = true;
                pic_load3.Visible = true;

                btn_fix_u.Enabled = false;
                btn_u_check.Enabled = true;
                btn_virus.Enabled = false;
                btn_virus_Clean.Enabled = false;
                Thread thread1 = new Thread(() =>
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        pic_U_Dick.Visible = false;
                        pic_load_U_Disk.Visible = true;
                        label15.Visible = false;
                    }));
                    //陈氏计时法
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 10000;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait3);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Start();
                });

                Thread thread2 = new Thread(() =>
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            File.Create(comboBox1.SelectedItem.ToString() + "1.txt").Dispose();
                            File.Delete(comboBox1.SelectedItem.ToString() + "1.txt");
                            pic_ok1.Visible = true;
                            pic_no1.Visible = false;
                        }
                        catch
                        {
                            pic_load2.Visible = false;
                            pic_load3.Visible = false;
                            pic_ok1.Visible = false;
                            pic_no1.Visible = true;
                            pic_ok2.Visible = false;
                            pic_no2.Visible = true;
                            pic_ok3.Visible = false;
                            pic_no3.Visible = true;
                            //label17.Visible = true;
                            return;
                        }

                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Interval = 5000;
                        timer.Elapsed += new System.Timers.ElapsedEventHandler(U_Check_V);
                        timer.AutoReset = false;
                        timer.Enabled = true;
                        timer.Start();
                    }));
                });

                thread1.Start();
                thread2.Start();
            }

            else
            {
                MessageBox.Show("请选择U盘位置", "提示");
            }
        }


        private void U_Check_V(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                string u_path = comboBox1.SelectedItem.ToString() + "autorun.inf";
                FileInfo fileInfo = new FileInfo(u_path);
                if (!fileInfo.Exists)
                {
                    pic_load2.Visible = false;
                    pic_ok2.Visible = false;
                    pic_no2.Visible = true;
                    label17.Visible = true;

                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 4000;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(U_Check_V_H);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Start();
                }
                else
                {
                    pic_load2.Visible = false;
                    pic_ok2.Visible = true;
                    pic_no2.Visible = false;
                    label17.Visible = true;

                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 4000;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(U_Check_V_H);
                    timer.AutoReset = false;
                    timer.Enabled = true;
                    timer.Start();
                }
         }));
        }

        private void U_Check_V_H(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                string u_path = comboBox1.SelectedItem.ToString() + "autorun.inf";
                FileInfo f = new FileInfo(u_path);
                if (f.Exists)
                {
                    if (f.Length > 0)
                    {
                        pic_load3.Visible = false;
                        pic_no3.Visible = true;
                    }
                    else
                    {
                        pic_load3.Visible = false;
                        pic_ok3.Visible = true;
                    }
                }
                else
                {
                    pic_load3.Visible = false;
                    pic_ok3.Visible = true;
                }
            }));
        }

        private void btn_virus_Clean_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string u_path = comboBox1.SelectedItem.ToString() + "autorun.inf";
                FileInfo f = new FileInfo(u_path);
                if (f.Exists)
                {
                    if (f.Length > 0)
                    {
                        File.Delete(u_path);

                        btn_fix_u.Enabled = false;
                        btn_u_check.Enabled = false;
                        btn_virus.Enabled = false;
                        btn_virus_Clean.Enabled = true;
                        pic_U_Dick.Visible = false;
                        pic_load_U_Disk.Visible = true;
                        label15.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("并没发现有病毒", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("并没发现有病毒", "提示");
                }
                //陈氏计时法
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(command_wait3);
                timer.AutoReset = false;
                timer.Enabled = true;
                timer.Start();
            }
            else
            {
                MessageBox.Show("请选择U盘位置", "提示");
            }
        }

    }
}
