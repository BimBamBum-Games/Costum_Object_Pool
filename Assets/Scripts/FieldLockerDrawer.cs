using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//Bu CustomPropertyDrawer Attribute Property Attribute Classindan tanimlanan classtan type alarak Property Drawer ilklendirilmesi yapilir.
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(FieldLocker))]
public class FieldLockerDrawer : PropertyDrawer {

    GUIStyle _titleStl, _valueStl;
    bool _isInitialized = false;

    public void SetInitialStyles() {
        //PropertyDrawer basesini direk cagir. Buna ek olarak bu tureyen classta initialize islemleri gerceklestir.
        _titleStl = new GUIStyle(EditorStyles.numberField);
        _titleStl.normal.textColor = Color.green;
        //EditorStyles birer asset ve aslinda Create menuden GUISkin ile olusturulmaktadir. Built-in olan burada islenmektedir.
        _valueStl = new GUIStyle(EditorStyles.numberField);
        _valueStl.normal.textColor = Color.yellow;
    }   

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if(_isInitialized == false) {
            //Bu OnEnable veya OnAwake gibi bir yapi olmadigindan yapay Ctor gorevi gorur.
            SetInitialStyles();
            _isInitialized = true;
        }

        EditorGUI.BeginProperty(position, label, property);
        
        Rect titleLabelPosition = 
            new (position.x, 
            position.y, 
            EditorGUIUtility.labelWidth, 
            position.height);

        //EditorGU.LabelField sadece readonly amacli kullanildigindan mouse eventlerine duyarsizdir. (Normal,Hover, Focus, Action)
        EditorGUI.LabelField(titleLabelPosition, property.displayName, _titleStl);

        //Bu matematiksel ifade ile Unity yerlesik field - label olusturma yapisinin aynisini meydana getirir.
        Rect valueLabelPosition = 
            new (position.x + EditorGUIUtility.labelWidth + 2, 
            position.y, 
            position.width - EditorGUIUtility.labelWidth - 4, 
            position.height);

        //EditorGUI.LabelField' in GUI.Label' dan farki mouse olaylarinda renk degisimi gibi durumlar saglamasidir.
        EditorGUI.LabelField(valueLabelPosition, property.intValue.ToString(), _valueStl);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        //Property Drawer' in yukseklik olusuturucusunun degerini override eden methoddur.
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif

#if UNITY_EDITOR
public class FieldLocker : PropertyAttribute {
    //Property Attribute classidir. Bu class PropertyDrawer tarafindan OnInspectorGUI tarafindan cagrilarak Inspector zuerinde cizdirilir.
}

#endif