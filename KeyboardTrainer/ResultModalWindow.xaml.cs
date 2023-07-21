using KeyboardTrainer.Services;
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
using System.Windows.Shapes;

namespace KeyboardTrainer
{
    public partial class ResultModalWindow : Window
    {
        TrainerData _res_data;

        public ResultModalWindow(TrainerData res_data)
        {
            InitializeComponent();

            _res_data = res_data;
        }

        private void Window_Result_Loaded(object sender, RoutedEventArgs e)
        {
            Label_Result.Content = $"Your result for this round: {_res_data.Characters} symbols in {_res_data.AmountTime} time";
        }
    }
}
