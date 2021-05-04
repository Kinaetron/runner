using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndersEditorCommon
{
    /// <summary>
    /// Manages loaded content in memory
    /// </summary>
    internal class ContentCache
    {
        /// <summary>
        /// Dictionary containing cache.  MD5 checksum of file information
        /// is linked to a generic representation of the object
        /// </summary>

        private Dictionary<string, CacheObject> cache;

        public ContentCache()
        {
            cache = new Dictionary<string, CacheObject>();
        }

        /// <summary>
        /// Add an object to the cache
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="obj">Object data</param>
        public void CacheObject<T>(string key, T obj)
        {
            // try and see if an object has already been cached with 
            // the provided key.  if so, we need to remove that
            // object before adding the new object to the cache
            bool alreadyCached = cache.ContainsKey(key);


            if (alreadyCached)
                cache.Remove(key);

            CacheObject cObj = new CacheObject(typeof(T), obj);
            cache.Add(key, cObj);

        }

        /// <summary>
        /// Returns true of an object is already cached using the 
        /// provided key, false if not
        /// </summary>
        public bool ObjectExists(string key)
        {
            return cache.ContainsKey(key);
        }

        /// <summary>
        /// GEt the cached object with the associated key.  Object is returned
        /// by reference, while the method itself returns true on success, or false
        /// on failure
        /// </summary>
        public bool GetObjectByKey<T>(string key, out T obj)
        {
            CacheObject cObj = null;

            bool success = cache.TryGetValue(key, out cObj);

            // if the retrieval succeeded, check and make sure
            // the types match before returning the object
            if (success)
            {
                // if the types match
                if (cObj.Type == typeof(T))
                {
                    obj = (T)cObj.Object;
                    return true;
                }
                else
                // otherwise return the default value
                // of the generic type
                {
                    obj = default(T);
                    return false;
                }
            }
            else
            // otherwise return the default value
            // of the generic type
            {
                obj = default(T);
                return false;
            }
        }
    }
}
