using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCDemo.ViewModels.Music
{
    public class RehearsalListItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Nullable<TimeSpan> Duration { get; set; }
        public string Location { get; set; }
    }
}