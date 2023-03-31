using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager {
    /// <summary>
    /// 单例模式
    /// 1.定义一个静态变量外部访问，内部构造
    /// 2.构造方法私有化，只能内部构造
    /// </summary>
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    private Transform canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
                canvasTransform = GameObject.Find("Canvas").transform;
            return canvasTransform;
        }
    }

    private float canvasScaleFactor = 0f;
    public float CanvasScaleFactor
    {
        get
        {
            if (canvasScaleFactor == 0)
                canvasScaleFactor = GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
            return canvasScaleFactor;
        }
    }

    private Dictionary<UIPanelType, string> panelPathDict;   //用于存储panel类型与path的对应
    private Dictionary<UIPanelType, BasePanel> panelDict;    //用于存储panel类型与实例的游戏物体的对应
    private Stack<BasePanel> panelStack;                  //用于存储显示的panel

    private UIManager() {
        ParseUIPanelTypeJson();
    }

    /// <summary>
    /// 将实例化的panel存储入栈并显示
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        //当前有显示界面就得进入暂停周期
        BasePanel newPanel = GetPanel(panelType);
        if (panelStack.Count > 0)
        {
            if (newPanel == panelStack.Peek())
            {
                newPanel.OnEnter();
                return newPanel;
            }
            //获得栈顶元素而不出栈
            BasePanel panel= panelStack.Peek();
            panel.OnPause();
        }
        panelStack.Push(newPanel);
        newPanel.OnEnter();
        return newPanel;
    }

    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if (panelStack.Count > 0)
        {
            BasePanel panel1 = panelStack.Pop();
            panel1.OnExit();
            if (panelStack.Count > 0)
            {
                BasePanel panel2 = panelStack.Peek();
                panel2.OnResume();
            }
        }
    }


    //[Serializable]//序列化对象用于接收json的数据
    class PanelTypePathJson
    {
        public List<UIPanelInfo> infoList;
    }

    private void ParseUIPanelTypeJson () {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
        //只能将数据转换为对象
        PanelTypePathJson jsonObject = JsonUtility.FromJson <PanelTypePathJson>(ta.text);
        foreach (UIPanelInfo info  in jsonObject.infoList)
        {
            panelPathDict.Add(info.panelType, info.path);
        }
	}

    /// <summary>
    /// 实例化请求显示的panel并存入字典
    /// </summary>
    /// <param name="panelType"></param>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }
        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);
        if (panel != null)
            return panel;
        string panelPath;
        panelPathDict.TryGetValue(panelType, out panelPath);
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(panelPath));
        go.transform.SetParent(CanvasTransform,false);
        panelDict.Add(panelType,go.GetComponent<BasePanel>());
        return go.GetComponent<BasePanel>();
    }

}
