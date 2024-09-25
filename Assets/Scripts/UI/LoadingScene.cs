using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider process;

    private AsyncOperation asyncOperation;

    // Start is called before the first frame update
    void Start()
    {
        // 使用协程异步加载场景
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false; // 如果为true，那么加载结束后直接就会跳转
        StartCoroutine(SetAllowActivation());
    }

    IEnumerator SetAllowActivation()
    {
        void SetProcess(float value)
        {
            process.value = value;
            process.handleRect.localScale = Mathf.Max((1 - value), 0.5f) * Vector3.one;
            process.handleRect.rotation = Quaternion.Euler(0, 0, -value * 720);
        }
        float nowProgress = 0;
        while (asyncOperation.progress < 0.9f && !asyncOperation.isDone)
        {
            while (nowProgress < asyncOperation.progress)
            {
                nowProgress += 0.002f;
                if (nowProgress > asyncOperation.progress)
                    nowProgress = asyncOperation.progress;
                SetProcess(nowProgress);
                yield return new WaitForEndOfFrame();
            }
        }
        nowProgress = 0.9f;
        while (nowProgress < 1)
        {
            nowProgress += 0.001f;
            SetProcess(nowProgress);
            yield return new WaitForEndOfFrame();
        }
        asyncOperation.allowSceneActivation = true;
    }
}
