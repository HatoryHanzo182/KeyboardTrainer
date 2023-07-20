using KeyboardTrainer.Services;
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
using System.Text.Json;
using System.IO;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private KeyboardTrainer.Services.TextGenerator _gtext;
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _current_time = 1 * 60;
        private TrainerData _trainer;
        private TrainerData _results;

        public MainWindow()
        {
            InitializeComponent();

            _gtext = new KeyboardTrainer.Services.TextGenerator();
            _trainer = new TrainerData();
            _results = new TrainerData();
        }

        private void Window_TrainerProcess_Loaded(object sender, RoutedEventArgs e) 
        { 
            TextBlock_TrainingText.Text = new string(_gtext._text.ToArray());

            GetResults();

            Label_BestResult.Content = $"Your best result: {_results.Characters} symbols, {_results.AmountTime} time";

            StartTimer();
        }

        private void GetResults() { _results = JsonSerializer.Deserialize<TrainerData>(System.IO.File.ReadAllText("trainer.json")); }

        #region Timer.
        private void StartTimer()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object send, EventArgs e)
        {
            if (_current_time > 0)
            {
                _current_time--;

                UpdateTimeFormat();
            }
            else
            {
                _timer.Stop();

                End();
            }
        }

        private void UpdateTimeFormat()
        {
            TimeSpan time_span = TimeSpan.FromSeconds(_current_time);

            Label_Timer.Content = string.Format("{0:D2}:{1:D2}", time_span.Minutes, time_span.Seconds);
        }
        #endregion

        #region Training Events.
        private void Window_HighlightButton_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.Shift) ||
                (char.ToLower(e.Key.ToString()[0]).ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.None) ||
                e.Key.ToString() == $"D{_gtext._text.Peek()}" || (_gtext._text.Peek().ToString() == " " && e.Key.ToString() == "Space"))
            {
                _gtext._text.Dequeue();
                _gtext._text.Enqueue(_gtext.Genirate());

                TextBlock_TrainingText.Text = new string(_gtext._text.ToArray());
                _trainer.Characters += 1;
            }
            FindButton(e.Key.ToString());
        }

        private void Window_WringOut_KeyUp(object sender, KeyEventArgs e)
        {
            Button button = FindButton(e.Key.ToString());

            if (button != null)
                button.Background = Brushes.Black;
        }
        #endregion

        #region Service methods.
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

        private void End()
        {
            _trainer.AmountTime = $"03:60";

            if(MessageBox.Show($"{_trainer.Characters} characters per {_trainer.AmountTime} time.\nStart over??", "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _current_time = 1 * 60;

                _timer.Start();
            }
            else
                this.Close();
            WriteResult();
        }

        private void WriteResult() => File.WriteAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "trainer.json"), JsonSerializer.Serialize(_trainer));
        #endregion

        private void Window_TrainerProcess_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
        }
    }
}
