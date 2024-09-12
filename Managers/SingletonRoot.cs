using UnityEngine;

namespace Dawud.BT.General
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SingletonRoot<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject); // Optional if you want the singleton to persist across scenes
            }
            else if (_instance != this)
            {
                Destroy(gameObject); // Destroy duplicate instances
            }
        }
    }

}
