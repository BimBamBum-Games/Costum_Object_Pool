using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-50)]
public class ObjectPoolManagerBase<T> : MonoBehaviour where T : Component {
    [Header("Orneklenecek Prefab - Prefab to Instantiate")]
    [SerializeField] public T elementOfPool;

    [Header("Ornekleme Sayisi - Number Of Instantiation")]
    [SerializeField] int _numberOfElements = 1;

    [SerializeField] bool _isActiveSampling = true;

    private ObjectPool<T> Pool;

    public int total, active, inactive;
 
    protected virtual void Awake() {
        //Pool classindan ornek olustur.
        Pool = ObjectPool<T>.GetObjectPooler(
            Factory,
            TurnOnn,
            TurnOff,
            _numberOfElements,
            _isActiveSampling);
    }

    protected virtual void Update() {
        UpdateSensors();
    }

    private void UpdateSensors() {
        total = Pool.total;
        active = Pool.active;
        inactive = Pool.inactive;
    }

    protected virtual T Factory() {   
        //Ilgili ornekleme metodu.
        return Instantiate(elementOfPool);
    }

    protected virtual void TurnOnn(T elementOfPool) {
        elementOfPool.gameObject.SetActive(true);
        //Debug.LogWarning("Kendimi Actim!");
    }

    protected virtual void TurnOff(T pln) {
        pln.gameObject.SetActive(false);
        //Debug.LogWarning("Kendimi Kapadim!");
    }

    public virtual T Get() {
        //Herhangi bir anda herhangi bir classin tetiklemesi sonucu ornek istenecektir. Bu siniftan tureyen sinifin Instance uzerinden bu metoda erisilecektir.
        return Pool.PickItUp();
    }

    public virtual void Push(T _pln) {
        //Turetilen siniftan erisilebilinecektir. Pool elementleri bu metodu static instance uzerinden cagirabilecektir.
        Pool.TakeItBack(_pln);
    }
}
