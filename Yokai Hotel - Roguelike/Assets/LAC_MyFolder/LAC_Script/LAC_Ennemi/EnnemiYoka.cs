using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiYoka : EnnemiBehaviour
{
    #region Idle
    [Header("Idle")]
    public Vector2 idleDir;
    Vector2 lastIdleDir;
    public bool normalDir; 

    float wallDetectRadius = 1;
    public List<Vector2> wallDirs = new List<Vector2>();
    #endregion


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public void StealthMove()
    {
        // detect wall
        List<Vector2> checkDirs = new List<Vector2>();
        for(int a = 0; a < 360; a += 90)
        {
            float aRad = Mathf.Deg2Rad * a;
            Vector2 checkDir = new Vector2(Mathf.Cos(aRad), Mathf.Sin(aRad));
            Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + checkDir * wallDetectRadius, 0.25f, obstructMask);

            if (hit)
                checkDirs.Add(checkDir);
            else if (checkDirs.Contains(checkDir))
                checkDirs.Remove(checkDir); 
        }
        if (wallDirs != checkDirs)
        {
            wallDirs.Clear();
            wallDirs.AddRange(checkDirs);

            Vector2 newIdleDir = idleDir;

            if (wallDirs.Count == 1)
                newIdleDir = new Vector2(wallDirs[0].y, wallDirs[0].x);

            if( wallDirs.Count == 2)
            {
                
            }
        }

        
    }
}
