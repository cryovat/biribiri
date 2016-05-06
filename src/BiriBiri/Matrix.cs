// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Bridge.Html5;

namespace BiriBiri
{
    public class Matrix : IEquatable<Matrix>
    {
        public readonly Float32Array Raw = new Float32Array(16);

        private const int M11 = 0;
        private const int M12 = 1;
        private const int M13 = 2;
        private const int M14 = 3;
        private const int M21 = 4;
        private const int M22 = 5;
        private const int M23 = 6;
        private const int M24 = 7;
        private const int M31 = 8;
        private const int M32 = 9;
        private const int M33 = 10;
        private const int M34 = 11;
        private const int M41 = 12;
        private const int M42 = 13;
        private const int M43 = 14;
        private const int M44 = 15;

        #region Public Constructors

        public Matrix()
        {
        }

        public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
float m32, float m33, float m34, float m41, float m42, float m43, float m44)
        {
            this.Raw[M11] = m11;
            this.Raw[M12] = m12;
            this.Raw[M13] = m13;
            this.Raw[M14] = m14;
            this.Raw[M21] = m21;
            this.Raw[M22] = m22;
            this.Raw[M23] = m23;
            this.Raw[M24] = m24;
            this.Raw[M31] = m31;
            this.Raw[M32] = m32;
            this.Raw[M33] = m33;
            this.Raw[M34] = m34;
            this.Raw[M41] = m41;
            this.Raw[M42] = m42;
            this.Raw[M43] = m43;
            this.Raw[M44] = m44;
        }

        #endregion

        #region Indexers

        public float this[int index]
        {
            get
            {
                if (index < 0 || 15 < index) throw new ArgumentOutOfRangeException();

                return Raw[index];
            }

            set
            {
                if (index < 0 || 15 < value) throw new ArgumentOutOfRangeException();

                Raw[index] = value;
            }
        }

        public float this[int row, int column]
        {
            get { return this[(row * 4) + column]; }

            set { this[(row * 4) + column] = value; }
        }

        #endregion

        #region Public Properties

        public Vector3 Backward
        {
            get { return new Vector3(this.Raw[M31], this.Raw[M32], this.Raw[M33]); }
            set
            {
                this.Raw[M31] = value.X.As<float>();
                this.Raw[M32] = value.Y.As<float>();
                this.Raw[M33] = value.Z.As<float>();
            }
        }

        public Vector3 Down
        {
            get { return new Vector3(-this.Raw[M21], -this.Raw[M22], -this.Raw[M23]); }
            set
            {
                this.Raw[M21] = -value.X.As<float>();
                this.Raw[M22] = -value.Y.As<float>();
                this.Raw[M23] = -value.Z.As<float>();
            }
        }

        public Vector3 Forward
        {
            get { return new Vector3(-this.Raw[M31], -this.Raw[M32], -this.Raw[M33]); }
            set
            {
                this.Raw[M31] = -value.X.As<float>();
                this.Raw[M32] = -value.Y.As<float>();
                this.Raw[M33] = -value.Z.As<float>();
            }
        }

        public Vector3 Left
        {
            get { return new Vector3(-this.Raw[M11], -this.Raw[M12], -this.Raw[M13]); }
            set
            {
                this.Raw[M11] = -value.X.As<float>();
                this.Raw[M12] = -value.Y.As<float>();
                this.Raw[M13] = -value.Z.As<float>();
            }
        }

        public Vector3 Right
        {
            get { return new Vector3(this.Raw[M11], this.Raw[M12], this.Raw[M13]); }
            set
            {
                this.Raw[M11] = value.X.As<float>();
                this.Raw[M12] = value.Y.As<float>();
                this.Raw[M13] = value.Z.As<float>();
            }
        }

        public Vector3 Translation
        {
            get { return new Vector3(this.Raw[M41], this.Raw[M42], this.Raw[M43]); }
            set
            {
                this.Raw[M41] = value.X.As<float>();
                this.Raw[M42] = value.Y.As<float>();
                this.Raw[M43] = value.Z.As<float>();
            }
        }

        public Vector3 Scale
        {
            get { return new Vector3(this.Raw[M11], this.Raw[M22], this.Raw[M33]); }
            set
            {
                this.Raw[M11] = value.X.As<float>();
                this.Raw[M22] = value.Y.As<float>();
                this.Raw[M33] = value.Z.As<float>();
            }
        }

        public Vector3 Up
        {
            get { return new Vector3(this.Raw[M21], this.Raw[M22], this.Raw[M23]); }
            set
            {
                this.Raw[M21] = value.X.As<float>();
                this.Raw[M22] = value.Y.As<float>();
                this.Raw[M23] = value.Z.As<float>();
            }
        }

        #endregion

        #region Public Methods

        public void Add(Matrix matrix)
        {
            Raw[M11] += matrix.Raw[M11];
            Raw[M12] += matrix.Raw[M12];
            Raw[M13] += matrix.Raw[M13];
            Raw[M14] += matrix.Raw[M14];
            Raw[M21] += matrix.Raw[M21];
            Raw[M22] += matrix.Raw[M22];
            Raw[M23] += matrix.Raw[M23];
            Raw[M24] += matrix.Raw[M24];
            Raw[M31] += matrix.Raw[M31];
            Raw[M32] += matrix.Raw[M32];
            Raw[M33] += matrix.Raw[M33];
            Raw[M34] += matrix.Raw[M34];
            Raw[M41] += matrix.Raw[M41];
            Raw[M42] += matrix.Raw[M42];
            Raw[M43] += matrix.Raw[M43];
            Raw[M44] += matrix.Raw[M44];
        }

        public static Matrix CreateIdentity()
        {
            var matrix = new Matrix();
            matrix.InitIdentity();
            return matrix;
        }

        public void InitIdentity()
        {
            MakeIdentity(Raw);
        }

        public static Matrix CreateFromAxisAngle(float xAxis, float yAxis, float zAxis, float angle)
        {
            var result = new Matrix();
            result.InitFromAxisAngle(xAxis, yAxis, zAxis, angle);
            return result;
        }

        public void InitFromAxisAngle(float xAxis, float yAxis, float zAxis, float angle)
        {
            float x = xAxis;
            float y = yAxis;
            float z = zAxis;
            float num2 = (float)Math.Sin((double)angle);
            float num = (float)Math.Cos((double)angle);
            float num11 = x * x;
            float num10 = y * y;
            float num9 = z * z;
            float num8 = x * y;
            float num7 = x * z;
            float num6 = y * z;
            Raw[M11] = num11 + (num * (1f - num11));
            Raw[M12] = (num8 - (num * num8)) + (num2 * z);
            Raw[M13] = (num7 - (num * num7)) - (num2 * y);
            Raw[M14] = 0;
            Raw[M21] = (num8 - (num * num8)) - (num2 * z);
            Raw[M22] = num10 + (num * (1f - num10));
            Raw[M23] = (num6 - (num * num6)) + (num2 * x);
            Raw[M24] = 0;
            Raw[M31] = (num7 - (num * num7)) + (num2 * y);
            Raw[M32] = (num6 - (num * num6)) - (num2 * x);
            Raw[M33] = num9 + (num * (1f - num9));
            Raw[M34] = 0;
            Raw[M41] = 0;
            Raw[M42] = 0;
            Raw[M43] = 0;
            Raw[M44] = 1;
        }

        public void InitOrthographic(float width, float height, float zNearPlane, float zFarPlane)
        {
            Raw[M11] = 2f / width;
            Raw[M12] = Raw[M13] = Raw[M14] = 0f;
            Raw[M22] = 2f / height;
            Raw[M21] = Raw[M23] = Raw[M24] = 0f;
            Raw[M33] = 1f / (zNearPlane - zFarPlane);
            Raw[M31] = Raw[M32] = Raw[M34] = 0f;
            Raw[M41] = Raw[M42] = 0f;
            Raw[M43] = zNearPlane / (zNearPlane - zFarPlane);
            Raw[M44] = 1f;
        }

        public void InitOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
        {
            Raw[M11] = (float)(2.0 / ((double)right - (double)left));
            Raw[M12] = 0.0f;
            Raw[M13] = 0.0f;
            Raw[M14] = 0.0f;
            Raw[M21] = 0.0f;
            Raw[M22] = (float)(2.0 / ((double)top - (double)bottom));
            Raw[M23] = 0.0f;
            Raw[M24] = 0.0f;
            Raw[M31] = 0.0f;
            Raw[M32] = 0.0f;
            Raw[M33] = (float)(1.0 / ((double)zNearPlane - (double)zFarPlane));
            Raw[M34] = 0.0f;
            Raw[M41] = (float)(((double)left + (double)right) / ((double)left - (double)right));
            Raw[M42] = (float)(((double)top + (double)bottom) / ((double)bottom - (double)top));
            Raw[M43] = (float)((double)zNearPlane / ((double)zNearPlane - (double)zFarPlane));
            Raw[M44] = 1.0f;
        }

        public static Matrix CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            var matrix = new Matrix();
            matrix.InitPerspective(width, height, nearPlaneDistance, farPlaneDistance);
            return matrix;
        }

        public void InitPerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            Raw[M11] = (2f * nearPlaneDistance) / width;
            Raw[M12] = Raw[M13] = Raw[M14] = 0f;
            Raw[M22] = (2f * nearPlaneDistance) / height;
            Raw[M21] = Raw[M23] = Raw[M24] = 0f;
            Raw[M33] = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            Raw[M31] = Raw[M32] = 0f;
            Raw[M34] = -1f;
            Raw[M41] = Raw[M42] = Raw[M44] = 0f;
            Raw[M43] = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
        }

        public void InitPerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance,
float farPlaneDistance)
        {
            if ((fieldOfView <= 0f) || (fieldOfView >= 3.141593f))
            {
                throw new ArgumentException("fieldOfView <= 0 or >= PI");
            }
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            float num = 1f / ((float)Math.Tan((double)(fieldOfView * 0.5f)));
            float num9 = num / aspectRatio;
            Raw[M11] = num9;
            Raw[M12] = Raw[M13] = Raw[M14] = 0;
            Raw[M22] = num;
            Raw[M21] = Raw[M23] = Raw[M24] = 0;
            Raw[M31] = Raw[M32] = 0f;
            Raw[M33] = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            Raw[M34] = -1;
            Raw[M41] = Raw[M42] = Raw[M44] = 0;
            Raw[M43] = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
        }

        public void InitPerspectiveOffCenter(float left, float right, float bottom, float top, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0f)
            {
                throw new ArgumentException("nearPlaneDistance <= 0");
            }
            if (farPlaneDistance <= 0f)
            {
                throw new ArgumentException("farPlaneDistance <= 0");
            }
            if (nearPlaneDistance >= farPlaneDistance)
            {
                throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
            }
            Raw[M11] = (2f * nearPlaneDistance) / (right - left);
            Raw[M12] = Raw[M13] = Raw[M14] = 0;
            Raw[M22] = (2f * nearPlaneDistance) / (top - bottom);
            Raw[M21] = Raw[M23] = Raw[M24] = 0;
            Raw[M31] = (left + right) / (right - left);
            Raw[M32] = (top + bottom) / (top - bottom);
            Raw[M33] = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            Raw[M34] = -1;
            Raw[M43] = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);
            Raw[M41] = Raw[M42] = Raw[M44] = 0;
        }

        public void InitRotationX(float radians)
        {
            MakeIdentity(Raw);

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            Raw[M22] = val1;
            Raw[M23] = val2;
            Raw[M32] = -val2;
            Raw[M33] = val1;
        }


        public void InitRotationY(float radians)
        {
            MakeIdentity(Raw);

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            Raw[M11] = val1;
            Raw[M13] = -val2;
            Raw[M31] = val2;
            Raw[M33] = val1;
        }

        public void InitRotationZ(float radians)
        {
            MakeIdentity(Raw);

            var val1 = (float)Math.Cos(radians);
            var val2 = (float)Math.Sin(radians);

            Raw[M11] = val1;
            Raw[M12] = val2;
            Raw[M21] = -val2;
            Raw[M22] = val1;
        }

        public void InitScale(float scale)
        {
            InitScale(scale, scale, scale);
        }

        public void InitScale(float xScale, float yScale, float zScale)
        {
            Raw[M11] = xScale;
            Raw[M12] = 0;
            Raw[M13] = 0;
            Raw[M14] = 0;
            Raw[M21] = 0;
            Raw[M22] = yScale;
            Raw[M23] = 0;
            Raw[M24] = 0;
            Raw[M31] = 0;
            Raw[M32] = 0;
            Raw[M33] = zScale;
            Raw[M34] = 0;
            Raw[M41] = 0;
            Raw[M42] = 0;
            Raw[M43] = 0;
            Raw[M44] = 1;
        }

        public static Matrix CreateTranslation(float xPosition, float yPosition, float zPosition)
        {
            var result = new Matrix();
            result.InitTranslation(xPosition, yPosition, zPosition);
            return result;
        }

        public void InitTranslation(float xPosition, float yPosition, float zPosition)
        {
            Raw[M11] = 1;
            Raw[M12] = 0;
            Raw[M13] = 0;
            Raw[M14] = 0;
            Raw[M21] = 0;
            Raw[M22] = 1;
            Raw[M23] = 0;
            Raw[M24] = 0;
            Raw[M31] = 0;
            Raw[M32] = 0;
            Raw[M33] = 1;
            Raw[M34] = 0;
            Raw[M41] = xPosition;
            Raw[M42] = yPosition;
            Raw[M43] = zPosition;
            Raw[M44] = 1;
        }

        public float Determinant()
        {
            float num22 = this.Raw[M11];
            float num21 = this.Raw[M12];
            float num20 = this.Raw[M13];
            float num19 = this.Raw[M14];
            float num12 = this.Raw[M21];
            float num11 = this.Raw[M22];
            float num10 = this.Raw[M23];
            float num9 = this.Raw[M24];
            float num8 = this.Raw[M31];
            float num7 = this.Raw[M32];
            float num6 = this.Raw[M33];
            float num5 = this.Raw[M34];
            float num4 = this.Raw[M41];
            float num3 = this.Raw[M42];
            float num2 = this.Raw[M43];
            float num = this.Raw[M44];
            float num18 = (num6 * num) - (num5 * num2);
            float num17 = (num7 * num) - (num5 * num3);
            float num16 = (num7 * num2) - (num6 * num3);
            float num15 = (num8 * num) - (num5 * num4);
            float num14 = (num8 * num2) - (num6 * num4);
            float num13 = (num8 * num3) - (num7 * num4);
            return ((((num22 * (((num11 * num18) - (num10 * num17)) + (num9 * num16))) -
                      (num21 * (((num12 * num18) - (num10 * num15)) + (num9 * num14)))) +
                     (num20 * (((num12 * num17) - (num11 * num15)) + (num9 * num13)))) -
                    (num19 * (((num12 * num16) - (num11 * num14)) + (num10 * num13))));
        }

        public void Divide(Matrix matrix)
        {
            Raw[M11] = Raw[M11] / matrix.Raw[M11];
            Raw[M12] = Raw[M12] / matrix.Raw[M12];
            Raw[M13] = Raw[M13] / matrix.Raw[M13];
            Raw[M14] = Raw[M14] / matrix.Raw[M14];
            Raw[M21] = Raw[M21] / matrix.Raw[M21];
            Raw[M22] = Raw[M22] / matrix.Raw[M22];
            Raw[M23] = Raw[M23] / matrix.Raw[M23];
            Raw[M24] = Raw[M24] / matrix.Raw[M24];
            Raw[M31] = Raw[M31] / matrix.Raw[M31];
            Raw[M32] = Raw[M32] / matrix.Raw[M32];
            Raw[M33] = Raw[M33] / matrix.Raw[M33];
            Raw[M34] = Raw[M34] / matrix.Raw[M34];
            Raw[M41] = Raw[M41] / matrix.Raw[M41];
            Raw[M42] = Raw[M42] / matrix.Raw[M42];
            Raw[M43] = Raw[M43] / matrix.Raw[M43];
            Raw[M44] = Raw[M44] / matrix.Raw[M44];
        }

        public void Divide(float divider)
        {
            float num = 1f / divider;
            Raw[M11] = Raw[M11] * num;
            Raw[M12] = Raw[M12] * num;
            Raw[M13] = Raw[M13] * num;
            Raw[M14] = Raw[M14] * num;
            Raw[M21] = Raw[M21] * num;
            Raw[M22] = Raw[M22] * num;
            Raw[M23] = Raw[M23] * num;
            Raw[M24] = Raw[M24] * num;
            Raw[M31] = Raw[M31] * num;
            Raw[M32] = Raw[M32] * num;
            Raw[M33] = Raw[M33] * num;
            Raw[M34] = Raw[M34] * num;
            Raw[M41] = Raw[M41] * num;
            Raw[M42] = Raw[M42] * num;
            Raw[M43] = Raw[M43] * num;
            Raw[M44] = Raw[M44] * num;
        }

        public bool Equals(Matrix other)
        {
            return ((((((this.Raw[M11] == other.Raw[M11]) && (this.Raw[M22] == other.Raw[M22])) &&
                       ((this.Raw[M33] == other.Raw[M33]) && (this.Raw[M44] == other.Raw[M44]))) &&
                      (((this.Raw[M12] == other.Raw[M12]) && (this.Raw[M13] == other.Raw[M13])) &&
                       ((this.Raw[M14] == other.Raw[M14]) && (this.Raw[M21] == other.Raw[M21])))) &&
                     ((((this.Raw[M23] == other.Raw[M23]) && (this.Raw[M24] == other.Raw[M24])) &&
                       ((this.Raw[M31] == other.Raw[M31]) && (this.Raw[M32] == other.Raw[M32]))) &&
                      (((this.Raw[M34] == other.Raw[M34]) && (this.Raw[M41] == other.Raw[M41])) &&
                       (this.Raw[M42] == other.Raw[M42])))) && (this.Raw[M43] == other.Raw[M43]));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Matrix)
            {
                flag = this.Equals((Matrix)obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (((((((((((((((this.Raw[M11].GetHashCode() + this.Raw[M12].GetHashCode()) +
                                 this.Raw[M13].GetHashCode()) + this.Raw[M14].GetHashCode()) +
                               this.Raw[M21].GetHashCode()) + this.Raw[M22].GetHashCode()) + this.Raw[M23].GetHashCode()) +
                            this.Raw[M24].GetHashCode()) + this.Raw[M31].GetHashCode()) + this.Raw[M32].GetHashCode()) +
                         this.Raw[M33].GetHashCode()) + this.Raw[M34].GetHashCode()) + this.Raw[M41].GetHashCode()) +
                      this.Raw[M42].GetHashCode()) + this.Raw[M43].GetHashCode()) + this.Raw[M44].GetHashCode());
        }

        public void Invert()
        {
            float num1 = Raw[M11];
            float num2 = Raw[M12];
            float num3 = Raw[M13];
            float num4 = Raw[M14];
            float num5 = Raw[M21];
            float num6 = Raw[M22];
            float num7 = Raw[M23];
            float num8 = Raw[M24];
            float num9 = Raw[M31];
            float num10 = Raw[M32];
            float num11 = Raw[M33];
            float num12 = Raw[M34];
            float num13 = Raw[M41];
            float num14 = Raw[M42];
            float num15 = Raw[M43];
            float num16 = Raw[M44];
            float num17 = (float)((double)num11 * (double)num16 - (double)num12 * (double)num15);
            float num18 = (float)((double)num10 * (double)num16 - (double)num12 * (double)num14);
            float num19 = (float)((double)num10 * (double)num15 - (double)num11 * (double)num14);
            float num20 = (float)((double)num9 * (double)num16 - (double)num12 * (double)num13);
            float num21 = (float)((double)num9 * (double)num15 - (double)num11 * (double)num13);
            float num22 = (float)((double)num9 * (double)num14 - (double)num10 * (double)num13);
            float num23 =
                (float)((double)num6 * (double)num17 - (double)num7 * (double)num18 + (double)num8 * (double)num19);
            float num24 =
                (float)-((double)num5 * (double)num17 - (double)num7 * (double)num20 + (double)num8 * (double)num21);
            float num25 =
                (float)((double)num5 * (double)num18 - (double)num6 * (double)num20 + (double)num8 * (double)num22);
            float num26 =
                (float)-((double)num5 * (double)num19 - (double)num6 * (double)num21 + (double)num7 * (double)num22);
            float num27 =
                (float)
                    (1.0 /
                     ((double)num1 * (double)num23 + (double)num2 * (double)num24 + (double)num3 * (double)num25 +
                      (double)num4 * (double)num26));

            Raw[M11] = num23 * num27;
            Raw[M21] = num24 * num27;
            Raw[M31] = num25 * num27;
            Raw[M41] = num26 * num27;
            Raw[M12] =
                (float)-((double)num2 * (double)num17 - (double)num3 * (double)num18 + (double)num4 * (double)num19) *
                num27;
            Raw[M22] =
                (float)((double)num1 * (double)num17 - (double)num3 * (double)num20 + (double)num4 * (double)num21) *
                num27;
            Raw[M32] =
                (float)-((double)num1 * (double)num18 - (double)num2 * (double)num20 + (double)num4 * (double)num22) *
                num27;
            Raw[M42] =
                (float)((double)num1 * (double)num19 - (double)num2 * (double)num21 + (double)num3 * (double)num22) *
                num27;
            float num28 = (float)((double)num7 * (double)num16 - (double)num8 * (double)num15);
            float num29 = (float)((double)num6 * (double)num16 - (double)num8 * (double)num14);
            float num30 = (float)((double)num6 * (double)num15 - (double)num7 * (double)num14);
            float num31 = (float)((double)num5 * (double)num16 - (double)num8 * (double)num13);
            float num32 = (float)((double)num5 * (double)num15 - (double)num7 * (double)num13);
            float num33 = (float)((double)num5 * (double)num14 - (double)num6 * (double)num13);
            Raw[M13] =
                (float)((double)num2 * (double)num28 - (double)num3 * (double)num29 + (double)num4 * (double)num30) *
                num27;
            Raw[M23] =
                (float)-((double)num1 * (double)num28 - (double)num3 * (double)num31 + (double)num4 * (double)num32) *
                num27;
            Raw[M33] =
                (float)((double)num1 * (double)num29 - (double)num2 * (double)num31 + (double)num4 * (double)num33) *
                num27;
            Raw[M43] =
                (float)-((double)num1 * (double)num30 - (double)num2 * (double)num32 + (double)num3 * (double)num33) *
                num27;
            float num34 = (float)((double)num7 * (double)num12 - (double)num8 * (double)num11);
            float num35 = (float)((double)num6 * (double)num12 - (double)num8 * (double)num10);
            float num36 = (float)((double)num6 * (double)num11 - (double)num7 * (double)num10);
            float num37 = (float)((double)num5 * (double)num12 - (double)num8 * (double)num9);
            float num38 = (float)((double)num5 * (double)num11 - (double)num7 * (double)num9);
            float num39 = (float)((double)num5 * (double)num10 - (double)num6 * (double)num9);
            Raw[M14] =
                (float)-((double)num2 * (double)num34 - (double)num3 * (double)num35 + (double)num4 * (double)num36) *
                num27;
            Raw[M24] =
                (float)((double)num1 * (double)num34 - (double)num3 * (double)num37 + (double)num4 * (double)num38) *
                num27;
            Raw[M34] =
                (float)-((double)num1 * (double)num35 - (double)num2 * (double)num37 + (double)num4 * (double)num39) *
                num27;
            Raw[M44] =
                (float)((double)num1 * (double)num36 - (double)num2 * (double)num38 + (double)num3 * (double)num39) *
                num27;


            /*


                        // Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix
            //
            // 1. Calculate the 2x2 determinants needed the 4x4 determinant based on the 2x2 determinants
            // 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I
            // 4. Divide adjugate matrix with the determinant to find the inverse

            float det1, det2, det3, det4, det5, det6, det7, det8, det9, det10, det11, det12;
            float detMatrix;
            FindDeterminants(ref matrix, out detMatrix, out det1, out det2, out det3, out det4, out det5, out det6,
                             out det7, out det8, out det9, out det10, out det11, out det12);

            float invDetMatrix = 1f / detMatrix;

            Matrix ret; // Allow for matrix and result to point to the same structure

            ret.M11 = (matrix.Raw[M22]*det12 - matrix.Raw[M23]*det11 + matrix.Raw[M24]*det10) * invDetMatrix;
            ret.M12 = (-matrix.Raw[M12]*det12 + matrix.Raw[M13]*det11 - matrix.Raw[M14]*det10) * invDetMatrix;
            ret.M13 = (matrix.Raw[M42]*det6 - matrix.Raw[M43]*det5 + matrix.Raw[M44]*det4) * invDetMatrix;
            ret.M14 = (-matrix.Raw[M32]*det6 + matrix.Raw[M33]*det5 - matrix.Raw[M34]*det4) * invDetMatrix;
            ret.M21 = (-matrix.Raw[M21]*det12 + matrix.Raw[M23]*det9 - matrix.Raw[M24]*det8) * invDetMatrix;
            ret.M22 = (matrix.Raw[M11]*det12 - matrix.Raw[M13]*det9 + matrix.Raw[M14]*det8) * invDetMatrix;
            ret.M23 = (-matrix.Raw[M41]*det6 + matrix.Raw[M43]*det3 - matrix.Raw[M44]*det2) * invDetMatrix;
            ret.M24 = (matrix.Raw[M31]*det6 - matrix.Raw[M33]*det3 + matrix.Raw[M34]*det2) * invDetMatrix;
            ret.M31 = (matrix.Raw[M21]*det11 - matrix.Raw[M22]*det9 + matrix.Raw[M24]*det7) * invDetMatrix;
            ret.M32 = (-matrix.Raw[M11]*det11 + matrix.Raw[M12]*det9 - matrix.Raw[M14]*det7) * invDetMatrix;
            ret.M33 = (matrix.Raw[M41]*det5 - matrix.Raw[M42]*det3 + matrix.Raw[M44]*det1) * invDetMatrix;
            ret.M34 = (-matrix.Raw[M31]*det5 + matrix.Raw[M32]*det3 - matrix.Raw[M34]*det1) * invDetMatrix;
            ret.M41 = (-matrix.Raw[M21]*det10 + matrix.Raw[M22]*det8 - matrix.Raw[M23]*det7) * invDetMatrix;
            ret.M42 = (matrix.Raw[M11]*det10 - matrix.Raw[M12]*det8 + matrix.Raw[M13]*det7) * invDetMatrix;
            ret.M43 = (-matrix.Raw[M41]*det4 + matrix.Raw[M42]*det2 - matrix.Raw[M43]*det1) * invDetMatrix;
            ret.M44 = (matrix.Raw[M31]*det4 - matrix.Raw[M32]*det2 + matrix.Raw[M33]*det1) * invDetMatrix;

            result = ret;
            */
        }

        public void Lerp(Matrix matrix1, Matrix matrix2, float amount)
        {
            Raw[M11] = matrix1.Raw[M11] + ((matrix2.Raw[M11] - matrix1.Raw[M11]) * amount);
            Raw[M12] = matrix1.Raw[M12] + ((matrix2.Raw[M12] - matrix1.Raw[M12]) * amount);
            Raw[M13] = matrix1.Raw[M13] + ((matrix2.Raw[M13] - matrix1.Raw[M13]) * amount);
            Raw[M14] = matrix1.Raw[M14] + ((matrix2.Raw[M14] - matrix1.Raw[M14]) * amount);
            Raw[M21] = matrix1.Raw[M21] + ((matrix2.Raw[M21] - matrix1.Raw[M21]) * amount);
            Raw[M22] = matrix1.Raw[M22] + ((matrix2.Raw[M22] - matrix1.Raw[M22]) * amount);
            Raw[M23] = matrix1.Raw[M23] + ((matrix2.Raw[M23] - matrix1.Raw[M23]) * amount);
            Raw[M24] = matrix1.Raw[M24] + ((matrix2.Raw[M24] - matrix1.Raw[M24]) * amount);
            Raw[M31] = matrix1.Raw[M31] + ((matrix2.Raw[M31] - matrix1.Raw[M31]) * amount);
            Raw[M32] = matrix1.Raw[M32] + ((matrix2.Raw[M32] - matrix1.Raw[M32]) * amount);
            Raw[M33] = matrix1.Raw[M33] + ((matrix2.Raw[M33] - matrix1.Raw[M33]) * amount);
            Raw[M34] = matrix1.Raw[M34] + ((matrix2.Raw[M34] - matrix1.Raw[M34]) * amount);
            Raw[M41] = matrix1.Raw[M41] + ((matrix2.Raw[M41] - matrix1.Raw[M41]) * amount);
            Raw[M42] = matrix1.Raw[M42] + ((matrix2.Raw[M42] - matrix1.Raw[M42]) * amount);
            Raw[M43] = matrix1.Raw[M43] + ((matrix2.Raw[M43] - matrix1.Raw[M43]) * amount);
            Raw[M44] = matrix1.Raw[M44] + ((matrix2.Raw[M44] - matrix1.Raw[M44]) * amount);
        }

        public void Multiply(Matrix matrix1, Matrix matrix2)
        {
            Raw[M11] = (((matrix1.Raw[M11] * matrix2.Raw[M11]) + (matrix1.Raw[M12] * matrix2.Raw[M21])) +
                        (matrix1.Raw[M13] * matrix2.Raw[M31])) + (matrix1.Raw[M14] * matrix2.Raw[M41]);
            Raw[M12] = (((matrix1.Raw[M11] * matrix2.Raw[M12]) + (matrix1.Raw[M12] * matrix2.Raw[M22])) +
                        (matrix1.Raw[M13] * matrix2.Raw[M32])) + (matrix1.Raw[M14] * matrix2.Raw[M42]);
            Raw[M13] = (((matrix1.Raw[M11] * matrix2.Raw[M13]) + (matrix1.Raw[M12] * matrix2.Raw[M23])) +
                        (matrix1.Raw[M13] * matrix2.Raw[M33])) + (matrix1.Raw[M14] * matrix2.Raw[M43]);
            Raw[M14] = (((matrix1.Raw[M11] * matrix2.Raw[M14]) + (matrix1.Raw[M12] * matrix2.Raw[M24])) +
                        (matrix1.Raw[M13] * matrix2.Raw[M34])) + (matrix1.Raw[M14] * matrix2.Raw[M44]);
            Raw[M21] = (((matrix1.Raw[M21] * matrix2.Raw[M11]) + (matrix1.Raw[M22] * matrix2.Raw[M21])) +
                        (matrix1.Raw[M23] * matrix2.Raw[M31])) + (matrix1.Raw[M24] * matrix2.Raw[M41]);
            Raw[M22] = (((matrix1.Raw[M21] * matrix2.Raw[M12]) + (matrix1.Raw[M22] * matrix2.Raw[M22])) +
                        (matrix1.Raw[M23] * matrix2.Raw[M32])) + (matrix1.Raw[M24] * matrix2.Raw[M42]);
            Raw[M23] = (((matrix1.Raw[M21] * matrix2.Raw[M13]) + (matrix1.Raw[M22] * matrix2.Raw[M23])) +
                        (matrix1.Raw[M23] * matrix2.Raw[M33])) + (matrix1.Raw[M24] * matrix2.Raw[M43]);
            Raw[M24] = (((matrix1.Raw[M21] * matrix2.Raw[M14]) + (matrix1.Raw[M22] * matrix2.Raw[M24])) +
                        (matrix1.Raw[M23] * matrix2.Raw[M34])) + (matrix1.Raw[M24] * matrix2.Raw[M44]);
            Raw[M31] = (((matrix1.Raw[M31] * matrix2.Raw[M11]) + (matrix1.Raw[M32] * matrix2.Raw[M21])) +
                        (matrix1.Raw[M33] * matrix2.Raw[M31])) + (matrix1.Raw[M34] * matrix2.Raw[M41]);
            Raw[M32] = (((matrix1.Raw[M31] * matrix2.Raw[M12]) + (matrix1.Raw[M32] * matrix2.Raw[M22])) +
                        (matrix1.Raw[M33] * matrix2.Raw[M32])) + (matrix1.Raw[M34] * matrix2.Raw[M42]);
            Raw[M33] = (((matrix1.Raw[M31] * matrix2.Raw[M13]) + (matrix1.Raw[M32] * matrix2.Raw[M23])) +
                        (matrix1.Raw[M33] * matrix2.Raw[M33])) + (matrix1.Raw[M34] * matrix2.Raw[M43]);
            Raw[M34] = (((matrix1.Raw[M31] * matrix2.Raw[M14]) + (matrix1.Raw[M32] * matrix2.Raw[M24])) +
                        (matrix1.Raw[M33] * matrix2.Raw[M34])) + (matrix1.Raw[M34] * matrix2.Raw[M44]);
            Raw[M41] = (((matrix1.Raw[M41] * matrix2.Raw[M11]) + (matrix1.Raw[M42] * matrix2.Raw[M21])) +
                        (matrix1.Raw[M43] * matrix2.Raw[M31])) + (matrix1.Raw[M44] * matrix2.Raw[M41]);
            Raw[M42] = (((matrix1.Raw[M41] * matrix2.Raw[M12]) + (matrix1.Raw[M42] * matrix2.Raw[M22])) +
                        (matrix1.Raw[M43] * matrix2.Raw[M32])) + (matrix1.Raw[M44] * matrix2.Raw[M42]);
            Raw[M43] = (((matrix1.Raw[M41] * matrix2.Raw[M13]) + (matrix1.Raw[M42] * matrix2.Raw[M23])) +
                        (matrix1.Raw[M43] * matrix2.Raw[M33])) + (matrix1.Raw[M44] * matrix2.Raw[M43]);
            Raw[M44] = (((matrix1.Raw[M41] * matrix2.Raw[M14]) + (matrix1.Raw[M42] * matrix2.Raw[M24])) +
                        (matrix1.Raw[M43] * matrix2.Raw[M34])) + (matrix1.Raw[M44] * matrix2.Raw[M44]);
        }

        public void Multiply(float scaleFactor)
        {
            Raw[M11] *= scaleFactor;
            Raw[M12] *= scaleFactor;
            Raw[M13] *= scaleFactor;
            Raw[M14] *= scaleFactor;
            Raw[M21] *= scaleFactor;
            Raw[M22] *= scaleFactor;
            Raw[M23] *= scaleFactor;
            Raw[M24] *= scaleFactor;
            Raw[M31] *= scaleFactor;
            Raw[M32] *= scaleFactor;
            Raw[M33] *= scaleFactor;
            Raw[M34] *= scaleFactor;
            Raw[M41] *= scaleFactor;
            Raw[M42] *= scaleFactor;
            Raw[M43] *= scaleFactor;
            Raw[M44] *= scaleFactor;
        }

        public void Multiply(Matrix matrix1, float scaleFactor)
        {
            Raw[M11] = matrix1.Raw[M11] * scaleFactor;
            Raw[M12] = matrix1.Raw[M12] * scaleFactor;
            Raw[M13] = matrix1.Raw[M13] * scaleFactor;
            Raw[M14] = matrix1.Raw[M14] * scaleFactor;
            Raw[M21] = matrix1.Raw[M21] * scaleFactor;
            Raw[M22] = matrix1.Raw[M22] * scaleFactor;
            Raw[M23] = matrix1.Raw[M23] * scaleFactor;
            Raw[M24] = matrix1.Raw[M24] * scaleFactor;
            Raw[M31] = matrix1.Raw[M31] * scaleFactor;
            Raw[M32] = matrix1.Raw[M32] * scaleFactor;
            Raw[M33] = matrix1.Raw[M33] * scaleFactor;
            Raw[M34] = matrix1.Raw[M34] * scaleFactor;
            Raw[M41] = matrix1.Raw[M41] * scaleFactor;
            Raw[M42] = matrix1.Raw[M42] * scaleFactor;
            Raw[M43] = matrix1.Raw[M43] * scaleFactor;
            Raw[M44] = matrix1.Raw[M44] * scaleFactor;
        }

        public void Negate()
        {
            Raw[M11] = -Raw[M11];
            Raw[M12] = -Raw[M12];
            Raw[M13] = -Raw[M13];
            Raw[M14] = -Raw[M14];
            Raw[M21] = -Raw[M21];
            Raw[M22] = -Raw[M22];
            Raw[M23] = -Raw[M23];
            Raw[M24] = -Raw[M24];
            Raw[M31] = -Raw[M31];
            Raw[M32] = -Raw[M32];
            Raw[M33] = -Raw[M33];
            Raw[M34] = -Raw[M34];
            Raw[M41] = -Raw[M41];
            Raw[M42] = -Raw[M42];
            Raw[M43] = -Raw[M43];
            Raw[M44] = -Raw[M44];
        }

        public void Subtract(Matrix matrix)
        {
            Raw[M11] = Raw[M11] - matrix.Raw[M11];
            Raw[M12] = Raw[M12] - matrix.Raw[M12];
            Raw[M13] = Raw[M13] - matrix.Raw[M13];
            Raw[M14] = Raw[M14] - matrix.Raw[M14];
            Raw[M21] = Raw[M21] - matrix.Raw[M21];
            Raw[M22] = Raw[M22] - matrix.Raw[M22];
            Raw[M23] = Raw[M23] - matrix.Raw[M23];
            Raw[M24] = Raw[M24] - matrix.Raw[M24];
            Raw[M31] = Raw[M31] - matrix.Raw[M31];
            Raw[M32] = Raw[M32] - matrix.Raw[M32];
            Raw[M33] = Raw[M33] - matrix.Raw[M33];
            Raw[M34] = Raw[M34] - matrix.Raw[M34];
            Raw[M41] = Raw[M41] - matrix.Raw[M41];
            Raw[M42] = Raw[M42] - matrix.Raw[M42];
            Raw[M43] = Raw[M43] - matrix.Raw[M43];
            Raw[M44] = Raw[M44] - matrix.Raw[M44];
        }

        public override string ToString()
        {
            return "{M11:" + M11 + " M12:" + M12 + " M13:" + M13 + " M14:" + M14 + "}"
                + " {M21:" + M21 + " M22:" + M22 + " M23:" + M23 + " M24:" + M24 + "}"
                + " {M31:" + M31 + " M32:" + M32 + " M33:" + M33 + " M34:" + M34 + "}"
                + " {M41:" + M41 + " M42:" + M42 + " M43:" + M43 + " M44:" + M44 + "}";
        }

        public void Transpose()
        {
            var copy = new Float32Array(Raw);

            Raw[M11] = copy[M11];
            Raw[M12] = copy[M21];
            Raw[M13] = copy[M31];
            Raw[M14] = copy[M41];

            Raw[M21] = copy[M12];
            Raw[M22] = copy[M22];
            Raw[M23] = copy[M32];
            Raw[M24] = copy[M42];

            Raw[M31] = copy[M13];
            Raw[M32] = copy[M23];
            Raw[M33] = copy[M33];
            Raw[M34] = copy[M43];

            Raw[M41] = copy[M14];
            Raw[M42] = copy[M24];
            Raw[M43] = copy[M34];
            Raw[M44] = copy[M44];
        }
        #endregion

        #region Private Static Methods

        private static void MakeIdentity(Float32Array array)
        {
            array[M11] = 1;
            array[M12] = 0;
            array[M13] = 0;
            array[M14] = 0;

            array[M21] = 0;
            array[M22] = 1;
            array[M23] = 0;
            array[M24] = 0;

            array[M31] = 0;
            array[M32] = 0;
            array[M33] = 1;
            array[M34] = 0;

            array[M41] = 0;
            array[M42] = 0;
            array[M43] = 0;
            array[M44] = 1;
        }

        #endregion

        public void TransformVector(Vector2 vector)
        {
            var x = (vector.X * Raw[M11]) + (vector.Y * Raw[M21]) + Raw[M31] + Raw[M41];
            var y = (vector.X * Raw[M12]) + (vector.Y * Raw[M22]) + Raw[M32] + Raw[M42];
            vector.X = x;
            vector.Y = y;
        }

        public void TransformVector(Vector3 vector)
        {
            var x = (vector.X * Raw[M11]) + (vector.Y * Raw[M21]) + (vector.Z * Raw[M31]) + Raw[M41];
            var y = (vector.X * Raw[M12]) + (vector.Y * Raw[M22]) + (vector.Z * Raw[M32]) + Raw[M42];
            var z = (vector.X * Raw[M13]) + (vector.Y * Raw[M23]) + (vector.Z * Raw[M33]) + Raw[M43];
            vector.X = x;
            vector.Y = y;
            vector.Z = z;
        }

        public void TransformRectangle(Rectangle rectangle)
        {
            var x = (rectangle.X * Raw[M11]) + (rectangle.Y * Raw[M21]) + Raw[M31] + Raw[M41];
            var y = (rectangle.X * Raw[M12]) + (rectangle.Y * Raw[M22]) + Raw[M32] + Raw[M42];

            var width = (rectangle.Width * Raw[M11]) + (rectangle.Height * Raw[M21]) + Raw[M31] + Raw[M41];
            var height = (rectangle.Width * Raw[M12]) + (rectangle.Height * Raw[M22]) + Raw[M32] + Raw[M42];

            rectangle.X = x;
            rectangle.Y = y;

            rectangle.Width = width;
            rectangle.Height = height;
        }
    }
}