using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xuanzhuanhole : MonoBehaviour
{

    public Vector3 rotationSpeed = new Vector3(0, 100, 0); // 可在 Inspector 中调整旋转速度

    private void OnEnable()
    {
        // 可选：启用时重置旋转
        transform.localRotation = Quaternion.identity; // 正确的重置方式
    }

    void Update()
    {
        // 持续旋转
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
