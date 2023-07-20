using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardTrainer.Services
{
    public class TrainerData
    {
        public String AmountTime { get; set; }
        public int Characters { get; set; }

        public TrainerData()
        {
            AmountTime = String.Empty;
            Characters = 0;
        }
    }
}
