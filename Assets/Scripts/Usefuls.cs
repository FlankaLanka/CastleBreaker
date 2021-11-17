using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Usefuls
{
    public static class UsefulConstant
    {
        public static int EnemyLayer = 8;
        public static int PlayerLayer = 9;
        public static int WallLayer = 10;
        public static int Hidden = 11;
        public static int Dashing = 12;
        public static int BulletLayer = 13;

        public static int EnemyLayerMask = 1<<EnemyLayer;
        public static int PlayerLayerMask = 1<<PlayerLayer;
        public static int WallLayerMask = 1<<WallLayer;
        public static int HiddenMask = 1<<Hidden;
        public static int DashingMask = 1<<Dashing;
        public static int BulletLayerMask = 1<<BulletLayer;

    }

    public static class UsefulFunction {
        public static Vector2 RotVector2(Vector2 v, float radius){
            return new Vector2(v.x * Mathf.Cos(radius) + v.y * Mathf.Sin(radius) , v.y * Mathf.Cos(radius) - v.x * Mathf.Sin(radius));
        }
    }

}
