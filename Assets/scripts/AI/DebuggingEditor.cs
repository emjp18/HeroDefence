using UnityEngine;
using UnityEditor;


public class DebuggingEditor : EditorWindow
{
    AiGrid2 grid;
    [MenuItem("Window/DebuggingEditor")]
   
    public static void ShowWindow()
    {
        GetWindow<DebuggingEditor>("Debug Generated Grids in runtime");
        
    }

    private void OnGUI()
    {
        
       
        if (GUILayout.Button("Fetch Grid"))
        {
            grid = FindObjectOfType<AiGrid2>();
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
