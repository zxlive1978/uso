using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using DCON_PC_DOTNET;
using CodeServer;
using System.Runtime.InteropServices;
using Realtime;

using Modbus.Device;
using System.IO.Ports;

using System.Threading;
using System.Threading.Tasks;
//using Unme.Common;
using System.Diagnostics;

//ввод из Messagebox(((
using Microsoft.VisualBasic;

namespace codeserveer
{
    public partial class Form1 : Form
    {
        
        public xml_query xml = new xml_query();

        public string server;
        public Int32 port;
        public TcpClient client;

        //Соообщение передаваемое через сокет сервер
        public string test;
        // Массив передаваемый через сокет сервер в ASCII
        public Byte[] data;
        public NetworkStream stream;
        public string message;

        //Список значений каналов по порядку
        public List<string> val_val_test = new List<string>();

        //i-7017
        short iSlot, iTimeout, iTotalChannel, iAddress, bChecksum;
        byte iComport;
        uint iBaudrate;
        public List<_7017> _7017s = new List<_7017>();
        public _7017 _7017;

        //i-7052
        public List<_7052> _7052s = new List<_7052>();
        public _7052 _7052;

        //i-7080
        public List<_7080> _7080s = new List<_7080>();
        public _7080 _7080;

        //Устройства
        //public List<object> list_devices=new List<object>();


        // СЕРВЕР КОДОВ DCOM
        public CodeServer.Client device;
        //public CodeServer.Sender sender_ = new CodeServer.Sender();
        //public CodeServer.Constants fff=10;
        public int[] datas;
        public int appID = 99;
        public string SelfHostName = "I-7017 ICPCON";
        public string AppIDName = "SUCK";
        public int chanCount = 8;

        ////Realtime DTCIS DCOM
        //public Realtime.IMSViewServiceClass dtcis =new IMSViewServiceClass();
        //public Realtime.IMSEventsFromServerClass dtcis_event = new IMSEventsFromServerClass();
        ////Список заказываемых параметров
        ////1020 Т/БЛОК, м
        ////1132 СКОРОСТЬ ИНСТ,м/с
        ////1024 НАД ЗАБОЕМ, м
        ////1022 ГЛУБИНА, м
        ////1149 ОБЪЕМ, м3
        ////1160 ПЛОТНОСТЬ,г/см3
        ////1175 РАСХОД, л/с
        ////1111 ХОДЫ, х/мин
        ////1103 ДАВЛЕНИЕ, МПа
        ////1101 ВЕС, т
        ////1102 НАГРУЗКА, т
        ////1107 МОМЕНТ, кН*м
        ////1200 Гсум
        ////1106 ОБОРОТЫ, об/м
        //public object param = new string[] { "1020", "1132","1024",
        //    "1022","1149","1160","1175","1111","1103","1101","1102","1107","1200","1106" };
        ////Значения списка, код параметра, код справочника, значение!!!!!
        //public object answer = new string[] { };
      
        

        //MODBUS
        SerialPort serial_port;
       
        //Поиск по ID
        //Ответ
        public string answer_r;
        //Комманда поиск
        public string command;

        //Настройки 
        public options opt= new options();
  
       
        public Form1()
        {
            InitializeComponent();
        }

        



        static void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 17235;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }



        
       

        //Конструктор запроса
        public void get_xml_template() {


            if (val_val_test.Count > 0)
            {

                string status = "1";
                textBox4.Text = val_val_test.Count.ToString();

                test = xml.xml_create_template(textBox1.Text, textBox2.Text, textBox3.Text,
                    val_val_test.Count.ToString(), xml.get_time(), val_val_test, status);
            }
        }

        //Соединение с сервером
        public void time_connect() {
            try
            {
                server = "127.0.0.1";
                port = 17235;
                client = new TcpClient(server, port);
            }
            catch { }
        
        }

        public void time_transfer() {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                data = System.Text.Encoding.ASCII.GetBytes(test);
                textBox7.Text = test;
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                textBox6.Text = responseData;
            }
            catch { }
        
        }

        public void time_disconnect() {
            try
            {
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Random r = new Random();
            
            ////Чтение данных из I-7017 ICPCON
            //read_com_data();

            //listView1.Clear();
            //add_to_tree();
            get_xml_template();
            //Передача в Сервер кодов
            time_transfer();


        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        public void id_read(){

            //ushort result;
            //result = Dcon.UART.Open_Com(3, 9600, 8, 0, 0);
            //byte[] s = new byte[50];
            //byte[] r = new byte[50];
            //ushort wt = 0;
            //string SS = "TEST" + "E";
            //s = System.Text.Encoding.ASCII.GetBytes(SS);
            //result = Serial.UART.Send_Receive_Cmd_WithChar(1, s, r, 100, 0, ref wt, 69);
            //string RR = System.Text.Encoding.ASCII.GetString(r, 0, r.Length);

        
        }

        public void read_com_data(EventArgs e)
        {
            //uint wt = 0;
            //string command="$05M";
            

            //byte[] bytes = new byte[command.Length * sizeof(char)];
            //byte[] r=new byte[50];

            //System.Buffer.BlockCopy(command.ToCharArray(), 0, bytes, 0, bytes.Length);
            //DCON.UART.Open_Com(3, 57600, 8, 0, 1);
            //DCON.UART.Send_Receive_Cmd(3,bytes,r,100,0,out wt);
            //DCON.UART.Close_Com(3);

            short[] AIValue = new short[8];
            short iRet;
            
            iRet = DCON.IO_Function.Read_AI_All_Hex(iComport, iAddress, iSlot, bChecksum, iTimeout, AIValue);
            if (Convert.ToBoolean(iRet))
            {
                checkBox2.Checked = false;
                checkBox2_Click(this, e);
                checkBox1.Checked = false;
                checkBox1_Click(this, e);
                listView1.Clear();
                MessageBox.Show("Ошибка связи: " + Convert.ToString(iRet));
               
            }
            else
            {
                
                ////Массив значений параметров по каналам
                //val_val_test.Clear();
                //for (int i = 0; i < 8; i++)
                //{
                //    //Отсечение отрицательных кодов
                //    if (AIValue[i] > 0)
                //    {
                //        val_val_test.Add(Convert.ToString(AIValue[i]));
                //    }
                //    else
                //    {
                //        val_val_test.Add("0");
                //    }
                //}
                ////Добавление в дерево
                //for (int i = 0; i < 8; i++)
                //{
                //    listView1.Items.Add("Канал " + (i + 1).ToString() + ": " + val_val_test[i]);
                //}

            }
        }

        public void add_to_tree() {
          
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                timer2.Enabled = true;
               
               
            }
            else

            {
                DCON.UART.Close_Com(iComport);
                timer2.Enabled = false;
                listView1.Clear();
                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DCON.UART.Close_Com(iComport);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            listView1.Clear();

            iComport = Convert.ToByte(textComport.Text);
            iAddress = Convert.ToInt16(7);
            iBaudrate = Convert.ToUInt32(textBaudrate.Text);
            iSlot = Convert.ToInt16(textSlot.Text);
            bChecksum = Convert.ToInt16(textChecksum.Text);
            iTimeout = Convert.ToInt16(textTimeout.Text);
            iTotalChannel = Convert.ToInt16(textTotalChannel.Text);
            DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);

            //Чтение данных из I-7017 ICPCON
            read_com_data(e);
            //Добавление в список
            
            //if (val_val_test.Count > 0)
            //{
            //    add_to_tree();
            //}
        }

        public void test_read_com_data()
        {

           
            //short[] AIValue = new short[8];
            //short iRet;

            //iRet = DCON.IO_Function.Read_AI_All_Hex(iComport, iAddress, iSlot, bChecksum, iTimeout, AIValue);
            


            //if (Convert.ToBoolean(iRet)) { 
            
            //}
            //else
            //{
            //    listView2.Items.Add("Адрес:"+iAddress);
            //}
        }
       
        //Поиск i-7017,i-7052,i-7080
        public void finder_ixxx(object sender, EventArgs e)
        {
            _7017s.Clear();
            _7052s.Clear();
            _7080s.Clear();

            //Корень
            treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            treeView1.Nodes.Clear();
            this.DoubleBuffered = true;
            TreeNode USO = new TreeNode();
            USO.Text = "УСО";
            USO.ImageIndex = 0;
            USO.SelectedImageIndex = 0;
            treeView1.Nodes.Add(USO);
            checkBox2.Checked = false;
            checkBox2_Click(this, e);
            checkBox1.Checked = false;
            checkBox1_Click(this, e);

            iComport = Convert.ToByte(textComport.Text);
            iAddress = Convert.ToInt16(7);
            iBaudrate = Convert.ToUInt32(textBaudrate.Text);
            iSlot = Convert.ToInt16(textSlot.Text);
            bChecksum = Convert.ToInt16(textChecksum.Text);
            iTimeout = Convert.ToInt16(textTimeout.Text);
            iTotalChannel = Convert.ToInt16(textTotalChannel.Text);

            progressBar1.Minimum = Convert.ToInt16(textBox8.Text);
            progressBar1.Maximum = Convert.ToInt16(textBox9.Text) + 1;
            progressBar1.Value = 0;
            for (Int16 i = Convert.ToInt16(textBox8.Text); i <= Convert.ToInt16(textBox9.Text); i++)
            {
                DCON.UART.Close_Com(iComport);

                iAddress = i;
                //DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);


                uint wt = 100;
                if (i < 16)
                {
                    command = "$0" + i.ToString("X") + "M";
                }
                else
                {
                    command = "$" + i.ToString("X") + "M";
                }

                byte[] bytes = Encoding.ASCII.GetBytes(command);
                byte[] r = new byte[20];


                DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);
                short iRet;
                iRet = DCON.UART.Send_Receive_Cmd(iComport, bytes, r, 100, 0, out wt);
                //DCON.UART.Close_Com(3);
                if (iRet == 0)
                {
                    string answer_r = Encoding.ASCII.GetString(r);
                    //listView2.Items.Add(answer_r);
                    TreeNode new_device = new TreeNode(answer_r);
                    new_device.SelectedImageIndex = 2;
                    new_device.ImageIndex = 2;
                    new_device.Name = answer_r;

                    USO.Nodes.Add(new_device);

                    //7017
                    if (answer_r.Contains("7017"))
                    {
                        
                        _7017 = new _7017();
                        _7017.name = answer_r;
                        _7017.address = Convert.ToInt16(int.Parse(answer_r.Substring(1, 2), System.Globalization.NumberStyles.HexNumber));
                        _7017.com = iComport;
                        _7017.baudrate = iBaudrate;
                        _7017.checksum = bChecksum;
                        _7017.slot = iSlot;
                        _7017.timeout = iTimeout;
                        _7017s.Add(_7017);
                        _7017.list_channels.Clear();
                        //Резерв каналов DCOM
                        numb_reserve += _7017.totalchanels;
                        for (int j = 0; j < _7017.totalchanels; j++)
                        {
                            string name_channel = _7017.chanels_type + j.ToString();
                            _7017.list_channels.Add(name_channel);
                            new_device.Nodes.Add(answer_r + name_channel, name_channel);
                            //Label name_chanel = new Label();
                            //test.Text = "suck";

                        }
                    }

                    //7052
                    if (answer_r.Contains("7052"))
                    {
                        

                        _7052 = new _7052();
                        _7052.name = answer_r;
                        _7052.address = Convert.ToInt16(int.Parse(answer_r.Substring(1, 2), System.Globalization.NumberStyles.HexNumber));
                        _7052.com = iComport;
                        _7052.baudrate = iBaudrate;
                        _7052.checksum = bChecksum;
                        _7052.slot = iSlot;
                        _7052.timeout = Convert.ToInt16(textBox14.Text);
                        _7052.totalchanels = Convert.ToInt16(textBox11.Text);
                        _7052.time_min = Convert.ToInt16(textBox15.Text);
                        _7052.time_max = Convert.ToInt16(textBox16.Text);
                        _7052s.Add(_7052);
                        _7052.list_channels.Clear();
                        //Резерв каналов DCOM
                        numb_reserve += _7052.totalchanels;
                        for (int j = 0; j < _7052.totalchanels; j++)
                        {
                            string name_channel = _7052.chanels_type + j.ToString();
                            _7052.list_channels.Add(name_channel);
                            new_device.Nodes.Add(answer_r + name_channel, name_channel);
                        }
                    }

                    //7080
                    if (answer_r.Contains("7080"))
                    {
                        numb_reserve++;

                        _7080 = new _7080();
                        _7080.name = answer_r;
                        _7080.address = Convert.ToInt16(int.Parse(answer_r.Substring(1, 2), System.Globalization.NumberStyles.HexNumber));
                        _7080.com = iComport;
                        _7080.baudrate = iBaudrate;
                        _7080.checksum = bChecksum;
                        _7080.slot = iSlot;
                        _7080.timeout = Convert.ToInt16(textBox13.Text);
                        _7080.time = Convert.ToUInt32(textBox13.Text);
                        _7080.Value_Base = Convert.ToInt64(textBox12.Text);
                        _7080s.Add(_7080);
                        _7080.list_channels.Clear();
                        for (int j = 0; j < 1; j++)
                        {
                            string name_channel = _7080.chanels_type + j.ToString();
                            _7080.list_channels.Add(name_channel);
                            new_device.Nodes.Add(answer_r + name_channel, name_channel);
                        }
                    }

                    



                    
                }
                treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.Normal;
                treeView1.ExpandAll();
                progressBar1.Value++;
            }
        }


        //Поиск i-7017,i-7052,i-7080
        private void button1_Click(object sender, EventArgs e)
        {
            //Поиск i-7017,i-7052,i-7080
        }

        //TreeNode ParentNode = new TreeNode();
        //    ParentNode.Text = "RootNode";
        //    ParentNode.ForeColor = Color.Black;
        //    ParentNode.BackColor = Color.White;
        //    ParentNode.ImageIndex = 0;
        //    ParentNode.SelectedImageIndex = 0;
        //    treeView1.Nodes.Add(ParentNode);           //Root node in TreeView

        //    TreeNode ChildNode1 = new TreeNode();
        //    ChildNode1.Text = "Child 1";
        //    ChildNode1.ForeColor = Color.Black;
        //    ChildNode1.BackColor = Color.White;
        //    ChildNode1.ImageIndex = 0;
        //    ChildNode1.SelectedImageIndex = 0;
        //    ParentNode.Nodes.Add(ChildNode1);         //Child node (parent is ParentNode)

        //    TreeNode ChildNode2 = new TreeNode();
        //    ChildNode2.Text = "Child 2";
        //    ChildNode2.ForeColor = Color.Black;
        //    ChildNode2.BackColor = Color.White;
        //    ChildNode2.ImageIndex = 0;
        //    ChildNode2.SelectedImageIndex = 0;
        //    ParentNode.Nodes.Add(ChildNode2);         //Child node (parent is ParentNode)

        //    TreeNode ChildNode3 = new TreeNode();
        //    ChildNode3.Text = "Child 3";
        //    ChildNode3.ForeColor = Color.Black;
        //    ChildNode3.BackColor = Color.White;
        //    ChildNode3.ImageIndex = 0;
        //    ChildNode3.SelectedImageIndex = 0;
        //    ParentNode.Nodes.Add(ChildNode3);         //Child node (parent is ParentNode)

        //    TreeNode ChildNode4 = new TreeNode();
        //    ChildNode4.Text = "Child 4";
        //    ChildNode4.ForeColor = Color.Black;
        //    ChildNode4.BackColor = Color.White;
        //    ChildNode4.ImageIndex = 0;
        //    ChildNode4.SelectedImageIndex = 0;
        //    ParentNode.Nodes.Add(ChildNode4);         //Child node (parent is ParentNode)

        //    TreeNode NextChildNode1 = new TreeNode();
        //    NextChildNode1.Text = "Next Child 1";
        //    NextChildNode1.ForeColor = Color.Black;
        //    NextChildNode1.BackColor = Color.White;
        //    NextChildNode1.ImageIndex = 1;
        //    NextChildNode1.SelectedImageIndex = 1;
        //    ChildNode3.Nodes.Add(NextChildNode1);         //Child node (parent is ChildNode3)

        //    TreeNode NextChildNode2 = new TreeNode();
        //    NextChildNode2.Text = "Next Child 2";
        //    NextChildNode2.ForeColor = Color.Black;
        //    NextChildNode2.BackColor = Color.White;
        //    NextChildNode2.ImageIndex = 1;
        //    NextChildNode2.SelectedImageIndex = 1;
        //    ChildNode3.Nodes.Add(NextChildNode2);         //Child node (parent is ChildNode3)

        //    treeView1.ExpandAll();


        //public void send_cmd_read_dcon() {

        //    byte[] out_buffer;
        //    byte[] in_buffer;
        //    UInt32  itimeo = Convert.ToUInt32(textComport.Text);
        //    UInt32 bChecksum = Convert.ToUInt32(textChecksum.Text);

        //    DCON.UART.Send_Receive_Cmd(iComport, out_buffer, in_buffer, itimeo, bChecksum, out itimeo);
        //}

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                timer1.Enabled = false;
                server = textBox5.Text;

                //Создание запроса
                get_xml_template();

                //Подключение к Серверу кодов, включение таймера передачи
                time_connect();
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                time_disconnect();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        { }


        public int numb_reserve;
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (_7052s[0].freq1.Count() > 0)
                //{

                //int totals_7017 = _7017s.Count *Convert.ToInt32( textTotalChannel.Text);
                //int totals_7052 = _7052s.Count * Convert.ToInt32(textBox11.Text);
                //int totals_7080

                //numb_reserve=totals_7017 + totals_7052;
                datas = new int[numb_reserve];
                int index_datas=0;

                //Несколько 7080
                if (_7080s.Count > 0)
                {
                    for (int i = 0; i < _7080s.Count; i++)
                    {
                        datas[index_datas] = Convert.ToInt32(_7080s[i].Value_Result);
                        index_datas++;
                    }
                }

                //Несколько 7052
                if (_7052s.Count > 0)
                {
                    for (int i = 0; i < _7052s.Count; i++)
                    {
                        //Число каналов
                        for (int j = 0; j < _7052s[i].totalchanels; j++)
                        {
                            datas[index_datas] = Convert.ToInt32(Math.Truncate(_7052s[i].freq1[j] * 1000));
                            index_datas++;
                        }
                    }
                }
               //i-7017 read
                if (_7017s.Count > 0)
                {
                    for (int i = 0; i < _7017s.Count; i++)
                    {
                        for (int j = 0; j < _7017s[i].totalchanels; j++)
                        {
                            datas[index_datas] = Convert.ToInt32(_7017s[i].AIValue[j]);
                            index_datas++;
                        }
                    }
                }













                    //for (int a = 0; a < datas.Length; a++)
                    //{
                    //    datas[a] = Convert.ToInt32( Math.Truncate(_7052s[0].freq1[a] * 1000));
                    //}

                    device.SendCodes(datas);
                //}
            }
            catch { }
           
            //device.Dispose();
            //sender_.SendCodes(SelfHostName,appID,SelfHostName,AppIDName,chanCount,datas);
            //sender_.
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer3.Stop();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        //Подключение через DCOM
        public void checkBox3_Click(object sender, EventArgs e)
        {
            //Инициализация переменных
            SelfHostName = textBox5.Text;
            appID = Convert.ToInt32(textBox2.Text);
            AppIDName = textBox1.Text;
            chanCount = Convert.ToInt32(textBox4.Text);


            //dtcis.ViewerInfo("STUDIO", "STUDIO");
            //dtcis.CheckConnection();
            //dtcis = new Client("STUDIO", "STUDIO"); ;
            //Создание DCOM объекта
            device = new Client(SelfHostName);
            if (checkBox3.Checked)
            {  
                //Регистрация на Сервере кодов через DCOM
                device.Identify(appID, SelfHostName, AppIDName, chanCount);

                //Если соединись с Сервером кодов, запускаем передачу данных по таймеру3
                if (device.Connected) { 
                    timer3.Start(); 
                }
            }
            else {
                //Отключение!
                //device.
                timer3.Stop();
                //device = null;
                //Marshal.FinalReleaseComObject(this.);
                //device = null;
                //device.Identify(appID, "", AppIDName, 0);
                //device = new Client(SelfHostName);
                //device.Dispose();
                
                device.Dispose();
                Dispose(true);
                
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }

        //Цикл опроса подготовка
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

            //checkBox1.BackColor = System.Drawing.Color.Black;
            //timer5.Enabled = true;
            //if (checkBox4.Checked)
            //{
            //    timer5.Start();
            //}
            //else {
            //    timer5.Stop();
            //}
        }

        byte[] bytes;
        byte[] r = new byte[20];
        uint wt = 300;
        short iRet = 255;
        uint time = 300;
        uint stop = 0;
        public void read_ICPcon_ID () { 
        
            
            iRet =DCON.UART.Send_Receive_Cmd(iComport, bytes, r, time, stop, out wt);
        }

        //Цикл опроса
        private void timer4_Tick(object sender, EventArgs e)
        {
            

            timer4.Stop();
            
            treeView1.Nodes.Clear();

            checkBox2.Checked = false;
            checkBox2_Click(this, e);
            checkBox1.Checked = false;
            checkBox1_Click(this, e);
            
            iComport = Convert.ToByte(textComport.Text);
            iAddress = Convert.ToInt16(7);
            iBaudrate = Convert.ToUInt32(textBaudrate.Text);
            iSlot = Convert.ToInt16(textSlot.Text);
            bChecksum = Convert.ToInt16(textChecksum.Text);
            iTimeout = Convert.ToInt16(textTimeout.Text);
            iTotalChannel = Convert.ToInt16(textTotalChannel.Text);

            progressBar1.Minimum = Convert.ToInt16(textBox8.Text);
            progressBar1.Maximum = Convert.ToInt16(textBox9.Text);
            progressBar1.Value = 0;
            for (Int16 i = Convert.ToInt16(textBox8.Text); i <= Convert.ToInt16(textBox9.Text); i++)
            {
                DCON.UART.Close_Com(iComport);

                iAddress = i;
                //DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);


                
                if (i < 10)
                {
                    command = "$0" + i + "M";
                }
                else
                {
                    command = "$" + i + "M";
                }

                bytes = Encoding.ASCII.GetBytes(command);
                

                
                DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);

                Task read_id = new Task(read_ICPcon_ID);
                Application.DoEvents();
                read_id.Start();
                Application.DoEvents();
                read_id.Wait();
                Application.DoEvents();
                //read_id.Join();
                //iRet = tread_read_ICPcon_ID(ref iComport, ref bytes, ref r, 100, 0, ref out wt);
                //DCON.UART.Close_Com(3);
                if (iRet == 0)
                {
                    answer_r = Encoding.ASCII.GetString(r);
                    TreeNode new_device = new TreeNode(answer_r);
                    new_device.ImageIndex = 1;
                    treeView1.Nodes.Add(new_device);

                }
                //read_id.Dispose();
                progressBar1.Value=i;
                //test_read_com_data();

            }
            DCON.UART.Close_Com(iComport);
            timer4.Start();
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;

        }
        
        private void button3_Click_1(object sender, EventArgs e)
        {
            
            if (_7017s.Count > 0) {

                for (int i = 0; i < _7017s.Count;i++)
                {
                    DCON.UART.Open_Com(_7017s[i].com, _7017s[i].baudrate, 8, 0, 1);

                    _7017s[i].iRet = DCON.IO_Function.Read_AI_All_Hex(_7017s[i].com, _7017s[i].address, _7017s[i].slot, _7017s[i].checksum, _7017s[i].timeout, _7017s[i].AIValue);
                    if (Convert.ToBoolean(_7017s[i].iRet))
                    {
                        //если ошибка
                        //MessageBox.Show("Ошибка связи: " + Convert.ToString(_7017s[i].iRet));
                        //this.Focus();
                        for (int j = 0; j < _7017s[i].totalchanels; j++)
                        {
                            TreeNode[] find = treeView1.Nodes.Find(_7017s[i].name + _7017s[i].list_channels[j], true);

                            if (find.Count() != 0)
                            {
                                find[0].StateImageIndex = 3;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < _7017s[i].totalchanels; j++)
                        {
                            TreeNode[] find = treeView1.Nodes.Find(_7017s[i].name + _7017s[i].list_channels[j], true);

                            if (find.Count() != 0)
                            {
                                //Проверка на отрицательное значение и занесение в дерево
                                if (_7017s[i].AIValue[j] >= 0)
                                {
                                    find[0].Text = _7017s[i].AIValue[j].ToString();
                                }
                                else {

                                    _7017s[i].AIValue[j] = 0;
                                    find[0].Text = "0";
                                
                                }
                            }

                        }

                    }

                    //treeView1._7017[i].list_channels
                    //TreeNode[] find = treeView1.ch.
                    
                }
                
            
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            int numb_channel_dt_cis = 0;


            //i-7080 read
            if (_7080s.Count > 0)
            {
                //Несколько 7080
                for (int i = 0; i < _7080s.Count; i++)
                {
                    DCON.UART.Open_Com(_7080s[i].com, _7080s[i].baudrate, 8, 0, 1);
                    
                    //Число каналов
                    for (int j = 0; j < 1; j++)
                    {
                        //Изменение значения в дереве
                        TreeNode[] find = treeView1.Nodes.Find(_7080s[i].name + _7080s[i].list_channels[j], true);
                        //Число каналов
                        numb_channel_dt_cis++;
                   
                        //Чтение первого канала
                        if (_7080s[i].address < 16)
                        {
                            command = "#0" + _7080s[i].address.ToString("X") + 0;
                        }
                        else
                        {
                            command = "#" + _7080s[i].address.ToString("X") + 0;
                        }
                        
                        _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);

                        //Чтение второго канала
                        if (_7080s[i].address < 16)
                        {
                            command = "#0" + _7080s[i].address.ToString("X") + 1;
                        }
                        else
                        {
                            command = "#" + _7080s[i].address.ToString("X") + 1;
                        }
                        _7080s[i].send_1_channel = Encoding.ASCII.GetBytes(command);
                        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_1_channel, _7080s[i].recieve_1_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
     
                        if (Convert.ToBoolean(_7080s[i].iRet))
                        {
                            //если ошибка
                            //Красный 7080
                            TreeNode[] find2 = treeView1.Nodes.Find(_7080s[i].name, true);
                            if (find2.Count() != 0)
                            {
                                find2[0].SelectedImageIndex = 3;
                                find2[0].ImageIndex = 3;
                            }
                            //Красный вывод 7080
                            for (int g = 0; g < 1; g++)
                            {
                                TreeNode[] find1 = treeView1.Nodes.Find(_7080s[i].name + _7080s[i].list_channels[g], true);

                                if (find1.Count() != 0)
                                {
                                    find1[0].SelectedImageIndex = 3;
                                    find1[0].ImageIndex = 3;
                                }

                            }
                        }
                        else
                        {
                            //Зеленый 7080
                            TreeNode[] find2 = treeView1.Nodes.Find(_7080s[i].name, true);
                            if (find2.Count() != 0)
                            {
                                find2[0].SelectedImageIndex = 2;
                                find2[0].ImageIndex = 2;
                            }
                            //Зеленый вывод 7080
                            find[0].SelectedImageIndex = 2;
                            find[0].ImageIndex = 2;

                            //Обрезание ">" и вычисление значения
                            string sttring7080_0= (Encoding.ASCII.GetString(_7080s[i].recieve_0_channel)).Substring(1);
                            long.TryParse(sttring7080_0, System.Globalization.NumberStyles.HexNumber, null, out _7080s[i].Value_0_cur);
                            //Обрезание ">" и вычисление значения
                            string sttring7080_1 = (Encoding.ASCII.GetString(_7080s[i].recieve_1_channel)).Substring(1);
                            long.TryParse(sttring7080_1, System.Globalization.NumberStyles.HexNumber, null, out _7080s[i].Value_1_cur);
                            //Вычисление значения BASE+A-B
                            //Коэффициент пересчета
                            if ((_7080s[i].Value_0_cur != _7080s[i].Value_0_old) | (_7080s[i].Value_1_cur != _7080s[i].Value_1_old))
                            {
                                //_7080s[i].Value_Result = 1000;// _7080s[i].Value_Base;
                                if (_7080s[i].Value_0_cur > _7080s[i].Value_0_old)
                                {
                                    _7080s[i].Value_Result += Convert.ToInt64((_7080s[i].Value_0_cur - _7080s[i].Value_0_old) * K_talbok);
                                }
                                if (_7080s[i].Value_1_cur > _7080s[i].Value_1_old)
                                {
                                    _7080s[i].Value_Result -= Convert.ToInt64((_7080s[i].Value_1_cur-_7080s[i].Value_1_old) * K_talbok);
                                }

                                //_7080s[i].Value_Result = Convert.ToInt64((_7080s[i].Value_0_cur - _7080s[i].Value_1_cur) * K_talbok);
                                _7080s[i].Value_0_old = _7080s[i].Value_0_cur;
                                _7080s[i].Value_1_old = _7080s[i].Value_1_cur;
                            }
                                                       

                            //////Коррекция кодов каналов счетчиков A и В
                            ////if ((_7080s[i].Value_0_cur > _7080s[i].Value_Base - 98000) | (_7080s[i].Value_1_cur > _7080s[i].Value_Base - 98000))
                            ////{

                            ////    if ((_7080s[i].Value_0_cur - _7080s[i].Value_1_cur) > 0)
                            ////    {
                            ////        //B=0 A=A-B
                            ////        //Сброс B=0 канала
                            ////        reset_counters_7080s(false, true);
                            ////        //A=A-B
                            ////        string hexxer = Convert.ToInt64(_7080s[i].Value_0_cur - _7080s[i].Value_1_cur).ToString("X");
                            ////        //string hexxer = Convert.ToInt64(6666).ToString("X");
                            ////        int rows = 8 - hexxer.Length;
                            ////        for (int n = 0; n < (rows); n++)
                            ////        {
                            ////            hexxer = hexxer.Insert(0, "0");
                            ////        }
                            ////        if (_7080s[i].address < 16)
                            ////        {
                            ////            command = "@0" + _7080s[i].address.ToString("X") + "P" + 0 + hexxer;
                            ////        }
                            ////        else
                            ////        {
                            ////            command = "@" + _7080s[i].address.ToString("X") + "P" + 0 + hexxer;
                            ////        }
                            ////        _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            ////        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);

                            ////        if (_7080s[i].address < 16)
                            ////        {
                            ////            command = "$0" + _7080s[i].address.ToString("X") + 6 + 0;
                            ////        }
                            ////        else
                            ////        {
                            ////            command = "$" + _7080s[i].address.ToString("X") + 6 + 0;
                            ////        }
                            ////        _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            ////        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                            ////        //Сброс B=0 канала
                            ////        reset_counters_7080s(false, true);
                            ////    }
                            ////    else
                            ////    {
                            ////        //A=0 B=B-A
                            ////        //Сброс A=0 канала
                            ////        reset_counters_7080s(true, false);
                            ////        //B=B-A
                            ////        string hexxer = Convert.ToInt64(_7080s[i].Value_1_cur - _7080s[i].Value_0_cur).ToString("X");
                            ////        int rows = 8 - hexxer.Length;
                            ////        for (int n = 0; n < (rows); n++)
                            ////        {
                            ////            hexxer = hexxer.Insert(0, "0");
                            ////        }
                            ////        if (_7080s[i].address < 16)
                            ////        {
                            ////            command = "@0" + _7080s[i].address.ToString("X") + "P" + 1 + hexxer;
                            ////        }
                            ////        else
                            ////        {
                            ////            command = "@" + _7080s[i].address.ToString("X") + "P" + 1 + hexxer;
                            ////        }
                            ////        _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            ////        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                            ////        if (_7080s[i].address < 16)
                            ////        {
                            ////            command = "$0" + _7080s[i].address.ToString("X") + 6 + 1;
                            ////        }
                            ////        else
                            ////        {
                            ////            command = "$" + _7080s[i].address.ToString("X") + 6 + 1;
                            ////        }
                            ////        _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            ////        _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                            ////        //Сброс A=0 канала
                            ////        reset_counters_7080s(true, false);
                            ////    }

                            ////}
                            //////Скидывание счетчиков
                            ////if (Math.Abs(_7080s[i].Value_0_cur - _7080s[i].Value_1_cur) >= _7080s[i].Value_Base)
                            ////{   //A-B=BASE A=0 B=0 Result=0
                            ////    reset_counters_7080s(true, true);
                            ////    _7080s[i].Value_Result = _7080s[i].Value_Base;
                            ////}
                            //Изменение значения в дереве
                            find[0].Text = "#" + numb_channel_dt_cis + " (" + _7080s[i].chanels_type + j + ")=" + _7080s[i].Value_Result.ToString() + " A:" + _7080s[i].Value_0_cur.ToString() + " B:" + _7080s[i].Value_1_cur.ToString();
                        }
                    }
                }
            }


            //i-7052 read
                if (_7052s.Count > 0)
                {
                    //Несколько 7052
                    for (int i = 0; i < _7052s.Count; i++)
                    {
                        DCON.UART.Open_Com(_7052s[i].com, _7052s[i].baudrate, 8, 0, 1);
                        //Число каналов
                           for (int j = 0; j < _7052s[i].totalchanels; j++)
                            {
                                //Изменение значения в дереве
                                TreeNode[] find = treeView1.Nodes.Find(_7052s[i].name + _7052s[i].list_channels[j], true);
                                //Число каналов
                                numb_channel_dt_cis++;
                               //Чтение массива значений из одного i-7052
                                _7052s[i].iRet = DCON.IO_Function.Read_DI_Counter(_7052s[i].com, _7052s[i].address, _7052s[i].slot, Convert.ToInt16(j), _7052s[i].totalchanels, _7052s[i].checksum, _7052s[i].timeout, out _7052s[i].DIValue[j]);
                             
                                 if (Convert.ToBoolean(_7052s[i].iRet))
                                {
                                    //если ошибка
                                    //Красный 7052
                                    TreeNode[] find2 = treeView1.Nodes.Find(_7052s[i].name, true);
                                    if (find2.Count() != 0)
                                    {
                                        find2[0].SelectedImageIndex = 3;
                                        find2[0].ImageIndex = 3;
                                    }
                                    //Красный вывод 7052
                                    for (int g = 0; g < _7052s[i].totalchanels; g++)
                                    {
                                        TreeNode[] find1 = treeView1.Nodes.Find(_7052s[i].name + _7052s[i].list_channels[g], true);

                                        if (find1.Count() != 0)
                                        {
                                            find1[0].SelectedImageIndex = 3;
                                            find1[0].ImageIndex = 3;
                                        }

                                    }
                                     //Сброс всех счетчиков
                                    for (int h = 0; h < _7052s[i].totalchanels; h++)
                                    {
                                        _7052s[i].stopWatch[h].Reset();

                                        _7052s[i].begin[h] = true;
                                    }

                                }
                                else
                                {
                                    //Зеленый 7052
                                    TreeNode[] find2 = treeView1.Nodes.Find(_7052s[i].name, true);
                                    if (find2.Count() != 0)
                                    {
                                        find2[0].SelectedImageIndex = 2;
                                        find2[0].ImageIndex = 2;
                                    }
                                    //Зеленый вывод 7052
                                    find[0].SelectedImageIndex = 2;
                                    find[0].ImageIndex = 2;

                                    //Запуск секундомеров
                                    if (_7052s[i].begin[j])
                                    {
                                        _7052s[i].stopWatch[j].Reset();
                                        _7052s[i].stopWatch[j].Start();
                                        _7052s[i].begin[j] = false;
                                        _7052s[i].value_begin[j] = Convert.ToInt32(_7052s[i].DIValue[j]);
                                    }
                                    else
                                    {
                                        _7052s[i].freq2[j] = _7052s[i].stopWatch[j].ElapsedMilliseconds;
                                        //Если изменилось значение счетчика и прошло больше 1с и меньше 5с, вычисляем частоту!
                                        if (Math.Abs(_7052s[i].value_begin[j] - _7052s[i].DIValue[j]) > 0 & (_7052s[i].freq2[j] > _7052s[i].time_min) & (_7052s[i].freq2[j] < _7052s[i].time_max))
                                        {
                                            //Проверка на переполнение
                                            if (Convert.ToInt32(_7052s[i].DIValue[j]) > _7052s[i].value_begin[j])
                                            {
                                                _7052s[i].value_cur[j] = Convert.ToInt32(_7052s[i].DIValue[j]) - _7052s[i].value_begin[j];
                                            }
                                            else {
                                                _7052s[i].value_cur[j] = Convert.ToInt32(_7052s[i].DIValue[j]) + (65536 - _7052s[i].value_begin[j]);
                                            }
                                            _7052s[i].freq1[j] = 1000 * (Convert.ToSingle(_7052s[i].value_cur[j]) / Convert.ToSingle(_7052s[i].stopWatch[j].ElapsedMilliseconds));

                                            find[0].Text = "#" + numb_channel_dt_cis + " (" + _7052s[0].chanels_type + j + ")=" + Convert.ToString(Math.Truncate(_7052s[i].freq1[j] * 1000));
                                                _7052s[i].stopWatch[j].Reset();

                                                _7052s[i].stopWatch[j].Stop();
                                            _7052s[i].begin[j] = true;
                                        }
                                        else
                                        {
                                            //Если время прошло больше 5 сек, то скидываем частоту в ноль и перезапускаем таймер
                                            if (_7052s[i].freq2[j] > _7052s[i].time_max)
                                            {
                                                find[0].Text = "#" + numb_channel_dt_cis + " (" + _7052s[0].chanels_type + j + ")=" + "0";
                                                _7052s[i].stopWatch[j].Reset();
                                                _7052s[i].stopWatch[j].Start();
                                                _7052s[i].begin[j] = false;
                                                _7052s[i].freq1[j] = 0;
                                                _7052s[i].value_begin[j] = Convert.ToInt32(_7052s[i].DIValue[j]);
                                            }
                                        }
                                    }
                                }
                            }
                    }
                }

                //i-7017 read
                if (_7017s.Count > 0)
                {
                    for (int i = 0; i < _7017s.Count; i++)
                    {
                        DCON.UART.Open_Com(_7017s[i].com, _7017s[i].baudrate, 8, 0, 1);

                        _7017s[i].iRet = DCON.IO_Function.Read_AI_All_Hex(_7017s[i].com, _7017s[i].address, _7017s[i].slot, _7017s[i].checksum, _7017s[i].timeout, _7017s[i].AIValue);
                        if (Convert.ToBoolean(_7017s[i].iRet))
                        {
                            //если ошибка
                            //Красный 7017
                            TreeNode[] find2 = treeView1.Nodes.Find(_7017s[i].name, true);
                            if (find2.Count() != 0)
                            {
                                find2[0].SelectedImageIndex = 3;
                                find2[0].ImageIndex = 3;
                            }
                            //Красный вывод 7017
                            for (int j = 0; j < _7017s[i].totalchanels; j++)
                            {
                                TreeNode[] find1 = treeView1.Nodes.Find(_7017s[i].name + _7017s[i].list_channels[j], true);

                                if (find1.Count() != 0)
                                {
                                    find1[0].SelectedImageIndex = 3;
                                    find1[0].ImageIndex = 3;
                                }
                               
                            }
                        }
                        else
                        {
                            //Зеленый 7017
                            TreeNode[] find2 = treeView1.Nodes.Find(_7017s[i].name, true);
                            if (find2.Count() != 0)
                            {
                                find2[0].SelectedImageIndex = 2;
                                find2[0].ImageIndex = 2;
                            }

                            for (int j = 0; j < _7017s[i].totalchanels; j++)
                            {
                                TreeNode[] find = treeView1.Nodes.Find(_7017s[i].name + _7017s[i].list_channels[j], true);
                                numb_channel_dt_cis++;
                                //Зеленый вывод 7017
                                find[0].SelectedImageIndex = 2;
                                find[0].ImageIndex = 2;

                                //Отсечение отрицательных значений
                                if (_7017s[i].AIValue[j] >= 0)
                                {
                                    find[0].Text = "#" + numb_channel_dt_cis + " (" + _7017s[i].chanels_type + j + ")=" + _7017s[i].AIValue[j].ToString();
                                }
                                else
                                {
                                    _7017s[i].AIValue[j] = 0;
                                    find[0].Text = "#" + numb_channel_dt_cis + " (" + _7017s[i].chanels_type + j + ")=" + "0";
                                }
                            }
                        }
                    }
                }

            
                timer5.Start();
            }

        //public void read_counter_7080(int i)
        //{
        //    //int i = 0;
        //    iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send, _7080s[i].recieve[j], _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
        //    //iRet = DCON.UART.Send_Receive_Cmd(iComport, bytes, r, time, stop, out wt);
        //}
        
            




        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_Click_1(object sender, EventArgs e)
        {
            
            //Поиск i-7017,i-7052,i-7080
            finder_ixxx(this,e);

            //Чтение i-7017,i-7052,i-7080
            timer5.Enabled = true;
            if (checkBox4.Checked)
            {
                checkBox5.Enabled = true;
                button3.Enabled = true;
                timer5.Start();
                checkBox4.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                checkBox5.Enabled = false;
                button3.Enabled = false;
                timer5.Stop();
                checkBox4.BackColor = System.Drawing.Color.LightGray;
            }
        }
      
        
        private void checkBox5_Click(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                //Инициализация переменных
                SelfHostName = textBox5.Text;
                appID = Convert.ToInt32(textBox2.Text);
                AppIDName = textBox1.Text;
                chanCount = numb_reserve;


                //Создание DCOM объекта
                device = new Client(SelfHostName);
                //Регистрация на Сервере кодов через DCOM
                device.Identify(appID, SelfHostName, AppIDName, chanCount);

                //Если соединись с Сервером кодов, запускаем передачу данных по таймеру3
                if (device.Connected)
                {
                    timer3.Start();
                    checkBox5.BackColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                checkBox5.BackColor = System.Drawing.Color.LightGray;
                //Отключение!
                timer3.Stop();
                device.Dispose();
                Application.Exit();

            }
            ////Realtime.ViewServiceDataFormatEnum.frmtVariantArray = 1;
            ////timer5.Enabled = true;
            //if (checkBox5.Checked)
            //{
            //checkBox5.BackColor = System.Drawing.Color.Green;
            ////dtcis= new CheckConnection();
            ////param.ad
            ////Listitems[0] = 10;

            ////Listitems = (Realtime.ViewServiceDataFormatEnum.frmtVariantArray)param;
            ////Realtime.ViewServiceDataFormatEnum.frmtVariantArray = param;
            ////Подключение к DTCIS
            ////param = 1;
            ////ar[0] = 1; ar[2] = 3;
            ////dtcis.ViewerInfo("VERONICA", "VERONICA");
            ////dtcis.SetModes(true, true);
            ////dtcis.SetDataTransferFormat(Realtime.ViewServiceDataFormatEnum.frmtVariantArray);
            ////dtcis.ClearList();
            ////dtcis.SetListItems(ref param);
            //////dtcis.Advise(dtcis);
            //////dtcis.GetListData(out  answer);


            //////dtcis.GetListData(out  dataarray);
            ////    //out DataArray
            //////Listitems = 10;
            //////Realtime.ViewServiceParamInfoEnum it = new ViewServiceParamInfoEnum();
            //////dtcis.SetListItems(ref Listitems);
            //////dtcis.Advise(dtcis_event);
            ////dtcis_event.EvDisconnectAndTest(false);
            ////dtcis_event.EvNewDataReady();
            //////dtcis.Advise(dtcis_event);
            //////Realtime.IIMSEventsService dtcisss;
            //////    //dtcisss.EvAutoSendData(
            ////dtcis.GetDataByList(param, out answer);
            ////dtcis.GetDataByList(param, out answer);
            ////dtcis.SetListItems(param);
            ////dtcis.GetListData(out answer);
            ////dtcis.SetListItems(param);
            //int a = 10;
            //a = 1000;
            ////dtcis.ge
            ////dtcis.CheckConnection();
            //}
            //else {
            //checkBox5.BackColor = System.Drawing.Color.Gray;
            ////dtcis.Unadvise();
              
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Поиск i-7017,i-7052,i-7080
            finder_ixxx(this, e);
            this.tabControl1.SelectedTab=this.tabPage3;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            tabPage1.Parent = null;
            tabPage4.Parent = null;
            //Чтение настроек
            Properties.Settings.Default.Reload();
            textComport.Text = Properties.Settings.Default.Textcomport;
            textBox2.Text = Properties.Settings.Default.Textbox2;
            textBox11.Text = Properties.Settings.Default.Textbox11;
            textBox14.Text = Properties.Settings.Default.Textbox14;
            textBox15.Text = Properties.Settings.Default.Textbox15;
            textBox16.Text = Properties.Settings.Default.Textbox16;
            textBox8.Text = Properties.Settings.Default.Textbox8;
            textBox9.Text = Properties.Settings.Default.Textbox9;
            textBaudrate.Text = Properties.Settings.Default.textBaudrate;
            textSlot.Text = Properties.Settings.Default.textSlot;
            textChecksum.Text = Properties.Settings.Default.textChecksum;
            textBox1.Text = Properties.Settings.Default.textBox1;
            textBox5.Text = Properties.Settings.Default.textBox5;
            textTotalChannel.Text = Properties.Settings.Default.textTotalChannel;
            textTimeout.Text = Properties.Settings.Default.textTimeout;
            textBox13.Text = Properties.Settings.Default.textBox13;
            textBox12.Text = Properties.Settings.Default.textBox12;
            textBox17.Text = Properties.Settings.Default.textBox17;
            textBox18.Text = Properties.Settings.Default.textBox18;
            textBox19.Text = Properties.Settings.Default.textBox19;
            textBox20.Text = Properties.Settings.Default.K_talblok.ToString();
            K_talbok = Properties.Settings.Default.K_talblok;
            //opt.ReadXML(out treeView1);
        }

        //Сброс счетчиков в 0 
        public void reset_counters_7080s(bool A, bool B) {
            bool on_timer = false;
            if (timer5.Enabled)
            {
                timer5.Enabled = false;
                on_timer = true;
            }
            if (_7080s.Count > 0)
            {
                //Несколько 7080
                for (int i = 0; i < _7080s.Count; i++)
                {
                    DCON.UART.Open_Com(_7080s[i].com, _7080s[i].baudrate, 8, 0, 1);

                    //Число каналов
                    for (int j = 0; j < 1; j++)
                    {
                        //Сброс первого канала
                        if (A)
                        {
                            if (_7080s[i].address < 16)
                            {
                                command = "@0" + _7080s[i].address.ToString("X") + "P" + 0 + "00000000";
                            }
                            else
                            {
                                command = "@" + _7080s[i].address.ToString("X") + "P" + 0 + "0000000";
                            }
                            _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                            
                            if (_7080s[i].address < 16)
                            {
                                command = "$0" + _7080s[i].address.ToString("X") + 6 + 0;
                            }
                            else
                            {
                                command = "$" + _7080s[i].address.ToString("X") + 6 + 0;
                            }
                            _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                        }
                        //Сброс второго канала
                        if (B)
                        {
                            if (_7080s[i].address < 16)
                            {
                                command = "@0" + _7080s[i].address.ToString("X") + "P" + 1 + "00000000";
                            }
                            else
                            {
                                command = "@" + _7080s[i].address.ToString("X") + "P" + 1 + "0000000";
                            }
                            _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                            if (_7080s[i].address < 16)
                            {
                                command = "$0" + _7080s[i].address.ToString("X") + 6 + 1;
                            }
                            else
                            {
                                command = "$" + _7080s[i].address.ToString("X") + 6 + 1;
                            }
                            _7080s[i].send_0_channel = Encoding.ASCII.GetBytes(command);
                            _7080s[i].iRet = DCON.UART.Send_Receive_Cmd(_7080s[i].com, _7080s[i].send_0_channel, _7080s[i].recieve_0_channel, _7080s[i].time, _7080s[i].stop, out _7080s[i].wt);
                        }
                    }
                }
            }
            if (on_timer)
            {
                timer5.Enabled = true;
            }
        }
        //public string hexxer;
        //Сброс счетчиков
        private void button2_Click(object sender, EventArgs e)
        {
            reset_counters_7080s(true,true);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //opt.WriteXML(ref treeView1);
        }

        //Посылка на табло
        public void post_to_table() {
            //Настройка порта
            iComport = Convert.ToByte(textBox17.Text);
            iBaudrate = Convert.ToUInt32(textBox18.Text);
            iTimeout = Convert.ToInt16(textBox19.Text);
            //Параметры
            //Тальблок: адрес=7,сегментов=3,цифра 00.0, 
            //[адрес,значение]
            string t_block = "[" + Convert.ToChar(7) + "14.0"+"]";
            //Тальблок: адрес=15,сегментов=31,гистограмма 00.0, 31 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string t_block_gis = "[" + Convert.ToChar(15) + Convert.ToChar(0) + Convert.ToChar(31) + Convert.ToChar(5) + Convert.ToChar(15) + "]";
            //Скорость инструмента: адрес=11,сегментов=3,цифра 0.00
            string v_instr = "[" + Convert.ToChar(11) + "25.0" + "]";
            //Над забоем: адрес=12,сегментов=4,цифра 000.0
            string nad_zabojem = "[" + Convert.ToChar(12) + "111.1" + "]";
            //Глубина: адрес=8,сегментов=5,цифра 0000.0
            string glubina = "[" + Convert.ToChar(8) + "2222.2" + "]";
            //Общий объем раствора(Плотность): адрес=3,сегментов=3,цифра 00.0
            string objem = "[" + Convert.ToChar(3) + "33.3" + "]";
            //Расход: адрес=2,сегментов=3,цифра 00.0
            string rashod = "[" + Convert.ToChar(2) + "44.4" + "]";
            //Ходы насоса: адрес=0,сегментов=3,цифра 00.0
            string xodiy = "[" + Convert.ToChar(0) + "13.4" + "]";
            //Давление на входе: адрес=16,сегментов=31,гистограмма 00.0, 31 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string davlenie_gis = "[" + Convert.ToChar(16) + Convert.ToChar(0) + Convert.ToChar(31) + Convert.ToChar(5) + Convert.ToChar(15) + "]";
            //Давление на входе : адрес=6,сегментов=3,цифра 00.0
            string davlenie = "[" + Convert.ToChar(6) + "60.4" + "]";
            //Вес на крюке : адрес=10,сегментов=4,цифра 000.0
            string ves = "[" + Convert.ToChar(10) + "120.4" + "]";
            //Вес на крюке:(Два сегмента) адрес=18,сегментов=31,круговая гистограмма 00.0, 31 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string ves_gis_1 = "[" + Convert.ToChar(18) + Convert.ToChar(0) + Convert.ToChar(11) + Convert.ToChar(0) + Convert.ToChar(0xFF) + "]";
            //Вес на крюке: адрес=19,сегментов=30,гистограмма 00.0, 30 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string ves_gis_2 = "[" + Convert.ToChar(19) + Convert.ToChar(0) + Convert.ToChar(0xFF) + Convert.ToChar(0xFF) + Convert.ToChar(0x1F) + "]";
            //Нагрузка на долото: адрес=14,сегментов=31,гистограмма 00.0, 31 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string nagruzka_gis = "[" + Convert.ToChar(14) + Convert.ToChar(1) + Convert.ToChar(10) + Convert.ToChar(5) + Convert.ToChar(15) + "]";
            //Нагрузка на долото : адрес=5,сегментов=3,цифра 00.0
            string nagruzka = "[" + Convert.ToChar(5) + "23.7" + "]";
            //Момент на роторе : адрес=1,сегментов=3,цифра 0.00
            string moment = "[" + Convert.ToChar(1) + "1.67" + "]";
            //Момент на роторе: адрес=13,сегментов=31,гистограмма 00.0, 31 диод
            //[адрес,зеленый/красный,число диодов знач.,число диодов нижн.уставка,число диодов верх.уставка], отключение сегментов 0xFF
            string moment_gis = "[" + Convert.ToChar(13) + Convert.ToChar(0) + Convert.ToChar(30) + Convert.ToChar(5) + Convert.ToChar(15) + "]";
            //Обороты ротора : адрес=4,сегментов=3,цифра 00.0
            string oboroti = "[" + Convert.ToChar(4) + "20.67" + "]";
            //Яркость табла
            string brigth_tablo= "[*" + Convert.ToChar(0x0e) + "]";

            //Строка параметров
            string post = t_block + t_block_gis +  v_instr + nad_zabojem +
                glubina + objem + rashod + xodiy +
                davlenie_gis + davlenie + ves +
                nagruzka_gis + nagruzka + oboroti + moment + moment_gis +
                brigth_tablo + ves_gis_1+ ves_gis_2;
            char len_string = Convert.ToChar(Convert.ToByte(post.Length));
            //Строка с заголовком и параметрами
            string all_post = ":" + len_string + post;
            byte[] buff = Encoding.ASCII.GetBytes(all_post);
            //buff[21] = 0xff;
            //buff[26] = 0xff;
            //buff[27] = 0xff;
            //Посылка на табло
            DCON.UART.Open_Com(iComport, iBaudrate, 8, 0, 1);
            DCON.UART.Send_Binary(iComport, buff, buff.Length);
            DCON.UART.Close_Com(iComport);
        
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                //Посылка на табло
                post_to_table();
                timer7.Start();
            }
            else { 
                timer7.Stop(); 
                DCON.UART.Close_Com(iComport); }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            post_to_table();
        }

        private void tabControl1_Deselected(object sender, TabControlEventArgs e)
        {
            Properties.Settings.Default.Textcomport=textComport.Text;
            Properties.Settings.Default.Textbox2= textBox2.Text;
            Properties.Settings.Default.Textbox11= textBox11.Text;
            Properties.Settings.Default.Textbox14= textBox14.Text;
            Properties.Settings.Default.Textbox15= textBox15.Text;
            Properties.Settings.Default.Textbox16= textBox16.Text;

            Properties.Settings.Default.Textbox8=textBox8.Text ;
            Properties.Settings.Default.Textbox9=textBox9.Text;
            Properties.Settings.Default.textBaudrate=textBaudrate.Text;
            Properties.Settings.Default.textSlot=textSlot.Text;
            Properties.Settings.Default.textChecksum= textChecksum.Text;
            Properties.Settings.Default.textBox1 = textBox1.Text;
            Properties.Settings.Default.textBox5 = textBox5.Text;
            Properties.Settings.Default.textTotalChannel= textTotalChannel.Text;
            Properties.Settings.Default.textTimeout= textTimeout.Text;
            Properties.Settings.Default.textBox13= textBox13.Text;
            Properties.Settings.Default.textBox12= textBox12.Text;
            Properties.Settings.Default.textBox17= textBox17.Text;
            Properties.Settings.Default.textBox18 = textBox18.Text;
            Properties.Settings.Default.textBox19 = textBox19.Text;

           

            Properties.Settings.Default.Save();

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox7_Click(object sender, EventArgs e)
        {
            //string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Your Message ", "Title", "Default Response");

            //    string message = "You did not enter a server name. Cancel this operation?";
            //    string caption = "Error Detected in Input";
            //    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            //    DialogResult result;

            //    // Displays the MessageBox.

            //    result = MessageBox.Show(message, caption, buttons);

            //    if (result == System.Windows.Forms.DialogResult.Yes)
            //    {

            //        // Closes the parent form.

            //        this.Close();

            //    }

            
        }

        //Коэффициент тальблока
        public double K_talbok;
        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            K_talbok = Convert.ToDouble(textBox20.Text);
            Properties.Settings.Default.K_talblok = K_talbok;
            Properties.Settings.Default.Save();
            if (_7080s.Count > 0)
            {
                /*for (int i = 0; i < _7080s.Count; i++)
                {
                    _7080s[i].Value_Base = _7080s[i].Value_Result;
                }*/
            }
        }

    }
}

//Send_Receive_Cmd 
//This  function  sends  a  command  string  to  RS485  Network  and  receives  the  
//response from RS485 Network. If the wCheckSum=1, this function automatically adds the two checksum bytes into the command string and also check the checksum status when  receiving  response  from  the  modules.  Note  that  the  end  of  sending  string  is  added [0x0D] to mean the termination of every command.
//Syntax: 
//Send_Receive_Cmd   (char   cPort,   char   szCmd[],   char   szResult[],   WORD wTimeOut, WORD wCheckSum, WORD *wT) 
//Input Parameter: 
//cPort:  1=COM1,       2=COM2,       3=COM3, 4=COM4 .... , 255=COM255 
//szCmd:   Sending       command       string       
//szResult:   Receiving the response string from the modules 
//wTimeOut:    Communicating  timeout setting, time unit = 1ms 
//wCheckSum:   0=DISABLE,  1=ENABLE  
//*wT:   Total time of send/receive interval, unit = 1 ms 
//Return Value:  NoError : OK 
//Others    :     Error       code       Others       :                     Error       code       


//[7017]
//DI_CHANNEL_COUNT=0
//DO_CHANNEL_COUNT=0
//AI_CHANNEL_COUNT=8
//AO_CHANNEL_COUNT=0
//COUNT_FREQ_CHANNEL_COUNT=0
//DESCRIPTION=8*AI (mA,mV,V)
//FORMDLL=UI_VC
//NAMESPACE=ICPDAS
//CLASS=AI_VC
//LANGUAGE=SAVED

//[7052]
//DI_CHANNEL_COUNT=8
//DO_CHANNEL_COUNT=0
//AI_CHANNEL_COUNT=0
//AO_CHANNEL_COUNT=0
//COUNT_FREQ_CHANNEL_COUNT=0
//DESCRIPTION=8*DI 
//FORMDLL=UI_DIO
//NAMESPACE=ICPDAS
//CLASS=DIO

//[7080]
//DI_CHANNEL_COUNT=0
//DO_CHANNEL_COUNT=2
//AI_CHANNEL_COUNT=0
//AO_CHANNEL_COUNT=0
//COUNT_FREQ_CHANNEL_COUNT=2
//DESCRIPTION=2*Counter/Frequency + 2*DO
//FORMDLL=UI_CntFreq
//NAMESPACE=ICPDAS
//CLASS=CounterFreq
//LANGUAGE=SAVED


//В программу «Сервер Кодов» версия 1-70(314) добавлен сервер приема данных от драйверов по протоколу TCP (сокет-сервер) порт 17235.

//На первые символы // не обращай внимания, это сишный комментарий.
//Текст заключенный <!-- … --> это XML-комментарий, который тоже не надо вставлять в пакет, так как это просто мое пояснение. Для меня не важно оформление и сдвиги как в XML, я все это проглочу, важны начало и конец каждого блока или значения, а также всего пакета. То есть можно писать все в одну строчку или каждый блок на новой строчке со сдвигами.
//<..> - начало блока или значения
//</…> - конец блока или значения

//Пакет Данных в кодовой таблице WINDOWS от клиента:

//  <driver>  - начало пакета
//
//      <!-- Блок информации о драйвере или устройстве, выводимое в окне Сервера Кодов.
//           Блок высылается один раз после установки соединения и при изменении
//           параметров устройства. Без этой информации поступающие данные будут
//           игнорироваться. -->
//      <info>  - начало блока
//          <id>123</id> - идентификатор хранения настроек привязки каналов в сервере (больше 0)
//          <name>Имя драйвера</name>     - наименование
//          <maxchannels>32</maxchannels> - количество каналов (больше 0)
//      </info> - конец блока
//
//      <!-- Блок данных каналов, поступающих от устройства. Список каналов находится
//           в интервале 1...<maxchannels> и может быть не полным, то есть меньше <maxchannels>.
//           - Значение канала <value> может быть только целым числом.
//           - Код состояния канала <status>:
//              0 - ошибка по каналу, значение неопределено
//              1 - корректное значение канала в цифровых кодах или др.величине
//              2 - значение после корректировки канала
//              3 - корректное значение канала в микровольтах
//              4 - корректное значение канала в микроамперах
//           - Дата блока данных <datetime> используется, если устройство является
//             синхронизирующим. -->
//      <data>  - начало блока
//          <datetime>YYYYMMDDHHNNSSsss</datetime> - дата и время блока данных
//          <channel>  - начало блока данных канала
//              <number>1</number>        - номер канала в драйвере
//              <value>1234567890</value> - значение канала
//              <status>1</status>        - состояние значения канала
//          </channel> - конец блока данных канала
//          ...
//      </data> - конец блока
//
//  </driver> - конец пакета

//Например:
//<driver>
//     <info>
//         <id>123</id>
//         <name>Хроматограф СГА-05</name>
//         <maxchannels>8</maxchannels>
//     </info>
//     <data>
//         <datetime>20070205114152200</datetime>   - это время 5.2.2007 11:41:52.200
//         <channel>
//             <number>1</number>
//             <value>1234567890</value>
//             <status>1</status>
//         </channel>
//         <channel>
//             <number>2</number>
//             <value>12345</value>
//             <status>1</status>
//         </channel>
//         <channel>
//             <number>5</number>
//             <value>67890</value>
//             <status>1</status>
//         </channel>
//     </data>
//</driver>

//На каждый такой пакет, распознанный сервером, сервер отвечает результатом операции в виде:

//  <driver>  - начало пакета
//
//      <!-- Блок возвращаемого результата. Блок возвращается клиенту на каждый пакет данных.
//           Список кодов ошибок:
//               больше 0 - успешно. Код возвращается только при приеме блока информации
//                   о драйвере или устройстве. Значение содержит максимальное количество
//                   каналов, которое Сервер Кодов смог выделить для данного устройства.
//               0 - успешно
//              -1 - нет места для размещения данных устройства
//              -2 - ошибка открытия Базы кодов, регистрация данных невозможна
//              -3 - неверная регистрационная информация
//              -4 - дублирование идентификатора. Драйвер с таким идентификатором уже
//                   присоединен к Серверу Кодов -->
//      <result>  - начало блока
//          <code>0</code>              - код выполнения операции
//          <error>Текст ошибки</error> - текст ошибки (необязательно)
//      </result> - конец блока
//
//  </driver> - конец пакета

//Например:
//<driver>
//     <result>
//         <code>0</code>
//         <error>Текст ошибки</error>
//     </result>
//</driver>

//Внешние программы реально-временного про-смотра и обработки данных
//Это программы, которые не возвращают результатов. Такими программами являются про-граммы визуализации, печати и т.п.. Они присоединяются к REALTIME как DCOM-клиенты по интерфейсу IIMSViewService и получают события по интерфейсу IIMSEventService. Интерфейсы описаны в файле REALTIME.TLB.

//IIMSViewService   {11A45860-0657-11D4-AF5F-0050DA79C21E}
//IIMSEventService   {11A45869-0657-11D4-AF5F-0050DA79C21E}

//При присоединении по интерфейсу IIMSViewService программа должна передать в REALTIME следующую информацию о себе с помощью метода IIMSViewService\ViewerInfo со следующими параметрами:
//•   имя программы обработки;
//•   сетевое имя компьютера, на котором работает программа;
//•   адрес переменной, по которой будет возвращен результат выполнения.

//Сервер интерфейса событий создается в клиенте, а в REALTIME пересылается указатель на него с помощью метода IIMSViewService\Advise. Для отсоединения сервера событий перед закрытием соединения вызовите метод IIMSViewService\Unadvise. По интерфейсу событий драйвер получает следующие события:
//•   IIMSEventService\EvDisconnectAndTest «проверка соединения или просьба за-крыть соединение» - эти события отличаются по флагу TestSignal:
//o   TRUE – сервер проверяет соединение, нужно только вернуть OK.
//o   FALSE – сервер просит закрыть соединение, так как он завершает выполне-ние. Рекомендуем восстанавливать соединение не менее чем через 30 се-кунд, так как возможно пользователь перезагружает компьютер.
//•   IIMSEventService\EvNewDataReady «событие прихода новых данных» - это со-бытие отсылается при окончании обработки новых данных.
//•   IIMSEventService\EvAutoSendData «событие автоматической отсылки данных клиенту» - это событие генерируется, если клиент заказал прием данных в автома-тическом режиме. По этому событию сервер отсылает новые данные по предвари-тельно заказанному списку параметров клиенту. Таким образом нет необходимости постоянно опрашивать сервер на наличие новых данных.

//Клиент в любое время при наличии связи с REALTIME может изменять или устанавли-вать режим передачи данных с помощью метода IIMSViewService\SetModes, имеющего следующие параметры:
//•   OnlyModified – флаг передачи только изменившихся значений. Включенный флаг уменьшает загрузку трафика сети и времени на передачу данных.
//•   UseAutoSend – флаг автоматической передачи новых данных. При включенном флаге REALTIME автоматически отсылает новые данные клиенту, то есть клиент работает по событиям, не тратя времени на постоянный или периодический опрос REALTIME об изменении данных. Этот флаг работает только при наличии сервера событий IIMSEventService в клиенте.

//Запрашивать данные REALTIME можно 2-мя способами:
//•   Прием данных по передаваемому списку параметров с помощью метода IIMSViewService\GetDataByList . В параметрах передаются:
//o   ListItems – массив кодов заказываемых параметров. Это значение типа VARIANT, содержащий в себе массив кодов параметров из справочника DTCIS.
//o   DataArray – ссылка на значение типа VARIANT, в который будут записаны значения заказанных параметров в формате, описанном выше.
//o   ResultFlag – флаг выполнения операции.
//•   Прием данных по заказанному списку параметров. Для этого используются сле-дующие методы:
//o   IIMSViewService\ClearList – очистить список заказываемых параметров в сервере;
//o   IIMSViewService\SetListItems – добавить параметры в список заказывае-мых параметров;
//o   IIMSViewService\GetListData – запрос данных по заказанному списку.
//Оба способа не исключают друг друга, то есть при необходимости вы можете переклю-чаться между ними в вашем приложении, при этом список заказываемых параметров не будет изменен.

//Передача данных клиентам осуществляется в виде значения типа VARIANT. В это значе-ние записан массив заказанных параметров. Каждый параметр представлен в виде массива 3-х чисел: код параметра, код единицы измерения, значение. Кодировка пара-метров в соответствии со справочником DTCIS. Для примера приводим код распаковки одного элемента массива присланных данных, написанный в среде C++Builder

//bool GetDataItem(Variant Data, SPVServiceDataItem *DataItem)
//{
//    try {
//        // инициализация
//        ZeroMemory(DataItem, sizeof(SPVServiceDataItem));
//        // распаковать код параметра
//        DataItem->Code = int(Data.GetElement(1));
//        // распаковать код единицы измерения
//        DataItem->UnitCode = int(Data.GetElement(2));
//        // распаковать значение параметра
//        DataItem->Value = Data.GetElement(3);
//        return true;
//        }
//    catch(...)
//        {
//        return false;
//        }
//}

//Передача информации на монитор бурильщика либо еще кудато, это по сути получение текущих данных с DTCIS.
//http://mudlogging.ru/index.php/topic,1691.0.html

//Для монитора бурильщика вообще есть RS485.

//Табло состоит из нескольких блоков цифровой индикации и нескольких блоков отображе-ния гистограмм. Каждый блок цифровой индикации представляет собой строку цифр, длиной до 8 символов. Управление всеми блоками осуществляется по последовательному порту при помощи управляющих строк следующего формата:
//Байт   Содержимое   Описание
//0   Символ ( : )
//(код 3Ah)   Заголовок посылаемого кадра
//1   Длина   Байт определяет длину кадра после этого байта (2 байта заголовка не включаются в длину кадра)
//2 - до кон-ца   Блоки дан-ных   Один или несколько блоков дан-ных, заключенные в квадратные скобки
//При помощи одной кадровой посылки можно отобразить данные как на одном блоке циф-ровой индикации или блоке отображения гистограмм, так и на нескольких.

//При помощи такого же блока данных можно установить яркость индикации. Для этого в отдельном блоке данных в байте адреса следует указать символ * (код 2Ah), за которым следует байт яркости, имеющий значение от 0 до 15.

//Формат блока данных:
//При помощи такого же блока данных можно установить яркость индикации.
//Байт   Содержимое   Описание
//0   Символ ( [ )
//(код 5Bh)   Начальный символ блока
//1   Адрес 0-19   Адрес блока индикации, для кото-рого предназначен блок
//2 … N-1   Строка ото-бражения   Строка цифр для отображения в блоке
//N   Символ ( ] )
//(код 5Dh)   Конечный символ блока
//Кроме цифр в строке могут присутствовать символы:
//AbCdEFhIiJLOPrtUu.-_
//Пример кадра управления блоками цифровой индикации отображает на блоках индикации информацию и устанавливает максимальную яркость свечения:

//: 40h [0 012.34][1 75.5][4 5.6][7 45.3][9 55.23][* 15h]

//Номер блока   Информация
//0   12.34
//1   75.5
//4   5.6
//7   45.3
//9   55.23

//Формат блока данных для блока отображения гис-тограмм
//Данные для блоков отображения гистограмм имеют аналогичный формат. После байта адреса идут 4 байта данных, имеющие следующий смысл:
//1)   Байт формата. Может быть равен 0 или не равен 0. В первом случае это формат "зеленый столбец, красные уставки", во втором случае - это "красный столбец, зе-леные уставки".
//2)   Размер столбца - количество поджигаемых светодиодных полосок. Если столбец поджигать не надо, то этот байт должен быть равен FFh.
//3)   3-й и 4-й байты задают позицию уставок. Если уставки поджигать не надо, то соот-ветствующий байт равен 0FFh.

//Особенности представления данных для круговой диаграммы
//        Круговая диаграмма представляет собой как бы два блока отображения гистограмм, из которых второй является продолжением первого. Для управления круговой диаграммой необходимо так составить блоки управления ими, чтобы не засветить столбец второй диа-граммы, когда отображаемая величина менее половины шкалы, и не засветить лишние ус-тавки (см. выше).

//[Block 0]
//Active=1
//Code=1020
//Address=7
//Type=1
//Name=Т/БЛОК, м
//Factor=1
//Digits=3
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=17.74

//[Block 1]
//Active=1
//Code=1020
//Address=15
//Type=2
//Factor=1
//Digits=32
//Precis=0
//Grid Lines=4
//Minimum=0
//Maximum=40
//Alarm Min=0
//Alarm Max=30
//Address2=0
//Value=17.74

//[Block 2]
//Active=1
//Code=1132
//Address=11
//Type=1
//Name=СКОРОСТЬ ИНСТ,м/с
//Factor=1
//Digits=3
//Precis=2
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=0

//[Block 3]
//Active=1
//Code=1024
//Address=12
//Type=1
//Name=НАД ЗАБОЕМ, м
//Factor=1
//Digits=4
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=0.629999999991924

//[Block 4]
//Active=1
//Code=1022
//Address=8
//Type=1
//Name=ГЛУБИНА, м
//Factor=1
//Digits=5
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=3328.55000000018

//[Block 5]
//Active=0
//Code=1149
//Address=3
//Type=1
//Name=ОБЪЕМ, м3
//Factor=1
//Digits=3
//Precis=0
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=163.63

//[Block 6]
//Active=1
//Code=1160
//Address=3
//Type=1
//Name=ПЛОТНОСТЬ,г/см3
//Factor=1
//Digits=3
//Precis=2
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=1.32

//[Block 7]
//Active=1
//Code=1175
//Address=2
//Type=1
//Name=РАСХОД, л/с
//Factor=1
//Digits=3
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=38.3557525528302

//[Block 8]
//Active=1
//Code=1111
//Address=0
//Type=1
//Name=ХОДЫ, х/мин
//Factor=1
//Digits=3
//Precis=0
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=72.32

//[Block 9]
//Active=1
//Code=1103
//Address=16
//Type=2
//Factor=0.1
//Digits=32
//Precis=0
//Grid Lines=5
//Minimum=0
//Maximum=25
//Alarm Min=1
//Alarm Max=20
//Address2=0
//Value=12.9144

//[Block 10]
//Active=1
//Code=1103
//Address=6
//Type=1
//Name=ДАВЛЕНИЕ, МПа
//Factor=0.1
//Digits=3
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=12.9144

//[Block 11]
//Active=1
//Code=1101
//Address=10
//Type=1
//Name=ВЕС, т
//Factor=1
//Digits=4
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=103.737

//[Block 12]
//Active=1
//Code=1101
//Address=18
//Type=3
//Factor=1
//Digits=63
//Precis=0
//Grid Lines=4
//Minimum=0
//Maximum=200
//Alarm Min=0
//Alarm Max=200
//Address2=19
//Value=103.737

//[Block 13]
//Active=1
//Code=1102
//Address=14
//Type=2
//Factor=1
//Digits=32
//Precis=0
//Grid Lines=3
//Minimum=0
//Maximum=30
//Alarm Min=2
//Alarm Max=20
//Address2=0
//Value=0

//[Block 14]
//Active=1
//Code=1102
//Address=5
//Type=1
//Name=НАГРУЗКА, т
//Factor=1
//Digits=3
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=0

//[Block 15]
//Active=1
//Code=1107
//Address=1
//Type=1
//Name=МОМЕНТ, кН*м
//Factor=1
//Digits=3
//Precis=0
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=28.79

//[Block 16]
//Active=1
//Code=1107
//Address=13
//Type=2
//Factor=1
//Digits=32
//Precis=0
//Grid Lines=3
//Minimum=0
//Maximum=300
//Alarm Min=0
//Alarm Max=200
//Address2=0
//Value=28.79

//[Block 17]
//Active=0
//Code=1200
//Address=1
//Type=1
//Name=Гсум
//Factor=1
//Digits=3
//Precis=1
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=0

//[Block 18]
//Active=0
//Code=1200
//Address=13
//Type=2
//Factor=1
//Digits=32
//Precis=0
//Grid Lines=4
//Minimum=0.01
//Maximum=100
//Alarm Min=0.01
//Alarm Max=10
//Address2=0
//Value=0

//[Block 19]
//Active=1
//Code=1106
//Address=4
//Type=1
//Name=ОБОРОТЫ, об/м
//Factor=1
//Digits=3
//Precis=0
//Grid Lines=0
//Minimum=0
//Maximum=0
//Alarm Min=0
//Alarm Max=0
//Address2=0
//Value=113.54