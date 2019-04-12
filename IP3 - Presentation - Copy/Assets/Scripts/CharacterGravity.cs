using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGravity : MonoBehaviour {

    private float initHeight;
    private Player pl;

    public float GetInitHeight() { return initHeight; }

    private void Start()
    {
        initHeight = transform.position.y;
        pl = GetComponent<Player>();
    }

    public void Update()
    {
            if(pl.GetState() != Player.Actions.THROWN)
        transform.position = new Vector3(transform.position.x, initHeight, transform.position.z);
    }
}
