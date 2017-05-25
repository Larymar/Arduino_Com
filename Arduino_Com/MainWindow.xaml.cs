using System;
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
            Start_button.Click += (sender, e) => myPort.DataReceived += MyPort_DataReceived;
            ALL_COM_SHOW();
            CBinit();
        }

        private void Start_button_Click(object sender, RoutedEventArgs e)
        {
            init_Port();
        }
        private void init_Port()
        {
            int baud;
            if (COMports.SelectedItem.ToString() != "COMPort" && !portstatus && int.TryParse(Baud.Text,out baud))
            {
                myPort = new SerialPort(COMports.SelectedItem.ToString(),baud );
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
            myPort.Read(buff, 0, buff.Length);
            var indata = Encoding.UTF8.GetString(buff);
           // var indata = myPort.ReadLine();

            // indata used by other thread, how to solve this problem
            //listBox.Dispatcher.Invoke(() => listBox.Items.Add(indata));
            this.Dispatcher.Invoke(() => log.Text += indata);
            Thread.Sleep(10);
        }
    
        private void CBinit()
        {
            File.Items.Add("File");
            File.SelectedIndex = 0;
        }

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {
            myPort.Write(Input_box.Text);
            
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
                COMports.SelectedIndex = 1;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
