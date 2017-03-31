using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace CsvTransformer.csv
{
    public class CsvReader
    {
        public int Process { get; set; } = 0;
        public List<List<double>> ReadStream(Stream stream, int start_row = 0, int start_column = 3)
        {
            var reader = new System.IO.StreamReader(stream);
            for (var i = 0; i < start_row; i++)
                reader.ReadLine();
            var answer = new List<List<double>>();
            var first_line = Readline(reader, start_column);
            for(var i = 0; i < first_line.Count; i++)
                answer.Add(new List<double>());
            var j = 0;
            first_line.ForEach(number =>
            {
                answer[j].Add(number);
                j++;
            });
            Process = 0;
            while (!reader.EndOfStream)
            {
                Process += 1;
                j = 0;
                Readline(reader, start_column)?.ForEach(number =>
                {
                    answer[j].Add(number);
                    j++;
                });
            }
            return answer;
        }

        public List<double> Readline(StreamReader reader, int start_column = 3)
        {
            var line = reader.ReadLine();
            var parts = line?.Split(',');
            return parts?.Where((part, i) => i > start_column).Select(part => Convert.ToDouble(part)).ToList();
        }
    }
}