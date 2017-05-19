using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;


namespace Arduino_Com
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort myPort;
        //public ObservableCollection<string> collection;
        public MainWindow()
        {
            InitializeComponent();
            button.Click += listener;
            //collection = new ObservableCollection<string>();
           // listBox.ItemsSource = collection;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            init_Port();
        }
        private void init_Port()
        {
            myPort = new SerialPort(numberPort(), 9600);
            myPort.Open();
        }
        private string numberPort()
        {
            return "COM"+Convert.ToInt32(textBox.Text);
        }
        private void listener(object sender, RoutedEventArgs e)
        {
            myPort.DataReceived += new SerialDataReceivedEventHandler(MyPort_DataReceived);
        }

        private void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender; // why we do it
            string indata = sp.ReadLine();
            try
            {
                // indata used by other thread, how to solve this problem
                //listBox.Items.Add(indata);
                //collection.Add(indata);
                listBox.Dispatcher.Invoke(() => listBox.Items.Add(indata));
            }
            catch(Exception exc) {
                var i = 5;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            myPort.Write(textBox1.Text);
        }
    }
}
