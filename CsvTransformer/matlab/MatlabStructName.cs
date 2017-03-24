using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvTransformer.matlab
{
    class MatlabStructName : IMatlabStructure
    {
        public override int Type => 1;

        public string Value { get; set; }

        public override int InnerLength => Value.Length;

        public override int AllLength => Convert.ToInt32(Math.Ceiling(Value.Length / 8.0)) * 8 + 8;

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            foreach (var b in Value)
                writer.Write((byte)b);
            var extra_length = (8 - Value.Length % 8) % 8;
            for (var i = 0; i < extra_length; i++)
                writer.Write((byte)0);
        }
    }
}
