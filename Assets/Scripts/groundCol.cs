using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCol : MonoBehaviour
{
    public Movement player;
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject != player.gameObject && other.gameObject.layer != 7)
        {
            player.isGrounded = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject != player.gameObject && other.gameObject.layer == 0)
        {
            player.isGrounded = false;
        }
    }
}
