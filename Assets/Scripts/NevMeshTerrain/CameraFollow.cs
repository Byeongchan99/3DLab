using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NevMeshTerrain
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player; // 플레이어의 Transform
        public Vector3 offset; // 플레이어와 카메라 사이의 오프셋(거리)

        void LateUpdate()
        {
            // 카메라의 위치를 플레이어의 위치 + 오프셋으로 설정
            transform.position = player.position + offset;
        }
    }
}
