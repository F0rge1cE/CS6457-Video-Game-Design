using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {
    [SerializeField]
    private Transform target;

    public float rotSpeed = 1.5f;

    private float _rotY;
	private float _rotX;
	private Vector3 distance;
	private Vector3 direction;

	private float scroll = 0f;

    private Vector3 _offset;


	// Use this for initialization
	void Start () {

		_rotX = transform.eulerAngles.x;
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position;



	}

	void LateUpdate () {

//			direction = target.position - transform.position;
			

			float horInput = Input.GetAxis ("Horizontal");
			if (horInput != 0) {
				_rotY += horInput * rotSpeed;
			} else {
			
//			Debug.Log (_rotX);
//				if (_rotX >= 0f && _rotX <= 90f) {
//					_rotX += Input.GetAxis ("Mouse Y") * rotSpeed * 3;
//				} else {
//					_rotX = _rotX;
//				}

				_rotY += Input.GetAxis ("Mouse X") * rotSpeed * 3;
			}
			
//			scroll = Input.GetAxis ("Mouse ScrollWheel");
//			distance += Input.GetAxis ("Mouse ScrollWheel") * direction * 3;
//			transform.position += distance;


//			Quaternion rotation = Quaternion.Euler (_rotX, _rotY, 0);
			Quaternion rotation = Quaternion.Euler (0, _rotY, 0);
			transform.position = target.position - (rotation * _offset); //+ distance;
			transform.LookAt (target);


//			Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
//			transform.position += distance;





	}
}
