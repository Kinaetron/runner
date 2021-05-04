using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndersEditorCommon
{
    /// <summary>
    /// Generic cache object
    /// </summary>
    internal class CacheObject
    {
        /// <summary>
        /// The type of the object
        /// </summary>
        private Type type;

        /// <summary>
        /// The object data
        /// </summary>
        private object obj;

        /// <summary>
        /// The type of the object
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// The object data
        /// </summary>
        public object Object
        {
            get { return obj; }
        }

        public CacheObject(Type type, object obj)
        {
            this.type = type;
            this.obj = obj;
        }
    }
}
