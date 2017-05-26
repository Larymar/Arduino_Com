using System;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows;
using System.Threading;

namespace Arduino_Com
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool portstatus = false;
        private SerialPort myPort;

        public MainWindow()
        {
            InitializeComponent();
            Start_button.Click += (sender, e) => { if (portstatus) myPort.DataReceived += MyPort_DataReceived; };
            ALL_COM_SHOW();
        }

        private void Start_button_Click(object sender, RoutedEventArgs e)
        {
            init_Port();
        }
        private void init_Port()
        {
            int baud;
            var baudstat = int.TryParse(Baud.Text, out baud);
            if (!baudstat && !portstatus)
            {
                MessageBox.Show("Проверьте значение baud,\n и введите его", "Baud",MessageBoxButton.OK,MessageBoxImage.Warning);
                Baud.Text = "baud";
            }
            if(!portstatus && COMports.SelectedItem.ToString() == "COMPort")
            {
                MessageBox.Show("Проверьте, выбран ли порт,\n и подключена ли плата", "Port", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (COMports.SelectedItem.ToString() != "COMPort" && !portstatus && baudstat)
            {
                int i = 5;
                if (myPort != null)
                    myPort.DataReceived -= this.MyPort_DataReceived;

                myPort = new SerialPort(COMports.SelectedItem.ToString(), baud);
                myPort.Open();
                Start_button.Content = "Закрыть";
                portstatus = true;
            }
            else
            {
                if (portstatus)
                {
                    myPort.Close();
                    Start_button.Content = "Открыть";
                    portstatus = false;
                }
            }

        }



        private void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buff = new byte[myPort.BytesToRead];

            var cb = myPort.Read(buff, 0, buff.Length);

            // System.Diagnostics.Debug.Print(string.Join(" ", buff.Take(cb).Select(n => Convert.ToString(n, 16).PadLeft(2, '0'))));
            var indata = Encoding.UTF8.GetString(buff, 0, cb);
            // var indata = myPort.ReadLine();

            // indata used by other thread, how to solve this problem
            //listBox.Dispatcher.Invoke(() => listBox.Items.Add(indata
            //TODO: Поправить ошибку
            Dispatcher.InvokeAsync(() => {
                log.Text += indata;
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            // Thread.Sleep(10);
        }

     

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {
            string sendtext = Input_box.Text.ToString();
            
            myPort.Write(sendtext);

        }
        private void ALL_COM_SHOW()
        {
            COMports.Items.Clear();
            string[] ports = SerialPort.GetPortNames();

            COMports.Items.Add("COMPort");
            foreach (string line in ports)
            {
                COMports.Items.Add(line);
            }

            COMports.SelectedIndex = 0;
            if (ports.Length > 1)
            {
                COMports.SelectedIndex = 2;
            }

            //System.Diagnostics.Process.Start()
        }

        private void log_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            log.SelectionStart = log.Text.Length;
            log.ScrollToEnd();
        }

        private void Input_box_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Input_box.Clear();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            log.Clear();
        }

        private void COMports_MouseDown(object sender, EventArgs e)
        {
            ALL_COM_SHOW();
        }
    }
}
