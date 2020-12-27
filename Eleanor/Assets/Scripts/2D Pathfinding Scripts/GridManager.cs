using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int height;
    public int width;
    public float nodeGap;
    public Graph graph;
    public LayerMask notPassable;

    private void Awake()
    {
        graph = BuildGraph(this.transform.position, height, width, nodeGap);
    }

    //build from upper left to lower right corner
    Graph BuildGraph(Vector2 startPos, int h, int w, float nGap)
    {
        Graph _graph = new Graph();
        _graph.edges = new Dictionary<Vector2, Vector2[]>();

        //fill graph
        for (int yStatus = 0; yStatus < h; ++yStatus)
        {
            for (int xStatus = 0; xStatus < w; ++xStatus)
            {
                //set position
                float posY = startPos.y - (yStatus * nGap);
                float posX = startPos.x + (xStatus * nGap);

                //check if pos is passable                
                Vector2 pos = new Vector2(posX, posY);
                if (Passable(pos))
                {
                    //set neighbors
                    Vector2[] posNeighbors = new Vector2[4];

                    var newPos = new Vector2(0, 0);
                    int n = 0;
                    //right - if not right edge, create right neighbor
                    if (pos.x < startPos.x + (nGap * (width - 1)))
                    {
                        newPos = new Vector2(pos.x + nGap, pos.y);
                        if (Passable(newPos))
                        {
                            posNeighbors[n] = newPos;
                            ++n;
                        }
                    }
                    //down - if not lower edge, create lower neighbor
                    if (pos.y > startPos.y - (nGap * (height - 1)))
                    {
                        newPos = new Vector2(pos.x, pos.y - nGap);
                        if (Passable(newPos))
                        {
                            posNeighbors[n] = newPos;
                            ++n;
                        }

                    }
                    //left - if not left edge, create left neighbor
                    if (pos.x > startPos.x)
                    {
                        newPos = new Vector2(pos.x - nGap, pos.y);
                        if (Passable(newPos))
                        {
                            posNeighbors[n] = newPos;
                            ++n;
                        }
                    }
                    //up - if not upper edge, create upper neighbor
                    if (pos.y < startPos.y)
                    {
                        newPos = new Vector2(pos.x, pos.y + nGap);
                        if (Passable(newPos))
                        {
                            posNeighbors[n] = newPos;
                            ++n;
                        }
                    }

                    Array.Resize(ref posNeighbors, 4 - (4 - n));

                    _graph.edges.Add(pos, posNeighbors);
                }              
            }
        }

        return _graph;
    }

    bool Passable(Vector2 location)
    {
        //check if location has walls
        if (Physics2D.OverlapPoint(location, notPassable) == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region visuals
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        try
        {
            for (int yStatus = 0; yStatus < height; ++yStatus)
            {
                for (int xStatus = 0; xStatus < width; ++xStatus)
                {
                    //set position
                    float posY = this.transform.position.y - (yStatus * nodeGap);
                    float posX = this.transform.position.x + (xStatus * nodeGap);

                    Vector2 pos = new Vector2(posX, posY);

                    Gizmos.DrawWireSphere(pos, 0.1f);
                }
            }
            Gizmos.color = Color.red;
            try
            {
                foreach (Vector2 pos in graph.edges.Keys)
                {
                    Gizmos.DrawWireSphere(pos, 0.2f);
                }
            }
            catch { }
        }
        catch
        {
            Debug.LogError("Grid has no values (Height, Width, Node Gap)!");
        }
    }
    #endregion
}

public class Graph
{
    public Dictionary<Vector2, Vector2[]> edges = new Dictionary<Vector2, Vector2[]>();

    //return neighbors of a position
    public Vector2[] Neighbors(Vector2 pos)
    {
        return edges[pos];
    }
}
