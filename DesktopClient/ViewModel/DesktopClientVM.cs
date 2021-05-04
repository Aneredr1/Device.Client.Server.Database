﻿using DesktopClient.Model;
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

namespace DesktopClient.ViewModel
{
    class DesktopClientVM : INotifyPropertyChanged
    {

        private List<Message> allMessages;
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
                    while (true)
                    {
                        ClientSocket.CreateConnection();

                        if (ClientSocket.TryAutorize())
                        {
                            OpenDataViewWindowMethod(wnd);
                            ClientSocket.GetMessages();
                            Thread thread = new Thread(GetMessages);
                            thread.Start();
                        }
                        else
                        {
                            bool IsAuth = false;

                            TextBox textBox = wnd.FindName("LoginBlock") as TextBox;
                            PasswordBox passwordBox = wnd.FindName("PassBlock") as PasswordBox;


                            if (textBox.Text == null)
                            {
                                ShowMessage("Введите логин");
                            }
                            else
                            {
                                //IsAuth = DataWorker.Check_authentification(comboBox.Text, passwordBox.Password);
                            }

                            IsAuth = ClientSocket.SendLoginAndPassword(textBox.Text, passwordBox.Password);

                            if (!IsAuth)
                            {
                                ShowMessage("Такой пары логин/пароль нет");
                            }
                        }
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

        void GetMessages()
        {
            while (true)
            {
                if (ClientSocket.newMessage != null)
                {
                    AllMessages.Add(ClientSocket.newMessage);
                    ClientSocket.newMessage = null;
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
