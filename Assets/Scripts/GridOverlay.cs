using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{ 
    public static GridOverlay instanse;
    public bool showGrid = true;

    public int gridSizeX = 30;
    private int gridSizeY = 0;
    public int gridSizeZ = 30;

    public float smallStep = 1;

    public float startX = 7;
    private float startY = 0;
    public float startZ = 7;

    private Material lineMaterial;

    public Color gridColor = new Color(0f, 1f, 0f, 1f);

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = this;
        }
    }

    public void GridEnabled(bool value)
    {
        if (showGrid == value)
        {
            showGrid = !value;
        }
        else
        {
            showGrid = value;
        }
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

            lineMaterial.SetInt("_ZWrite", 0);
        }
    } 

    void OnPostRender()
    {
        if (smallStep <= 0.01f)
            smallStep = 0.5f; 

        CreateLineMaterial();
        //текущий материал
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (showGrid)
        {
            GL.Color(gridColor); 
            //Layers
            for (float j = 0; j <= gridSizeY; j += smallStep)
            {
                //X
                for (float i = 0; i <= gridSizeZ; i += smallStep)
                {
                    GL.Vertex3(startX, startY + j, startZ + i);
                    GL.Vertex3(startX + gridSizeX, startY + j, startZ + i);
                }

                //Z
                for (float i = 0; i <= gridSizeX; i += smallStep)
                {
                    GL.Vertex3(startX + i, startY + j, startZ);
                    GL.Vertex3(startX + i, startY + j, startZ + gridSizeZ);
                }
            } 
            //Y
            for (float i = 0; i <= gridSizeZ; i += smallStep)
            {
                for (float k = 0; k <= gridSizeX; k += smallStep)
                {
                    GL.Vertex3(startX + k, startY, startZ + i);
                    GL.Vertex3(startX + k, startY + gridSizeY, startZ + i);
                }
            }
        } 
        GL.End();
    }
}
