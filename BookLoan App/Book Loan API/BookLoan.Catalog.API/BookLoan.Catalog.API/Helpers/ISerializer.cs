using System.IO;

namespace BookLoanMicroservices.Helpers
{
    public interface ISerializer
    {
        void Serialize(Stream stream, object graph);
        object Deserialize(Stream stream);
    }
}