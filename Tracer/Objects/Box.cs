﻿using CSharp_Path_Tracer.Tracer.Rendering;
using System;
using System.Numerics;

namespace CSharp_Path_Tracer.Tracer.Objects
{
    internal class Box : Construction
    {
        protected Vector3 Position;
        protected Vector3 Dimensions;
        public Box(Vector3 position, Vector3 dimensions, Func<Intersection, Material> func) : base(func)
        {
            Position = position;
            Dimensions = dimensions;
        }

        public override Intersection Intersect(Vector3 rayOrigin, Vector3 rayDirection)  
        {
            // translate box to the origin
            Vector3 translated = rayOrigin - Position;
            Vector3 rd = rayDirection + new Vector3(1E-15f);
            Vector3 m = Vector3.Divide(Vector3.One, rd);
            Vector3 n = m * translated;
            Vector3 k = Vector3.Abs(m) * Dimensions;
            Vector3 t1 = -n - k;
            Vector3 t2 = -n + k;

            float tN = MathF.Max(t1.X, MathF.Max(t1.Y, t1.Z));
            float tF = MathF.Min(t2.X, MathF.Min(t2.Y, t2.Z));
            if (tN > tF || tF < 0.0f) return new Intersection(new Vector3(), new Vector3(), -1.0f);

            Vector3 normal = tN > 0.0f ? VectorUtil.Step(new Vector3(tN), t1) : VectorUtil.Step(t2, new Vector3(tF));

            float t = tN > 0.0f ? tN : tF;
            normal *= -VectorUtil.Sign(rayDirection);

            Vector3 position = rayOrigin + rayDirection * t;
            return new Intersection(position, Vector3.Normalize(normal), t);
        }
    }
}
