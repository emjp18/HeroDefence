using UnityEngine;
using UnityEditor;
using TMPro;

//public class DebuggingEditor : EditorWindow
//{
//    AiGrid[] grid;
//    [MenuItem("Window/DebuggingEditor")]
   
//    public static void ShowWindow()
//    {
//        GetWindow<DebuggingEditor>("Debug Generated Grids in runtime");
        
//    }

//    private void OnGUI()
//    {
        
       
//        if (GUILayout.Button("Fetch Grid"))
//        {
//            grid = FindObjectsOfType<AiGrid>();
//        }
//        if (GUILayout.Button("Visualize grid 1 AI Nodes"))
//        {
//            grid[0].VisualizeAINodes();
            
//        }
//        if (GUILayout.Button("Visualize grid 1 Quad Tree"))
//        {
//            grid[0].VisualizeQuadNodes();
            
//        }
//        if (GUILayout.Button("Visualize grid 2 AI Nodes"))
//        {
//            grid[1].VisualizeAINodes();

//        }
//        if (GUILayout.Button("Visualize grid 2 Quad Tree"))
//        {
//            grid[1].VisualizeQuadNodes();

//        }
//    }
    ////////////////////////////?????????????????????????????????
    //////
    //[MenuItem("Tools/Find Missing references in scene")]
    //public static void FindMissingReferences()
    //{
        
    //    var objects = GameObject.FindObjectOfType();

    //    foreach (var go in objects)
    //    {
    //        var components = go.GetComponents();

    //        foreach (var c in components)
    //        {
    //            SerializedObject so = new SerializedObject(c);
    //            var sp = so.GetIterator();

    //            while (sp.NextVisible(true))
    //            {
    //                if (sp.propertyType == SerializedPropertyType.ObjectReference)
    //                {
    //                    if (sp.objectReferenceValue == null && sp.objectReferenceInstanceIDValue != 0)
    //                    {
    //                        ShowError(FullObjectPath(go), sp.name);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    //private static void ShowError(string objectName, string propertyName)
    //{
    //    Debug.LogError("Missing reference found in: " + objectName + ", Property : " + propertyName);
    //}

    //private static string FullObjectPath(GameObject go)
    //{
    //    return go.transform.parent == null ? go.name : FullObjectPath(go.transform.parent.gameObject) + "/" + go.name;
    //}

//}
//