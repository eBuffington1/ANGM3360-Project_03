using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{
    private Collider _collider;
    Rigidbody _rb;

    [SerializeField] GameObject _waypointObject;
    private GameObject _waypoint;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float distLeniency = 0.5f;

    private bool _selected = false;
    private bool _inAction = false;
    private float _distCheck;

    private float _targetX;
    private float _thisY;
    private float _targetZ;
    private Vector3 _targetPos;

    private void Awake()
    {
        _thisY = this.transform.position.y;
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Checks on initial left click
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(_waypoint);

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
                //Debug.Log(hit.point);

                _targetPos = new Vector3(_targetX,
                                         _thisY,
                                         _targetZ);
                transform.LookAt(_targetPos);

                Destroy(_waypoint);
                _waypoint = Instantiate(_waypointObject, _targetPos, transform.rotation);
            }

            _inAction = true;
        }

        // Check if near waypoint
        _distCheck = Vector3.Distance(transform.position, _targetPos);
        if(_distCheck < distLeniency)
        {
            _inAction = false;
            Destroy(_waypoint);
        }

        // Move to target destination if active
        if (_inAction == true)
        {
            _rb.velocity = transform.forward * speed;
            transform.LookAt(_targetPos);
        }
        else
        {
            _rb.velocity = transform.forward * 0;
        }
    }
}
