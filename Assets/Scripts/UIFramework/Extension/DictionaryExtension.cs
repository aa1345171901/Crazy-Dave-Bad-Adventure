using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 扩展方法类  this后为需要扩展的类
/// </summary>
public static class DictionaryExtension {

    public static TValue TryGet<TKey, TValue>(this Dictionary<TKey,TValue> dict,TKey key)
    {
        TValue tValue;
        dict.TryGetValue(key,out tValue);
        return tValue;
    }
}
