using System.Text;

namespace SagaImpl.Common.Extension
{
    public static class Utils
    {
        public static string GetString(this byte[] array)
        {
            return Encoding.UTF8.GetString(array);
        }
    }
}
