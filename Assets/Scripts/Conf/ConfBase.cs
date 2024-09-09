using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ConfBaseItem
{
    public int id;
    public bool isVariant;

    public ConfBaseItem CloneBase()
    {
        return MemberwiseClone() as ConfBaseItem;
    }
}

public class ConfBase<TItem> where TItem : ConfBaseItem
{
    public bool isTryGetIDError;
    public string confName;

    public Dictionary<int, ConfBaseItem> allConfDic = new Dictionary<int, ConfBaseItem>();

    private List<TItem> itemsList = new List<TItem>();
    public IReadOnlyList<TItem> items => itemsList as IReadOnlyList<TItem>;

    public virtual void Init()
    {
    }

    public virtual void OnInit()
    {
    }

    /// <summary>
    /// 添加Item
    /// </summary>
    public virtual void AddItem(ConfBaseItem item)
    {
        if (Application.isEditor)
        {
            if (allConfDic.ContainsKey(item.id))
            {
                Debug.LogError(confName + "表ID重复：" + item.id);
            }
        }
        allConfDic[item.id] = item;
        itemsList.Add(item as TItem);
    }

    /// <summary>
    /// 获取Item
    /// </summary>
    public virtual T GetItemObject<T>(int id) where T : ConfBaseItem
    {
        ConfBaseItem t = null;

        allConfDic.TryGetValue(id, out t);

        if (Application.isEditor && isTryGetIDError && t == null && id != 0)
        {
            Debug.LogWarning(GetType().Name + "：找不到ID " + id);
        }

        return t as T;
    }
}