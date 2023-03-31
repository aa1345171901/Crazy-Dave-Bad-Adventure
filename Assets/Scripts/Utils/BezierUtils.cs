using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    public static class BezierUtils
    {
        /// <summary>
        /// 线性
        /// </summary>
        /// <param name="p0">起点</param>
        /// <param name="p1">终点</param>
        /// <param name="t">【0-1】</param>
        /// <returns></returns>
        public static Vector3 BezierPoint(Vector3 p0, Vector3 p1, float t)
        {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// 二阶曲线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 result = (1 - t) * p0p1 + t * p1p2;
            return result;
        }

        /// <summary>
        /// 三阶曲线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 result;
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 p2p3 = (1 - t) * p2 + t * p3;
            Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
            Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
            result = (1 - t) * p0p1p2 + t * p1p2p3;
            return result;
        }

        /// <summary>
        /// 多阶曲线  （可以递归 有多组线性组合）
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(float t, List<Vector3> p)
        {
            if (p.Count < 2)
                return p[0];
            List<Vector3> newP = new List<Vector3>();
            for (int i = 0; i < p.Count - 1; i++)
            {
                Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
                newP.Add(p0p1);
            }
            return BezierPoint(t, newP);
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组(二阶)
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="controlPoint">控制点</param>
        /// <param name="endPoint">目标点</param>
        /// <param name="segmentNum">采样点的数量</param>
        /// <returns>存储贝塞尔曲线点的数组</returns>
        public static Vector3[] GetBeizerPointList(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int segmentNum)
        {
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierPoint(startPoint, controlPoint, endPoint, t);
                path[i - 1] = pixel;
            }
            return path;
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组(多阶)
        /// </summary>
        /// <param name="segmentNum">采样点的数量</param>
        /// <param name="p">控制点集合</param>
        /// <returns></returns>
        public static Vector3[] GetBeizerPointList(int segmentNum, List<Vector3> p)
        {
            Vector3[] path = new Vector3[segmentNum];
            for (int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierPoint(t, p);
                path[i - 1] = pixel;
            }
            return path;
        }

    }
}