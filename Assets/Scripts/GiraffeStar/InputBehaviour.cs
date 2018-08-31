using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GiraffeStar
{
    /// <summary>
    /// UI 가 아닌 게임오브젝트 클릭 제어용
    /// </summary>
    public class InputBehaviour : MonoBehaviour
    {
        bool isDragging;
        float mouseDownTime;
        float clickThreshold = 0.3f;
        float holdThreshold = 0.7f;
        float dragThreshold = 0.5f;
        Vector3 inputPos;

        public Vector3 Offset { get; private set; }

        public Action OnInputDown;
        public Action OnClick;
        public Action OnHoldEnd;
        //public Action onMouseDragStart;
        public Action OnDrag;
        public Action OnDragEnd;
        public Action OnInputUp;



        void OnMouseDown()
        {
            inputPos = Input.mousePosition;
            var worldPos = Camera.main.ScreenToWorldPoint(inputPos).OverrideZ(0f);
            Offset = transform.position - worldPos;

            if(OnInputDown != null)
            {
                OnInputDown();
            }

            mouseDownTime = Time.realtimeSinceStartup;
        }

        void OnMouseDrag()
        {
            isDragging = true;
            if(OnDrag != null)
            {
                OnDrag();
            }
        }

        // MouseUp 시점을 EventSystem 보다 늦추기 위해 이렇게 구현
        IEnumerator OnMouseUp()
        {
            yield return new WaitForFixedUpdate();

            var inputPos = Input.mousePosition;

            if (OnDragEnd != null && isDragging)
            {
                OnDragEnd();
            }

            if(OnHoldEnd != null && Time.realtimeSinceStartup > mouseDownTime + holdThreshold)
            {
                if(Vector3.Distance(inputPos.OverrideZ(0f), Input.mousePosition.OverrideZ(0f)) < 0.1f)
                {
                    OnHoldEnd();
                }
            }

            // onclick 이 잘 작동안한다.. 나중에 한번 제대로 보자
            if(OnClick != null && Time.realtimeSinceStartup < mouseDownTime + clickThreshold)
            {
                OnClick();
            }

            isDragging = false;

            if(OnInputUp != null)
            {
                OnInputUp();
            }
        }
    }
}


