using System;
using System.Collections.Generic;

namespace BlazorApp.Shared
{
    public class Flashcard
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
