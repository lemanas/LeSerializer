using System;

namespace JsonSerializer.TestObjects
{
    class NameId
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class NameIdWithNumber : NameId 
    {
        public int[] Array { get; set; }
    }

    class DateName : NameId
    {
        public DateTime Date { get; set; }
    }
}
