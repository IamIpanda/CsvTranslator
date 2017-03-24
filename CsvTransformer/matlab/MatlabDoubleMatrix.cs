using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvTransformer.matlab
{
    class MatlabDoubleMatrix : IMatlabStructure
    {
        public override int Type => 9;

        public override int InnerLength => Value.Count * 8;

        public override int AllLength => InnerLength + 8;

        public List<Double> Value { get; set; }

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            foreach (var d in Value)
                writer.Write(d);
        }
    }
}
