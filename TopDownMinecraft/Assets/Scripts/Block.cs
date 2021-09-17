using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { Grass, Stone, Bedrock };

public struct BlockProperties
{
    public BlockType blockType;
    public Texture tex;
    public Texture transparentTex;
    public Texture surfaceTransparentTex;
    public bool isBreakable;

    public BlockProperties(BlockType blockType, Texture tex, Texture transparentTex, Texture surfaceTransparentTex, bool isBreakable)
    {
        this.blockType = blockType;
        this.tex = tex;
        this.transparentTex = transparentTex;
        this.surfaceTransparentTex = surfaceTransparentTex;
        this.isBreakable = isBreakable;
    }
}

public class Block : MonoBehaviour
{
    // Quads
    private GameObject topFace_;
    private GameObject bottomFace_;
    private GameObject northFace_;
    private GameObject eastFace_;
    private GameObject southFace_;
    private GameObject westFace_;

    // 6 neighboring  blocks
    private GameObject topNeighbor_;
    private GameObject bottomNeighbor_;
    private GameObject northNeighbor_;
    private GameObject southNeighbor_;
    private GameObject eastNeighbor_;
    private GameObject westNeighbor_;

    // Info for the chunk
    private int chunkID_;
    private Point3D indexInChunk_;

    private BlockProperties properties_;

    public static int blockSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateFaces()
    {
        topFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        topFace_. transform.position = transform.position + blockSize / 2f * Vector3.up;
        topFace_. transform.Rotate(Vector3.right, 90f);
        topFace_. layer = LayerMask.NameToLayer("Ground");

        bottomFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        bottomFace_.transform.position = transform.position + blockSize / 2f * Vector3.down;
        bottomFace_.transform.Rotate(Vector3.right, -90f);
        bottomFace_.layer = LayerMask.NameToLayer("Block Faces");

        northFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        northFace_.transform.position = transform.position + blockSize / 2f * Vector3.forward;
        northFace_.transform.Rotate(Vector3.up, 180f);
        northFace_.layer = LayerMask.NameToLayer("Block Faces");

        southFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        southFace_.transform.position = transform.position + blockSize / 2f * Vector3.back;
        southFace_.layer = LayerMask.NameToLayer("Block Faces");
        // Don't need to rotate this one

        eastFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        eastFace_.transform.position = transform.position + blockSize / 2f * Vector3.right;
        eastFace_.transform.Rotate(Vector3.up, -90f);
        eastFace_.layer = LayerMask.NameToLayer("Block Faces");

        westFace_ = GameObject.CreatePrimitive(PrimitiveType.Quad);
        westFace_.transform.position = transform.position + blockSize / 2f * Vector3.left;
        westFace_.transform.Rotate(Vector3.up, 90f);
        westFace_.layer = LayerMask.NameToLayer("Block Faces");
    }

    public void ApplyTexture(Texture tex)
    {
        topFace_.GetComponent<Renderer>().material.mainTexture = tex;
        bottomFace_.GetComponent<Renderer>().material.mainTexture = tex;
        northFace_.GetComponent<Renderer>().material.mainTexture = tex;
        southFace_.GetComponent<Renderer>().material.mainTexture = tex;
        eastFace_.GetComponent<Renderer>().material.mainTexture = tex;
        westFace_.GetComponent<Renderer>().material.mainTexture = tex;
    }
    public void ApplyMainTexture()
    {
        ApplyTexture(properties_.tex);
    }
    public void ApplyTransparentTexture()
    {
        ApplyTexture(properties_.transparentTex);
    }

    public void ApplySurfaceTransparentTexture()
    {
        ApplyTexture(properties_.surfaceTransparentTex);
    }

    public void SetChunkID(int inputChunkID)
    {
        chunkID_ = inputChunkID;
    }

    public void SetIndexInChunk(int x, int y, int z)
    {
        indexInChunk_ = new Point3D(x, y, z);
    }

    public void EnableRendering(bool input)
    {
        if (topNeighbor_ == null)
        {
            topFace_.SetActive(input);
        }
        if (bottomNeighbor_ == null)
        {
            bottomFace_.SetActive(true);
        }
        if (northNeighbor_ == null)
        {
            northFace_.SetActive(true);
        }
        if (southNeighbor_ == null)
        {
            southFace_.SetActive(true);
        }
        if (eastNeighbor_ == null)
        {
            eastFace_.SetActive(true);
        }
        if (westNeighbor_ == null)
        {
            westFace_.SetActive(true);
        }
    }

    // Set the neighboring blocks
    // and deactivate each face that is covered
    public void SetTopNeighbor(GameObject neighbor)
    {
        topNeighbor_ = neighbor;
        topFace_.SetActive(topNeighbor_ == null);
    }
    public void SetBottomNeighbor(GameObject neighbor)
    {
        bottomNeighbor_ = neighbor;
        bottomFace_.SetActive(bottomNeighbor_ == null);
    }
    public void SetNorthNeighbor(GameObject neighbor)
    {
        northNeighbor_ = neighbor;
        northFace_.SetActive(northNeighbor_ == null);
    }
    public void SetSouthNeighbor(GameObject neighbor)
    {
        southNeighbor_ = neighbor;
        southFace_.SetActive(southNeighbor_ == null);
    }
    public void SetEastNeighbor(GameObject neighbor)
    {
        eastNeighbor_ = neighbor;
        eastFace_.SetActive(eastNeighbor_ == null);
    }
    public void SetWestNeighbor(GameObject neighbor)
    {
        westNeighbor_ = neighbor;
        westFace_.SetActive(westNeighbor_ == null);
    }

    // Getters
    public int GetChunkID()
    {
        return chunkID_;
    }
    public Point3D GetIndexInChunk()
    {
        return indexInChunk_;
    }
    public GameObject GetTopNeighbor()
    {
        return topNeighbor_;
    }
    public GameObject GetBottomNeighbor()
    {
        return bottomNeighbor_;
    }
    public GameObject GetNorthNeighbor()
    {
        return northNeighbor_;
    }
    public GameObject GetSouthNeighbor()
    {
        return southNeighbor_;
    }
    public GameObject GetEastNeighbor()
    {
        return eastNeighbor_;
    }
    public GameObject GetWestNeighbor()
    {
        return westNeighbor_;
    }

    public bool IsBreakable()
    {
        return properties_.isBreakable;
    }
    public BlockType GetBlockType()
    {
        return properties_.blockType;
    }

    public void RemoveSelf()
    {
        if (topNeighbor_)
        {
            topNeighbor_.GetComponent<Block>().SetBottomNeighbor(null);
        }
        if (bottomNeighbor_)
        {
            bottomNeighbor_.GetComponent<Block>().SetTopNeighbor(null);
        }
        if (topNeighbor_)
        {
            topNeighbor_.GetComponent<Block>().SetBottomNeighbor(null);
        }
        if (northNeighbor_)
        {
            northNeighbor_.GetComponent<Block>().SetSouthNeighbor(null);
        }
        if (southNeighbor_)
        {
            southNeighbor_.GetComponent<Block>().SetNorthNeighbor(null);
        }
        if (eastNeighbor_)
        {
            eastNeighbor_.GetComponent<Block>().SetWestNeighbor(null);
        }
        if (westNeighbor_)
        {
            westNeighbor_.GetComponent<Block>().SetEastNeighbor(null);
        }
        Destroy(topFace_);
        Destroy(bottomFace_);
        Destroy(northFace_);
        Destroy(southFace_);
        Destroy(eastFace_);
        Destroy(westFace_);
    }
}
