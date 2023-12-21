using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(0)]
public class ElementOfPool : MonoBehaviour {

    //Pool Manager tarafindan uretilecek olan Monobehaviour Componentine sahip prefab scriptidir.
    [SerializeField] float _spanTime;
    [HideInInspector] public float timeMeter = 0;

    private void Start() {

    }

    private void OnDisable() {
        StopCoroutine(ICountSpanTime());
    }

    private void Push2Pool() {
        //Bu prefabin isi bittiginde poola geri gonder.
        ObjectPoolManager.Instance.Push(this);
    }
    private IEnumerator ICountSpanTime() {
        //Pool spawn esnasinda bu ienumerator metodu yonetir. GC olusturmayacaktir. new ile yield olusturulmamaktadir. Ayrica dinamik olarak degistirmeye musait.
        timeMeter = 0;
        while(timeMeter < _spanTime) {
            timeMeter += Time.deltaTime;
            yield return null;
        }      
        
        Push2Pool();
    }
    public void CountSpanTime() {
        StartCoroutine(ICountSpanTime());
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ElementOfPool))]
public class ElementOfPoolEditor : Editor {
    GUIContent _label_guic0;
    GUIStyle _label_guis0;
    SerializedProperty _timeMeter;
    private void OnEnable() {

        _label_guic0 = new GUIContent() { text = "Elements Of Pool" };
        _label_guis0 = new GUIStyle();
        _label_guis0.normal.textColor = Color.green;
        _label_guis0.alignment = TextAnchor.MiddleCenter;
        _label_guis0.fontSize = 12;
        _timeMeter = serializedObject.FindProperty(nameof(ElementOfPool.timeMeter));
    }

    public override void OnInspectorGUI() {
        
        EditorGUILayout.LabelField(_label_guic0, _label_guis0);
        EditorGUILayout.Space(10);
        base.OnInspectorGUI();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(_timeMeter);
        EditorGUI.EndDisabledGroup();
    }
}
#endif