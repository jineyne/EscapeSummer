using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public WordType type=WordType.Adjective;
    public float RayStartOffset = 0.6f;
    public float RayEndOffset = 0.9f;
    public TextBox left;
    public TextBox right;
    public Node node;
    public GameObject imageChild;

    private bool flags = false;

    public TextBox CheckSubject()
{

        RaycastHit2D hit;
        Debug.DrawRay(transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, new Color(0, 1, 0));
        hit = Physics2D.Raycast(transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, 1);
        if (hit && hit.collider.CompareTag("TextBox") && hit.collider.GetComponent<TextBox>() != null)
            return hit.collider.GetComponent<TextBox>();
        else
            return null;
    }
    public TextBox CheckAdjective()
    {
        RaycastHit2D hit;
        Debug.DrawRay(transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, new Color(0, 1, 0));
        hit = Physics2D.Raycast(transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, 1);
        if (hit &&hit.collider.CompareTag("TextBox") && hit.collider.GetComponent<TextBox>() != null)
            return hit.collider.GetComponent<TextBox>();
        else
            return null;
    }

    public void CanInteract()
    {

    }

    void Update()
    {
        imageChild.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y / -0.5f);

        if (type==WordType.Verb)
        {
            left = CheckSubject();
            right = CheckAdjective();

            if (left != null && right != null) {
                var op = (node as Op);
                op.LeftNode = left.node;
                op.RightNode = right.node;

                if (!flags && op.LeftNode != null && op.RightNode != null) {
                    VirtualMachine.Instance.TryExecute(node);
                    flags = true;
                }
            } else {
                flags = false;
            }
        }
       
    }

    // Update is called once per frame
}
