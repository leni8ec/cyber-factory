using System;
using System.Collections.Generic;
using System.Linq;
using CyberFactory.Basics.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberFactory.Common.Services.GameObjects {
    // todo: maybe use it as "Util" instead of "Service"
    [UsedImplicitly]
    public class GameObjectsService {

        public const string DEFAULT_ROOT_PATH = "GameObjects";

        private GameObject defaultRootGameObject;

        /// Cache for created objects under default root
        private readonly Dictionary<string, GameObject> defaultRootCache = new();
        /// Cache for created custom root objects
        private readonly Dictionary<string, GameObject> rootCache = new();
        /// Temp variable root game objects from a default scene  
        private readonly List<GameObject> rootGameObjectsBucket = new();

        /// <summary>
        /// Create new GameObject by path <br/>
        /// (use 'ActiveScene' variable and 'transform.Find' to search path items)<br/><br/>
        /// ⚠️ note: can be expensive on a huge number of calls!<br/>
        /// </summary>
        /// <param name="path">use '/' before path to specify scene root object, otherwise root default value will be used</param>
        /// <param name="name">Name of GameObject</param>
        /// <param name="components">Components on the game objects</param>
        /// <returns></returns>
        public GameObject Create(string path, string name = null, params Type[] components) {
            if (string.IsNullOrEmpty(path)) {
                Debug.LogError("Path is empty!");
                return CreateInternal(name, components);
            }

            string[] paths = PathUtil.Split(path);
            int pathShift = 0; // offset of the first created object in the path

            // Find or create a root GameObject
            GameObject parent;
            string rootPart = paths[0];
            bool isCustomRoot = rootPart.Length == 0; // custom root - if path started from '/' (it means 0 symbols before)
            if (isCustomRoot) {
                // path[0] - it's a delimiter,
                // path[1] - it's a root object name
                string rootName = paths[1];
                parent = GetRoot(rootName); // use custom specified root
                pathShift += 2;
            } else {
                parent = GetDefaultRoot(); // use predefined root variable
                string parentName = paths[0];
                if (defaultRootCache.TryGetValue(parentName, out var cachedRoot)) {
                    if (cachedRoot) parent = cachedRoot;
                    pathShift += 1;
                }
            }

            // Find or Create full GameObject path
            if (paths.Length >= 0 + pathShift) {
                for (int i = 0 + pathShift; i < paths.Length; i++) {
                    string part = paths[i];
                    var transform = parent.transform.Find(part);
                    if (!transform) transform = CreateInternal(parent, part).transform;
                    parent = transform.gameObject;
                    if (!isCustomRoot && i == pathShift) { // cache first parent for default root objects
                        defaultRootCache[part] = transform.gameObject;
                    }
                }
            }

            // Create game object itself
            return CreateInternal(parent, name, components);
        }

        public GameObject Create(string name = null, params Type[] components) {
            return CreateInternal(GetDefaultRoot().transform, name, components);
        }

        private GameObject CreateInternal(GameObject parent, string name = null, params Type[] components) {
            return CreateInternal(parent.transform, name, components);
        }

        private GameObject CreateInternal(Transform parent, string name = null, params Type[] components) {
            var gameObject = CreateInternal(name, components);
            gameObject.transform.SetParent(parent);
            return gameObject;
        }

        private GameObject CreateInternal(string name = null, params Type[] components) {
            if (name is { Length: 0 }) Debug.LogWarning("GameObject name is empty!");
            GameObject gameObject;
            if (components == null) gameObject = new GameObject(name);
            else gameObject = new GameObject(name, components);
            return gameObject;
        }


        /// <summary>
        /// Get default root object
        /// </summary>
        /// <returns>predefined root object</returns>
        private GameObject GetDefaultRoot() {
            if (!defaultRootGameObject) defaultRootGameObject = new GameObject(DEFAULT_ROOT_PATH);
            return defaultRootGameObject;
        }

        /// <summary>
        /// Find for scene root objects or create new specified root
        /// </summary>
        /// <param name="rootObjectName">root object name</param>
        /// <returns>already exists or newly created root object</returns>
        private GameObject GetRoot(string rootObjectName) {
            // Try get cached value
            if (rootCache.TryGetValue(rootObjectName, out var cachedRoot)) {
                if (cachedRoot) return cachedRoot;
            }

            // Try to find scene already exists object or create new
            SceneManager.GetActiveScene().GetRootGameObjects(rootGameObjectsBucket);
            var root = rootGameObjectsBucket.FirstOrDefault(go => go.name == rootObjectName);
            if (!root) root = CreateInternal(rootObjectName);

            // assign and clear variables
            rootCache[rootObjectName] = root;
            if (rootCache.Count > 100) Debug.LogWarning("RootCache has more than 100 objects!");
            rootGameObjectsBucket.Clear();
            return root;
        }


        public string GetPath(GameObject gameObject) {
            return GetPath(gameObject.transform);
        }

        public string GetPath(Transform transform) {
            string path = transform.name;
            while (transform.parent) {
                path = PathUtil.Combine(transform.parent.name, path, true);
                transform = transform.parent;
            }
            return path;
        }

    }
}