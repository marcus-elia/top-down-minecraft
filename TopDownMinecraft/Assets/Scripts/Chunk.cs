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

    private GameObject[,,] blocks = new GameObject[CS, WH, CS];

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
    }
