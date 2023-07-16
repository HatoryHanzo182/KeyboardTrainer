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
            string pressed_key = e.Key.ToString();

            Button button = FindButton(pressed_key);
        }

        private void Window_WringOut_KeyUp(object sender, KeyEventArgs e)
        {
            string pressed_key = e.Key.ToString();
            Button button = FindButton(pressed_key);
            
            if (button != null) 
                button.Background = Brushes.Black;
        }

        private Button FindButton(string key)
        {
            foreach (Button item in Grid_BoardLayout.Children)
            {
                if ($"D{item.Content}" == key)
                {
                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01aa49"));
                    return item;
                }
                else if (item.Content.ToString() == key)
                {
                    if (item.Content.ToString() == "Space" || item.Content.ToString() == "LeftShift" || item.Content.ToString() == "Return")
                    {
                        item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a823f6"));
                        return item;
                    }

                    item.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f78900"));
                    return item;
                }
            }
            return null;
        }
    }
}
