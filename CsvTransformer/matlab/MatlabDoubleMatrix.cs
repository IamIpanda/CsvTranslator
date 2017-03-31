using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvTransformer.matlab
{
    class MatlabDoubleMatrix : IMatlabStructure
    {
        public MatlabDoubleMatrix(List<List<double>> value)
        {
            Value = value;
        }

        public MatlabDoubleMatrix(List<double> value)
        {
            Value = new List<List<double>> {value};
        }

        public MatlabDoubleMatrix(double value)
        {
            Value = new List<List<double>> {new List<double> {value}};
        }

        public override int Type => 9;

        public override int InnerLength => Value.Count == 0 ? 0 : Value.Count * Value[0].Count * 8;

        public override int AllLength => InnerLength + 8;

        public List<List<double>> Value { get; set; }

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            foreach (var l in Value)
                foreach (var d in l)
                    writer.Write(d);
        }
    }
}
