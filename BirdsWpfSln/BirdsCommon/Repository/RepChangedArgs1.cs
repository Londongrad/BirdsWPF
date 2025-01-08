namespace BirdsCommon.Repository
{
    public class RepChangedArgs : EventArgs
    {
        public RepChangedAction Action { get; }

        public RepChangedArgs(RepChangedAction action)
        {
            Action = action;
        }

        public static readonly RepChangedArgs Reset = new RepChangedArgs(RepChangedAction.Reset);

    }
}
