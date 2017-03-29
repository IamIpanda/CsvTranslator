using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvTransformer.matlab
{
    class MatlabSize : IMatlabStructure
    {
        public MatlabSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override int Type => 5;

        public override int InnerLength => 8;

        public override int AllLength => 16;

        public Int32 Width { get; set; }

        public Int32 Height { get; set; }

        public override void WriteToStream(BinaryWriter writer)
        {
            WriteStreamTagHeader(writer);
            writer.Write(Width);
            writer.Write(Height);
        }
    }
}
