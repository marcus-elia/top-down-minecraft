// Math helper contains math functions

using UnityEngine;


// The point2D is a Vector2 with ints instead of floats
public struct Point2D
{
    public int x, z;
    public Point2D(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}

public class MathHelper
{
    public static int NearestPerfectSquare(int n)
    {
        int squareJumpAmount = 3;
        int curSquare = 1;
        int prevSquare = 0;
        while (curSquare < n)
        {
            prevSquare = curSquare;
            curSquare += squareJumpAmount;
            squareJumpAmount += 2;  // the difference between consecutive squares is odd integer
        }
        if (n - prevSquare > curSquare - n)
        {
            return curSquare;
        }
        else
        {
            return prevSquare;
        }
    }
    // Assuming n is a perfect square, return the square root of n as an int
    public static int Isqrt(int n)
    {
        return (int)Mathf.Round(Mathf.Sqrt(n));
    }
    // Convert a ChunkID to the coordinates of the chunk
    public static Point2D ChunkIDtoPoint2D(int n)
    {
        int s = NearestPerfectSquare(n);
        int sq = Isqrt(s);
        if (s % 2 == 0)
        {
            if (n >= s)
            {
                return new Point2D(sq / 2, -sq / 2 + n - s);
            }
            else
            {
                return new Point2D(sq / 2 - s + n, -sq / 2);
            }
        }
        else
        {
            if (n >= s)
            {
                return new Point2D(-(sq + 1) / 2, (sq + 1) / 2 - 1 - n + s);
            }
            else
            {
                return new Point2D(-(sq + 1) / 2 + s - n, (sq + 1) / 2 - 1);
            }
        }
    }

    // Convert the coordinates of the chunk to the ChunkID
    public static int ChunkCoordsToChunkID(int a, int b)
    {
        // Bottom Zone
        if (b > 0 && a >= -b && a < b)
        {
            return 4 * b * b + 3 * b - a;
        }
        // Left Zone
        else if (a < 0 && b < -a && b >= a)
        {
            return 4 * a * a + 3 * a - b;
        }
        // Top Zone
        else if (b < 0 && a <= -b && a > b)
        {
            return 4 * b * b + b + a;
        }
        // Right Zone
        else if (a > 0 && b <= a && b > -a)
        {
            return 4 * a * a + a + b;
        }
        // Only a=0, b=0 is not in a zone
        else
        {
            return 0;
        }
    }
    // Wrapper function
    public static int Point2DtoChunkID(Point2D p)
    {
        return ChunkCoordsToChunkID(p.x, p.z);
    }

    // Get IDs of neighboring chunks
    public static int GetNorthChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.z += 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetSouthChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.z -= 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetEastChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x += 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetWestChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x -= 1;
        return Point2DtoChunkID(chunkCoords);
    }
    // And diagonal neighbor chunks
    public static int GetSouthWestChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x -= 1;
        chunkCoords.z -= 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetNorthWestChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x -= 1;
        chunkCoords.z += 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetNorthEastChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x += 1;
        chunkCoords.z += 1;
        return Point2DtoChunkID(chunkCoords);
    }
    public static int GetSouthEastChunkID(int id)
    {
        Point2D chunkCoords = ChunkIDtoPoint2D(id);
        chunkCoords.x += 1;
        chunkCoords.z -= 1;
        return Point2DtoChunkID(chunkCoords);
    }

    // Get ID of chunk containing a location
    public static int GetIDOfChunk(Vector3 location)
    {
        int x = Mathf.FloorToInt(location.x / Chunk.CS);
        int z = Mathf.FloorToInt(location.z / Chunk.CS);
        return ChunkCoordsToChunkID(x, z);
    }

    // Get quadrant within chunk containing location
    public static Point2D GetChunkIDAndQuadrantInChunk(Vector3 location)
    {
        // Find the chunk's ID
        int x = Mathf.FloorToInt(location.x / Chunk.CS);
        int z = Mathf.FloorToInt(location.z / Chunk.CS);
        int id = ChunkCoordsToChunkID(x, z);

        // Find the local coords
        float localX = location.x - x * Chunk.CS;
        float localZ = location.z - z * Chunk.CS;

        // Find the quadrant
        int quadrant = Mathf.FloorToInt(localX / (Chunk.CS / 2)) + 2* Mathf.FloorToInt(localZ / (Chunk.CS / 2));

        return new Point2D(id, quadrant);
    }

    // Get the three other id's of chunks around the given chunk that should be included
    // based on the quadrant
    public static int[] GetNearbyChunkIDs(int chunkID, int quadrant)
    {
        int[] ids = new int[3];
        switch(quadrant)
        {
            case 0:
                ids[0] = GetSouthChunkID(chunkID);
                ids[1] = GetSouthWestChunkID(chunkID);
                ids[2] = GetWestChunkID(chunkID);
                break;
            case 1:
                ids[0] = GetSouthChunkID(chunkID);
                ids[1] = GetSouthEastChunkID(chunkID);
                ids[2] = GetEastChunkID(chunkID);
                break;
            case 2:
                ids[0] = GetWestChunkID(chunkID);
                ids[1] = GetNorthWestChunkID(chunkID);
                ids[2] = GetNorthChunkID(chunkID);
                break;
            case 3:
                ids[0] = GetEastChunkID(chunkID);
                ids[1] = GetNorthEastChunkID(chunkID);
                ids[2] = GetNorthChunkID(chunkID);
                break;
            default:
                Debug.LogError("quadrant should be 0,1,2, or 3, but received " + quadrant.ToString());
                break;
        }

        return ids;
    }
}
