using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // This is the chunk size, how blocks per side there are in a chunk. A chunk is a
    // rectangular prism of blocks whose base is a CS x CS square.
    public static int CS = 16;

    // The max height of the world is also static
    public static int WH = 16;

    // The int id of the chunk
    private int id_;
    private Point2D chunkCoords_;

    // Neighboring chunks
    private Chunk northNeighbor_;
    private Chunk eastNeighbor_;
    private Chunk southNeighbor_;
    private Chunk westNeighbor_;

    // This stores all of the blocks
    private GameObject[,,] blocks = new GameObject[CS, WH + 1, CS];

    // Reference the ChunkManager
    private ChunkManager manager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableChunk(bool input)
    {
        for (int i = 0; i < CS; i++)
        {
            for (int j = 0; j < WH; j++)
            {
                for (int k = 0; k < CS; k++)
                {
                    blocks[i, j, k].GetComponent<Block>().EnableRendering(input);
                }
            }
        }
    }

    // Setters and Initializers
    public void SetChunkID(int inputID)
    {
        id_ = inputID;
        chunkCoords_ = MathHelper.ChunkIDtoPoint2D(id_);
        CalculatePosition();
    }

    private void CalculatePosition()
    {
        transform.position = new Vector3(chunkCoords_.x * CS, 0, chunkCoords_.z * CS);
    }

    public void InitializeBlocks()
    {
        // The unbreakable layer at the bottom
        BlockProperties bedrock = manager.materialManager.bedrock;
        for (int x = 0; x < CS; x++)
        {
            for (int z = 0; z < CS; z++)
            {
                GameObject block = new GameObject();
                block.AddComponent<Block>();
                block.transform.SetParent(transform);
                block.transform.localPosition = new Vector3(x + 0.5f, 0.5f, z + 0.5f);
                block.GetComponent<Block>().CreateFaces();
                block.GetComponent<Block>().SetProperties(bedrock);
                block.GetComponent<Block>().ApplyMainMaterial();
                block.GetComponent<Block>().SetChunkID(id_);
                block.GetComponent<Block>().SetIndexInChunk(x, 0, z);
                blocks[x, 0, z] = block;
            }
        }

        SetVerticalBlockNeighbors();
        SetInteriorBlockNeighbors();
    }

    // The top and bottom neighbors of the blocks are only within this chunk
    private void SetVerticalBlockNeighbors()
    {
        for (int x = 0; x < CS; x++)
        {
            for (int y = 0; y <= WH - 1; y++)
            {
                for (int z = 0; z < CS; z++)
                {
                    // Set the top neighbor
                    if (blocks[x, y, z] != null)
                    {
                        blocks[x, y, z].GetComponent<Block>().SetTopNeighbor(blocks[x, y + 1, z]);
                    }
                    // Set the bottom neighbor of the one above this
                    if (blocks[x, y + 1, z] != null)
                    {
                        blocks[x, y + 1, z].GetComponent<Block>().SetBottomNeighbor(blocks[x, y, z]);
                    }
                }
            }
        }
    }

    // The interior block neighbors also are only within this chunk
    private void SetInteriorBlockNeighbors()
    {
        for (int x = 0; x < CS - 1; x++)
        {
            for (int y = 0; y <= WH; y++)
            {
                for (int z = 0; z < CS - 1; z++)
                {
                    // Set the north neighbor
                    if (blocks[x, y, z] != null)
                    {
                        blocks[x, y, z].GetComponent<Block>().SetNorthNeighbor(blocks[x, y, z + 1]);
                    }
                    // Set the south neighbor of the one north of this
                    if (blocks[x, y, z + 1] != null)
                    {
                        blocks[x, y, z + 1].GetComponent<Block>().SetSouthNeighbor(blocks[x, y, z]);
                    }
                    // Set the east neighbor
                    if (blocks[x, y, z] != null)
                    {
                        blocks[x, y, z].GetComponent<Block>().SetEastNeighbor(blocks[x + 1, y, z]);
                    }
                    // Set the west neighbor of the one east of this
                    if (blocks[x + 1, y, z] != null)
                    {
                        blocks[x + 1, y, z].GetComponent<Block>().SetWestNeighbor(blocks[x, y, z]);
                    }
                }
            }
        }
        // Set the neighbors of the north and east edges
        for (int y = 0; y <= WH; y++)
        {
            // East edge
            for (int z = 0; z < CS - 1; z++)
            {
                if (blocks[CS - 1, y, z] != null)
                {
                    blocks[CS - 1, y, z].GetComponent<Block>().SetNorthNeighbor(blocks[CS - 1, y, z + 1]);
                }
                if (blocks[CS - 1, y, z + 1] != null)
                {
                    blocks[CS - 1, y, z + 1].GetComponent<Block>().SetSouthNeighbor(blocks[CS - 1, y, z]);
                }
            }
            // North edge
            for (int x = 0; x < CS - 1; x++)
            {
                if (blocks[x, y, CS - 1] != null)
                {
                    blocks[x, y, CS - 1].GetComponent<Block>().SetEastNeighbor(blocks[x + 1, y, CS - 1]);
                }
                if (blocks[x + 1, y, CS - 1] != null)
                {
                    blocks[x + 1, y, CS - 1].GetComponent<Block>().SetWestNeighbor(blocks[x, y, CS - 1]);
                }
            }
        }
    }

    public void SetNorthNeighbor(Chunk neighbor)
    {
        northNeighbor_ = neighbor;
        northNeighbor_.SetSouthNeighborChunkOnly(this);

        // Give the blocks their neighbors
        int z = CS - 1;
        for (int y = 0; y <= WH; y++)
        {
            for (int x = 0; x < CS; x++)
            {
                if (blocks[x, y, z] != null)
                {
                    GameObject neighborBlock = northNeighbor_.GetComponent<Chunk>().GetBlocks()[x, y, 0];
                    blocks[x, y, z].GetComponent<Block>().SetNorthNeighbor(neighborBlock);
                }
            }
        }
    }
    public void SetSouthNeighbor(Chunk neighbor)
    {
        southNeighbor_ = neighbor;
        southNeighbor_.SetNorthNeighborChunkOnly(this);

        // Give the blocks their neighbors
        int z = 0;
        for (int y = 0; y <= WH; y++)
        {
            for (int x = 0; x < CS; x++)
            {
                if (blocks[x, y, z] != null)
                {
                    GameObject neighborBlock = southNeighbor_.GetComponent<Chunk>().GetBlocks()[x, y, CS - 1];
                    blocks[x, y, z].GetComponent<Block>().SetSouthNeighbor(neighborBlock);
                }
            }
        }
    }
    public void SetEastNeighbor(Chunk neighbor)
    {
        eastNeighbor_ = neighbor;
        eastNeighbor_.SetWestNeighborChunkOnly(this);

        // Give the blocks their neighbors
        int x = CS - 1;
        for (int y = 0; y <= WH; y++)
        {
            for (int z = 0; z < CS; z++)
            {
                if (blocks[x, y, z] != null)
                {
                    GameObject neighborBlock = eastNeighbor_.GetComponent<Chunk>().GetBlocks()[0, y, z];
                    blocks[x, y, z].GetComponent<Block>().SetEastNeighbor(neighborBlock);
                }
            }
        }
    }
    public void SetWestNeighbor(Chunk neighbor)
    {
        westNeighbor_ = neighbor;
        westNeighbor_.SetEastNeighborChunkOnly(this);

        // Give the blocks their neighbors
        int x = 0;
        for (int y = 0; y <= WH; y++)
        {
            for (int z = 0; z < CS; z++)
            {
                if (blocks[x, y, z] != null)
                {
                    GameObject neighborBlock = westNeighbor_.GetComponent<Chunk>().GetBlocks()[CS - 1, y, z];
                    blocks[x, y, z].GetComponent<Block>().SetWestNeighbor(neighborBlock);
                }
            }
        }
    }

    public void SetNorthNeighborChunkOnly(Chunk input)
    {
        northNeighbor_ = input;
    }
    public void SetSouthNeighborChunkOnly(Chunk input)
    {
        southNeighbor_ = input;
    }
    public void SetEastNeighborChunkOnly(Chunk input)
    {
        eastNeighbor_ = input;
    }
    public void SetWestNeighborChunkOnly(Chunk input)
    {
        westNeighbor_ = input;
    }

    // Getters
    public GameObject[,,] GetBlocks()
    {
        return blocks;
    }
}
