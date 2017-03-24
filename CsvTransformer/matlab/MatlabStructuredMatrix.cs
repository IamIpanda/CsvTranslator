using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvTransformer.matlab
{
    class MatlabStructuredMatrix : IMatlabStructure
    {
        public override int Type => 14;

        public override int InnerLength => Value.AllLength + 48;

        public override int AllLength => InnerLength;

        public MatlabSize Size { get; set; }

        public MatlabStructName Name { get; set; }

        public IMatlabStructure Value { get; set; }

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            // 拼造格
            writer.Write((Int32)6);
            writer.Write((Int32)8);
            writer.Write((Int32)6);
            writer.Write((Int32)0);
            Size.WriteToStream(writer);
            Name.WriteToStream(writer);
            Value.WriteToStream(writer);
        }
    }
}
