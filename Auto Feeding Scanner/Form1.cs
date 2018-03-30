using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Threading;
using System.Collections;

namespace Auto_Feeding_Scanner
{
    public partial class Form1 : Form
    {
        private SerialPort Port = new SerialPort();
        private System.Timers.Timer timer = new System.Timers.Timer(1000);
        static int maxImg = 0;

        static int MAXBUFF = 524288; //512kb

        private List<byte> buffer = new List<byte>(MAXBUFF);
        private byte[] camdata = new byte[MAXBUFF];
        static bool checke = false;
        static bool is_header = false;
        static bool is_camdone = false;
        static bool is_sigleMode = true;
        static bool is_BMP = false;
        static bool is_running = true;
        String path;

        string[] resolution_2640 = { "160x120", "176x144", "320x240", "352x288", "640x480", "800x600", "1024x768", "1280x1024", "1600x1200" };
        string[] resolution_5642 = { "320x240", "640x480", "1280x720", "1920x1080", "2048x1563", "2592x1944" };

        int FirstTime = 0;
        int count = 0;
        int datas = 0;
        int length = 0;

        bool is_2640 = false;
        bool is_5642 = false;

        public delegate void UpdateTxtEventHandler(string msg);
        public UpdateTxtEventHandler updateTxt;
        public UpdateTxtEventHandler deleteTxt;

        public delegate void UpdateDatEventHandler(string msg);
        public UpdateDatEventHandler updateDat;

        public delegate void UpdateResolutionEventHandler(string[] msg);
        public UpdateResolutionEventHandler updateResolution;

        public delegate void UpdateFPSEventHandler(string msg);
        public UpdateFPSEventHandler updateFPS;

        Thread handledata;
        Thread datareceived;
       


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Get all existing ports
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            //Add port name to combobox
            comPort.Items.AddRange(ports);
            comPort.SelectedIndex = (comPort.Items.Count > 0) ? 0 : -1;        

            cBoxResolution.Enabled = false;
            
            cBoxResolution.SelectedIndex = cBoxResolution.Items.IndexOf("320x240");

            startScan.Enabled = Port.IsOpen;

            //Set com port parameters
            Port.NewLine = "\r\n";
            Port.ReadTimeout = 500;
            Port.WriteTimeout = 500;
            //Add com port data receive event

            textPath.Text = Application.StartupPath;
            path = textPath.Text;

            handledata = new Thread(new ThreadStart(handleDatas));
            handledata.IsBackground = true;
            handledata.Start();

            datareceived = new Thread(new ThreadStart(DataReceived));
            datareceived.IsBackground = true;
            datareceived.Start();
           
            //takeIamges.Start();

            updateTxt = new UpdateTxtEventHandler(UpdateTxtMethod);
            deleteTxt = new UpdateTxtEventHandler(DeleteTxtMethod);
            updateDat = new UpdateDatEventHandler(UpdateDatMethod);
            updateResolution = new UpdateResolutionEventHandler(UpdateResolutionMethod);
            updateFPS = new UpdateFPSEventHandler(UpdateFPSMethod);

        }
        public void UpdateTxtMethod(string msg)
        {
           
            //MessageBox.Show(msg);
            
            DebugRichTextBox.AppendText(msg);
            DebugRichTextBox.ScrollToCaret();
        }

        public void UpdateDatMethod(string msg)
        {
            //labBytes.Text = msg;
        }

        public void UpdateFPSMethod(string msg)
        {
            //labfps.Text = msg;
        }

        public void DeleteTxtMethod(string msg)
        {
            DebugRichTextBox.Text = msg;
            DebugRichTextBox.ScrollToCaret();
        }

        public void UpdateResolutionMethod(string[] msg)
        {
            int i = cBoxResolution.Items.Count - 1;
            for (; i >= 0; i--)
            {
                cBoxResolution.Items.RemoveAt(i);
            }
            cBoxResolution.Items.AddRange(msg);
            cBoxResolution.SelectedIndex = cBoxResolution.Items.IndexOf("320x240");
        }


        

        public void handleDatas()
        {
            while (true)
            {
                if (is_running == true)
                {
                    if (is_BMP == false)//JPEG MODE
                    {
                        while ((buffer.Count > 2))
                        {

                            if (count == 4)
                            {
                                float x = Environment.TickCount - FirstTime;
                                string fps = (4.0 / (x / 1000.0)).ToString("0.0");
                                this.BeginInvoke(updateFPS, fps);
                                count = 0;
                                FirstTime = Environment.TickCount;
                            }

                            if ((is_header == false) && (buffer[0] == 0xFF) && (buffer[1] == 0xD8))
                            {

                                is_header = true;
                               
                                break;
                            }
                            else if ((is_header == true) && (is_camdone == false))//(is_header == true) && (buffer.LastIndexOf(0xD9) != -1) && (buffer[buffer.LastIndexOf(0xD9) - 1] == 0xFF) && (buffer[buffer.LastIndexOf(0xD9)] == 0xD9))
                            {
                                int position = 0;
                                while (((buffer[position] != 0xFF) || (buffer[position + 1] != 0xD9)))
                                {
                                    position += 1;
                                    if ((position + 2) >= buffer.Count)
                                    {
                                        break;
                                    }
                                }
                                if ((buffer[position] == 0xFF) && (buffer[position + 1] == 0xD9))
                                {
                                    is_camdone = true;
                                    length = position + 2;
                                    Array.Copy(buffer.ToArray(), camdata, position + 2);
                                    buffer.RemoveRange(0, position + 2);
                                }
                                break;
                                //is_camdone = true;

                                //int i = buffer.LastIndexOf(0xD9) + 1;
                                //int m = buffer.Count - buffer.LastIndexOf(0xD9) - 1;
                                //buffer.RemoveRange(i, m);
                            }
                            else if (is_header == false)
                            {
                                byte[] temp = new byte[5];
                                //if(buffer[0]==0x00)
                                //    buffer.RemoveAt(0);
                                temp[0] = buffer[0];
                                if (buffer[0] == 0x0D)
                                    buffer.RemoveAt(0);
                                if ((buffer[0] == 0x4F) && (buffer[1] == 0x56) && (is_2640 == false) && (is_5642 == false) && (buffer.Count > 5))
                                {

                                    if ((buffer[2] == 0x32) && (buffer[3] == 0x36) && (buffer[4] == 0x34) && (buffer[5] == 0x30))
                                    {
                                        is_2640 = true;
                                        //cBoxResolution.Items.AddRange(resolution_2640);
                                        Invoke(updateResolution, new object[] { resolution_2640 });
                                        //BeginInvoke(new UpdateResolutionEventHandler(UpdateResolutionMethod), resolution_2640);
                                        //labCAM.Text = "OV2640 connected";
                                    }
                                    else if ((buffer[2] == 0x35) && (buffer[3] == 0x36) && (buffer[4] == 0x34) && (buffer[5] == 0x32))
                                    {
                                        is_5642 = true;
                                        Invoke(updateResolution, new object[] { resolution_5642 });
                                        //BeginInvoke(new UpdateResolutionEventHandler(UpdateResolutionMethod),resolution_5642);
                                        //labCAM.Text = "OV5642 connected";
                                    }
                                }
                                //this.DebugRichTextBox.AppendText(System.Text.Encoding.ASCII.GetString(temp, 0, 1));
                                BeginInvoke(updateTxt, System.Text.Encoding.ASCII.GetString(temp, 0, 1));
                                buffer.RemoveAt(0);
                            }
                            else
                                break;
                        }
                        if (is_camdone == true)
                        {

                            BytesToFile(camdata);
                            DispPictureBox1.Image = BytesToBitmap(camdata);
                            if (is_sigleMode != true)
                                count += 1;

                            

                                
                            
                            Array.Clear(camdata, 0, camdata.Length);
                            //Clean the buffer
                            is_header = false;
                            is_camdone = false;
                            //buffer.RemoveRange(0, buffer.Count);
                        }
                    }
                    else //********BMP Mode***********/
                    {
                        while ((buffer.Count > 2))
                        {
                            if ((is_header == false) && (buffer[0] == 0xFF) && (buffer[1] == 0xAA))
                            {
                                is_header = true;
                                
                            }
                            else if ((is_header == true) && (is_camdone == false))
                            {
                                int position = 0;
                                while (((buffer[position] != 0xBB) || (buffer[position + 1] != 0xCC)))
                                {
                                    position += 1;
                                    if ((position + 2) >= buffer.Count)
                                    {
                                        break;
                                    }
                                }
                                if ((buffer[position] == 0xBB) && (buffer[position + 1] == 0xCC))
                                {
                                    is_camdone = true;
                                    length = position - 2;
                                    if (length < 153664)
                                    {
                                        is_camdone = false;
                                        buffer.RemoveRange(0, buffer.Count);
                                        break;
                                    }
                                    Array.Copy(buffer.ToArray(), 2, camdata, 0, length);
                                    buffer.RemoveRange(0, position + 2);
                                }
                                break;
                            }
                            else if (is_header == false)
                            {
                                byte[] temp = new byte[5];
                                //if(buffer[0]==0x00)
                                //    buffer.RemoveAt(0);
                                temp[0] = buffer[0];
                                if (buffer[0] == 0x0D)
                                    buffer.RemoveAt(0);
                                BeginInvoke(updateTxt, System.Text.Encoding.ASCII.GetString(temp, 0, 1));
                                buffer.RemoveAt(0);
                            }
                            else
                                break;
                        }
                        if (is_camdone == true)
                        {
                            BytesToFile(camdata);
                            DispPictureBox1.Image = BytesToBitmap(camdata);                          

                             BytesToFile(camdata);
                            Array.Clear(camdata, 0, camdata.Length);
                            is_header = false;
                            is_camdone = false;
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }


        private void BytesToFile(byte[] bu)
        {
            byte[] image_data = new byte[length];
            string savepath;
            path = textPath.Text;
            String cam = "OV5642";
                     
            savepath = path + "\\" + cam + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";

            FileStream fs = File.Create(savepath);
            Array.Copy(bu, image_data, length);
            fs.Write(image_data, 0, image_data.Length);
            fs.Close();

        }
        private Image BytesToBitmap(byte[] bu)
        {
            MemoryStream stream = null;
            byte[] image_data = new byte[length];
            Bitmap resize_img = new Bitmap(DispPictureBox1.Width, DispPictureBox1.Height);
            Graphics graphic = Graphics.FromImage(resize_img);

            //image_data = buffer.ToArray();
            Array.Copy(bu, image_data, length);

            try
            {
                stream = new MemoryStream(image_data);

                Bitmap result = new Bitmap(stream);

                graphic.InterpolationMode = InterpolationMode.High;

                graphic.DrawImage(result, new Rectangle(0, 0, DispPictureBox1.Width, DispPictureBox1.Height));
                graphic.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stream.Close();
            }
            return new Bitmap((Image)resize_img);

        }

        private void DataReceived()
        {
            while (true)
            {
                if (is_running == true)
                {
                    try
                    {
                        if ((Port.IsOpen == true) && (Port.BytesToRead > 0))
                        {
                            int n = Port.BytesToRead;
                            byte[] buf = new byte[n];
                            datas += n;
                            //labBytes.Text = datas.ToString();
                            this.BeginInvoke(updateDat, datas.ToString());
                            Port.Read(buf, 0, n);

                            buffer.AddRange(buf);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                Thread.Sleep(10);
            }

        }


        private string ToHexString(byte[] data)
        {
            string hexString = string.Empty;
            StringBuilder strB = new StringBuilder();
            for (int i = 0; i < length; i++)
                strB.Append(data[i].ToString("X2"));
            hexString = strB.ToString();
            return hexString;
        }


        private void pathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDlg1 = new FolderBrowserDialog();
            fbDlg1.Description = "Please select a folder to save images !";
            if (fbDlg1.ShowDialog() == DialogResult.OK)
            {
                textPath.Text = fbDlg1.SelectedPath;
            }
        }

        private void startScan_Click(object sender, EventArgs e)
        {
            //scanIamges();  
            //takeIamges.Start();    

            char[] tx_data = new char[2];
            tx_data[1] = (char)0x00;

            if (startScan.Text == "Scan")
            {
                tx_data[0] = (char)0x20;
                startScan.Text = "Stop";
                cBoxResolution.Enabled = false;
            }
            else if (startScan.Text == "Stop")
            {
                tx_data[0] = (char)0x21;
                startScan.Text = "Scan";
                cBoxResolution.Enabled = true;
            }

            try
            {
                if (Port.IsOpen)
                    Port.Write(tx_data, 0, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }

        }

        private void comPort_Click(object sender, EventArgs e)
        {

            int i = comPort.Items.Count - 1;
            for (; i >= 0; i--)
            {
                comPort.Items.RemoveAt(i);
            }

            string[] ports = SerialPort.GetPortNames();
            //Sort port by numbers
            Array.Sort(ports);
            //Add port name to combobox
            comPort.Items.AddRange(ports);
            comPort.SelectedIndex = (comPort.Items.Count > 0) ? 0 : -1;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (Port.IsOpen)
            {
                is_running = false;
                try
                {
                    Port.DiscardInBuffer();
                    Port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.DebugRichTextBox.AppendText("\rPort Closed.\n");
             
                is_2640 = false;
                is_5642 = false;
                is_header = false;
                is_camdone = false;
                buffer.RemoveRange(0, buffer.Count);
            }
            else
            {
                is_running = true;
                Port.PortName = comPort.Text;
                Port.BaudRate = 921600;
                Port.DtrEnable = false;//true;
                try
                {
                    Port.Open();
                }
                catch (Exception ex)
                {
                    Port = new SerialPort();
                    MessageBox.Show(ex.Message);
                }
                Port.DtrEnable = true;// false;
                //while (true)
                //{
                //    if (is_ack == true)
                //    {
                //        is_ack = false;
                //        break;
                //    }
                //}
                this.DebugRichTextBox.AppendText("Port Opened.\n");

                cBoxResolution.Enabled = true;
                startScan.Enabled = true;
                pathButton.Enabled = true;
            }

            }

        private void cBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            char[] tx_data = new char[2];
            tx_data[1] = (char)0x00;

            switch (cBoxResolution.SelectedIndex)
            {
                case 0:
                    tx_data[0] = (char)0x00;
                    break;
                case 1:
                    tx_data[0] = (char)0x01;
                    break;
                case 2:
                    tx_data[0] = (char)0x02;
                    break;
                case 3:
                    tx_data[0] = (char)0x03;
                    break;
                case 4:
                    tx_data[0] = (char)0x04;
                    break;
                case 5:
                    tx_data[0] = (char)0x05;
                    break;
                case 6:
                    tx_data[0] = (char)0x06;
                    break;
                case 7:
                    tx_data[0] = (char)0x07;
                    break;
                case 8:
                    tx_data[0] = (char)0x08;
                    break;
                default:
                    break;
            }
            try
            {
                if (Port.IsOpen)
                    Port.Write(tx_data, 0, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR");
            }
        }
        }
}
