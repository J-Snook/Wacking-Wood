using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    List<Vector3> _vertices = new List<Vector3>();
    List<Vector3> _normals = new List<Vector3>();
    List<Vector2> _uvs = new List<Vector2>();
    int _submeshIndex;

    public List<Vector3> Vertices
    {
        get {return _vertices;}
        set {_vertices = value;}
    }
    public List<Vector3> Normals
    {
        get {return _normals;}
        set {_normals = value;}
    }
    public List<Vector2> UVs
    {
        get {return _uvs;}
        set {_uvs = value;}
    }
    public int SubMeshIndex
    {
        get {return _submeshIndex;}
        set {_submeshIndex = value;}
    }

    public Triangle(Vector3[] _verts, Vector3[] _norms, Vector2[] _us, int _subInds)
    {
        EmptySelf();

        _vertices.AddRange(_verts);
        _normals.AddRange(_norms);
        _uvs.AddRange(_us);

        _submeshIndex = _subInds;
    }

    public void EmptySelf()
    {
        _vertices = new List<Vector3>();
        _normals = new List<Vector3>();
        _uvs = new List<Vector2>();
        _submeshIndex = 0;
    }
}
