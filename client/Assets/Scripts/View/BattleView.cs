using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public abstract class BattleView<T> : MonoBehaviour where T : BattleView<T>
    {
        protected static readonly List<T> ViewPool = new List<T>();

        protected static T FetchFromPool()
        {
            var index = ViewPool.FindIndex(v => v is T);
            if (index < 0) return null;

            var view = ViewPool[index];
            ViewPool.RemoveAt(index);
            view.gameObject.SetActive(true);
            return view;
        }

        protected static void ReturnToPool(T view)
        {
            view.gameObject.SetActive(false);
            ViewPool.Add(view);
        }

        public void ReturnToPool()
        {
            ReturnToPool((T)this);
        }
    }
}