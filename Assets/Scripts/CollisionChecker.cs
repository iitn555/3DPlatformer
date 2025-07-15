using UnityEngine;

public class CollisionChecker
{

    private static MYRECT m_tRect = new MYRECT();

    

    public static bool LineCollsion(GameObject pObjFootLine, GameObject pBoxHeadLine)
    {

        MYRECT rc1 = Update_GameObject(pObjFootLine);
        MYRECT rc2 = Update_GameObject(pBoxHeadLine);

        if (rc1.left < rc2.right &&
        rc1.right > rc2.left &&
        rc1.FootTop >= rc2.HeadBot &&
        rc1.bottom <= rc2.top)
        {
            return true;
        }

        return false;

    }

    public static bool RectCollsionAndPush(GameObject pObj, GameObject pColliedObj)
    {

        MYRECT rc1 = Update_GameObject(pObj);
        MYRECT rc2 = Update_GameObject(pColliedObj);

        if (rc1.left < rc2.right &&
            rc1.right > rc2.left &&
            rc1.top > rc2.bottom &&
            rc1.bottom < rc2.top)
        {
            PushDestObjPosition(pObj, pColliedObj);
            return true;

        }

        return false;
    }

    public static void PushDestObjPosition(GameObject rDestObj, GameObject pBox)
    {
        //MYRECT rc1 = rDestObj.m_tRect;
        //MYRECT rc2 = pBox.m_tRect;

        MYRECT rc1 = Update_GameObject(rDestObj);
        MYRECT rc2 = Update_GameObject(pBox);

        var rDestObjPosition = rDestObj.transform.position;
        var pBoxPosition = pBox.transform.position;

        if (rDestObjPosition.x < pBoxPosition.x) // rDestObj 가 왼쪽
        {
            if (rDestObjPosition.y < pBoxPosition.y) // rDestObj가 위쪽
            {

                float A = rc1.right - rc2.left;
                float B = rc1.top - rc2.bottom;
                if (A < B) // 겹친 면적을 비교해서 어디서 부딪혔는지 추정
                    rDestObjPosition.x -= A;
                else
                    rDestObjPosition.y -= B;
            }
            else //rDestObj가 아래쪽
            {
                float A = rc1.right - rc2.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x -= A;
                else
                    rDestObjPosition.y += B;
            }
        }
        else
        {
            if (rDestObjPosition.y < pBoxPosition.y)
            {
                float A = rc2.right - rc1.left;
                float B = Mathf.Abs(rc2.bottom - rc1.top);
                if (A < B)
                    rDestObjPosition.x += A;
                else
                    rDestObjPosition.y -= B;
            }
            else
            {
                float A = rc2.right - rc1.left;
                float B = rc2.top - rc1.bottom;
                if (A < B)
                    rDestObjPosition.x += A;
                else
                    rDestObjPosition.y += B;
            }
        }


        rDestObj.transform.position = rDestObjPosition;



    }



    public static MYRECT Update_GameObject(GameObject _obj)
    {
        var m_vPos = _obj.transform.position;
        var m_vSize = _obj.transform.localScale;

        m_tRect.left = m_vPos.x - m_vSize.x * 0.5f;
        m_tRect.right = m_vPos.x + m_vSize.x * 0.5f;
        m_tRect.top = m_vPos.y + m_vSize.y * 0.5f;
        m_tRect.bottom = m_vPos.y - m_vSize.y * 0.5f;
        m_tRect.front = m_vPos.z + m_vSize.z * 0.5f;
        m_tRect.back = m_vPos.z - m_vSize.z * 0.5f;
        m_tRect.HeadBot = m_tRect.top - 0.1f;
        m_tRect.FootTop = m_tRect.bottom + 0.1f;

        return m_tRect;

    }



    //public static bool RectCollsionAndPush(CGameObject pObj, CGameObject pColliedObj)
    //{
    //    MYRECT rc1 = pObj.m_tRect;
    //    MYRECT rc2 = pColliedObj.m_tRect;

    //    if (rc1.left < rc2.right &&
    //        rc1.right > rc2.left &&
    //        rc1.top > rc2.bottom &&
    //        rc1.bottom < rc2.top)
    //    {
    //        PushDestObjPosition(pObj, pColliedObj);
    //        //Debug.LogError("true");

    //        return true;

    //    }

    //    return false;
    //}


    //public static bool LineCollsion(CGameObject pObjFootLine, CGameObject pBoxHeadLine)
    //{

    //    MYRECT rc1 = pObjFootLine.m_tRect;
    //    MYRECT rc2 = pBoxHeadLine.m_tRect;

    //    if (rc1.left < rc2.right &&
    //    rc1.right > rc2.left &&
    //    rc1.FootTop >= rc2.HeadBot &&
    //    rc1.bottom <= rc2.top)
    //    {
    //        //PushDestObj(pObj, pBox);

    //        return true;

    //    }

    //    return false;

    //}


    //public static bool CheckFallingLine(CGameObject pObjFootLine, CGameObject pBoxHeadLine, float FootTop, float Bottom)
    //{

    //    MYRECT rc1 = pObjFootLine.m_tRect;
    //    MYRECT rc2 = pBoxHeadLine.m_tRect;

    //    if (rc1.left < rc2.right &&
    //        rc1.right > rc2.left &&
    //        FootTop > rc2.HeadBot &&
    //        Bottom < rc2.top)
    //    {

    //        return true;

    //    }

    //    return false;

    //}



}