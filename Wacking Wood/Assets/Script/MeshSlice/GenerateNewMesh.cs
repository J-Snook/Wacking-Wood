using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewMesh
{
    List<Vector3> _vertices = new List<Vector3>();
    List<Vector3> _normals = new List<Vector3>();
    List<Vector2> _uvs = new List<Vector2>();
    List<List<int>> _subMeshIndices = new List<List<int>>();

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
    public List<List<int>> SubMeshIndices
    {
        get {return _subMeshIndices;}
        set {_subMeshIndices = value;}
    }

    public void AddTriangle(Triangle _tri)
    {
        int currentVertCount = _vertices.Count;

        _vertices.AddRange(_tri.Vertices);
        _normals.AddRange(_tri.Normals);
        _uvs.AddRange(_tri.UVs);

        if(_subMeshIndices.Count < _tri.SubMeshIndex + 1)
        {
            for(int i = _subMeshIndices.Count; i < _tri.SubMeshIndex + 1; i++)
            {
                _subMeshIndices.Add(new List<int>());
            }
        }

        for(int i = 0; i < 3; i++)
        {
            _subMeshIndices[_tri.SubMeshIndex].Add(currentVertCount + i);
        }
    }

    public Mesh GetGeneratedMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(_vertices);
        mesh.SetNormals(_normals);
        mesh.SetUVs(0, _uvs);
        mesh.SetUVs(1, _uvs);

        mesh.subMeshCount = _subMeshIndices.Count;
        for(int i = 0; i < _subMeshIndices.Count; i++)
        {
            mesh.SetTriangles(_subMeshIndices[i], i);
        }

        return mesh;
    }
}
