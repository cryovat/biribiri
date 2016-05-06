// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Text;
using Bridge;

namespace BiriBiri
{
    public class Vector3 : IEquatable<Vector3>
    {
        private static readonly Vector3 Zero = CreateZero();

        #region Private Fields

        public static Vector3 CreateZero() { return new Vector3(0f, 0f, 0f); }
        public static Vector3 CreateOne() { return new Vector3(1f, 1f, 1f); }
        public static Vector3 CreateUnitX() { return new Vector3(1f, 0f, 0f); }
        public static Vector3 CreateUnitY() { return new Vector3(0f, 1f, 0f); }
        public static Vector3 CreateUnitZ() { return new Vector3(0f, 0f, 1f); }
        public static Vector3 CreateUp() { return new Vector3(0f, 1f, 0f); }
        public static Vector3 CreateDown() { return new Vector3(0f, -1f, 0f); }
        public static Vector3 CreateRight() { return new Vector3(1f, 0f, 0f); }
        public static Vector3 CreateLeft() { return new Vector3(-1f, 0f, 0f); }
        public static Vector3 CreateForward() { return new Vector3(0f, 0f, -1f); }
        public static Vector3 CreateBackward() { return new Vector3(0f, 0f, 1f); }

        #endregion

        #region Public Fields

        [FieldProperty]
        public double X { get; set; }

        [FieldProperty]
        public double Y { get; set; }

        [FieldProperty]
        public double Z { get; set; }

        #endregion

        #region Internal Properties

        internal string DebugDisplayString
        {
            get
            {
                return string.Concat(
                    this.X.ToString(), "  ",
                    this.Y.ToString(), "  ",
                    this.Z.ToString()
                );
            }
        }

        #endregion

        #region Constructors

        public Vector3()
        {
        }

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(double value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        public Vector3(Vector2 value, double z)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
        }

        #endregion

        #region Public Methods

        public void Add(Vector3 value1, Vector3 value2)
        {
            X = value1.X += value2.X;
            Y = value1.Y += value2.Y;
            Z = value1.Z += value2.Z;
        }

        public void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, double amount1, double amount2)
        {
            X = MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2);
            Y = MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2);
            Z = MathHelper.Barycentric(value1.Z, value2.Z, value3.Z, amount1, amount2);
        }

        public void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, double amount)
        {
            X = MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount);
            Y = MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount);
            Z = MathHelper.CatmullRom(value1.Z, value2.Z, value3.Z, value4.Z, amount);
        }

        public void Clamp(Vector3 min, Vector3 max)
        {
            X = MathHelper.Clamp(X, min.X, max.X);
            Y = MathHelper.Clamp(Y, min.Y, max.Y);
            Z = MathHelper.Clamp(Z, min.Z, max.Z);
        }

        public void Cross(Vector3 vector1, Vector3 vector2)
        {
            var x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
            var y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
            var z = vector1.X * vector2.Y - vector2.X * vector1.Y;
            X = x;
            Y = y;
            Z = z;
        }

        public double DistanceTo(Vector3 value)
        {
            return Math.Sqrt(DistanceToSquared(value));
        }

        public double DistanceToSquared(Vector3 value)
        {
            return (X - value.X) * (X - value.X) +
                    (Y - value.Y) * (Y - value.Y) +
                    (Z - value.Z) * (Z - value.Z);
        }

        public void Divide(Vector3 value)
        {
            X /= value.X;
            Y /= value.Y;
            Z /= value.Z;
        }

        public void Divide(double divider)
        {
            double factor = 1 / divider;
            X *= factor;
            Y *= factor;
            Z *= factor;
        }

        public static double Dot(Vector3 value1, Vector3 value2)
        {
            return value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;

            var other = (Vector3)obj;
            return X == other.X &&
                    Y == other.Y &&
                    Z == other.Z;
        }

        public bool Equals(Vector3 other)
        {
            return X == other.X &&
                    Y == other.Y &&
                    Z == other.Z;
        }

        public override int GetHashCode()
        {
            return (int)(this.X + this.Y + this.Z);
        }

        public void Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, double amount)
        {
            X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
            Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
            Z = MathHelper.Hermite(value1.Z, tangent1.Z, value2.Z, tangent2.Z, amount);
        }

        public double Length()
        {
            return DistanceTo(Zero);
        }

        public double LengthSquared()
        {
            return DistanceToSquared(Zero);
        }

        public void Lerp(Vector3 value1, Vector3 value2, double amount)
        {
            X = MathHelper.Lerp(value1.X, value2.X, amount);
            Y = MathHelper.Lerp(value1.Y, value2.Y, amount);
            Z = MathHelper.Lerp(value1.Z, value2.Z, amount);
        }

        public void LerpPrecise(Vector3 value1, Vector3 value2, double amount)
        {
            X = MathHelper.LerpPrecise(value1.X, value2.X, amount);
            Y = MathHelper.LerpPrecise(value1.Y, value2.Y, amount);
            Z = MathHelper.LerpPrecise(value1.Z, value2.Z, amount);
        }

        public void Max(Vector3 value1, Vector3 value2)
        {
            X = MathHelper.Max(value1.X, value2.X);
            Y = MathHelper.Max(value1.Y, value2.Y);
            Z = MathHelper.Max(value1.Z, value2.Z);
        }

        public void Min(Vector3 value1, Vector3 value2)
        {
            X = MathHelper.Min(value1.X, value2.X);
            Y = MathHelper.Min(value1.Y, value2.Y);
            Z = MathHelper.Min(value1.Z, value2.Z);
        }

        public void Multiply(Vector3 value)
        {
            X *= X;
            Y *= Y;
            Z *= Z;
        }

        public void Multiply(double scaleFactor)
        {
            X *= scaleFactor;
            Y *= scaleFactor;
            Z *= scaleFactor;
        }

        public void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        public void Normalize()
        {
            var factor = DistanceTo(Zero);
            factor = 1f / factor;
            X = X * factor;
            Y = Y * factor;
            Z = Z * factor;
        }

        public void Reflect(Vector3 vector, Vector3 normal)
        {
            // I is the original array
            // N is the normal of the incident plane
            // R = I - (2 * N * ( DotProduct[ I,N] ))
            // inline the dotProduct here instead of calling method
            double dotProduct = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            X = vector.X - (2.0f * normal.X) * dotProduct;
            Y = vector.Y - (2.0f * normal.Y) * dotProduct;
            Z = vector.Z - (2.0f * normal.Z) * dotProduct;
        }

        public void SmoothStep(Vector3 value1, Vector3 value2, double amount)
        {
            X = MathHelper.SmoothStep(value1.X, value2.X, amount);
            Y = MathHelper.SmoothStep(value1.Y, value2.Y, amount);
            Z = MathHelper.SmoothStep(value1.Z, value2.Z, amount);
        }

        public void Subtract(Vector3 value)
        {
            X -= value.X;
            Y -= value.Y;
            Z -= value.Z;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            sb.Append("{X:");
            sb.Append(this.X);
            sb.Append(" Y:");
            sb.Append(this.Y);
            sb.Append(" Z:");
            sb.Append(this.Z);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        public Vector3 Copy()
        {
            return new Vector3(X, Y, Z);
        }
    }
}