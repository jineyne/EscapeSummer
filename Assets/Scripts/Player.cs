using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;


public class Player : MonoBehaviour
{
    public float TileOffset;
    public float MoveTime = 1f;
    private bool Moving = false;
    public bool focus = false;
    public bool direction_fixed = false;
    public IntValue Power;
    public float RayStartOffset = 0.6f;
    public float RayEndOffset = 0.9f;
    public float LiftHeight = 0.5f;
    public GameObject Map;
    public Vector3 LiftBoxLeftPos;
    public Vector3 LiftBoxRightPos;
    public Vector3 LiftBoxUpPos;
    public Vector3 LiftBoxDownPos;
    public Vector3 forward;
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Right;
    public KeyCode Left;
    public KeyCode Interact;
    public GameObject interact = null;
    public GameObject interact_target = null;
    public List<string> MovableTagList = new List<string>();
    public List<string> InteractableTagList = new List<string>();
    private Animator animator;
    public void focus_on()
    {
        focus = true;
    }
    public void focus_off()
    {
        focus = false;
    }
    void Start()
    {
        Value value = null;
        VirtualMachine.Instance.TryGetVar("PlayerPower", ref value);
        Power = value as IntValue;
        animator = GetComponent<Animator>();
        this.UpdateAsObservable()
             .Where(_ => focus && Input.GetKey(Up) && Check_Tile(Direction.Up) && Check_Tile_Interact(Direction.Up))
             .Subscribe(_ => Move(Direction.Up))
             .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => focus && Input.GetKey(Down) && Check_Tile(Direction.Down) && Check_Tile_Interact(Direction.Down))
            .Subscribe(_ => Move(Direction.Down))
            .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => focus && Input.GetKey(Right) && Check_Tile(Direction.Right) && Check_Tile_Interact(Direction.Right))
            .Subscribe(_ => Move(Direction.Right))
            .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => focus && Input.GetKey(Left) && Check_Tile(Direction.Left) && Check_Tile_Interact(Direction.Left))
            .Subscribe(_ => Move(Direction.Left))
            .AddTo(gameObject);
        this.UpdateAsObservable()
           .Where(_ => focus && Input.GetKeyDown(Interact) && interact==null && interact_target !=null)
           .Subscribe(_ => DoInteract(interact_target))
           .AddTo(gameObject);
        this.UpdateAsObservable()
           .Where(_ => focus && Input.GetKeyUp(Interact) && interact != null && interact.GetComponent<TextBox>()!=null)
           .Subscribe(_ => DoNotInteract(interact))
           .AddTo(gameObject);
        this.UpdateAsObservable()
          .Where(_ => focus && Input.GetKeyDown(Interact) && animator.GetBool("Lifting") && interact.GetComponent<Box>() != null)
          .Subscribe(_ => DoNotInteract(interact))
          .AddTo(gameObject);
        this.UpdateAsObservable()
            .Where(_ => focus && Input.GetKeyDown(Left) || Input.GetKeyDown(Right) || Input.GetKeyDown(Down) || Input.GetKeyDown(Up))
            .Subscribe(_ => { animator.SetBool("Walk", true); })
            .AddTo(gameObject);
      
        this.UpdateAsObservable()
          .Where(_ => focus && animator.GetBool("Walk"))
          .Subscribe(_ => { animator.SetBool("Walk", Moving); })
          .AddTo(gameObject);


    }

    public void DoInteract(GameObject target)
    {
        interact = target;
        if (interact.GetComponent<TextBox>() != null)
        {
            animator.SetBool("Push", true);
            direction_fixed = true;
        }
        else if(interact.GetComponent<Box>() != null)
        {
            if(Power.Value >= interact.GetComponent<Box>().Weight)
            {
                animator.SetBool("Lift", true);
            }
            else
            {
                interact = null;
            }
        }

    }
    public void Lift()
    {
        if (interact && interact.GetComponent<Box>() != null)
        {
            focus_off();
            interact.transform.SetParent(transform);
            interact.GetComponent<BoxCollider2D>().enabled = false;
            Vector3 pos = Vector3.zero;

            if (forward == Vector3.left)
            {
                pos = LiftBoxLeftPos;
            }
            else if (forward == Vector3.right)
            {
                pos = LiftBoxRightPos;
            }
            else if (forward == Vector3.up)
            {
                pos = LiftBoxUpPos;

            }
            else if (forward == Vector3.down)
            {
                pos = LiftBoxDownPos;
            }
            interact.transform.DOLocalMove(pos, 0.5f)
                .OnComplete(() => { focus_on();
                }
                );
            animator.SetBool("Lift", false);
            animator.SetBool("Lifting", true);

        }
    }
  

    public void Release()
    {
        focus_off();
        if (interact && interact.GetComponent<Box>() != null)
        {

            interact.transform.DOMove(transform.position + forward * TileOffset, 0.5f)
                .OnComplete(() => { focus_on();
                    interact.transform.SetParent(Map.transform);
                    interact.GetComponent<BoxCollider2D>().enabled = true;
                    direction_fixed = false;
                    interact = null;
                }
                );
            animator.SetBool("Release", false);
            animator.SetBool("Lifting", false);
            interact.GetComponent<SpriteRenderer>().sortingLayerName= "Player";

        }
    }
    private Direction Vector3ToDirection(Vector3 v)
    {
        if (v == Vector3.left)
        {
            return Direction.Left;
        }
        else if(v == Vector3.right)
        {
            return Direction.Right;
        }
        else if (v == Vector3.up)
        {
            return Direction.Up;

        }
        else
        {
            return Direction.Down;
        }
    }
    public void DoNotInteract(GameObject target)
    {
        if (interact && interact.GetComponent<Box>() != null && Check_Tile(Vector3ToDirection(forward)))
        {
            animator.SetBool("Release", true);
        }
        else if (interact && interact.GetComponent<TextBox>() != null)
        {
            animator.SetBool("Push", false);
            direction_fixed = false;
            interact = null;
        }
    }
    public void FixBoxPosition()
    {
        if(forward==Vector3.up)
        {
            interact.GetComponent<SpriteRenderer>().sortingLayerName= "Player";
        }
        else
        {
            interact.GetComponent<SpriteRenderer>().sortingLayerName="Lift";
        }

        if (forward == Vector3.left)
        {
            interact.transform.localPosition = LiftBoxLeftPos;
        }
        else if (forward == Vector3.right)
        {
            interact.transform.localPosition = LiftBoxRightPos;
        }
        else if (forward == Vector3.up)
        {
            interact.transform.localPosition = LiftBoxUpPos;
            
        }
        else if (forward == Vector3.down)
        {
            interact.transform.localPosition = LiftBoxDownPos;
        }
    }
    void Update()
    {
        if (animator.GetBool("Lifting"))
        {
            FixBoxPosition();
        }
        RaycastHit2D hit;
        GetComponent<SpriteRenderer>().sortingOrder= (int)(transform.position.y / -0.5f); // layering
        Debug.DrawRay(transform.position + forward * RayStartOffset, forward * RayEndOffset, new Color(0, 1, 0));
        hit = Physics2D.Raycast(transform.position + forward * RayStartOffset, forward * RayEndOffset);
        if(interact == null && hit && InteractableTagList.Contains(hit.collider.tag) )
        {
            if(hit.collider.GetComponent<TextBox>() !=null)
            {
                hit.collider.GetComponent<TextBox>().CanInteract();
                interact_target = hit.collider.gameObject;
            }
            else if(hit.collider.GetComponent<Box>() != null)
            {
               hit.collider.GetComponent<Box>().CanInteract();
               interact_target = hit.collider.gameObject;
            }
        }
        else
        {
            interact_target = null;
        }
    }
    public bool Check_Tile_Interact(Direction direction)
    {
        RaycastHit2D hit;
        if (interact == null || interact.GetComponent<Box>() != null)
        {
            return true;
        }
        else
        {
            if (direction == Direction.Left)
            {
                if (forward * -1 == Vector3.left)
                {
                    return true;
                }
                Debug.DrawRay(interact.transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, new Color(0, 1, 0));
                hit = Physics2D.Raycast(interact.transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, 1);
                if (hit && MovableTagList.Contains(hit.collider.tag))
                {
                    return true;
                }
            }
            else if (direction == Direction.Right)
            {
                if (forward * -1 == Vector3.right)
                {
                    return true;
                }
                Debug.DrawRay(interact.transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, new Color(1, 0, 0));
                hit = Physics2D.Raycast(interact.transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, 1);
                if (hit && MovableTagList.Contains(hit.collider.tag))
                {
                    return true;
                }
            }
            else if (direction == Direction.Up)
            {
                if (forward * -1 == Vector3.up)
                {
                    return true;
                }
                Debug.DrawRay(interact.transform.position + Vector3.up * RayStartOffset, Vector3.up * RayEndOffset, new Color(0, 1, 0));
                hit = Physics2D.Raycast(interact.transform.position + Vector3.up * RayStartOffset, Vector3.up * RayEndOffset, 1);
                if (hit && MovableTagList.Contains(hit.collider.tag))
                {
                    return true;
                }
            }
            else if (direction == Direction.Down)
            {
                if (forward * -1 == Vector3.down)
                {
                    return true;
                }
                Debug.DrawRay(interact.transform.position + Vector3.down * RayStartOffset, Vector3.down * RayEndOffset, new Color(0, 1, 0));
                hit = Physics2D.Raycast(interact.transform.position + Vector3.down * RayStartOffset, Vector3.down * RayEndOffset, 1);
                if (hit && MovableTagList.Contains(hit.collider.tag))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool Check_Tile(Direction direction)
    {
        RaycastHit2D hit;
        if (direction == Direction.Left)
        {
            if (forward == Vector3.left && interact != null && interact.GetComponent<TextBox>() != null)
            {
                return true;
            }
            if (!direction_fixed)
            { 
                  forward = Vector3.left;
                animator.SetInteger("X", (int)forward.x);
                animator.SetInteger("Y", (int)forward.y);
            }
            Debug.DrawRay(transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, new Color(0, 1, 0));
            hit = Physics2D.Raycast(transform.position + Vector3.left * RayStartOffset, Vector3.left * RayEndOffset, 1);
            if (hit && MovableTagList.Contains(hit.collider.tag))
            {
                return true;
            }
        }
        else if (direction == Direction.Right)
        {
            if (forward == Vector3.right && interact != null && interact.GetComponent<TextBox>() != null)
            {
                return true;
            }
            if (!direction_fixed)
            {
                forward = Vector3.right;
                animator.SetInteger("X", (int)forward.x);
                animator.SetInteger("Y", (int)forward.y);
            }
            Debug.DrawRay(transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, new Color(0, 1, 0));
            hit = Physics2D.Raycast(transform.position + Vector3.right * RayStartOffset, Vector3.right * RayEndOffset, 1);
            if (hit && MovableTagList.Contains(hit.collider.tag))
            {
                return true;
            }
        }
        else if (direction == Direction.Up)
        {
            if (forward == Vector3.up && interact != null && interact.GetComponent<TextBox>() != null)
            {
                return true;
            }
            if (!direction_fixed)
            {
                forward = Vector3.up;
                animator.SetInteger("X", (int)forward.x);
                animator.SetInteger("Y", (int)forward.y);
            }
            Debug.DrawRay(transform.position + Vector3.up * RayStartOffset, Vector3.up * RayEndOffset, new Color(0, 1, 0));
            hit = Physics2D.Raycast(transform.position + Vector3.up * RayStartOffset, Vector3.up * RayEndOffset, 1);
            if (hit && MovableTagList.Contains(hit.collider.tag))
            {
                return true;
            }
        }
        else if (direction == Direction.Down)
        {
            if (forward == Vector3.down && interact != null && interact.GetComponent<TextBox>() != null)
            {
                return true;
            }
            if (!direction_fixed)
            {
                forward = Vector3.down;
                animator.SetInteger("X", (int)forward.x);
                animator.SetInteger("Y", (int)forward.y); 
            }
            Debug.DrawRay(transform.position + Vector3.down * RayStartOffset, Vector3.down * RayEndOffset, new Color(0, 1, 0));
            hit = Physics2D.Raycast(transform.position + Vector3.down * RayStartOffset, Vector3.down * RayEndOffset, 1);
            if (hit && MovableTagList.Contains(hit.collider.tag))
            {
                return true;
            }
        }
        return false;
    }
    public void Move(Direction direction)
    {
        if (!Moving)
        {
            Moving = true;
            if (direction == Direction.Left)
            {
                
                if(interact != null)
                    interact.transform.DOMove(interact.transform.position + Vector3.left * TileOffset, MoveTime);
                transform.DOMove(transform.position+Vector3.left * TileOffset, MoveTime)
                    .OnComplete(() => { Moving = false; });
            }
            else if (direction == Direction.Right)
            {
                
                if (interact != null)
                    interact.transform.DOMove(interact.transform.position + Vector3.right * TileOffset, MoveTime);
                transform.DOMove(transform.position + Vector3.right * TileOffset, MoveTime)
                    .OnComplete(() => { Moving = false; });
            }
            else if (direction == Direction.Up)
            {
                
                if (interact != null)
                    interact.transform.DOMove(interact.transform.position + Vector3.up * TileOffset, MoveTime);
                transform.DOMove(transform.position + Vector3.up * TileOffset, MoveTime)
                    .OnComplete(() => { Moving = false; });
            }
            else if (direction == Direction.Down)
            {
               
                if (interact != null)
                    interact.transform.DOMove(interact.transform.position + Vector3.down * TileOffset, MoveTime);
                transform.DOMove(transform.position + Vector3.down * TileOffset, MoveTime)
                    .OnComplete(() => { Moving = false; });
            }
        }
    }
}
