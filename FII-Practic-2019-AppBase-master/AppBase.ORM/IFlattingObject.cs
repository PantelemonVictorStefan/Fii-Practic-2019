using System.Collections.Generic;

namespace AppBase.ORM
{
    internal interface IFlattingObject<T> where T : class
    {
        /// <summary>
        /// Flatten object
        /// </summary>
        /// <returns>A collection of object</returns>
        HashSet<T> Flatten();

        /// <summary>
        /// Flatten object
        /// </summary>
        /// <param name="bag">A reference to a collection where all objects will be collected</param>
        void Flatten(ref HashSet<T> bag);
    }
}
