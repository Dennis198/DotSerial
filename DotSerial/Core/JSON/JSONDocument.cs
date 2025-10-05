using DotSerial.Core.General;


namespace DotSerial.Core.JSON
{
    internal class JSONDocument : DSDocument
    {
        public override void Load(string fileName)
        {
            throw new NotImplementedException();
        }

        public override void Save(string fileName)
        {
            if (null == Tree)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }
    }
}
