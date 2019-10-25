using Record.Console.App.Contracts.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Record.Console.App.Contracts
{
    public class RecordManager
    {
        public Dictionary<string, string> PathDictionary { get; set; } = new Dictionary<string, string>();

        public static List<string> FormatOptions = new List<string> { "CSV", "TSV" };
        public char FormatSeperator { get; set; }
        public int FieldsPerRecords { get; set; }
        public static string RecordKey => "Records";
        public static string CorrectRecordsKey => "Correct Records";
        public static string IncorrectRecordsKey => "Incorrect Records";

        public (string recordKey, string correctKey, string incorrectKey) GetRecordKeys()
        {
            return (RecordKey, CorrectRecordsKey, IncorrectRecordsKey);
        }

        public bool SetFormat(string format)
        {
            if (!FormatOptions.Any(option => format.Is(option))) return false;

            FormatSeperator = format.Is("csv") ? ',' : '\t';

            return true;
        }

        public bool SetFieldsPerRecord(int fields)
        {
            if (fields <= 0) return false;

            FieldsPerRecords = fields;

            return true;
        }
        public bool SetPath(string key, string path)
        {
            if (PathDictionary.ContainsKey(key))
            {
                return false;
            }

            if (key.Is(RecordKey))
            {
                CreateDefaultPaths(path);
            }

            PathDictionary.Add(key, path);

            return true;
        }

        public string GetTextFromPath(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return PathDictionary[key];
        }
        

        public string GetPath(string key)
        {
            if (!PathDictionary.ContainsKey(key))
            {
                return string.Empty;
            }

            return PathDictionary[key];
        }

        public void ValidateAndWriteRecords(List<string> records)
        {
            // get all valid records
            var invalidRecords = records.Where(record => !record.IsProperFormat(FormatSeperator, FieldsPerRecords)).Select(r => new Record { IsValid = false, Value = r }).ToList();

            WriteRecords(invalidRecords, IncorrectRecordsKey);

            // get all valid records
            var validRecords = records.Where(record => record.IsProperFormat(FormatSeperator, FieldsPerRecords)).Select(r => new Record { IsValid = true, Value = r }).ToList();

            WriteRecords(validRecords, CorrectRecordsKey);

            return;
        }
        private void WriteRecords(List<Record> records, string key)
        {
            if (!records.Any())
            {
                return;
            }

            File.WriteAllText(PathDictionary[key], records.AsString());

            return;
        }

        private void CreateDefaultPaths(string recordPath)
        {
            var recordPathWithoutExtension = recordPath.Substring(0, recordPath.Length - 4);

            SetPath(CorrectRecordsKey, $"{recordPathWithoutExtension}-correct.txt");

            SetPath(IncorrectRecordsKey, $"{recordPathWithoutExtension}-incorrect.txt");
        }

    }
}
