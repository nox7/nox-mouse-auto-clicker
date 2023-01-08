using Interval_Auto_Clicker.KeyboardUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Interval_Auto_Clicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isRunning = false;

        public MainWindow()
        {
            new GlobalKeyboardHookUtility(this);
            InitializeComponent();
        }

        public void onStartKeyPressed()
        {
            if (!isRunning)
            {
                isRunning = true;
                Debug.WriteLine("Auto clicker is now running.");
            }
            else
            {
                // Ignore, it's already running.
                Debug.WriteLine("Cannot start. Already running.");
            }
        }
    }
}
