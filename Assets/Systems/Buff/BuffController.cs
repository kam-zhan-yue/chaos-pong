using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private readonly List<Buff> _buffs = new();
    
    public void ApplyBuff(Buff buff)
    {
        buff.Apply();
        _buffs.Add(buff);
    }

    private void Update()
    {
        for (int i = _buffs.Count - 1; i >= 0; --i)
        {
            _buffs[i].Tick(Time.deltaTime);
            if(_buffs[i].Expired)
                _buffs.RemoveAt(i);
        }
    }
}
