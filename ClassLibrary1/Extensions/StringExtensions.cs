using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Record.Console.App.Contracts.Extensions
{
    public static class StringExtensions
    {
        public static List<string> AsCollectionFromFile(this string text)
        {
            var recordCollection = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            // remove header string 
            recordCollection.RemoveAt(0);

            return recordCollection;
        }
        public static bool IsProperFormat(this string text, char seperator, int fieldCount)
        {
            // edge case for almost properly formated records
            if (text.StartsWith(",") || text.StartsWith("\t") || text.EndsWith(",") || text.EndsWith("\t")) return false;

            // seperators should be between fields, so there should always be one less seperator than fields
            var correctSeparatorCount = text.Count(s => s == seperator) == fieldCount - 1;
            var correctFieldCount = text.Split(seperator).Length == fieldCount;


            return (!correctSeparatorCount || !correctFieldCount) ? false : true;
        }

        public static bool Is(this string text, string comparer)
        {
            return text.Equals(comparer, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
