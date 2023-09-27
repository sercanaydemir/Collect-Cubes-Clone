using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace CaseStudy.Editor
{
    public class LevelEditor : EditorWindow
    {
        [MenuItem("Tools/LevelEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditor>();
            window.titleContent = new GUIContent("Level Editor");
            window.Show();
        }
        
        public List<Grid> grids = new List<Grid>();
        private Texture2D paintingTexture;
        private ColorField _colorField;
        private Color brushColor;
        private int brushSize = 16;
        private bool isPainting;
        private bool isGridDrawed;
        private void OnGUI()
        {
            // if (grids.Count < 1)
            // {
            //     for (int i = 0; i < 16; i++)
            //     {
            //         for (int j = 0; j < 16; j++)
            //         {
            //             Vector3 center = new Vector2(16 + i * 16, 16 + j * 16);
            //
            //             Grid grid = new Grid() { centerPoint = center };
            //             grid.CalculatePoints(32);
            //             grids.Add(grid);
            //         }
            //         
            //     }
            //     
            //     
            // }
            
            
            
            if (paintingTexture == null)
            {
                paintingTexture = new Texture2D(512, 512);
                for (int i = 0; i < paintingTexture.height; i++)
                {
                    for (int j = 0; j < paintingTexture.width; j++)
                    {
                        paintingTexture.SetPixel(i,j,new Color(0,0,0,0));
                        paintingTexture.GetPixel(i, j);
                    }
                }
                
            }
            
            GUI.Box(new Rect(44, 44,512,512),paintingTexture);

            // if (!isGridDrawed)
            // {
            //     for (int j = 0; j < 16; j++)
            //     {
            //         for (int i = 0; i < 16; i++)
            //         {
            //             Vector2 pos = new Vector2(44 + 32 * j, 44 + 32 * i);
            //             GUI.Box(new Rect(pos, new Vector2(32, 32)), i.ToString());
            //         }
            //
            //         if (j == 15)
            //             isGridDrawed = true;
            //     }
            // }
            //GUI.DrawTexture(new Rect(44, 44, 512, 512), paintingTexture);
            
            GUI.Box(new Rect(40,600,500,150),"Create Texture ");
            if (_colorField == null)
            {
                _colorField = new ColorField();
                _colorField.style.width = 64;
                _colorField.style.top = 650;
                _colorField.style.left = 94;
                rootVisualElement.Add(_colorField);
            }
            
            if (GUI.Button(new Rect(60,600,90,20),"Create Texture"))
            {
                SaveTextureToAssets();
            }
            
            Event e = Event.current;
            
            Debug.LogError(e.mousePosition);
            
            brushColor = _colorField.value;
            brushColor.a = 1;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        isPainting = true;
                    }
                    break;
            
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        isPainting = false;
                    }
                    break;
            
                case EventType.MouseDrag:
                    if (isPainting)
                    {
                        Vector2 mousePos = e.mousePosition - new Vector2(44,44);
                        Debug.LogError("Mouse Pos " + mousePos);
                        DrawOnTexture(mousePos);
                        Repaint();
                    }
                    break;
            }
            
            
        }
        
        private void DrawOnTexture(Vector2 point)
        {
            int x = (int)point.x;
            int y = (int)point.y;
            
            
            for (int i = -brushSize / 2; i < brushSize / 2; i++)
            {
                for (int j = -brushSize / 2; j < brushSize / 2; j++)
                {
                    if (x + i >= 0 && x + i < paintingTexture.width && y + j >= 0 && y + j < paintingTexture.height)
                    {
                        Grid g = FindGridClosestPoint(new Vector2(x + i, (y + j) * -1));

                        for (int k = 0; k < g.insidePoints.Count; k++)
                        {
                            //paintingTexture.SetPixel((int)g.insidePoints[k].x,(int)g.insidePoints[k].y, brushColor);
                            Debug.LogError("painted coordinate: " + g.insidePoints[k].x +" "+ (int)g.insidePoints[k].y);
                        }

                    }
                }
            }

            paintingTexture.Apply();
        }
        

        private void SaveTextureToAssets()
        {
            if (paintingTexture != null)
            {
                // Specify the path where you want to save the texture within the Assets folder
                string path = "Assets/Resources/"; // Change this path as needed
                string fileName = "MySavedTexture.png"; // Change the filename and extension as needed

                // Combine the path and filename to create the full save path
                string fullPath = path + fileName;

                // Encode the texture to a PNG file and save it to the specified path
                byte[] bytes = paintingTexture.EncodeToPNG();
                System.IO.File.WriteAllBytes(fullPath, bytes);
                
                // Refresh the AssetDatabase to make Unity aware of the new asset
                AssetDatabase.Refresh();
                Debug.Log("Texture saved to: " + fullPath);
            }
            else
            {
                Debug.LogWarning("TextureToSave is not assigned.");
            }
        }

        Grid FindGridClosestPoint(Vector3 point)
        {
            float distance = Mathf.Infinity;
            Grid g = new Grid();
            for (int i = 0; i < grids.Count; i++)
            {
                if (Vector3.Distance(grids[i].centerPoint, point) < distance)
                {
                    distance = Vector3.Distance(grids[i].centerPoint, point);
                    g = grids[i];
                }
            }

            return g;
        }
        public struct Grid
        {
            public Vector2 centerPoint;

            public List<Vector2> insidePoints;

            public void CalculatePoints(int gridSize)
            {
                Vector2 minSize = centerPoint - new Vector2(gridSize/2, gridSize/2);
                Vector2 maxSize = centerPoint + new Vector2(gridSize/2, gridSize/2);

                insidePoints = new List<Vector2>();
                for (int i = (int) minSize.x; i < maxSize.x; i++)
                {
                    for (int j = (int) minSize.y; j < maxSize.y; j++)
                    {
                        insidePoints.Add(new Vector2(i,j));
                    }
                }
            }
        }
    }
}