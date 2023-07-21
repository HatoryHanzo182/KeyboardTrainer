using KeyboardTrainer.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Text.Json;
using System.IO;

namespace KeyboardTrainer
{
    //*
    // * Application window "KeyboardTrainer".
    // * The window displays the text that the user must enter on the keyboard, as well as a timer that counts down the training time.
    // * The class contains logic to generate random text, handle keystrokes, highlight correctly entered characters,
    // * count the number of characters, and exercise time. When the training time expires, a modal window opens with
    // * the results of the training, where the user can save his best result. The class also implements logic to prevent window
    // * resizing and to update the time format in the timer.
    //*

    public partial class MainWindow : Window
    {
        private KeyboardTrainer.Services.TextGenerator _gtext;
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _current_time = 3 * 60;
        private TrainerData _trainer;
        private TrainerData _results;
        private ResultModalWindow _modal_result;
        
        public MainWindow()
        {
            InitializeComponent();

            _gtext = new KeyboardTrainer.Services.TextGenerator();
            _trainer = new TrainerData();
            _results = new TrainerData();
        }

        private void Window_TrainerProcess_Loaded(object sender, RoutedEventArgs e) 
        {
            MarkSelectedCharacter(new string(_gtext._text.ToArray()));

            GetResults();

            Label_BestResult.Content = $"Your best result: {_results.Characters} symbols, {_results.AmountTime} time";

            StartTimer();
        }

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
            if (CorrectValue(e))
            {
                _gtext._text.Dequeue();
                _gtext._text.Enqueue(_gtext.Genirate());

                MarkSelectedCharacter(new string(_gtext._text.ToArray()));
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

        private bool CorrectValue(KeyEventArgs e)
        {
            if ((e.Key.ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.Shift) ||
                (char.ToLower(e.Key.ToString()[0]).ToString() == _gtext._text.Peek().ToString() && e.KeyboardDevice.Modifiers == ModifierKeys.None) ||
                e.Key.ToString() == $"D{_gtext._text.Peek()}" || (_gtext._text.Peek().ToString() == " " && e.Key.ToString() == "Space"))
                return true;
            return false;
        }

        private void MarkSelectedCharacter(string text)
        {
            Run letters = new Run(text.Substring(0, 1));

            TextBlock_TrainingText.Inlines.Clear();
            letters.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d60000"));
            TextBlock_TrainingText.Inlines.Add(letters);
            letters = new Run(text.Substring(1));
            letters.Foreground = Brushes.White;
            TextBlock_TrainingText.Inlines.Add(letters);
        }

        private void End()
        {
            _modal_result = new ResultModalWindow(_trainer);
            _trainer.AmountTime = $"03:60";

            if(_modal_result.ShowDialog() == true)
            {
                WriteResult();
                GetResults();

                _trainer.Characters = 0;
                _trainer.AmountTime = String.Empty;
                _current_time = 1 * 60;

                _timer.Start();
            }
            else
                this.Close();
            WriteResult();
        }

        private void WriteResult()
        {
            if (_trainer.Characters > _results.Characters)
                File.WriteAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "trainer.json"), JsonSerializer.Serialize(_trainer));
        }

        private void GetResults() =>_results = JsonSerializer.Deserialize<TrainerData>(System.IO.File.ReadAllText("trainer.json"));
        #endregion

        private void Window_TrainerProcess_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
        }
    }
}
