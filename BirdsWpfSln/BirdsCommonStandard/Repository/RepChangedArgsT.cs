namespace BirdsCommonStandard
{
    public class RepChangedArgs<TId> : RepChangedArgs
        where TId : IdDto
    {
        public TId OldItem { get; }
        public TId NewItem { get; }

        private RepChangedArgs(RepChangedAction action, TId oldItem, TId newItem)
            : base(action) 
        {
            OldItem = oldItem;
            NewItem = newItem;
        }

        public static RepChangedArgs<TId> Add(TId newItem) => new RepChangedArgs<TId>(RepChangedAction.Add, null, newItem);

        public static RepChangedArgs<TId> Remove(TId oldItem) => new RepChangedArgs<TId>(RepChangedAction.Remove, oldItem, null);

        public static RepChangedArgs<TId> Update(TId oldItem,TId newItem) => new RepChangedArgs<TId>(RepChangedAction.Update, oldItem, newItem);
    }
}
