using UnityEngine;
using UnityEditor;


public class DebuggingEditor : EditorWindow
{
    AiGrid grid;
    [MenuItem("Window/DebuggingEditor")]
   
    public static void ShowWindow()
    {
        GetWindow<DebuggingEditor>("Debug Generated Grids in runtime");
        
    }

    private void OnGUI()
    {
        
       
        if (GUILayout.Button("Fetch Grid"))
        {
            grid = FindObjectOfType<AiGrid>();
        }
        if (GUILayout.Button("Visualize AI Nodes"))
        {
            grid.VisualizeAINodes();
            
        }
        if (GUILayout.Button("Visualize Quad Tree"))
        {
            grid.VisualizeQuadNodes();
            
        }
    }
   

}
