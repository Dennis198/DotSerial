
namespace DotSerial.Core.General
{
    internal abstract class DSDocument
    {
        protected DSNode? Tree;

        public abstract void Save(string fileName);

        public abstract void Load(string fileName);
    }
}
