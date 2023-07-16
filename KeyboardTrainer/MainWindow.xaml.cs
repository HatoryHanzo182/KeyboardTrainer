using System;
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

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_HighlightButton_KeyDown(object sender, KeyEventArgs e)
        {
            char pressed_key = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            Button button = FindButton(pressed_key);

            if (button != null)
                button.Background = Brushes.Orange;
        }

        private void Window_WringOut_KeyUp(object sender, KeyEventArgs e)
        {
            char pressed_key = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            Button button = FindButton(pressed_key);

            if (button != null)
                button.Background = Brushes.Black;
        }

        private Button FindButton(char key)
        {
            foreach (var item in Grid_BoardLayout.Children)
            {
                if (item is Button found && found.Content is string content && content == key.ToString())
                    return found;
            }
            return null;
        }
    }
}
