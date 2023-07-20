using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private KeyboardTrainer.Services.TextGenerator _gtext;
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _current_time = 5 * 60;

        public MainWindow()
        {
            InitializeComponent();

            _gtext = new KeyboardTrainer.Services.TextGenerator();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Window_TrainerProcess_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock_TrainingText.Text = new string(_gtext._text.ToArray());
        }

        private void Timer_Tick(object send, EventArgs e)
        {
            if (_current_time > 0)
            {
                _current_time--;

                UpdateTimeLabel();
            }
            else
                _timer.Stop();
        }

        private void Window_HighlightButton_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.Shift) ||
                (char.ToLower(e.Key.ToString()[0]).ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.None) || 
                e.Key.ToString() == $"D{_gtext._text.Peek()}" || (_gtext._text.Peek().ToString() == " " && e.Key.ToString() == "Space"))
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

        private void UpdateTimeLabel()
        {
            TimeSpan time_span = TimeSpan.FromSeconds(_current_time);

            Label_Timer.Content = string.Format("{0:D2}:{1:D2}", time_span.Minutes, time_span.Seconds);
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
