namespace GenealogicalTreeCource.Class
{
    public struct PersonId
    {
        public int MyId { get; }
        public int FatherId { get; }
        public int MotherId { get;}

        public List<int> WifesId { get; }
        public List<int> ChildrenId { get; }

        public PersonId(int myId, int fatherId, int motherId, List<int> wifesId, List<int> childrenId)
        {
            MyId = myId;
            FatherId = fatherId;
            MotherId = motherId;
            WifesId = wifesId;
            ChildrenId = childrenId;
        }

        public void AddChild(int newChildId) => ChildrenId.Add(newChildId);
    }
}
