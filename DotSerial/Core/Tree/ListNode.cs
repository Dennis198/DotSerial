namespace DotSerial.Core.Tree
{
    internal class ListNode : InnerNodeDecorater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrappedNode">Wrapped node</param>
        public ListNode(IDSNode wrappedNode) : base(wrappedNode)
        {}
    }
}