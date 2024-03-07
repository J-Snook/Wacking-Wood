using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All the Information needed for a Triangle
/// </summary>
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

    /// <summary>
    /// Creating a Triangle
    /// </summary>
    /// <param name="_verts">The Points of the Triangle</param>
    /// <param name="_norms">The Upwards Direction of the Face</param>
    /// <param name="_us">The Texture Coordinates of the Face</param>
    /// <param name="_subInds">Which SubMesh index does this Triangle belong to</param>
    public Triangle(Vector3[] _verts, Vector3[] _norms, Vector2[] _us, int _subInds)
    {
        EmptySelf();

        _vertices.AddRange(_verts);
        _normals.AddRange(_norms);
        _uvs.AddRange(_us);

        _submeshIndex = _subInds;
    }

    /// <summary>
    /// Completely Resets the Object
    /// </summary>
    public void EmptySelf()
    {
        _vertices = new List<Vector3>();
        _normals = new List<Vector3>();
        _uvs = new List<Vector2>();
        _submeshIndex = 0;
    }
}
