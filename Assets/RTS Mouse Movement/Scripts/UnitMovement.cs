using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{
    private Collider _collider;
    Rigidbody _rb;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float distLeniency = 1.0f;

    private bool _selected = false;
    private bool _inAction = false;

    private float _targetX = 0.00f;
    private float _thisY;
    private float _targetZ = 0.00f;
    private Vector3 _targetPos;

    private void Awake()
    {
        _thisY = this.transform.position.y;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Checks on initial left clikc
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {


                if (hit.collider.gameObject == this.gameObject)
                {
                    _selected = true;
                }
                else
                {
                    _selected = false;

                }
            }
        }

        // Checks if left click held down
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {


                if (hit.collider.gameObject == this.gameObject)
                {
                    _selected = true;
                }
            }
        }

        // Move to right click point if selected
        if (Input.GetMouseButtonDown(1) && _selected == true)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                _targetX = hit.point.x;
                _targetZ = hit.point.z;
                Debug.Log(hit.point);

                Vector3 _targetPos = new Vector3(_targetX,
                                                1,
                                                _targetZ);
                transform.LookAt(_targetPos);
            }

            Debug.Log("Active");
            _inAction = true;
        }

        // Check if at target destination
        /*if((this.transform.position.x == _targetX) && (this.transform.position.z == _targetZ))
        {
            Debug.Log("Inactive");
            _inAction = false;
        }*/

        if(Vector3.Distance(transform.position, _targetPos) < distLeniency)
        {
            Debug.Log("Inactive");
            _inAction = false;
        }

            // Move to target destination if active
            if (_inAction == true)
        {
            _rb.velocity = transform.forward * speed;
        }
        else
        {
            _rb.velocity = transform.forward * 0;
        }
    }
}
