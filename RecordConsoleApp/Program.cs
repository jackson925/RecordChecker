using Record.Console.App.Contracts;
using Record.Console.App.Contracts.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var recordManager = new RecordManager();

            (var recordKey, var correctKey, var incorrectKey) = recordManager.GetRecordKeys();

            // get and set the path for the records file 

            (var key, var path) = GetPathFor(recordKey);

            recordManager.SetPath(key, path);

            recordManager.SetFormat(GetFormat());

            recordManager.SetFieldsPerRecord(GetFieldsPerRecord());

            // read the records file and turn it into a collection of strings

            var recordCollection = File.ReadAllText(recordManager.GetTextFromPath(recordKey)).AsCollectionFromFile();

            // validate record format and write to target output files

            recordManager.ValidateAndWriteRecords(recordCollection);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished writing records to output destinations");
            Console.WriteLine($"Incorrect Records Location: {recordManager.PathDictionary[incorrectKey]}");
            Console.WriteLine($"Correct Records Location: {recordManager.PathDictionary[correctKey]}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadLine();
        }


        static (string key, string path) GetPathFor(string key, bool checkIfExists = true)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"Please provide the path for the {key} file:");
            var path = Console.ReadLine();

            // error checking for if the file exsists where necessary 

            while (checkIfExists && !File.Exists(path))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("File Provided Does Not Exist");

                // repeating this method if provided path is invalid

                (key, path) = GetPathFor(key);
            }

            return (key, path);
        }

        static string GetFormat()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Is the file format CSV (comma-separated values) or TSV (tab-seperated values) Enter CSV or TSV:");
            var format = Console.ReadLine();
            
            // error check for if the provided format is a valid option
            while (!RecordManager.FormatOptions.Any(option => format.Is(option)))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Not An Acceptable Format. {Environment.NewLine}Please provide one of the following options: ({RecordManager.FormatOptions.Aggregate((op1, op2) => $"{op1}, {op2}")})");
                
                // repeating this method if provided format is not valid

                format = GetFormat();
            }

            return format;
        }

        static int GetFieldsPerRecord()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("How many fields should each record have?");
            int fieldsPerRecord;

            try
            {
                // attempting to convert user input to int
                fieldsPerRecord = Convert.ToInt32(Console.ReadLine());
            }
            // if that fails, set `fieldsPerRow` to 0
            catch(Exception ex)
            {
                fieldsPerRecord = 0;
            }

            // error check for if the user provided a reasonable number of fields per row

            while(fieldsPerRecord <= 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"Not An Acceptable Number of Fields.");
                fieldsPerRecord = GetFieldsPerRecord();
            }

            return fieldsPerRecord;
        }
    }
}
