using DesktopClient.Model;
using DesktopClient.Model.Sockets;
using DesktopClient.Models;
using DesktopClient.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DesktopClient.ViewModel
{
    class DesktopClientVM : INotifyPropertyChanged
    {
        public Thread FillTable;

        public DesktopClientVM()
        {
            FillTable = new Thread(GetMessages);
        }

        private List<Message> allMessages = new List<Message>();
        public List<Message> AllMessages
        {
            get { return allMessages; }
            set
            {
                allMessages = value;
                NotifyPropertyChanged("AllMessages");
            }
        }

        private void OpenDataViewWindowMethod(Window wnd)
        {
            DataViewWindow dataWindow = new DataViewWindow();
            dataWindow.Show();
            wnd.Close();
        }

        private RelayCommand openDataViewWindow;
        public RelayCommand OpenDataViewWindow
        {
            get
            {
                return openDataViewWindow ?? new RelayCommand(obj =>
                {
                    Window wnd = obj as Window;
                    bool IsAuth = false;

                    TextBox textBox = wnd.FindName("LoginBlock") as TextBox;
                    PasswordBox passwordBox = wnd.FindName("PassBlock") as PasswordBox;

                    if (textBox.Text == null)
                    {
                        ShowMessage("Введите логин");
                    }
                    else
                    {
                        ClientSocket.CreateConnection();
                        IsAuth = ClientSocket.TryAutorize(textBox.Text, passwordBox.Password);
                    }
                    if (IsAuth)
                    {
                        OpenDataViewWindowMethod(wnd);

                        ClientSocket.StartReceiveMessages();
                        FillTable.Start();

                    }
                    else ShowMessage("Такой пары логин/пароль нет");
                        
                }
                    );
            }
        }

        private RelayCommand stopReceive;
        public RelayCommand StopReceive
        {
            get
            {
                return stopReceive ?? new RelayCommand(obj =>
                {
                    if (ClientSocket.StartRecieve.IsAlive)
                    {
                        ClientSocket.StartRecieve.Abort();
                        ClientSocket.clientSocket.Close();
                    }
                }
                );
            }
        }

        private RelayCommand resumeReceive;
        public RelayCommand ResumeReceive
        {
            get
            {
                return resumeReceive ?? new RelayCommand(obj =>
                {
                    if (!ClientSocket.StartRecieve.IsAlive)
                    {
                        if (!ClientSocket.StartReceiveMessages())
                        {
                            AuthWindow authWnd = new AuthWindow();
                        }
                        ;
                    }
                }
                );
            }
        }

        private void UpdateView()
        {
            DataViewWindow.AllMessagesView.ItemsSource = null;
            DataViewWindow.AllMessagesView.Items.Clear();
            DataViewWindow.AllMessagesView.ItemsSource = AllMessages;
            DataViewWindow.AllMessagesView.Items.Refresh();
        }

        private void ShowMessage(string message)
        {
            MessageWindow messageView = new MessageWindow(message);
            SetCenterPositionAndOpen(messageView);
        }

        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public void GetMessages()
        {
            while (true)
            {
                if (ClientSocket.newMessage != null)
                {
                    AllMessages.Add(ClientSocket.newMessage);
                    ClientSocket.newMessage = null;
                    Application.Current.Dispatcher.Invoke(new System.Action(() => UpdateView())); 
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
