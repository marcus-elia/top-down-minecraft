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

    // Neighboring chunks
    private Chunk northNeighbor;
    private Chunk eastNeighbor;
    private Chunk southNeighbor;
    private Chunk westNeighbor;

    private GameObject[,,] blocks = new GameObject[CS, WH, CS];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
