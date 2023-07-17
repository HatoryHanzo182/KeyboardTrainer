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
        KeyboardTrainer.Services.TextGenerator _gtext;

        public MainWindow()
        {
            InitializeComponent();

            _gtext = new KeyboardTrainer.Services.TextGenerator();
        }

        private void Window_TrainerProcess_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock_TrainingText.Text = new string(_gtext._text.ToArray());
        }

        private void Window_HighlightButton_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.Shift) ||
                (char.ToLower(e.Key.ToString()[0]).ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.None) || 
                e.Key.ToString() == $"D{_gtext._text.Peek().ToString()}" || (_gtext._text.Peek().ToString() == " " && e.Key.ToString() == "Space"))
            {
                _gtext._text.Dequeue();
                _gtext._text.Enqueue(_gtext.Genirate());
                TextBlock_TrainingText.Text = new string(_gtext._text.ToArray());
            }
            FindButton(e.Key.ToString());
        }

        private void Window_WringOut_KeyUp(object sender, KeyEventArgs e)
        {
            Button button = FindButton(e.Key.ToString());
            
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
