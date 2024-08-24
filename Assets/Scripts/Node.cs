using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    NodeManager nodeManager;

    public bool isBottom;
    public bool isStop;
    public NodeManager.NodeInfo nodeInfo = NodeManager.NodeInfo.Normal;

    public int enegy = 1;
    public float invisiblePos = 7.0f;
    public float invisibleTimer = 2.0f;
    public float sizeTimer = 1.5f;

    public bool isItem;
    
    private void Start()
    {
        SetNode();
        nodeManager = FindObjectOfType<NodeManager>();
    }

    private void Update()
    {
        if (!isStop)
        {
            transform.position += new Vector3(0, -nodeManager.speed * Time.deltaTime, 0);

            if (this.transform.position.y < 0)
            {
                Destroy(gameObject);
            }

            if(nodeInfo == NodeManager.NodeInfo.Invisibile)
            {
                if (transform.position.y < 14.0f && transform.position.y > nodeManager.speed * invisibleTimer)
                {
                    if(GetComponent<Renderer>().material.color.a > 0)
                    {
                        GetComponent<Renderer>().material.color -= new Color(0, 0, 0, 1.0f * Time.deltaTime);
                    }
                }
                else if(transform.position.y <= nodeManager.speed * invisibleTimer)
                {
                    if(GetComponent<Renderer>().material.color.a < 1)
                    {
                        GetComponent<Renderer>().material.color += new Color(0, 0, 0, 1.0f * Time.deltaTime);
                    }
                }
            }

            if(nodeInfo == NodeManager.NodeInfo.Size)
            {
                if(transform.position.y < 14.0f && transform.position.y > nodeManager.speed * sizeTimer)
                {
                    float conSize = Random.Range(10, 101) / 100.0f;
                    float changeValue = nodeManager.speed * Time.deltaTime;
                    Vector3 changeVector3 = new Vector3(changeValue, changeValue, changeValue);
                    if(conSize > transform.localScale.x)
                    {
                        transform.localScale += changeVector3;
                    }
                    else if(conSize < transform.localScale.x)
                    {
                        transform.localScale -= changeVector3;
                    }
                }
                else if(transform.position.y <= nodeManager.speed * sizeTimer)
                {
                    transform.localScale = Vector3.one;
                }
            }
        }

    }
    public void SetNode()
    {
        this.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }
}
