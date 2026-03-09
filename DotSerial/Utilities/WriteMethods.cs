using System.Text;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to verious methods for writing.
    /// </summary>
    internal static class WriteMethods
    {
        /// <summary>
        /// Adds identation
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="count">Count of intdentaion</param>
        /// <param name="size">Size of one intdentaion</param>
        internal static void AddIndentation(StringBuilder sb, int count, int size)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (count < 0)
            {
                throw new ArgumentException(count.ToString());
            }

            sb.Append(CommonConstants.WhiteSpace, count * size);
        }
    }
}