using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-49)]
public class ObjectPoolManager : ObjectPoolManagerBase<ElementOfPool> {
    //ObjectPoolManager classidir.
    public static ObjectPoolManager Instance;

    [Header("Ornekleme Araligi - Instantiation Time Interval")]
    [Min(0.001f)]
    [SerializeField] float _timeInterval = 1f;
    float _timeMeter;

    [SerializeField] bool _activateInUpdate;

    protected override void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(Instance);
        }
        base.Awake();
    }

    protected override void Update() {
        base.Update();
        if (_activateInUpdate) {
            return;
        }

        if (_timeMeter < _timeInterval) {
            _timeMeter += Time.deltaTime;
        }
        else {
            _timeMeter = 0;
            ElementOfPool eop = Get();
            eop.transform.position = transform.position;
            eop.CountSpanTime();
        }      
    }
}

