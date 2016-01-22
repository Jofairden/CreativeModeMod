using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CreativeMode
{
    public static class Tools
    {
        public static int[] FindIndex(this Array haystack, object needle)
        {
            if (haystack.Rank == 1)
                return new[] { Array.IndexOf(haystack, needle) };

            var found = haystack.OfType<object>()
                              .Select((v, i) => new { v, i })
                              .FirstOrDefault(s => s.v.Equals(needle));
            if (found == null)
                throw new Exception("needle not found in set");

            var indexes = new int[haystack.Rank];
            var last = found.i;
            var lastLength = Enumerable.Range(0, haystack.Rank)
                                       .Aggregate(1,
                                           (a, v) => a * haystack.GetLength(v));
            for (var rank = 0; rank < haystack.Rank; rank++)
            {
                lastLength = lastLength / haystack.GetLength(rank);
                var value = last / lastLength;
                last -= value * lastLength;

                var index = value + haystack.GetLowerBound(rank);
                if (index > haystack.GetUpperBound(rank))
                    throw new IndexOutOfRangeException();
                indexes[rank] = index;
            }

            return indexes;
        }

        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }

        public static void FindCoordinates<T>(this T[,] source, int index, out int x, out int y)
        {
            int counter = 0;
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    counter++;
                    if (counter == index)
                    {
                        x = i;
                        y = j;
                        return;
                    }
                }
            }

            x = -1;
            y = -1;
        }

        public static bool GetCommandSlangs(string[,] haystack, string needle)
        {
            for (int i = 0; i < haystack.GetLength(0); i++)
            {
                for (int y = 0; y < haystack.GetLength(1); y++)
                {
                    if (haystack[y, 0].Contains(needle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static IEnumerable<T> SliceRow<T>(this T[,] array, int row)
        {
            for (var i = array.GetLowerBound(1); i <= array.GetUpperBound(1); i++)
            {
                yield return array[row, i];
            }
        }

        public static IEnumerable<T> SliceColumn<T>(this T[,] array, int column)
        {
            for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
            {
                yield return array[i, column];
            }
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }

        public static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append('.');
            }
            return builder.ToString();
        }

        public static string ConvertStringArrayToStringJoin(string[] array)
        {
            //
            // Use string Join to concatenate the string elements.
            //
            string result = string.Join(".", array);
            return result;
        }

        // Thanks for the AMAZING Levenshtein! 
        /* 
            In 1965 Vladmir Levenshtein created a distance algorithm. 
            This tells us the number of edits needed to turn one string into another. 
            With Levenshtein distance, we measure similarity and match approximate strings with fuzzy logic.
        */
        public static int EditDistance(string source, string target, int threshold)
        {

            int length1 = source.Length;
            int length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                Swap(ref target, ref source);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            int[] dCurrent = new int[maxi + 1];
            int[] dMinus1 = new int[maxi + 1];
            int[] dMinus2 = new int[maxi + 1];
            int[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Rotate
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Initialize
                int minDistance = int.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    int cost = source[im1] == target[jm1] ? 0 : 1;

                    int del = dCurrent[im1] + 1;
                    int ins = dMinus1[i] + 1;
                    int sub = dMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;
                }
                jm1++;
                if (minDistance > threshold) { return int.MaxValue; }
            }

            int result = dCurrent[maxi];
            return (result > threshold) ? int.MaxValue : result;
        }

        static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }


        public static int IndexOfMin(this IList<int> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", "self");
            }

            int min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] < min)
                {
                    min = self[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public static bool GetValidPrefix(int type, int pre)
        {
            int[] preArray =  new int[] {   1,
                                            2,
                                            3,
                                            4,
                                            5,
                                            6,
                                            7,
                                            8,
                                            9,
                                            10,
                                            11,
                                            12,
                                            13,
                                            14,
                                            15,
                                            36,
                                            37,
                                            38,
                                            53,
                                            54,
                                            55,
                                            39,
                                            40,
                                            56,
                                            41,
                                            57,
                                            42,
                                            43,
                                            44,
                                            45,
                                            46,
                                            47,
                                            48,
                                            49,
                                            50,
                                            51,
                                            59,
                                            60,
                                            61,
                                            81, };
            int[] preArray1 = new int[] {   36,
                                            37,
                                            38,
                                            53,
                                            54,
                                            55,
                                            39,
                                            40,
                                            56,
                                            41,
                                            57,
                                            59,
                                            60,
                                            61, };
            int[] preArray2 = new int[] {   16,
                                            17,
                                            18,
                                            19,
                                            20,
                                            21,
                                            22,
                                            23,
                                            24,
                                            25,
                                            58,
                                            36,
                                            37,
                                            38,
                                            53,
                                            54,
                                            55,
                                            39,
                                            40,
                                            56,
                                            41,
                                            57,
                                            42,
                                            43,
                                            44,
                                            45,
                                            46,
                                            47,
                                            48,
                                            49,
                                            50,
                                            51,
                                            59,
                                            60,
                                            61,
                                            82, };
            int[] preArray3 = new int[] {   26,
                                            27,
                                            28,
                                            29,
                                            30,
                                            31,
                                            32,
                                            33,
                                            34,
                                            35,
                                            52,
                                            36,
                                            37,
                                            38,
                                            53,
                                            54,
                                            55,
                                            39,
                                            40,
                                            56,
                                            41,
                                            57,
                                            42,
                                            43,
                                            44,
                                            45,
                                            46,
                                            47,
                                            48,
                                            49,
                                            50,
                                            51,
                                            59,
                                            60,
                                            61,
                                            83, };
            int[] preArray4 = new int[] {   36,
                                            37,
                                            38,
                                            53,
                                            54,
                                            55,
                                            39,
                                            40,
                                            56,
                                            41,
                                            57,
                                            59,
                                            60,
                                            61, };

            
            if (type == 0 || pre == 0) return false;
            else if (type == 1 || type == 4 || (type == 6 || type == 7) || (type == 10 || type == 24 || (type == 45 || type == 46)) || (type == 65 || type == 103 || (type == 104 || type == 121) || (type == 122 || type == 155 || (type == 190 || type == 196))) || (type == 198 || type == 199 || (type == 200 || type == 201) || (type == 202 || type == 203 || (type == 204 || type == 213)) || (type == 217 || type == 273 || (type == 367 || type == 368) || (type == 426 || type == 482 || (type == 483 || type == 484)))) || (type == 653 || type == 654 || (type == 656 || type == 657) || (type == 659 || type == 660 || (type == 671 || type == 672)) || (type == 674 || type == 675 || (type == 676 || type == 723) || (type == 724 || type == 757 || (type == 776 || type == 777))) || (type == 778 || type == 787 || (type == 795 || type == 797) || (type == 798 || type == 799 || (type == 881 || type == 882)) || (type == 921 || type == 922 || (type == 989 || type == 990) || (type == 991 || type == 992 || (type == 993 || type == 1123))))) || (type == 1166 || type == 1185 || (type == 1188 || type == 1192) || (type == 1195 || type == 1199 || (type == 1202 || type == 1222)) || (type == 1223 || type == 1224 || (type == 1226 || type == 1227) || (type == 1230 || type == 1233 || (type == 1234 || type == 1294))) || (type == 1304 || type == 1305 || (type == 1306 || type == 1320) || (type == 1327 || type == 1506 || (type == 1507 || type == 1786)) || (type == 1826 || type == 1827 || (type == 1909 || type == 1917) || (type == 1928 || type == 2176 || (type == 2273 || type == 2608)))) || (type == 2341 || type == 2330 || (type == 2320 || type == 2516) || (type == 2517 || type == 2746 || (type == 2745 || type == 3063)) || (type == 3018 || type == 3211 || (type == 3013 || type == 3258) || (type == 3106 || type == 3065 || (type == 2880 || type == 3481))) || (type == 3482 || type == 3483 || (type == 3484 || type == 3485) || (type == 3487 || type == 3488 || (type == 3489 || type == 3490)) || (type == 3491 || type == 3493 || (type == 3494 || type == 3495) || (type == 3496 || type == 3497 || (type == 3498 || type == 3500)))))) || (type == 3501 || type == 3502 || (type == 3503 || type == 3504) || (type == 3505 || type == 3506 || (type == 3507 || type == 3508)) || (type == 3509 || type == 3511 || (type == 3512 || type == 3513) || (type == 3514 || type == 3515 || (type == 3517 || type == 3518))) || (type == 3519 || type == 3520 || (type == 3521 || type == 3522) || (type == 3523 || type == 3524 || type == 3525) || (type >= 3462 && type <= 3466 || type >= 2772 && type <= 2786 || (type == 3349 || type == 3352 || (type == 3351))))))
            {
                return (preArray.Contains(pre)) ? true : false; 
                //((IList<int>)preArray).Clear();
            }
            else if (type == 162 || type == 160 || (type == 163 || type == 220) || (type == 274 || type == 277 || (type == 280 || type == 383)) || (type == 384 || type == 385 || (type == 386 || type == 387) || (type == 388 || type == 389 || (type == 390 || type == 406))) || (type == 537 || type == 550 || (type == 579 || type == 756) || (type == 759 || type == 801 || (type == 802 || type == 1186)) || (type == 1189 || type == 1190 || (type == 1193 || type == 1196) || (type == 1197 || type == 1200 || (type == 1203 || type == 1204)))) || (type == 1228 || type == 1231 || (type == 1232 || type == 1259) || (type == 1262 || type == 1297 || (type == 1314 || type == 1325)) || (type == 1947 || type == 2332 || (type == 2331 || type == 2342) || (type == 2424 || type == 2611 || (type == 2798 || type == 3012))) || (type == 3473 || type == 3098 || (type == 3368))))
            {
                return (preArray1.Contains(pre)) ? true : false;
            }
            else if (type == 39 || type == 44 || (type == 95 || type == 96) || (type == 98 || type == 99 || (type == 120 || type == 164)) || (type == 197 || type == 219 || (type == 266 || type == 281) || (type == 434 || type == 435 || (type == 436 || type == 481))) || (type == 506 || type == 533 || (type == 534 || type == 578) || (type == 655 || type == 658 || (type == 661 || type == 679)) || (type == 682 || type == 725 || (type == 758 || type == 759) || (type == 760 || type == 796 || (type == 800 || type == 905)))) || (type == 923 || type == 964 || (type == 986 || type == 1156) || (type == 1187 || type == 1194 || (type == 1201 || type == 1229)) || (type == 1254 || type == 1255 || (type == 1258 || type == 1265) || (type == 1319 || type == 1553 || (type == 1782 || type == 1784))) || (type == 1835 || type == 1870 || (type == 1910 || type == 1929) || (type == 1946 || type == 2223 || (type == 2269 || type == 2270)) || (type == 2624 || type == 2515 || (type == 2747 || type == 2796) || (type == 2797 || type == 3052 || (type == 2888 || type == 3019))))) || (type == 3029 || type == 3007 || (type == 3008 || type == 3210) || (type == 3107 || type == 3245 || (type == 3475 || type == 3540)) || (type == 3480 || type == 3486 || (type == 3492 || type == 3498) || (type == 3504 || type == 3510 || (type == 3516 || type == 3350))) || (type == 3546)))
            {
                return (preArray2.Contains(pre)) ? true : false;
            }
            else if (type == 64 || type == 112 || (type == 113 || type == (int)sbyte.MaxValue) || (type == 157 || type == 165 || (type == 218 || type == 272)) || (type == 494 || type == 495 || (type == 496 || type == 514) || (type == 517 || type == 518 || (type == 519 || type == 683))) || (type == 726 || type == 739 || (type == 740 || type == 741) || (type == 742 || type == 743 || (type == 744 || type == 788)) || (type == 1121 || type == 1155 || (type == 1157 || type == 1178) || (type == 1244 || type == 1256 || (type == 1260 || type == 1264)))) || (type == 1266 || type == 1295 || (type == 1296 || type == 1308) || (type == 1309 || type == 1313 || (type == 1336 || type == 1444)) || (type == 1445 || type == 1446 || (type == 1572 || type == 1801) || (type == 1802 || type == 1930 || (type == 1931 || type == 2188))) || (type == 2622 || type == 2621 || (type == 2584 || type == 2551) || (type == 2366 || type == 2535 || (type == 2365 || type == 2364)) || (type == 2623 || type == 2750 || (type == 2795 || type == 3053) || (type == 3051 || type == 3209 || (type == 3014 || type == 3105))))) || (type == 2882 || type == 3269 || (type == 3006 || type == 3377) || (type == 3069 || type == 2749 || (type == 3249 || type == 3476)) || (type == 3474 || type == 3531 || (type == 3541 || type == 3542) || (type == 3569 || type == 3570 || (type == 3571 || type == 3531)))))
            {
                return (preArray3.Contains(pre)) ? true : false;
            }
            else if (type == 55 || type == 119 || (type == 191 || type == 284) || (type == 670 || type == 1122 || (type == 1513 || type == 1569)) || (type == 1571 || type == 1825 || (type == 1918 || type == 3054) || (type == 3262 || type >= 3278 && type <= 3292)) || (type >= 3315 && type <= 3317 || (type == 3389 || type == 3030) || (type == 3543)))
            {
                return (preArray4.Contains(pre)) ? true : false;
            }
            else
            {
                return false;
            }
        }
    }
}
