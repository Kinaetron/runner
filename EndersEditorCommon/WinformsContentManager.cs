using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace EndersEditorCommon
{
    public class WinformsContentManager
    {
        private ContentBuilder contentBuilder;
        private ContentManager contentManager;

        /// <summary>
        /// RAM cache for storing loaded content
        /// </summary>
        private ContentCache contentCache;

        public WinformsContentManager(ContentBuilder contentBuilder, ContentManager contentManager)
        {
            this.contentBuilder = contentBuilder;
            this.contentManager = contentManager;

            contentCache = new ContentCache();
        }

        /// <summary>
        /// Template to load content through content pipeline.  This version of
        /// method will not add the built content to the cache. 
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="fileName">File Name</param>
        /// <param name="contentImporter">Optional custom importer</param>
        /// <param name="contentProcessor">Optional custom processor</param>
        public T LoadContent<T>(string fileName,
            string contentImporter,
            string contentProcessor,
            out string buildError)
        {
            return LoadContent<T>(fileName, contentImporter, contentProcessor, out buildError, true, false);
        }

        /// <summary>
        /// Template to load content through content pipeline.  This version of 
        /// the method can add the built object to the cache.  
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="fileName">File Name</param>
        /// <param name="contentImporter">Optional custom importer</param>
        /// <param name="contentProcessor">Optional custom processor</param>
        /// <param name="forceRebuild">Force rebuild regardless of cache</param>
        /// <param name="addToCache">Should this content be cached</param>
        /// <returns></returns>
        public T LoadContent<T>(string fileName,
            string contentImporter,
            string contentProcessor,
            out string buildError,
            bool forceRebuild,
            bool addToCache)
        {
            buildError = null;

            // compute content name as a checksum of the type and full filename
            string contentKey = null;

            // if we are caching this object, compute a unique key
            // otherwise create a random string
            //contentKey = FileHelper.ComputerStringHash(typeof(T).ToString() + fileName);
            if (addToCache)
            {
                contentKey = System.IO.Path.GetFileName(fileName) + " " + typeof(T).ToString();
                contentKey = FileHelper.ComputerStringHash(contentKey);
            }
            else
                contentKey = Rand.RandomString(25, true);

            T content = default(T);

            // if we haven't already built this content, or if we are forcing a rebuild, 
            // we will actually build the file

            // commented out as we are no longer using HD caching
            //bool cached = System.IO.File.Exists(contentBuilder.CacheDirectory + "/" + contentKey + ".xnb");

            bool cached = contentCache.ObjectExists(contentKey);

            if (!cached || forceRebuild)
            {
                // build content from content on hard drive
                content = BuildContent<T>(fileName, contentKey, contentImporter, contentProcessor, out buildError, addToCache);
            }
            else
            // otherwise try and load the xnb we have already cached
            {
                bool cachedSuccess = contentCache.GetObjectByKey(contentKey, out content);

                // if that retrieval failed for some reason, rebuilt the content
                if (!cachedSuccess)
                    content = BuildContent<T>(fileName, contentKey, contentImporter, contentProcessor, out buildError, addToCache);

                // commented out as we are no longer using HD caching
                //content = contentManager.Load<T>(contentName);
            }
            return (T)content;
        }

        /// <summary>
        /// Manually update the cached copy of an object with the associated filename
        /// </summary>
        public void UpdateCache<T>(string fileName, T obj)
        {
            // compute content name as a checksum of the type and full filename
            string contentKey = System.IO.Path.GetFileName(fileName) + " " + typeof(T).ToString();
            contentKey = FileHelper.ComputerStringHash(contentKey);

            contentCache.CacheObject<T>(contentKey, obj);
        }

        /// <summary>
        /// Template to build content using the content pipeline
        /// </summary>
        /// <typeparam name="T">Object Type</typeparam>
        /// <param name="fileName">File Name</param>
        /// <param name="contentKey">Content cache Key</param>
        /// <param name="contentImporter">Optional custom importer</param>
        /// <param name="contentProcessor">Optional custom processor</param>
        /// <param name="buildError">Reference to build error messages</param>
        /// <param name="addToCache">Should this content be cached</param>
        /// <returns></returns>
        private T BuildContent<T>(string fileName,
            string contentKey,
            string contentImporter,
            string contentProcessor,
            out string buildError,
            bool addToCache)
        {
            T content = default(T);

            // clear previous content          
            contentBuilder.Clear();

            // add new content to be built
            contentBuilder.Add(fileName, contentKey, contentImporter, contentProcessor);

            // Build this new model data.
            buildError = contentBuilder.Build();

            if (string.IsNullOrEmpty(buildError))
            {
                // If the build succeeded, use the ContentManager to
                // load the temporary .xnb file that we just created.
                content = contentManager.Load<T>(contentKey);

                // add newly built content to cache if needed
                if (addToCache)
                    contentCache.CacheObject<T>(contentKey, content);
            }

            return content;
        }
    }
}
