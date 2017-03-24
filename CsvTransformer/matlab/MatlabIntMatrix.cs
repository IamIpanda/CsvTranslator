using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CsvTransformer.matlab
{
    class MatlabIntMatrix : IMatlabStructure
    {
        public override int Type => 2;

        public override int InnerLength => Value.Count * 4;

        public override int AllLength => InnerLength + 8;

        public List<Int32> Value { get; set; }

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            foreach (var d in Value)
                writer.Write(d);
        }
    }
}
