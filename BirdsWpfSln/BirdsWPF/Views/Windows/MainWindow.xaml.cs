﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace BirdsWPF.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //public MainWindow(MainWindowViewModel viewModel)
        //{
        //    DataContext = viewModel;
        //    InitializeComponent();
        //}
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}