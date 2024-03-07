using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    private static Mesh baseMesh;
    
    /// <summary>
    /// Returns a properly formatted Triangle
    /// </summary>
    /// <param name="_triangleIndexA">Point 1</param>
    /// <param name="_triangleIndexB">Point 2</param>
    /// <param name="_triangleIndexC">Point 3</param>
    /// <param name="_submeshIndex"> Which SubMesh does this triangle belong to</param>
    /// <returns>Triangle</returns>
    public static Triangle GetTriangle(int _triangleIndexA, int _triangleIndexB, int _triangleIndexC, int _submeshIndex)
    {
        Vector3[] verticesToSend =
        {
            baseMesh.vertices[_triangleIndexA],
            baseMesh.vertices[_triangleIndexB],
            baseMesh.vertices[_triangleIndexC]
        };

        Vector3[] normalsToSend =
        {
            baseMesh.normals[_triangleIndexA],
            baseMesh.normals[_triangleIndexB],
            baseMesh.normals[_triangleIndexC]
        };

        Vector2[] uvsToSend = 
        {
            baseMesh.uv[_triangleIndexA],
            baseMesh.uv[_triangleIndexB],
            baseMesh.uv[_triangleIndexC]
        };

        return new Triangle(verticesToSend, normalsToSend, uvsToSend, _submeshIndex);
    }

    /// <summary>
    /// Cuts the given object based on a series of parameters, applies a rigidbody to the two halves aswell
    /// </summary>
    /// <param name="_baseGameObject">The Object to be cut</param>
    /// <param name="_slicePoint">Where is the cut going to happen</param>
    /// <param name="_fillMaterial">Material to put on new Object</param>
    /// <param name="usePlane">Are we using a custom plane</param>
    /// <param name="_plane">Plane used to cut object</param>
    public static void Cut(GameObject _baseGameObject, Vector3 _slicePoint, Material _fillMaterial, bool usePlane, Plane _plane = new Plane())
    {
        Plane slicePlane = new Plane();
        if (usePlane)
        {
            slicePlane = _plane;
        }
        else
        {
            // Generate a plane using the local space down of the object for normal and local space of the object for the hit point
            slicePlane = new Plane(_baseGameObject.transform.InverseTransformDirection(-Vector3.up), _baseGameObject.transform.InverseTransformPoint(_slicePoint));
        }
        // Getting the Mesh component of the GameObject we are cutting
        baseMesh = _baseGameObject.GetComponent<MeshFilter>().mesh;
        List<Vector3> addedVertices = new List<Vector3>();

        GenerateNewMesh alphaMesh = new GenerateNewMesh();
        GenerateNewMesh betaMesh = new GenerateNewMesh();

        // Loop through all the SubMeshes, some meshes are multiple meshes merged together, very annoying
        for(int i = 0; i < baseMesh.subMeshCount; i++)
        {
            // Loop through all the triangles of the SubMesh we are on
            int[] subMeshIndices = baseMesh.GetTriangles(i);
            for(int j = 0; j < subMeshIndices.Length; j+=3)
            {
                int triangleIndexA = subMeshIndices[j];
                int triangleIndexB = subMeshIndices[j + 1];
                int triangleIndexC = subMeshIndices[j + 2];

                Triangle currentTriangle = GetTriangle(triangleIndexA, triangleIndexB, triangleIndexC, i);

                bool triangleAAlpha = slicePlane.GetSide(baseMesh.vertices[triangleIndexA]);
                bool triangleBAlpha = slicePlane.GetSide(baseMesh.vertices[triangleIndexB]);
                bool triangleCAlpha = slicePlane.GetSide(baseMesh.vertices[triangleIndexC]);

                // If all of the Verts of this Triangle lie on the same side of the Plane then we don't need to slice it
                if(triangleAAlpha && triangleBAlpha && triangleCAlpha)
                {
                    alphaMesh.AddTriangle(currentTriangle);
                } // Same as the last comment but otherway around
                else if(!triangleAAlpha && !triangleBAlpha && !triangleCAlpha)
                {
                    betaMesh.AddTriangle(currentTriangle);
                }
                else // We know for sure that 2 of the edges cross the plane and therefore get Bisected
                {
                    BisectTriangle(slicePlane, currentTriangle, triangleAAlpha, triangleBAlpha, triangleCAlpha, alphaMesh, betaMesh, addedVertices);
                }
            }
        }
        
        FillCut(addedVertices, slicePlane, alphaMesh, betaMesh);

        Mesh finishedAlphaMesh = alphaMesh.GetGeneratedMesh();
        Mesh finishedBetaMesh = betaMesh.GetGeneratedMesh();

        // var isn't great but there a bunch of different colliders
        var baseColliders = _baseGameObject.GetComponents<Collider>();
        foreach(var col in baseColliders) 
        {
            Destroy(col);
        }
        _baseGameObject.GetComponent<MeshFilter>().mesh = finishedAlphaMesh;
        var collider = _baseGameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = finishedAlphaMesh;
        collider.convex = true;

        Material[] materials = new Material[finishedAlphaMesh.subMeshCount];
        for(int i = 0; i < finishedAlphaMesh.subMeshCount; i++)
        {
            materials[i] = _baseGameObject.GetComponent<MeshRenderer>().material;
        }
        _baseGameObject.GetComponent<MeshRenderer>().materials = materials;
        _baseGameObject.AddComponent<Rigidbody>();

        GameObject beta = new GameObject();
        beta.transform.position = _baseGameObject.transform.position;
        beta.transform.rotation = _baseGameObject.transform.rotation;
        beta.transform.localScale = _baseGameObject.transform.localScale;
        beta.AddComponent<MeshRenderer>();

        materials = new Material[finishedBetaMesh.subMeshCount];
        for(int i = 0; i < finishedBetaMesh.subMeshCount; i++)
        {
            materials[i] = _baseGameObject.GetComponent<MeshRenderer>().material;
        }
        beta.GetComponent<MeshRenderer>().materials = materials;
        beta.AddComponent<MeshFilter>().mesh = finishedBetaMesh;

        beta.AddComponent<MeshCollider>().sharedMesh = finishedBetaMesh;
        var colliders = beta.GetComponents<MeshCollider>();
        foreach (var col in colliders)
        {
            col.convex = true;
        }
        
        var betaRigidbody = beta.AddComponent<Rigidbody>();
        betaRigidbody.AddRelativeForce(-slicePlane.normal * 250f);
    }

    /// <summary>
    /// A triangle lies on both sides of the plane and needs to be cut
    /// </summary>
    /// <param name="_slicePlane">The definition of the slicing plane</param>
    /// <param name="_triangle">The triangle we want to cut</param>
    /// <param name="_triAAlpha">Does point 1 lie above or below the plane</param>
    /// <param name="_triBAlpha">Does point 2 lie above or below the plane</param>
    /// <param name="_triCAlpha">Does point 3 lie above or below the plane</param>
    /// <param name="_alphaMesh">The Information for the Mesh of one side of the plane</param>
    /// <param name="_betaMesh">The Information for the Mesh of one side of the plane</param>
    /// <param name="_addedVerts">All the verts we have created so far</param>
    private static void BisectTriangle(Plane _slicePlane, Triangle _triangle, bool _triAAlpha, bool _triBAlpha, bool _triCAlpha, GenerateNewMesh _alphaMesh, GenerateNewMesh _betaMesh, List<Vector3> _addedVerts)
    {
        // Convenience
        List<bool> alphaSide = new List<bool>();
        alphaSide.Add(_triAAlpha);
        alphaSide.Add(_triBAlpha);
        alphaSide.Add(_triCAlpha);

        // Define new Triangles, the information for it is empty but of the right size
        Triangle alphaTriangle = new Triangle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);
        Triangle betaTriangle = new Triangle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);

        bool alpha = false;
        bool beta = false;

        /* We check if the next (i) vert is on the alpha side and if it is then we check if the last one was on the same
         side and if it was then we make the first and second bits of information based of of the old triangle.
         */
        for(int i = 0; i < 3; i++)
        {
            if(alphaSide[i])
            {
                if(!alpha)
                {
                    alpha = true;

                    alphaTriangle.Vertices[0] = _triangle.Vertices[i];
                    alphaTriangle.Vertices[1] = alphaTriangle.Vertices[0];

                    alphaTriangle.UVs[0] = _triangle.UVs[i];
                    alphaTriangle.UVs[1] = alphaTriangle.UVs[0];

                    alphaTriangle.Normals[0] = _triangle.Normals[i];
                    alphaTriangle.Normals[1] = alphaTriangle.Normals[0];
                }
                else
                {
                    alphaTriangle.Vertices[1] = _triangle.Vertices[i];
                    alphaTriangle.Normals[1] = _triangle.Normals[i];
                    alphaTriangle.UVs[1] = _triangle.UVs[i];
                }
            }
            else
            {
                if(!beta)
                {
                    beta = true;

                    betaTriangle.Vertices[0] = _triangle.Vertices[i];
                    betaTriangle.Vertices[1] = betaTriangle.Vertices[0];

                    betaTriangle.UVs[0] = _triangle.UVs[i];
                    betaTriangle.UVs[1] = betaTriangle.UVs[0];

                    betaTriangle.Normals[0] = _triangle.Normals[i];
                    betaTriangle.Normals[1] = betaTriangle.Normals[0];
                }
                else
                {
                    betaTriangle.Vertices[1] = _triangle.Vertices[i];
                    betaTriangle.Normals[1] = _triangle.Normals[i];
                    betaTriangle.UVs[1] = _triangle.UVs[i];
                }
            }
        }

        // Find the point of intersection between the sides
        float distance;
        _slicePlane.Raycast(new Ray(alphaTriangle.Vertices[0], (betaTriangle.Vertices[0] - alphaTriangle.Vertices[0]).normalized), out distance);

        // Calculate Normalized distance and lerp to find the intersecion point
        float normalizedDistance = distance / (betaTriangle.Vertices[0] - alphaTriangle.Vertices[0]).magnitude;
        Vector3 alphaVert = Vector3.Lerp(alphaTriangle.Vertices[0], betaTriangle.Vertices[0], normalizedDistance);
        _addedVerts.Add(alphaVert);

        // Lerp intersection points for normals and uv
        Vector3 alphaNormal = Vector3.Lerp(alphaTriangle.Normals[0], betaTriangle.Normals[0], normalizedDistance);
        Vector2 alphaUV = Vector2.Lerp(alphaTriangle.UVs[0], betaTriangle.UVs[0], normalizedDistance);

        // Do the same as the previous for the second vertex
        _slicePlane.Raycast(new Ray(alphaTriangle.Vertices[1], (betaTriangle.Vertices[1] - alphaTriangle.Vertices[1]).normalized), out distance);
        
        normalizedDistance = distance / (betaTriangle.Vertices[1] - alphaTriangle.Vertices[1]).magnitude;
        Vector3 betaVert = Vector3.Lerp(alphaTriangle.Vertices[1], betaTriangle.Vertices[1], normalizedDistance);
        _addedVerts.Add(betaVert);
                
        Vector3 betaNormal = Vector3.Lerp(alphaTriangle.Normals[1], betaTriangle.Normals[1], normalizedDistance);
        Vector2 betaUV = Vector2.Lerp(alphaTriangle.UVs[1], betaTriangle.UVs[1], normalizedDistance);

        // Construct the First triangle for the alpha side
        Triangle currentTriangle;
        Vector3[] updatedVertices = {alphaTriangle.Vertices[0], alphaVert, betaVert};
        Vector3[] updatedNormals = {alphaTriangle.Normals[0], alphaNormal, betaNormal};
        Vector2[] updatedUVs = {alphaTriangle.UVs[0], alphaUV, betaUV};

        currentTriangle = new Triangle(updatedVertices, updatedNormals, updatedUVs, _triangle.SubMeshIndex);

        // Are the points not all in the same place
        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            // If the face plane isn't facing the right way then flip the triangle
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            _alphaMesh.AddTriangle(currentTriangle);
        }

        // Construct the Second triangle for the alpha side
        updatedVertices = new Vector3[] {alphaTriangle.Vertices[0], alphaTriangle.Vertices[1], betaVert};
        updatedNormals = new Vector3[] {alphaTriangle.Normals[0], alphaTriangle.Normals[1], betaNormal};
        updatedUVs = new Vector2[] {alphaTriangle.UVs[0], alphaTriangle.UVs[1], betaUV};

        currentTriangle = new Triangle(updatedVertices, updatedNormals, updatedUVs, _triangle.SubMeshIndex);

        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            _alphaMesh.AddTriangle(currentTriangle);
        }

        // Construct the First triangle for the beta side
        updatedVertices = new Vector3[] {betaTriangle.Vertices[0], alphaVert, betaVert};
        updatedNormals = new Vector3[] {betaTriangle.Normals[0], alphaNormal, betaNormal};
        updatedUVs = new Vector2[] {betaTriangle.UVs[0], alphaUV, betaUV};

        currentTriangle = new Triangle(updatedVertices, updatedNormals, updatedUVs, _triangle.SubMeshIndex);

        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            _betaMesh.AddTriangle(currentTriangle);
        }

        // Construct the Second triangle for the beta side
        updatedVertices = new Vector3[] {betaTriangle.Vertices[0], betaTriangle.Vertices[1], betaVert};
        updatedNormals = new Vector3[] {betaTriangle.Normals[0], betaTriangle.Normals[1], betaNormal};
        updatedUVs = new Vector2[] {betaTriangle.UVs[0], betaTriangle.UVs[1], betaUV};

        currentTriangle = new Triangle(updatedVertices, updatedNormals, updatedUVs, _triangle.SubMeshIndex);

        if(updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if(Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            _betaMesh.AddTriangle(currentTriangle);
        }
    }

    /// <summary>
    /// Move the points around so the face is the right way
    /// </summary>
    /// <param name="_triangle">The Triangle to flip</param>
    private static void FlipTriangle(Triangle _triangle)
    {
        Vector3 vec3Temp = _triangle.Vertices[2];
        _triangle.Vertices[2] = _triangle.Vertices[0];
        _triangle.Vertices[0] = vec3Temp;

        vec3Temp = _triangle.Normals[2];
        _triangle.Normals[2] = _triangle.Normals[0];
        _triangle.Normals[0] = vec3Temp;

        Vector2 vec2Temp = _triangle.UVs[2];
        _triangle.UVs[2] = _triangle.UVs[0];
        _triangle.UVs[0] = vec2Temp;
    }

    /// <summary>
    /// Fill the Empty space left from the cut
    /// </summary>
    /// <param name="_addedVertices">All the Vertices we created in the slicing process</param>
    /// <param name="_plane">The plane we used to cut</param>
    /// <param name="alphaMesh">Half of the cut mesh</param>
    /// <param name="betaMesh">Half of the cut mesh<</param>
    public static void FillCut(List<Vector3> _addedVertices, Plane _plane, GenerateNewMesh alphaMesh, GenerateNewMesh betaMesh)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> polygon = new List<Vector3>();

        for(int i = 0; i < _addedVertices.Count; i++)
        {
            if(!vertices.Contains(_addedVertices[i]))
            {
                polygon = new List<Vector3>();
                polygon.Add(_addedVertices[i]);
                polygon.Add(_addedVertices[i + 1]);

                vertices.Add(_addedVertices[i]);
                vertices.Add(_addedVertices[i + 1]);

                CheckPairs(_addedVertices, vertices, polygon);
                Fill(polygon, _plane, alphaMesh, betaMesh);
            }
        }
    }

    /// <summary>
    /// Check to find pairs in the added vertices that connect to the polygon
    /// </summary>
    /// <param name="_addedVertices">The vertices we have added</param>
    /// <param name="_vertices">List of the verts we have used so far</param>
    /// <param name="_polygon">The Current polygon we are working on</param>
    public static void CheckPairs(List<Vector3> _addedVertices, List<Vector3> _vertices, List<Vector3> _polygon)
    {
        bool isDone = false;
        while(!isDone)
        {
            isDone = true;
            // Iterate through pairs of vertices
            for(int i = 0; i < _addedVertices.Count; i+=2)
            {
                // Does the current pair complete the polygon
                if(_addedVertices[i] == _polygon[_polygon.Count - 1] && !_vertices.Contains(_addedVertices[i + 1]))
                {
                    // Extend by adding second vert
                    isDone = false;
                    _polygon.Add(_addedVertices[i + 1]);
                    _vertices.Add(_addedVertices[i + 1]);
                }
                else if(_addedVertices[i + 1] == _polygon[_polygon.Count - 1] && !_vertices.Contains(_addedVertices[i]))
                {
                    // Extend by adding first vert
                    isDone = false;
                    _polygon.Add(_addedVertices[i]);
                    _vertices.Add(_addedVertices[i]);
                }
            }
        }
    }

    /// <summary>
    /// Using the current vertices fill the newly exposed faces of the mesh
    /// </summary>
    /// <param name="_vertices">The vertices we want to make triangles out of</param>
    /// <param name="_plane">The plane we used to slice</param>
    /// <param name="alphaMesh">Half of the sliced mesh</param>
    /// <param name="betaMesh">Half of the sliced mesh</param>
    private static void Fill(List<Vector3> _vertices, Plane _plane, GenerateNewMesh alphaMesh, GenerateNewMesh betaMesh)
    {
        // Centre position of the polygon
        Vector3 centrePosition = new Vector3();
        for(int i = 0; i < _vertices.Count; i++)
        {
            centrePosition += _vertices[i];
        }
        centrePosition /= _vertices.Count;

        // Define up based on the cut planes normal
        Vector3 up = new Vector3()
        {
            x = _plane.normal.x,
            y = _plane.normal.y,
            z = _plane.normal.z
        };

        // Calculate whats perpendicular to the plane
        Vector3 alpha = Vector3.Cross(_plane.normal, up);

        Vector3 displacement = new Vector3();
        Vector2 uv1 = new Vector2();
        Vector2 uv2 = new Vector2();

        for(int i = 0; i < _vertices.Count; i++)
        {
            // Calculate displacement and uv for the current vertex
            displacement = _vertices[i] - centrePosition;
            uv1 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, Vector3.left),
                y = .5f + Vector3.Dot(displacement, Vector3.up)
            };

            // now for the next vertex
            displacement = _vertices[(i + 1) % _vertices.Count] - centrePosition;
            uv2 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, Vector3.left),
                y = .5f + Vector3.Dot(displacement, Vector3.up)
            };

            // Define the arrays for the important stuff
            Vector3[] vertices = {_vertices[i], _vertices[(i + 1) % _vertices.Count], centrePosition};
            Vector3[] normals = {-_plane.normal, -_plane.normal, -_plane.normal};
            Vector2[] uvs = {uv1, uv2, new Vector2(.5f, .5f)};

            Triangle currentTriangle = new Triangle(vertices, normals, uvs, baseMesh.subMeshCount + 1);

            // Check if faces are the wrong way around and flip them if needed
            if(Vector3.Dot(Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]), normals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            alphaMesh.AddTriangle(currentTriangle);

            // Flip the normals for the beta triangle
            normals = new[] {_plane.normal, _plane.normal, _plane.normal};
            currentTriangle = new Triangle(vertices, normals, uvs, baseMesh.subMeshCount + 1);

            if(Vector3.Dot(Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]), normals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }
            betaMesh.AddTriangle(currentTriangle);
            // Footnote - Beta Triangle is just Alpha Triangle but on the other mesh
        }
    }
}
