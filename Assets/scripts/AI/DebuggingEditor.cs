using UnityEngine;
using UnityEditor;


public class DebuggingEditor : EditorWindow
{
    AiGrid[] grid;
    [MenuItem("Window/DebuggingEditor")]
   
    public static void ShowWindow()
    {
        GetWindow<DebuggingEditor>("Debug Generated Grids in runtime");
        
    }

    private void OnGUI()
    {
        
       
        if (GUILayout.Button("Fetch Grid"))
        {
            grid = FindObjectsOfType<AiGrid>();
        }
        if (GUILayout.Button("Visualize grid 1 AI Nodes"))
        {
            grid[0].VisualizeAINodes();
            
        }
        if (GUILayout.Button("Visualize grid 1 Quad Tree"))
        {
            grid[0].VisualizeQuadNodes();
            
        }
        if (GUILayout.Button("Visualize grid 2 AI Nodes"))
        {
            grid[1].VisualizeAINodes();

        }
        if (GUILayout.Button("Visualize grid 2 Quad Tree"))
        {
            grid[1].VisualizeQuadNodes();

        }
    }
   

}
//