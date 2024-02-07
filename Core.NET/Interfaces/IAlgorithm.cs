using Core.NET.Algorithms;

namespace Core.NET.Interfaces
{
    public interface IAlgorithm
    {
        ComputeHash ComputeHashFile(string path);
        ComputeHash ComputeHashString(string str);
        ComputeHash ComputeHashFile(System.IO.Stream stream);
        ComputeHash ComputeHashString(System.IO.Stream stream);
        ComputeHash ComputeHashFile(byte[] data);
        ComputeHash ComputeHashString(byte[] data);
    }
}
