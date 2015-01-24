using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(BossController))]
public class BossControllerEditor : Editor {

    [SerializeField]
    private int amount = 0;
    [SerializeField]
    private List<BossAttack> attacks;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if ( attacks == null ) {
            attacks = ( target as BossController ).Attacks;
            if (attacks == null) {
                attacks = new List<BossAttack>();
                amount = 0;
            } else {
                amount = attacks.Count;
            }
        }

        if ( amount < 0 ) {
            amount = 0;
        }

        if ( attacks.Count != amount ) {
            if ( attacks.Count > amount ) {
                int a = attacks.Count - amount;
                attacks.RemoveRange( amount, a );
            } else if ( attacks.Count < amount ) {
                int a = amount - attacks.Count;
                for ( int i = 0; i < a; i++ ) {
                    attacks.Add( new BossAttack() );
                }
            }
        }

        amount = EditorGUILayout.IntField( "Amount", amount );
        foreach ( var item in attacks ) {
            var rect = EditorGUILayout.GetControlRect();
            float hw = rect.width / 2;

            item.TimeFromStart = EditorGUI.FloatField( new Rect( rect.x, rect.y, hw, rect.height ), item.TimeFromStart );
            item.Type = (AttackType)EditorGUI.EnumPopup( new Rect( rect.x + hw, rect.y, hw, rect.height ), item.Type );
        }
        ( target as BossController ).Attacks = attacks;
    }
}
