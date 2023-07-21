using KeyboardTrainer.Services;
using System.Windows;

namespace KeyboardTrainer
{
    //* 
    // * Modal window for displaying training results in the "KeyboardTrainer" application.
    // * The window is used to show the user information about the number of correctly entered
    // * characters and the time spent on training.
    // * The class takes TrainerData as a constructor parameter to get the results of the current round of training.
    //*

    public partial class ResultModalWindow : Window
    {
        TrainerData _res_data;

        public ResultModalWindow(TrainerData res_data)
        {
            InitializeComponent();

            _res_data = res_data;
        }

        private void Window_Result_Loaded(object sender, RoutedEventArgs e) => 
            Label_Result.Content = $"Your result for this round: {_res_data.Characters} symbols in {_res_data.AmountTime} time\nDo you want again???";

        private void Button_OK_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;

        private void Button_Cancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = false;
    }
}
