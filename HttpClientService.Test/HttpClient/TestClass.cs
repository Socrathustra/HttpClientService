using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientService.Test.Helpers
{
    public class TestClass : IComparable
    {
        public string A { get; set; }
        public int B { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is TestClass other)
            {
                return (this.A == other.A && this.B == other.B) ? 0 : this.B > other.B ? 1 : -1;
            }

            return -1;
        }
    }
}
