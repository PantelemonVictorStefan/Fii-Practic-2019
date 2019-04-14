using System;

namespace AppBase.ORM
{
    public class ModelException : Exception
    {
        public ModelException(string msg)
            : base(msg)
        {
        }

        public ModelException(string msg, Exception innerEx)
            : base(msg, innerEx)
        {
        }
    }
}
