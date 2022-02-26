using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kmty.NURBS {
    public class SurfaceCpsData {
        public int order;
        public Vector2Int size;
        public Vector2Int count;
        public List<CP> cps;

        public Vector2 width => new Vector2(size.x / (float)(count.x - 1), size.y / (float)(count.y - 1));
        public int Convert(int x, int y) => x + y * count.x;
        public int Convert(Vector2Int v) => v.x + v.y * count.x;
        public Vector2Int Convert(int i) => new Vector2Int(i % count.x, Mathf.FloorToInt(i / (float)count.x));
        public SurfaceCpsData(int o, Vector2Int s, Vector2Int c, List<CP> cp) { order = o; size = s; count = c; cps = cp; }
    }
}
