using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    private Vector3 initialPosition; // Lưu vị trí ban đầu của giáp

    void Start()
    {
        // Lưu vị trí ban đầu khi đối tượng được tạo
        initialPosition = transform.position;
    }

    public void ResetItem()
    {
        // Đặt lại giáp về vị trí ban đầu và bật lại nếu bị ẩn
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }
}
