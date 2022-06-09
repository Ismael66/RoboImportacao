namespace Plugin
{
    public abstract partial class Base
    {
        protected enum Stage
        {
            PreValidation = 10,
            PreOperation = 20,
            PostOperation = 40
        }
        protected enum Mode
        {
            Asynchronous = 1,
            Synchronous = 0
        }
        protected enum MessageName
        {
            Create,
            Update
        }
    }
}