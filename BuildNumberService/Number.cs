using System;

namespace BuildNumberService
{
    public class Number
    {
        public string Key { get; set; }
        public int LastVersion { get; set; }
        public DateTime ObtainedAt { get; set; }
    }
}