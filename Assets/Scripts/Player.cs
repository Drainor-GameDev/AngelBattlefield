using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool attack;
    public Animator anim;
    public float speed;
    public GameObject skin;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }
    public void MovePlayer(Vector3 direction, Vector3 finalPos)
    {
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        skin.transform.rotation = Quaternion.LookRotation(-dir);
        transform.Translate(-dir * speed * Time.deltaTime);
        float xPos = direction.x / Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        float rot = Mathf.Rad2Deg * Mathf.Acos(xPos);
        if(float.IsNaN(rot))
        {
            rot = 0;
        }
        print(rot);
    }
}
