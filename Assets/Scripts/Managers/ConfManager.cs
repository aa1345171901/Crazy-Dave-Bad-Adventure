using System.Collections;
using System.Collections.Generic;
using TopDownPlate;
using UnityEngine;

public class ConfManager : BaseManager<ConfManager>
{
    public ConfMgr confMgr { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        confMgr = new ConfMgr();
        confMgr.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
