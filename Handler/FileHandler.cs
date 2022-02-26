using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace kmty.NURBS {
    public class FileHandler : MonoBehaviour {

        public List<SurfaceHandler> surfaceHandlerList { get; protected set; } = new List<SurfaceHandler>();
        public GameObject mesh;
        async void Start() {
            List<List<Vector3>> surfaces = await ProcessInput();

            foreach (List<Vector3> surface in surfaces)
            {
                GameObject surfaceMesh = GameObject.Instantiate(mesh, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                surfaceMesh.GetComponent<SurfaceHandler>().surfacePoints = surface;
            }
        }

        void OnDestroy() {
            // surfaceList[0].Dispose();
            // vtcs.Dispose();
        }

        public void Reset() {
            // if (surfaceList[0] != null) surfaceList[0].Dispose();
            // surfaceList[0] = new Surface(data.cps, data.order, data.count.x, data.count.y);
        }

        private static async Task<string> ReadAllTextAsync(string filePath)  
        {  
            using (FileStream sourceStream = new FileStream(filePath,  
                FileMode.Open, FileAccess.Read, FileShare.Read,  
                bufferSize: 4096, useAsync: true))  
            {  
                StringBuilder sb = new StringBuilder();  

                byte[] buffer = new byte[0x1000];  
                int numRead;  
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)  
                {  
                    string text = Encoding.UTF8.GetString(buffer, 0, numRead);  
                    sb.Append(text);  
                }  

                return sb.ToString();  
            }  
        }  
        
        async Task<List<List<Vector3>>> ProcessInput() {
            string destFileName = Path.Combine(Application.dataPath, "test.bv");
            string bvData = await ReadAllTextAsync(destFileName);

            char[] delim = new char[] { '\r', '\n' };
            string[] output = bvData.Split(delim, StringSplitOptions.RemoveEmptyEntries);

            List<Vector3> surfacePoints = new List<Vector3>{};
            List<List<Vector3>> surfaces = new List<List<Vector3>>{};

            for (int i = 0; i < output.Length; i++)
            {
                string[] values = output[i].Split(' ');
                if (values.Length == 2)
                {
                    int sizeX = Convert.ToInt32(values[0]) + 1;
                    int sizeY = Convert.ToInt32(values[1]) + 1;

                    for (int j = 0; j < sizeX * sizeY; j++)
                    {
                        string[] coords = output[i + j + 1].Split(' ');
                        if (coords.Length == 3)
                        {
                            float x = Convert.ToSingle(coords[0]);
                            float y = Convert.ToSingle(coords[1]);
                            float z = Convert.ToSingle(coords[2]);

                            Vector3 coordVector = new Vector3(x, y, z);
                            surfacePoints.Add(coordVector);
                        }

                    }

                    surfacePoints.Add(new Vector3(sizeX, sizeY, 0));
                    surfaces.Add(new List<Vector3>(surfacePoints));
                    surfacePoints.Clear();
                }
            }

            return surfaces;
        }
    }
}
