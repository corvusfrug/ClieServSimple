using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;

namespace client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPAddress ServerIP = null;
        private IPEndPoint ipEndPoint = null;
        private int port = 1001;
        private Socket send = null;
        private string NicName = "Пользователь";
        public MainWindow()
        {
            InitializeComponent();
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            ServerIP = ipHost.AddressList[0];
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            try {
                if (tbAddres.Text.Length > 0)
                    ServerIP = new IPAddress(ParseAdres(tbAddres.Text.ToString()));
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неверно введен IP! "+ ex.Message);
                return;
            }

            try
            {
                if (tbPort.Text.Length > 0)
                    port = int.Parse(tbPort.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверно введен порт! "+ ex.Message);
                return;
            }

            ipEndPoint = new IPEndPoint(ServerIP, port);

            // Создаем сокет
            send = new Socket(ServerIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            send.Connect(ipEndPoint);

            //Лочим и анлочим
            tbAddres.IsEnabled = false;
            tbPort.IsEnabled = false;
            btnConnect.IsEnabled = false;

            tbMessage.IsEnabled = true;
            tbChat.IsEnabled = true;
            btnWrite.IsEnabled = true;

            tbChat.Text += "Для завершения общения с сервером введи сообщение Shutdown\n";
            // Сообщение
            byte[] bytes = new byte[1024];
            int tmpMessage;
            // Получаем ответ от сервера о готовности
            tmpMessage = send.Receive(bytes);
            tbChat.Text += Encoding.UTF8.GetString(bytes, 0, tmpMessage) + "\n";

            // Выключаем и закрываем сокет в EndConnect();

        }

        private byte[] ParseAdres(string arg)
        {
            string[] tmpArr = arg.Split('.');
            byte[] result = new byte[tmpArr.Length];
            
            for (int i=0;i<tmpArr.Length;++i)
            {
                result[i] = byte.Parse(tmpArr[i]);
            }
            return result;
        }

        private void EndConnect()
        {
            // Выключаем и закрываем сокет
            if (send.IsBound)
            {
                send.Shutdown(SocketShutdown.Both);
                send.Close();

                //Лочим и анлочим
                tbAddres.IsEnabled = true;
                tbPort.IsEnabled = true;
                btnConnect.IsEnabled = true;

                tbMessage.IsEnabled = false;
                tbChat.IsEnabled = false;
                btnWrite.IsEnabled = false;
            }
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            
            byte[] bytes = new byte[1024];
            string message = tbMessage.Text.ToString();
            tbMessage.Text = "";
            if (message == "") message = "\n";
            message = NicName + " (Клиент)-> "+ message + "\n";

            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            int bytesSent = send.Send(msg);

            tbChat.Text += message + "\n";
            // Получаем ответ от сервера
            int bytesRec = send.Receive(bytes);

            tbChat.Text += Encoding.UTF8.GetString(bytes, 0, bytesRec);

            // Выходим если введена команда на выключение
            if (message.IndexOf("Shutdown") != -1)
                EndConnect();


        }
    }
}
