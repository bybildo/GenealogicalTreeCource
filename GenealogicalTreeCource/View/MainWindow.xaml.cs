﻿using GenealogicalTreeCource.Class;
using System.Windows;
using System.Windows.Input;

namespace GenealogicalTreeCource
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PersonTree personTree = new PersonTree();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            AdministratorButton.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            AdministratorButton.Visibility = Visibility.Hidden;
        }
    }
}
