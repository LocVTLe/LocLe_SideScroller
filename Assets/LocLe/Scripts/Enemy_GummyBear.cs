using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Mechanics;


public class Enemy_GummyBear : KinematicObject
{
    private float height = 0.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        height = transform.localScale.y;
        StartCoroutine("Jump");
    }


    IEnumerator Jump()
    {
        while (true)
        {

            float shrinkRatio = 0.8f;
            float shrinkHeight = height * shrinkRatio / 10;
            //float shrinkColliderHeight = colliderHeight * shrinkRatio / 10;

            while (transform.localScale.y > shrinkHeight * 10)
            {

                transform.localScale -= new Vector3(0, shrinkHeight, 0);
                //cc.size.Set(cc.size.x, cc.size.y - shrinkColliderHeight);
                yield return new WaitForSeconds(0.03f);
            }

            yield return new WaitForSeconds(1f);

            Bounce(12);
            yield return new WaitForSeconds(0.02f);

            while (transform.localScale.y < height)
            {
                transform.localScale += new Vector3(0, shrinkHeight, 0);
                //cc.size.Set(cc.size.x, cc.size.y + shrinkColliderHeight);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1f);

        }
    }
}
