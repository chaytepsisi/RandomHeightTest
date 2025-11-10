using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomHeightTest
{
    internal class Stats
    {
        public static readonly int[] binLimits_128 = new int[] { 2, 4, 6, 8, 10, 13, 17, 129 };
        public static readonly int[] binLimits_256 = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
        public static readonly int[] binLimits_1024 = new int[] { 5, 10, 15, 21, 28, 36, 48, 1025 };
        public static readonly int[] binLimits_4096 = new int[] { 10, 20, 31, 43, 57, 74, 98, 4097 };

        public static readonly double[] binProbs_128 = new double[] { 0.139689, 0.135456, 0.127369, 0.116131, 0.102666, 0.128248, 0.117815, 0.132625 };
        public static readonly double[] binProbs_256 = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        public static readonly double[] binProbs_1024 = new double[] { 0.124154, 0.121052, 0.115524, 0.127587, 0.129867, 0.121004, 0.127047, 0.133765 };
        public static readonly double[] binProbs_4096 = new double[] { 0.124147, 0.12116, 0.126569, 0.126457, 0.128536, 0.125499, 0.12189, 0.125743 };
    }
}
