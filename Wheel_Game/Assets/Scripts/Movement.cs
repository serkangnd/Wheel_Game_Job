using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Speed Settings")]
    public float movementSpeed;
    public float controlSpeed;
    public Rigidbody rbPlayer;

    //Touch settings
    [Header("Touch Settings")]
    [SerializeField] bool isTouching;


    public Animator animator;
    public Camera cam;
    public GameManager.PlayerState playerState;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Make sure its sync
        playerState = GameManager.inst.playerState;

        GetInput();
        //GetInputMobile();
        Move();

    }

    //Testing Inputs
    void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            //Check if still is working or not
            if (GameManager.inst.playerState != GameManager.PlayerState.Finish)
            {
                isTouching = true;
                GameManager.inst.playerState = GameManager.PlayerState.Playing;
            }          
        }
        else
        {
            isTouching = false;
        }
    }
    void GetInputMobile()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (GameManager.inst.playerState != GameManager.PlayerState.Finish)
            {
                isTouching = true;
                GameManager.inst.playerState = GameManager.PlayerState.Playing;
            }
        }
        else
        {
            isTouching = false;
        }
    }

    void Move()
    {

        if (playerState == GameManager.PlayerState.Playing)
        {       
           transform.position += movementSpeed * Time.deltaTime * Vector3.forward;
           animator.SetTrigger("GameStart");
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;

        //Vector3 touchPos = Input.touches[0].position;
        //touchPos.z = cam.transform.localPosition.z;

        if (isTouching)
        {
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                GameObject firstWheel = WheelList.instance.wheels[0];
                Vector3 hitVec = hit.point;
                hitVec.y = firstWheel.transform.localPosition.y;
                hitVec.z = firstWheel.transform.localPosition.z;

                firstWheel.transform.localPosition = Vector3.MoveTowards(firstWheel.transform.localPosition, hitVec, Time.deltaTime * controlSpeed);
            }

        }
    }

}
