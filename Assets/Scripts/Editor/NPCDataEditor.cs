using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

public class NPCDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/NPC")]
    private static void OpenWindow() {
        GetWindow<NPCDataEditor>().Show();
    }

    private CreateNewNPC createNewNPC;

    protected override void OnDestroy() {
        base.OnDestroy();

        if(createNewNPC != null) {
            DestroyImmediate(createNewNPC.npc);
        }
    }

    protected override OdinMenuTree BuildMenuTree() {

        var tree = new OdinMenuTree();

        createNewNPC = new CreateNewNPC();
        tree.Add("Create NPC", createNewNPC);
        tree.AddAllAssetsAtPath("NPC Data", "Assets/Resources/NPC/NPC Data", typeof(NPCData));

        return tree;
    }


    protected override void OnBeginDrawEditors() {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if(SirenixEditorGUI.ToolbarButton("Delete Curent")) {
                NPCData asset = selected.SelectedValue as NPCData;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewNPC {

        public CreateNewNPC() {
            npc = ScriptableObject.CreateInstance<NPCData>();
            npc.name = "New NPC";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public NPCData npc;

        [Button("Add New NPC SO")]
        private void CreateNewDNPCData() {
            AssetDatabase.CreateAsset(npc, "Assets/Resources/NPC/NPC Data" + npc.name + ".asset");
            AssetDatabase.SaveAssets();

            npc = ScriptableObject.CreateInstance<NPCData>();
            npc.name = "New NPC";
        }
    }

}
