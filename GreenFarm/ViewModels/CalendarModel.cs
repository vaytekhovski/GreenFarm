using System;
using System.Collections.Generic;
using GreenFarm.Models;

namespace GreenFarm.ViewModels
{
    public class CalendarModel
    {
        public CalendarModel()
        {
            Today = new List<OrderElement>();
            Tomorrow = new List<OrderElement>();
            DayAfterTomorrow = new List<OrderElement>();
            Next = new List<OrderElement>();
        }

        public List<OrderElement> Today { get; set; }
        public List<OrderElement> Tomorrow { get; set; }
        public List<OrderElement> DayAfterTomorrow { get; set; }
        public List<OrderElement> Next { get; set; }



    }
}
