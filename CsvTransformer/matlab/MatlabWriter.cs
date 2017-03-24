using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CsvTransformer.matlab
{
    class MatlabWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.ASCII;
        protected BinaryWriter binary_writer;
        public MatlabWriter(Stream stream)
        {
            binary_writer = new BinaryWriter(stream);
        }
        
        public void WriteHead()
        {
            var head = "MATLAB 5.0 MAT-file, Platform: PCWIN64";
            binary_writer.Write(head);
            for (var i = head.Length; i < 124; i++)
                binary_writer.Write((byte)0);
            binary_writer.Write((byte)1);
            binary_writer.Write((byte)0x49);
            binary_writer.Write((byte)0x4d);
        }

        public void Write(IMatlabStructure struc)
        {
            struc.WriteToStream(binary_writer);
        }

        public override void Close()
        {
            binary_writer.Close();
            base.Close();
        }
    }
}
