using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected PlayerInfo playerInfo = new();
    
    public virtual void Init(PlayerInfo info)
    {
        playerInfo = info;
        
        if(!string.IsNullOrEmpty(info.id))
            gameObject.name = info.id;
    }
    
    public abstract void SetServe();
}
