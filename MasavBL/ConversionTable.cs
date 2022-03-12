using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasavBL
{
    public class ConversionTable
    {
        private static List<KeyValuePair<char, char>> keyValuePairs = new List<KeyValuePair<char, char>>{
            new KeyValuePair<char, char>('&','א'),
            new KeyValuePair<char, char>('A', 'ב'),
            new KeyValuePair<char, char>('B', 'ג'),
            new KeyValuePair<char, char>('C', 'ד'),
            new KeyValuePair<char, char>('D', 'ה'),
            new KeyValuePair<char, char>('E', 'ו'),
            new KeyValuePair<char, char>('F', 'ז'),
            new KeyValuePair<char, char>('G', 'ח'),
            new KeyValuePair<char, char>('H', 'ט'),
            new KeyValuePair<char, char>('I', 'י'),
            new KeyValuePair<char, char>('J', 'ך'),
            new KeyValuePair<char, char>('K', 'כ'),
            new KeyValuePair<char, char>('L', 'ל'),
            new KeyValuePair<char, char>('M', 'ם'),
            new KeyValuePair<char, char>('N', 'מ'),
            new KeyValuePair<char, char>('O', 'ן'),
            new KeyValuePair<char, char>('P', 'נ'),
            new KeyValuePair<char, char>('Q', 'ס'),
            new KeyValuePair<char, char>('R', 'ע'),
            new KeyValuePair<char, char>('S', 'ף'),
            new KeyValuePair<char, char>('T', 'פ'),
            new KeyValuePair<char, char>('U', 'ץ'),
            new KeyValuePair<char, char>('V', 'צ'),
            new KeyValuePair<char, char>('W', 'ק'),
            new KeyValuePair<char, char>('X', 'ר'),
            new KeyValuePair<char, char>('Y', 'ש'),
            new KeyValuePair<char, char>('Z', 'ת') };
        

        public static char ConvertFromHebrew (char value)
        {
            var r = keyValuePairs.Find(i => i.Value == value);
            return r.Key == 0? value: r.Key;
        }

        public static char ConvertFromEnglish(char key)
        {
            var r = keyValuePairs.Find(i => i.Key == key);
            return r.Key == 0 ? key : r.Value;
        }

        public static string ConvertFromHebrew(string value)
        {
            var charList = new List<char>();
            foreach (var c in value.ToCharArray())
                charList.Add(ConvertFromHebrew(c));
            return new string(charList.ToArray());
        }

        public static string ConvertFromEnglish(string value)
        {
            var charList = new List<char>();
            foreach (var c in value.ToCharArray())
                charList.Add(ConvertFromEnglish(c));
            return new string(charList.ToArray());
        }
    }
}
