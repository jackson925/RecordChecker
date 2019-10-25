using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Record.Console.App.Contracts.Extensions
{
    public static class RecordExtensions
    {
        public static string AsString(this List<Record> records)
        {
            if (!records.Any())
            {
                return string.Empty;
            }

            return records.Select(s => s.Value).Aggregate((r1, r2) => $"{r1} {Environment.NewLine} {r2}");
        }
    }
}
