using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

public class Hook : MonoBehaviour
{
	public LineRenderer line;
	public LineRenderer hook_aim;
	private DistanceJoint2D joint;
	private Vector3 targetPos;
	private RaycastHit2D hit;
	public float distance = 10f;
	public LayerMask mask;
	public float step = 0.03f;
	private Vector2 connectPoint;
	private Vector2 launch_speed;
	private bool was_hooked;
	private Color clear_red = Color.red;
	private Color clear_green = Color.green;
	private Color clear_yellow = Color.yellow;

	// Use this for initialization
	void Start()
	{
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;
		line.GetComponent<LineRenderer>().startWidth = 0.3f;
		line.GetComponent<LineRenderer>().endWidth = 0.3f;
		line.enabled = false;
		clear_red.a = 0.2f;
		clear_green.a = 0.2f;
		clear_yellow.a = 0.2f;
		hook_aim.GetComponent<LineRenderer>().startWidth = 0.2f;
		hook_aim.GetComponent<LineRenderer>().endWidth = 0.2f;
		hook_aim.enabled = false;
		was_hooked = false;
	}

	// Update is called once per frame
	void Update()
	{
		launch_speed = GetComponent<Rigidbody2D>().velocity;
		targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		targetPos.z = 0;
		hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

		if (!was_hooked && hit.collider != null)
        {
			if (hit.collider.tag == "Hook")
			{
				hook_aim.GetComponent<LineRenderer>().startColor = clear_green;
				hook_aim.GetComponent<LineRenderer>().endColor = clear_yellow;
			}
			else
            {
				hook_aim.GetComponent<LineRenderer>().startColor = clear_red;
				hook_aim.GetComponent<LineRenderer>().endColor = clear_red;
			}
			hook_aim.enabled = true;
			hook_aim.SetPosition(0, transform.position);
			hook_aim.SetPosition(1, hit.point);
		}
		else
        {
			hook_aim.enabled = false;
        }

			if (joint.distance > .5f)
				joint.distance -= step;
		/*else
		{
			line.enabled = false;
			joint.enabled = false;

		}*/


		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			if (hit.collider != null) //&& hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)

			{
				if (hit.collider.tag == "Hook")
				{
					was_hooked = true;
					joint.enabled = true;
					//	Debug.Log (hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
					connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
					connectPoint.x /= hit.collider.transform.localScale.x;
					connectPoint.y /= hit.collider.transform.localScale.y;
					//Debug.Log(connectPoint);
					joint.connectedAnchor = connectPoint;

					//joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
					//		joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
					joint.distance = Vector2.Distance(transform.position, connectPoint);

					line.enabled = true;
					line.SetPosition(0, transform.position);
					//line.SetPosition(1, connectPoint);

					line.GetComponent<HookRatio>().grabPos = connectPoint;

				}
			}
		}
		line.SetPosition(1, connectPoint);//joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

		if (Input.GetKey(KeyCode.Mouse1))
		{

			line.SetPosition(0, transform.position);
		}


		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			joint.enabled = false;
			line.enabled = false;
			if (was_hooked)
				GetComponent<Rigidbody2D>().velocity += launch_speed;
			was_hooked = false;
		}

	}
}