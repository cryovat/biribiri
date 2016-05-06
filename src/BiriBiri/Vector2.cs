// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Bridge;
using Bridge.Html5;

namespace BiriBiri
{
    public class Vector2 : IEquatable<Vector2>
    {
        #region Constants

        public static Vector2 Zero { get { return new Vector2(0, 0); } }
        public static Vector2 One { get { return new Vector2(1, 1); } }
        public static Vector2 UnitX { get { return new Vector2(1, 0); } }
        public static Vector2 UnitY { get { return new Vector2(0, 1); } }

        #endregion

        public readonly Float32Array Components = new Float32Array(2);

        [FieldProperty]
        public double X { get; set; }
        [FieldProperty]
        public double Y { get; set; }

        private Vector2()
        {
        }

        public Vector2(double value)
        {
            X = value;
            Y = value;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region Operators

        public void Add(double value)
        {
            X += value;
            Y += value;
        }

        public void Add(double x, double y, double scale)
        {
            X += x * scale;
            Y += y * scale;
        }

        public void Add(Vector2 vector)
        {
            X += vector.X;
            Y += vector.Y;
        }

        public void Add(Vector2 vector, double scale)
        {
            X += vector.X * scale;
            Y += vector.Y * scale;
        }

        public void Subtract(double value)
        {
            X -= value;
            Y -= value;
        }

        public void Subtract(double x, double y, double scale)
        {
            X -= x * scale;
            Y -= y * scale;
        }

        public void Subtract(Vector2 vector)
        {
            X -= vector.X;
            Y -= vector.Y;
        }

        public void Subtract(Vector2 vector, double scale)
        {
            X -= vector.X * scale;
            Y -= vector.Y * scale;
        }

        public void Multiply(double value)
        {
            X *= value;
            Y *= value;
        }

        public void Multiply(double x, double y, double scale)
        {
            X *= x * scale;
            Y *= y * scale;
        }

        public void Multiply(Vector2 vector)
        {
            X *= vector.X;
            Y *= vector.Y;
        }

        public void Multiply(Vector2 vector, double scale)
        {
            X *= vector.X * scale;
            Y *= vector.Y * scale;
        }

        public void Divide(double value)
        {
            X /= value;
            Y /= value;
        }

        public void Divide(double x, double y, double scale)
        {
            X /= x * scale;
            Y /= y * scale;
        }

        public void Divide(Vector2 vector)
        {
            X /= vector.X;
            Y /= vector.Y;
        }

        public void Divide(Vector2 vector, double scale)
        {
            X /= vector.X * scale;
            Y /= vector.Y * scale;
        }

        public bool EqualTo(double value)
        {
            return X == value && Y == value;
        }

        public bool EqualTo(double x, double y)
        {
            return X == x && Y == y;
        }

        public bool EqualTo(Vector2 vector)
        {
            return X == vector.X && Y == vector.Y;
        }

        #endregion

        public void Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, double amount1, double amount2)
        {
            X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
        }

        public void CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, double amount)
        {
            X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
        }

        public void Clamp(Vector2 value1, Vector2 min, Vector2 max)
        {
            X = MathHelper.Clamp(value1.X, min.X, max.X);
            Y = MathHelper.Clamp(value1.Y, min.Y, max.Y);
        }

        public double DistanceTo(Vector2 vector)
        {
            return Distance(this, vector);
        }

        public static double Distance(Vector2 value1, Vector2 value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        public double DistanceToSquared(Vector2 vector)
        {
            return DistanceSquared(this, vector);
        }

        public static double DistanceSquared(Vector2 value1, Vector2 value2)
        {
            double v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        public double DotWith(Vector2 vector)
        {
            return Dot(this, vector);
        }

        public static double Dot(Vector2 value1, Vector2 value2)
        {
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return Equals((Vector2)obj);
            }

            return false;
        }

        public bool Equals(Vector2 vector)
        {
            return (X == vector.X) && (Y == vector.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public double Length()
        {
            return Math.Sqrt((X * X) + (Y * Y));
        }

        public double LengthSquared()
        {
            return (X * X) + (Y * Y);
        }

        public void Lerp(Vector2 value1, Vector2 value2, double amount)
        {
            X = MathHelper.Lerp(value1.X, value2.X, amount);
            Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
        }

        public void LerpPrecise(Vector2 value1, Vector2 value2, double amount)
        {
            X = MathHelper.LerpPrecise(value1.X, value2.X, amount);
            Y = MathHelper.LerpPrecise(value1.Y, value2.Y, amount);
        }

        public void Max(Vector2 value1, Vector2 value2)
        {
            X = value1.X > value2.X ? value1.X : value2.X;
            Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        public void Min(Vector2 value1, Vector2 value2)
        {
            X = value1.X < value2.X ? value1.X : value2.X;
            Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        public void Negate()
        {
            X = -X;
            Y = -Y;
        }

        public void NegateTo(Vector2 vector)
        {
            vector.X = -X;
            vector.Y = -Y;
        }

        public void Normalize()
        {
            var val = 1.0f / Math.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        public void Reflect(Vector2 vector, Vector2 normal)
        {
            double val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            X = vector.X - (normal.X * val);
            Y = vector.Y - (normal.Y * val);
        }

        public void SmoothStep(Vector2 value1, Vector2 value2, double amount)
        {
            X = MathHelper.SmoothStep(value1.X, value2.X, amount);
            Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
        }

        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + "}";
        }

        public Vector2 Copy()
        {
            return new Vector2(X, Y);
        }
    }
}
