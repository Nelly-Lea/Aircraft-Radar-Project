﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AirTraffic.Radar;
//On peut mettre les fenetres ds PL, et pa sujet elle a mis les 3 couches model, viewmodel et view
// elle a fait des dossiers par fenetre
namespace AirTraffic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RadarView radarview;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonRadar(object sender, RoutedEventArgs e)
        {
            if (!(MainWindowUC.Content is RadarView))
            {
                radarview = new RadarView();
            }
            MainWindowUC.Content = radarview;

        }
    }
}
