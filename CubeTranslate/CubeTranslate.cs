using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTranslate : MonoBehaviour
{
    //オブジェクトの進行ベクトル
    [SerializeField] private Vector3 velocity;
    //移動オブジェクトの位置
    [SerializeField] private Vector3 initialposition;
    //対象オブジェクトの位置
    [SerializeField] private Vector3 targetPosition ;
    //対象オブジェクトへの到着時間
    public float interval;

    void Update()
    {
        var acceleration = Vector3.zero;

        //s(変位)= v0(初速度)*t(時間) + 1/2*a(加速度)*t^2(時間)
        var diff = targetPosition - initialposition;
        acceleration += (diff - velocity * interval) * 2f / (interval * interval);

        interval -= Time.deltaTime;
        if (interval < 0f)
        {
            return;
        }

        velocity += acceleration * Time.deltaTime;
        initialposition += velocity * Time.deltaTime;
        transform.position = initialposition;
    }
}
