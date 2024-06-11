using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] Renderer m_TextureRender;
    [SerializeField] MeshFilter m_MeshFilter;
    [SerializeField] MeshRenderer m_MeshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        m_TextureRender.sharedMaterial.mainTexture = texture;
        m_TextureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        m_MeshFilter.sharedMesh = meshData.CreateMesh();
        m_MeshRenderer.sharedMaterial.mainTexture = texture;
    }
}
