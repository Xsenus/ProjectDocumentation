namespace Core.NET.Algorithms
{
    public class ComputeHash
    {
        public ComputeHash(string valueString, byte[] valueByte)
        {
            ValueString = valueString;
            ValueByte = valueByte;
        }

        public string ValueString { get; private set; }
        public byte[] ValueByte { get; private set; }
    }
}
