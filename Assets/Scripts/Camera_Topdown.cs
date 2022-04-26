using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Topdown : MonoBehaviour
{
    #region Variables
    public Transform m_target;
    
    [SerializeField]
    private float m_height = 10f;
    
    [SerializeField]
    private float m_distance = 10f;
    
    [SerializeField]
    private float m_angle = 0f;
    
    [SerializeField]
    private float m_smoothSpeed = 0.5f;

    private Vector3 refVelocity;
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        HandleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
    }
    #endregion

    #region Helper Methods
    protected virtual void HandleCamera() {
        if(!m_target){
            return;
        }

        //Build world position vector
        Vector3 worldPosition = (Vector3.forward * -m_distance) + (Vector3.up * m_height);
        Debug.DrawLine(m_target.position, worldPosition, Color.red);

        //Build our rotated vector
        Vector3 rotatedVector = Quaternion.AngleAxis(m_angle, Vector3.up) * worldPosition;
        Debug.DrawLine(m_target.position, rotatedVector, Color.green);

        //Move our position
        Vector3 flatTargetPosition = m_target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotatedVector;
        Debug.DrawLine(m_target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, m_smoothSpeed);
        transform.LookAt(flatTargetPosition);
    }
    
    // void OnDrawGizmos(){
    //     Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
    //     if(m_target){
    //         Gizmos.DrawLine(transform.position, m_target.position);
    //         Gizmos.DrawSphere(m_target.position, 1.5f);
    //     }
    //     Gizmos.DrawSphere(transform.position, 1.5f);
    // }
    #endregion
}
