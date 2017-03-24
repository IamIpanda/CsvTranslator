using System;
using System.Collections.Generic;
using System.Text;

namespace CsvTransformer.matlab
{
    public abstract class IMatlabStructure
    {
        public abstract Int32 Type { get; }
        public abstract Int32 InnerLength { get; }
        public abstract Int32 AllLength { get; }
        public abstract void WriteToStream(System.IO.BinaryWriter writer);
        public void WriteStreamTagHeader(System.IO.BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(InnerLength);
        }
    }
}
