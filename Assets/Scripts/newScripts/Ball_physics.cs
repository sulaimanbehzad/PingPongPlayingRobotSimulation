using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ball_physics : MonoBehaviour
{
    public Rigidbody rb;
    public float Cd = 0.54F;
    public float po = 1.184F;
    public float A = 12.5E-04F;
    //public float Cm = 0.069F;  //Nakashima+, 2010
    public float Cm = 0.25F;  //Tanaka, 2015
    public float m = 2.7E-03F;
    public float r = 0.02F;

    public float et = 0.93F;
    public float mu = 0.25F;
    public float er = 0.81F;
    public float kp = 1.9E-03F;

    public bool is2D=true;
    public bool isSpin=true;
    float m_isSpin = 1F;
    // left-handed to right-handed
    private Matrix4x4 Ax = Matrix4x4.zero;
    private Matrix4x4 inv_Ax = Matrix4x4.zero;
    // for 2D & 3D
    private Matrix4x4 Cx = Matrix4x4.identity;
    private Matrix4x4 Cw = Matrix4x4.identity;
    
    // Start is called before the first frame update
    void Start()
    {
        Set_ball();
        Set_Matrix();
        if (!isSpin)
            m_isSpin = 0;
        else
            m_isSpin = 1;
        //rb.angularVelocity = new Vector3(-100, 0, 0);
        //rb.velocity = new Vector3(0, 0, -10);
        //Time.timeScale = 0.2F;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Air_resistance();
        rb.AddForce(new Vector3(0, -9.81F * m, 0));
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("court_A") || collision.gameObject.CompareTag("court_B"))
        {
            Bound_on_court();
        }
        else if (collision.gameObject.CompareTag("racket_A") || collision.gameObject.CompareTag("racket_B"))
        {
            Bound_on_racket(collision);
        }
        else if (collision.gameObject.CompareTag("net"))
        {
            Conntact_net();
        }
        else if (collision.gameObject.CompareTag("wall"))
        {
            //Debug.Log("wall");
        }
    }

    void Set_ball()
    {
        rb = GetComponent<Rigidbody>();
        rb.transform.localScale = Vector3.one * 2 * r;
        rb.mass = m;
        rb.inertiaTensor = Vector3.one * 2 / 3 * m * r * r;
        rb.maxAngularVelocity = Mathf.Infinity;
    }

    void Set_Matrix()
    {
        Ax.m02 = 1;
        Ax.m10 = -1;
        Ax.m21 = 1;
        Ax.m33 = 1;
        inv_Ax.m01 = -1;
        inv_Ax.m12 = 1;
        inv_Ax.m20 = 1;
        inv_Ax.m33 = 1;
        if (is2D)
        {
            Cx.m00 = 0;
            Cw.m11 = 0;
            Cw.m22 = 0;
        }
    }

    void Bound_on_court()
    {
        Vector3 v = rb.velocity;
        Vector3 w = m_isSpin * rb.angularVelocity;
        Vector3 vtan = new Vector3(v.x, 0, v.z) + Vector3.Cross(w, new Vector3(0, -r, 0));
        float alpha = mu * (1 + et) * Mathf.Abs(v.y / vtan.magnitude);
        float vs = 1 - 5 / 2 * alpha;
        if (vs < 0)
        {
            alpha = 2 / 5;
        }
        Matrix4x4 Av = Matrix4x4.zero;
        Av.m00 = 1 - alpha;
        Av.m11 = 1 - alpha;
        Av.m22 = -et;
        Matrix4x4 Bv = Matrix4x4.zero;
        Bv.m01 = -alpha * r;
        Bv.m10 = alpha * r;
        Matrix4x4 Aw = Matrix4x4.zero;
        Aw.m01 = 3 * alpha / 2 / r;
        Aw.m10 = -3 * alpha / 2 / r;
        Matrix4x4 Bw = Matrix4x4.zero;
        Bw.m00 = 1 - 3 / 2 * alpha;
        Bw.m11 = 1 - 3 / 2 * alpha;
        Bw.m22 = 1;
        Av = inv_Ax * Av * Ax;
        Bv = inv_Ax * Bv * Ax;
        Aw = inv_Ax * Aw * Ax;
        Bw = inv_Ax * Bw * Ax;
        Vector3 res_v = Av * v + Bv * w;
        Vector3 res_w = Aw * v + Bw * w;
        rb.velocity = Cx * res_v;
        rb.angularVelocity = Cw * res_w * m_isSpin;
    }

    void Bound_on_racket(Collider collision)
    {
        Rigidbody racket_Rb = collision.gameObject.GetComponentInParent<Rigidbody>();
        DeltaAgent1 racket = collision.gameObject.GetComponentInParent<DeltaAgent1>();
        Vector3 rel_v = rb.velocity - racket_Rb.velocity;
        Vector3 w = m_isSpin * rb.angularVelocity;
        Quaternion q = racket_Rb.transform.rotation;
        // convert to racket coordinate
        Vector3 rot_v = Quaternion.Inverse(q) * rel_v;
        Vector3 rot_w = Quaternion.Inverse(q) * w;

        float Kv = kp / m;
        float Kw = kp / rb.inertiaTensor.x;
        Matrix4x4 Av = Matrix4x4.zero;
        Av.m00 = 1 - Kv;
        Av.m11 = 1 - Kv;
        Av.m22 = -er;
        Matrix4x4 Bv = Matrix4x4.zero;
        Bv.m01 = -r * Kv;
        Bv.m10 = r * Kv;
        Matrix4x4 Aw = Matrix4x4.zero;
        Aw.m01 = r * Kw;
        Aw.m10 = -r * Kw;
        Matrix4x4 Bw = Matrix4x4.zero;
        Bw.m00 = 1 - Kw * r * r;
        Bw.m11 = 1 - Kw * r * r;
        Bw.m22 = 1;
        Av = inv_Ax * Av * Ax;
        Bv = inv_Ax * Bv * Ax;
        Aw = inv_Ax * Aw * Ax;
        Bw = inv_Ax * Bw * Ax;
        // bound
        Vector3 res_rot_v = Av * rot_v + Bv * rot_w;
        Vector3 res_rot_w = Aw * rot_v + Bw * rot_w;
        // convert to world coordinate
        rel_v = q * res_rot_v;
        Vector3 res_w = q * res_rot_w;
        Vector3 res_v = rel_v + racket_Rb.velocity;
        res_v = Cx * res_v;
        res_w = Cw * res_w * m_isSpin;
        racket.BallTouch = Quaternion.Inverse(q) * (m * res_v - m * rb.velocity);
        racket.Hitting_V = racket_Rb.velocity;
        racket.Hitting_Q = racket_Rb.rotation;
        rb.velocity = res_v;
        rb.angularVelocity = res_w;
    }

    void Conntact_net()
    {
        Vector3 x = rb.position;
        Vector3 v = rb.velocity;
        Vector3 w = rb.angularVelocity;
        if(x.y > 0.865)
        {
            // 適当な挙動（ネットイン）
            rb.velocity = new Vector3(0.2F * v.x, 0.2F * Math.Abs(v.y), 0.2F * v.z);
            rb.angularVelocity = 0.2F * w;
        }
        else
        {
            // 適当な挙動
            rb.velocity = new Vector3(0.2F * v.x, 0.2F * v.y, -0.2F * v.z);
            rb.angularVelocity = 0.2F * w;
        }
    }

    void Air_resistance()
    {
        Vector3 Fd = -1F / 2F * Cd * po * A * rb.velocity.magnitude * rb.velocity;
        Vector3 Fm = 4F / 3F * Cm * (float)Math.PI * po * (float)Math.Pow(r, 3) * Vector3.Cross(m_isSpin * rb.angularVelocity, rb.velocity);
        Vector3 Td = -1F / 64F * Cd * po * 4F / 3F * (float)Math.Pow(r, 3) * rb.angularVelocity.magnitude * rb.angularVelocity * m_isSpin;
        Fd = Cx * Fd;
        Fm = Cx * Fm;
        Td = Cw * Td;
        rb.AddForce(Fd + Fm);
        rb.AddTorque(Td);
    }
}
