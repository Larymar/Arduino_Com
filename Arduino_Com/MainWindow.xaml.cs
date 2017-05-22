﻿using System;
using System.Text;
using System.IO.Ports;
using System.Windows;


namespace Arduino_Com
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region string LogText

        public string LogText
        {
            get { return (string)GetValue(LogTextProperty); }
            set { SetValue(LogTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogTextProperty =
            DependencyProperty.Register("LogText", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        #endregion

        private SerialPort myPort;
        public MainWindow()
        {
            InitializeComponent();
            button.Click += listener;
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
            return "COM" + Convert.ToInt32(textBox.Text);
        }
        private void listener(object sender, RoutedEventArgs e)
        {
            myPort.DataReceived += MyPort_DataReceived;
        }

        private void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var buff = new byte[myPort.BytesToRead];
            myPort.Read(buff, 0, buff.Length);
            var indata = Encoding.UTF8.GetString(buff);
            try
            {
                // indata used by other thread, how to solve this problem
                //listBox.Dispatcher.Invoke(() => listBox.Items.Add(indata));
                this.Dispatcher.Invoke(() => this.LogText += indata);
            }
            catch { }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            myPort.Write(textBox1.Text);
            textBox1.Clear();
        }
    }
}
