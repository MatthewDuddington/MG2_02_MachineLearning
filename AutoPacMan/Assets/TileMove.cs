using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMove : MonoBehaviour {

    Vector2 nextTile;
    Vector2 curDir;

    void FixedUpdate()
    {
        if(nextTile != (Vector2)transform.position)
        {
            curDir = nextTile - (Vector2)transform.position;
            if(curDir != Vector2.zero)
            {
                //nothing
            }
        }
    }

    Vector2 getNextTile()
    {

        return nextTile;
    }
    void setNextTile(Vector2 tile)
    {
        nextTile = tile;
    }

    Vector2 getDirNormal()
    {
        return curDir.normalized;
    }

    public bool isValidMove(Vector2 dir)
    {

        return checkDirCollision(transform.position, dir);
    }

    bool checkDirCollision(Vector2 pos, Vector2 dir)
    {
        return checkCollision(pos + dir);
    }

    public bool moveToDir(Vector2 dir, float speed)
    {
        if (isAt((Vector2)transform.position + (Vector2)dir)) return true;
        if (this.isValidMove(dir))
        {
            // This will smoothly move pacman to its destination, based on speed
            Vector2 p = Vector2.MoveTowards(
                (Vector2)transform.position, (Vector2)transform.position + dir, speed
            );
            // actually move to the calcualted vector
            this.GetComponent<Rigidbody2D>().MovePosition(p);
            return true;
        }
        return false;

    }

    public bool checkCollision(Vector2 target)
    {
        int layerMask = (1 << 0);

        RaycastHit2D hit = Physics2D.Linecast(target, (Vector2)transform.position, layerMask);
        return (hit.collider == null);
    }

    bool isAt(Vector2 target)
    {
        return (Vector2)transform.position == target;
    }

    public bool moveTo(Vector3 target, float speed)
    {
        if (this.checkCollision(target))
        {
            return this.moveTo_do(target, speed);
        }
        return false;
    }

    public bool moveTo_do(Vector2 target, float speed)
    {
        if (!this.isAt(target))
        {
            // This will smoothly move pacman to its destination, based on speed
            Vector2 p = Vector2.MoveTowards(this.transform.position, target, speed);
            // actually move to the calcualted vector
            this.GetComponent<Rigidbody2D>().MovePosition(p);
            return true;
        }
        return false;
    }
}
