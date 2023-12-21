using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> {
    //Belki Strategy ve Command Pattern Design ile Komponentlere Gore Ornek Havuzlari Yapilabilir.

    Queue<T> _pool;
    Func<T> _blochTFactory;
    Action<T> _setEnabled;  
    //Teslim ederken.
    Action<T> _setPassive; 
    //Teslim alirken.
    int _numberOfElements;
    bool _isDynamic;

    public int total, active, inactive;

    private ObjectPool(
        Func<T> blochTFactory,
        Action<T> setPassive,
        Action<T> setEnabled,
        int numberOfElements,
        bool isDynamicInstantiation) {   
        //Bloch Ctor Pattern
        _pool = new Queue<T>();
        _blochTFactory = blochTFactory;
        _setEnabled = setEnabled;
        _setPassive = setPassive;
        _numberOfElements = numberOfElements;
        _isDynamic = isDynamicInstantiation;
        AddIntoQuee();
    }

    private void AddIntoQuee() {   
        //Instantiate prefabs and enqueue them into Queue.
        for (int i = 0; i < _numberOfElements; i++) {   
            //Awake tanimlanmamis bile olsa calisir. Hemen ardindan Disable edilir. Bu nedenle Start yerine Awake kullanilir.
            T entity = _blochTFactory();
            _pool.Enqueue(entity);
            _setPassive(entity);
            total++;
        }
    }

    public static ObjectPool<T> GetObjectPooler(
        Func<T> blochTFactory,
        Action<T> setPassive,
        Action<T> setEnabled,
        int numberOfElements,
        bool isDynamicInstantiation) {   //Bloch Factory
        return new ObjectPool<T>(blochTFactory,
            setEnabled,
            setPassive,
            numberOfElements,
            isDynamicInstantiation);
    }

    public T PickItUp() {   
        //Kuyruktan yeni bir tane ceker ve onu siranin sonuna geri ekler. Default(T) class icin null deger dondurur.
        T youngFish = default(T);
        if (_pool.Count > 0) {
            youngFish = _pool.Dequeue();
        }
        else if (_isDynamic) {   
            //Queue elemani yetersizse yenisini olustur.
            youngFish = _blochTFactory();
            total++;
        }
        _setEnabled(youngFish);
        active++;
        inactive = total - active;
        return youngFish;
    }

    public void TakeItBack(T oldFish) {   
        //Kuyrugun sonuna tekrar ekler.
        _setPassive(oldFish);
        _pool.Enqueue(oldFish);
        active--;
        inactive = total - active;
    }
}
